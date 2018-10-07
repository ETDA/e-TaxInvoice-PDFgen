using System;
using System.Windows;
using System.Windows.Documents;
using System.Collections.Generic;
using eTaxInvoicePdfGenerator.Entity;
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.util;
using eTaxInvoicePdfGenerator.Dialogs;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for Buyer.xaml
    /// </summary>
    public partial class Buyer : Window
    {
        private int currentPage = 1;
        private int totalPage = 1;
        private int pageSize = 15;
        private string sortingProperty = "id";
        private string sortingDirection = "ASC";
        public Buyer()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            showList(1);
            prevBtn.IsEnabled = false;
        }

        private void showList(int page)
        {
            try
            {
                this.currentPage = page;
                int listSize = new BuyerDao().count();
                totalPage = (int)Math.Ceiling((double)listSize / (double)this.pageSize);
                if (totalPage == 0)
                {
                    this.currentPage = 0;
                }
                pageLb.Content = string.Format("หน้า {0} จาก {1}", this.currentPage, this.totalPage);
                checkPagination();

                // add item list
                listView.ItemsSource = new BuyerDao().listView(page, this.pageSize, this.sortingProperty, this.sortingDirection);
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void checkPagination()
        {
            if (currentPage == totalPage)
            {
                nextBtn.IsEnabled = false;
                if (totalPage == 1)
                {
                    prevBtn.IsEnabled = false;
                }
                else
                {
                    prevBtn.IsEnabled = true;
                }
            }
            else
            {
                nextBtn.IsEnabled = true;
                if (currentPage == 1)
                {
                    prevBtn.IsEnabled = false;
                }
                else
                {
                    prevBtn.IsEnabled = true;
                }
            }

            if (new BuyerDao().count() == 0)
            {
                delBtn.IsEnabled = false;
            }
            else
            {
                delBtn.IsEnabled = true;
            }
        }

        private void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink button = sender as Hyperlink;
            BuyerList buyer = button.DataContext as BuyerList;
            BuyerConfig config = new BuyerConfig();
            config.id = buyer.id;
            config.Show();
            this.Hide();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            BuyerConfig config = new BuyerConfig();
            config.Show();
            this.Hide();
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            var items = listView.ItemsSource;
            List<BuyerList> selectedItems = new List<BuyerList>();
            foreach (BuyerList item in items)
            {
                if (item.isSelected)
                {
                    selectedItems.Add(item);
                }
            }
            if (selectedItems.Count > 0)
            {
                DelNo dn = new DelNo();
                dn.ShowDialog();
                switch (dn.response)
                {
                    case DelNo.RESULT_YES:
                        try
                        {
                            foreach (BuyerList item in items)
                            {
                                if (item.isSelected)
                                {
                                    new BuyerDao().remove(item.id);
                                }
                            }
                            showList(1);
                        }
                        catch (Exception ex)
                        {
                            new AlertBox(ex.Message).ShowDialog();
                        }
                        break;
                    case DelNo.RESULT_NO:
                        break;
                    default:
                        return;
                }
            }
            else
            {
                new AlertBox("กรุณาเลือกรายการที่ต้องการลบ").ShowDialog();
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void prevBtn_Click(object sender, RoutedEventArgs e)
        {
            this.currentPage -= 1;
            this.showList(currentPage);
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            this.currentPage += 1;
            this.showList(currentPage);
        }

        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            if (column == null || column.Column.DisplayMemberBinding == null)
            {
                return;
            }


            if (_sortColumn == column)
            {
                // Toggle sorting direction 
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                   ListSortDirection.Descending :
                                                   ListSortDirection.Ascending;
            }
            else
            {
                // Remove arrow from previously sorted header 
                if (_sortColumn != null)
                {
                    _sortColumn.Column.HeaderTemplate = null;
                    //_sortColumn.Column.Width = _sortColumn.ActualWidth - 10;
                }

                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
                //column.Column.Width = column.ActualWidth + 10;
            }

            if (_sortDirection == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate = Resources["ArrowUp"] as DataTemplate;
                this.sortingDirection = "ASC";
            }
            else
            {
                column.Column.HeaderTemplate = Resources["ArrowDown"] as DataTemplate;
                this.sortingDirection = "DESC";
            }

            string header = string.Empty;

            // if binding is used and property name doesn't match header content 
            Binding b = _sortColumn.Column.DisplayMemberBinding as Binding;
            if (b != null)
            {
                header = b.Path.Path;
                if (header == "taxid_no")
                {
                    this.sortingProperty = "tax_id";
                }
                else
                {
                    this.sortingProperty = header;
                }
            }
            listView.ItemsSource = new BuyerDao().listView(this.currentPage, this.pageSize, this.sortingProperty, this.sortingDirection);
        }

        private void shutdownBtn_Click(object sender, RoutedEventArgs e)
        {
            YesNo yn = new YesNo("ต้องการปิดโปรแกรมหรือไม่", "ยืนยันการออกจากการโปรแกรม");
            yn.ShowDialog();
            switch (yn.response)
            {
                case YesNo.RESULT_YES:
                    break;
                case YesNo.RESULT_NO:
                    return;
                default:
                    return;
            }
            Application.Current.Shutdown();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
