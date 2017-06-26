using System;
using System.Windows;
using eTaxInvoicePdfGenerator.Forms;
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Dialogs;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace eTaxInvoicePdfGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Process proc = Process.GetCurrentProcess();
            //int count = Process.GetProcesses().Where(p =>
            //                 p.ProcessName == proc.ProcessName).Count();
            //if (count == 0)
            //{
            InitializeComponent();
            //}

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void sellerConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            Seller seller = new Seller();
            seller.Show();
            this.Hide();
        }

        private void buyerConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            Buyer buyer = new Buyer();
            buyer.Show();
            this.Hide();
        }

        private void itemConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            Item item = new Item();
            item.Show();
            this.Hide();
        }

        private void cre8TaxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsSellerExist())
            {
                Invoice invoice = new Invoice();
                invoice.Show();
                this.Hide();
            }
            else
            {
                new AlertBox("กรุณาบันทึกข้อมูล ในเมนู ตั้งค่าผู้ขาย ก่อนดำเนินการสร้างเอกสาร").ShowDialog();
            }
        }

        private void cre8DebitNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsSellerExist())
            {
                DebitNote debit = new DebitNote();
                debit.Show();
                this.Hide();
            }
            else
            {
                new AlertBox("กรุณาบันทึกข้อมูล ในเมนู ตั้งค่าผู้ขาย ก่อนดำเนินการสร้างเอกสาร").ShowDialog();
            }
        }

        private void cre8CreditNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsSellerExist())
            {
                CreditNote credit = new CreditNote();
                credit.Show();
                this.Hide();
            }
            else
            {
                new AlertBox("กรุณาบันทึกข้อมูล ในเมนู ตั้งค่าผู้ขาย ก่อนดำเนินการสร้างเอกสาร").ShowDialog();
                //MessageBox.Show("กรุณาบันทึกข้อมูล ในหน้า ตั้งค่า-ผู้ขายก่อน");
            }
        }

        private bool IsSellerExist()
        {
            if (new SellerDao().count() > 0)
            {
                return true;
            }
            return false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
