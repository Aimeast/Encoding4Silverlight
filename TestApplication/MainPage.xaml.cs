using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TestApplication
{
    public partial class MainPage : UserControl
    {
        WebClient _wc = new WebClient();

        public MainPage()
        {
            InitializeComponent();
            _wc.Encoding = new GB2312.GB2312Encoding();
            _wc.DownloadStringCompleted += (ss, ee) =>
            {
                try
                {
                    if (!ee.Cancelled)
                        txtResult.Text = ee.Result;
                }
                catch { }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _wc.CancelAsync();
                _wc.DownloadStringAsync(new Uri(txtUrl.Text));
            }
            catch { }
        }
    }
}
