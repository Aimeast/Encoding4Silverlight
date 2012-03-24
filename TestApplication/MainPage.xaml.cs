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
using System.IO;

namespace TestApplication
{
    public partial class MainPage : UserControl
    {
        WebClient _wc = new WebClient();

        public MainPage()
        {
            InitializeComponent();

            _wc.OpenReadCompleted += (ss, ee) =>
            {
                try
                {
                    if (!ee.Cancelled)
                        using (StreamReader reader = new StreamReader(ee.Result, new GB2312.GB2312Encoding()))
                            txtResult.Text = reader.ReadToEnd();
                }
                catch { }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _wc.CancelAsync();
                _wc.OpenReadAsync(new Uri(txtUrl.Text));
            }
            catch { }
        }
    }
}
