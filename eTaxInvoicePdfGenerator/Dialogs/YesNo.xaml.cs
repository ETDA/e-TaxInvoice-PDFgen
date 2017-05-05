using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eTaxInvoicePdfGenerator.Dialogs
{
    /// <summary>
    /// Interaction logic for YesNo.xaml
    /// </summary>
    public partial class YesNo : Window
    {
        public string msg { get; set; }
        public string title { get; set; }
        public string response { get; set; }
        public const string RESULT_YES = "YES";
        public const string RESULT_NO = "NO";
        public YesNo(string msg,string title)
        {
            this.msg = msg;
            this.title = title;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                msgLb.Text = this.msg;
                this.Title = this.title;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.response = RESULT_YES;
            this.Close();
        }

        private void noBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.response = RESULT_NO;
            this.Close();
        }
    }
}
