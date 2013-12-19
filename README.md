# Encoding for Silverlight

_This project is moved from http://encoding4silverlight.codeplex.com/_

**This project is to support all .Net supported encodings for Silverlight & WP7 Application. Now, support DBCS (Double-Byte Character Set) encodings only. This contains GB2312, BIG5, shift_jis, Johab and so on.**

This project is fully implemented all Encoding functions therefore you can use this project as easy as .Net Framework built-in encoding. In order to use the project, you must embed an addition binary data files into your assembly. Following features have been implemented:

- Immediate decode array bytes and encode string or array chars
- Use `StreamReader` and `StreamWriter` to read and write stream simple


## Sample
```csharp
WebClient _wc = new WebClient();
_wc.Encoding = DBCSCodePage.DBCSEncoding.GetDBCSEncoding("gb2312");
_wc.DownloadStringCompleted += (ss, ee) => txtResult.Text = ee.Result;
_wc.DownloadStringAsync(new Uri(txtUrl.Text));
```


## How to Use
- Copy `DBCSEncoding.cs` into your project
- Copy binary data files into your project as you need, Change the **Build Action** as **Embedded Resource** for binary data files
- Download the example and binary data files from [Release](http://github.com/Aimeast/Encoding4Silverlight/releases) page
- **Note:** You can fewer embed binary data files only you want to support. Use `DBCSEncoding.GetDBCSEncoding(name)` to get an instance, supported name are listed below
- **Remember:** No alias name support


## Supported Character Set

| Code Page | Name | Display Name |
| :------ | :------: | :------: |
| 932 | shift_jis | Japanese (Shift-JIS) |
| 936 | gb2312 | Chinese Simplified (GB2312) |
| 949 | ks_c_5601-1987 | Korean |
| 950 | big5 | Chinese Traditional (Big5) |
| 1361 | Johab | Korean (Johab) |
| 10001 | x-mac-japanese | Japanese (Mac) |
| 10002 | x-mac-chinesetrad | Chinese Traditional (Mac) |
| 10003 | x-mac-korean | Korean (Mac) |
| 10008 | x-mac-chinesesimp | Chinese Simplified (Mac) |
| 20000 | x-Chinese-CNS | Chinese Traditional (CNS) |
| 20001 | x-cp20001 | TCA Taiwan |
| 20002 | x-Chinese-Eten | Chinese Traditional (Eten) |
| 20003 | x-cp20003 | IBM5550 Taiwan |
| 20004 | x-cp20004 | TeleText Taiwan |
| 20005 | x-cp20005 | Wang Taiwan |
| 20261 | x-cp20261 | T.61 |
| 20932 | EUC-JP | Japanese (JIS 0208-1990 and 0212-1990) |
| 20936 | x-cp20936 | Chinese Simplified (GB2312-80) |
| 20949 | x-cp20949 | Korean Wansung |
| 50227 | x-cp50227 | Chinese Simplified (ISO-2022) |
| 51936 | EUC-CN | Chinese Simplified (EUC) |
| 51949 | euc-kr | Korean (EUC) |


## Contributing
You are welcome to share your point of view on [Issues](http://github.com/Aimeast/Encoding4Silverlight/issues) page or send a [Pull Request](http://github.com/Aimeast/Encoding4Silverlight/pulls) to enjoy your achievement.


## License
The MIT license
