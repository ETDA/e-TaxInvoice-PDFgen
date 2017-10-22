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
using System.Data;
using System.IO;

//Hello tHIS IS TAG 
namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for ETaxInvoice.xaml
    /// </summary>
    public partial class ETaxInvoice : Window
    {
        private bool _isReportViewerLoaded;
        string db_path = "";
        public ETaxInvoice()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog(); //{ Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif" };
            var result = ofd.ShowDialog();
            textBox.Text = ofd.FileName;
            db_path = ofd.FileName;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //using (StreamReader sr = new StreamReader("..\\..\\Resources\\template.xml"))
            //{
            //    string line = sr.ReadToEnd();
            //    line.IndexOf()
            //}

            using (Report.InvoiceGenerator invoicegen = new Report.InvoiceGenerator())
            {
                /*get inv field for test*/
                invoicegen.create("ETDA000007");
                var a = invoicegen.getStringXml();
                var b = invoicegen.getBytePdf();
                var c = invoicegen.getByteXml();
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {

        }
    }

}
