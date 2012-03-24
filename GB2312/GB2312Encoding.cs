/*
 * ****************************************************
 *     Copyright (c) Aimeast.  All rights reserved.
 * ****************************************************
 */

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GB2312
{
    public sealed class GB2312Encoding : Encoding
    {
        private const char LEAD_BYTE_CHAR = '\uFFFE';

        static GB2312Encoding()
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("Not supported big endian platform.");
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            int byteCount = 0;
            ushort u;
            char c;

            for (int i = 0; i < count; index++, byteCount++, i++)
            {
                c = chars[index];
                u = Map.UnicodeToGB2312(c);
                if (u > 0xff)
                    byteCount++;
            }

            return byteCount;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int byteCount = 0;
            ushort u;
            char c;

            for (int i = 0; i < charCount; charIndex++, byteIndex++, byteCount++, i++)
            {
                c = chars[charIndex];
                u = Map.UnicodeToGB2312(c);
                if (u == 0 && c != 0)
                {
                    bytes[byteIndex] = 0x3f;    // 0x3f == '?'
                }
                else if (u < 0x100)
                {
                    bytes[byteIndex] = (byte)u;
                }
                else
                {
                    bytes[byteIndex] = (byte)((u >> 8) & 0xff);
                    byteIndex++;
                    byteCount++;
                    bytes[byteIndex] = (byte)(u & 0xff);
                }
            }

            return byteCount;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return GetCharCount(bytes, index, count, null);
        }

        private int GetCharCount(byte[] bytes, int index, int count, GB2312Decoder decoder)
        {
            int charCount = 0;
            ushort u;
            char c;

            for (int i = 0; i < count; index++, charCount++, i++)
            {
                u = 0;
                if (decoder != null && decoder.pendingByte != 0)
                {
                    u = decoder.pendingByte;
                    decoder.pendingByte = 0;
                }

                u = (ushort)(u << 8 | bytes[index]);
                c = Map.GB2312ToUnicode(u);
                if (c == LEAD_BYTE_CHAR)
                {
                    if (i < count - 1)
                    {
                        index++;
                        i++;
                    }
                    else if (decoder != null)
                    {
                        decoder.pendingByte = bytes[index];
                        return charCount;
                    }
                }
            }

            return charCount;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return GetChars(bytes, byteIndex, byteCount, chars, charIndex, null);
        }

        private int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, GB2312Decoder decoder)
        {
            int charCount = 0;
            ushort u;
            char c;

            for (int i = 0; i < byteCount; byteIndex++, charIndex++, charCount++, i++)
            {
                u = 0;
                if (decoder != null && decoder.pendingByte != 0)
                {
                    u = decoder.pendingByte;
                    decoder.pendingByte = 0;
                }

                u = (ushort)(u << 8 | bytes[byteIndex]);
                c = Map.GB2312ToUnicode(u);
                if (c == LEAD_BYTE_CHAR)
                {
                    if (i < byteCount - 1)
                    {
                        byteIndex++;
                        i++;
                        u = (ushort)(u << 8 | bytes[byteIndex]);
                        c = Map.GB2312ToUnicode(u);
                    }
                    else if (decoder == null)
                    {
                        c = '\0';
                    }
                    else
                    {
                        decoder.pendingByte = bytes[byteIndex];
                        return charCount;
                    }
                }
                if (c == 0 && u != 0)
                    chars[charIndex] = '?';
                else
                    chars[charIndex] = c;
            }

            return charCount;
        }

        public override int GetMaxByteCount(int charCount)
        {
            if (charCount < 0)
                throw new ArgumentOutOfRangeException("charCount");
            long count = charCount + 1;
            count *= 2;
            if (count > int.MaxValue)
                throw new ArgumentOutOfRangeException("charCount");
            return (int)count;

        }

        public override int GetMaxCharCount(int byteCount)
        {
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException("byteCount");
            long count = byteCount + 3;
            if (count > int.MaxValue)
                throw new ArgumentOutOfRangeException("byteCount");
            return (int)count;
        }

        public override Decoder GetDecoder()
        {
            return new GB2312Decoder(this);
        }

        public override string WebName
        {
            get
            {
                return "gb2312";
            }
        }

        private static class Map
        {
            private static ushort[] _gb2312ToUnicode = null;
            private static ushort[] _unicodeToGb2312 = null;

            static Map()
            {
                _gb2312ToUnicode = new ushort[0x10000];
                _unicodeToGb2312 = new ushort[0x10000];

                /*
                 * According to many feedbacks, add this automatic function for find resource in revision 1.0.0.1.
                 * We suggest that use the old method as below if you understand how to embed the resource.
                 * Please make sure the gb2312.bin file was correctly embedded if throw an exception at here.
                 */
                //using (Stream stream = typeof(Map).Assembly.GetManifestResourceStream(typeof(Map).Namespace + ".gb2312.bin"))
                using (Stream stream = typeof(Map).Assembly.GetManifestResourceStream(typeof(Map).Assembly.GetManifestResourceNames().First(s => s.EndsWith(".gb2312.bin"))))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (int i = 0; i < 0xffff; i++)
                    {
                        ushort u = reader.ReadUInt16();
                        _unicodeToGb2312[i] = u;
                    }
                    for (int i = 0; i < 0xffff; i++)
                    {
                        ushort u = reader.ReadUInt16();
                        _gb2312ToUnicode[i] = u;
                    }
                }
            }

            public static char GB2312ToUnicode(ushort code)
            {
                return (char)_gb2312ToUnicode[code];
            }
            public static ushort UnicodeToGB2312(char ch)
            {
                return _unicodeToGb2312[ch];
            }
        }

        private sealed class GB2312Decoder : Decoder
        {
            private GB2312Encoding _encoding = null;

            public GB2312Decoder(GB2312Encoding encoding)
            {
                _encoding = encoding;
            }

            public override int GetCharCount(byte[] bytes, int index, int count)
            {
                return _encoding.GetCharCount(bytes, index, count, this);
            }

            public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
            {
                return _encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex, this);
            }

            public byte pendingByte;
        }
    }
}
