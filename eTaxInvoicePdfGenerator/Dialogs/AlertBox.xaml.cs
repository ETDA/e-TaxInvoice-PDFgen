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
    /// Interaction logic for AlertBox.xaml
    /// </summary>
    public partial class AlertBox : Window
    {
        public string msg { get; set; }
        public string title { get; set; }
        public string response { get; set; }
        public const string RESULT_OK = "OK";
        public AlertBox(string content)
        {
            InitializeComponent();
            msgLb.Text = content;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.response = RESULT_OK;
            this.Close();
        }
    }
}
