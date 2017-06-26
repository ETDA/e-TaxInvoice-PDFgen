using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using eTaxInvoicePdfGenerator.Dao;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for TermNConditions.xaml
    /// </summary>
    public partial class TermNConditions : Window
    {
        public TermNConditions()
        {
            InitializeComponent();
        }

        private void MoveNext()
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigObj config = new ConfigDao().select("TnCAcceptant");
            config.value = "1";
            new ConfigDao().save(config);
            MoveNext();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigObj config = new ConfigDao().select("TnCAcceptant");
            bool status = Convert.ToBoolean(Convert.ToInt16(config.value));
            if (status)
            {
                MoveNext();
            }
        }

        public static bool IsScrolledToEnd(TextBox textBox)
        {
            return textBox.VerticalOffset + textBox.ViewportHeight == textBox.ExtentHeight;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            nextBtn.IsEnabled = true;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            nextBtn.IsEnabled = false;
        }

        private void contentTb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsScrolledToEnd(contentTb))
            {
                acceptChkbox.IsEnabled = true;
            }else
            {
                acceptChkbox.IsEnabled = false;
            }
        }

        private void contentTb_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsScrolledToEnd(contentTb))
            {
                acceptChkbox.IsEnabled = true;
            }else
            {
                acceptChkbox.IsEnabled = false;
            }
        }
    }
}
