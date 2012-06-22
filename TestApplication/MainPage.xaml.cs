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
        string[] _supported = new[] { "GB2312", "BIG5" };

        WebClient _wc = new WebClient();
        string _baseUrl;

        public MainPage()
        {
            InitializeComponent();

            _baseUrl = txtUrl.Text;
            comboEncoding.ItemsSource = _supported;
            comboEncoding.SelectedIndex = 0;

            _wc.OpenReadCompleted += (ss, ee) =>
            {
                try
                {
                    if (!ee.Cancelled)
                        using (StreamReader reader = new StreamReader(ee.Result, DBCSCodePage.DBCSEncoding.GetDBCSEncoding(_supported[comboEncoding.SelectedIndex]), false))
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

        private void comboEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtUrl.Text = _baseUrl + _supported[comboEncoding.SelectedIndex].ToLower() + ".txt";
        }
    }
}
