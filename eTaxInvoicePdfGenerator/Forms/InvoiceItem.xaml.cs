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
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Entity;
using System.Text.RegularExpressions;
using eTaxInvoicePdfGenerator.Dialogs;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for InvoiceItem.xaml
    /// </summary>
    public partial class InvoiceItem : Window
    {
        public InvoiceItemObj itemObj;
        public InvoiceItem()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();
            if (itemObj != null)
            {
                showData();
            }
            else
            {
                is_item.IsChecked = true;
            }
        }

        private void init()
        {
            try
            {
                List<ItemObj> itemList = new ItemDao().list();
                itemNameCbb.DisplayMemberPath = "name";
                itemNameCbb.SelectedValue = "id";
                itemNameCbb.ItemsSource = itemList;

                List<CodeList> codeList = new CodeListDao().list();
                unitCbb.DisplayMemberPath = "description";
                unitCbb.SelectedValuePath = "code";
                unitCbb.ItemsSource = codeList;

            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void showData()
        {
            try
            {
                itemNameCbb.Text = itemObj.itemName;
                quantityTb.Text = itemObj.quantity.ToString();
                pricePerUnitTb.Text = itemObj.pricePerUnit.ToString();
                discountTb.Text = itemObj.discount.ToString();
                discountTotalTb.Text = itemObj.discountTotal.ToString();
                if (itemObj.is_service)
                {
                    is_service.IsChecked = true;
                    unitCbb.IsEnabled = false;
                }
                else
                {
                    is_item.IsChecked = true;
                    unitCbb.IsEnabled = true;
                }
                unitCbb.Text = itemObj.unit;
                itemCodeTb.Text = itemObj.itemCode;
                itemCodeInterTb.Text = itemObj.itemCodeInter;
                calculateSum();
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void itemNameCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (itemNameCbb.SelectedIndex > -1)
            {
                setItem((ItemObj)itemNameCbb.SelectedItem);
                calculateSum();
            }
        }

        private void setItem(ItemObj selectedItem)
        {
            pricePerUnitTb.Text = selectedItem.pricePerUnit.ToString();
            unitCbb.Text = selectedItem.unit;
            itemCodeTb.Text = selectedItem.itemCode;
            itemCodeInterTb.Text = selectedItem.itemCodeInter;
            if (selectedItem.isService)
            {
                is_service.IsChecked = true;
            }else
            {
                is_item.IsChecked = true;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            saveData();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isChange())
            {
                YesNoCancel ync = new YesNoCancel("ต้องการบันทึกข้อมูลหรือไม่", "ยืนยันการออกจากการทำงาน");
                ync.ShowDialog();
                switch (ync.response)
                {
                    case YesNoCancel.RESULT_YES:
                        if (!saveData())
                        {
                            return;
                        }
                        break;
                    case YesNoCancel.RESULT_NO:
                        break;
                    case YesNoCancel.RESULT_CANCEL:
                        return;
                    default:
                        return;
                }
            }
            this.Close();
        }

        private bool saveData()
        {
            try
            {
                validateData();
                if (itemObj == null)
                {
                    itemObj = new InvoiceItemObj();
                }
                itemObj.itemName = itemNameCbb.Text;
                itemObj.discount = Convert.ToDouble(discountTotalTb.Text);
                itemObj.itemCode = itemCodeTb.Text;
                itemObj.itemCodeInter = itemCodeInterTb.Text;
                itemObj.pricePerUnit = Convert.ToDouble(pricePerUnitTb.Text);
                itemObj.pricePerUnitText = itemObj.pricePerUnit.ToString();
                itemObj.discount = Convert.ToDouble(discountTb.Text);
                itemObj.quantity = Convert.ToInt32(quantityTb.Text);
                itemObj.quantityText = itemObj.quantity.ToString();
                itemObj.discountTotal = Convert.ToDouble(discountTotalTb.Text);
                itemObj.itemTotal = Convert.ToDouble(itemTotalTb.Text);
                itemObj.itemTotalText = itemObj.itemTotal.ToString().Replace(",","");
                if (itemObj.discount == 0.0)
                {
                    itemObj.discountText = itemObj.discountTotal.ToString() + " บาท";
                }
                else
                {
                    itemObj.discountText = itemObj.discount + " % " + itemObj.discountTotal.ToString() + " บาท";

                }
                if (is_service.IsChecked.Value)
                {
                    itemObj.is_service = true;
                    itemObj.unit = "";
                    itemObj.unitXml = "";
                }
                else
                {
                    itemObj.is_service = false;
                    CodeList codelist = (CodeList)unitCbb.SelectedItem;
                    if (codelist == null)
                    {
                        if (unitCbb.Text == "")
                        {
                            itemObj.unitXml = "";
                        }
                        else
                        {
                            itemObj.unitXml = "ZZ";
                        }
                        itemObj.unit = unitCbb.Text;
                    }
                    else
                    {
                        itemObj.unit = codelist.description;
                        itemObj.unitXml = codelist.code;
                    }
                }
                this.DialogResult = true;
                return true;
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
                return false;
            }

        }

        private void validateData()
        {
            util.Validator validator = new util.Validator();
            
            validator.validateNameCbb(itemNameCbb, "ชื่อสินค้า/บริการ", 256, true);
            validator.validateDoubleRate(discountTb, "ส่วนลดต่อรายการ", 99.99);
            int quantity = validator.validateQuantity(quantityTb, 5);
            double pricePerUnit = validator.validatePrice(pricePerUnitTb);
            double itemTotal = pricePerUnit * quantity;
            validator.validateDiscount(discountTb, itemTotal);
            if (is_item.IsChecked.Value)
            {
                validator.validateUnit(unitCbb);
            }
            validator.validateItemCode(itemCodeTb);
            validator.validateItemCodeInter(itemCodeInterTb);
        }

        private bool isChange()
        {
            if (itemObj == null)
            {
                return itemNameCbb.Text != "" || is_item.IsChecked.Value != true || quantityTb.Text != "1" ||
                    discountTb.Text != "0.00" || pricePerUnitTb.Text != "" || unitCbb.Text != "" ||
                    itemCodeTb.Text != "" || itemCodeInterTb.Text != "";
            }
            else
            {
                return itemNameCbb.Text != itemObj.itemName || is_service.IsChecked.Value != itemObj.is_service ||
                    quantityTb.Text != itemObj.quantity.ToString() || discountTb.Text != itemObj.discount.ToString() ||
                    pricePerUnitTb.Text != itemObj.pricePerUnit.ToString() || !unitCbb.Text.Equals(itemObj.unit) ||
                    itemCodeTb.Text != itemObj.itemCode || itemCodeInterTb.Text != itemObj.itemCodeInter;
            }
        }

        private void calculateSum()
        {
            double discount = 0.0;
            bool parseDiscount = double.TryParse(discountTb.Text, out discount);
            double pricePerUnit = 0.0;
            bool parsePrice = double.TryParse(pricePerUnitTb.Text, out pricePerUnit);
            double quantity = 0;
            bool parseQuantity = double.TryParse(quantityTb.Text, out quantity);
            if (parseDiscount && parsePrice && parseQuantity)
            {
                double discountTotal = (quantity * discount / 100 * pricePerUnit);
                if (discount > 0.0)
                {
                    discountTotalTb.Text = discountTotal.ToString("N");
                    itemTotalTb.Text = ((pricePerUnit * quantity) - discountTotal).ToString("N");
                }
                else
                {
                    if (double.TryParse(discountTotalTb.Text, out discountTotal))
                    {
                        itemTotalTb.Text = ((pricePerUnit * quantity) - discountTotal).ToString("N");
                    }
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveData();
            }
        }

        private void discountTb_KeyUp(object sender, KeyEventArgs e)
        {
            calculateSum();
        }

        private void quantityTb_KeyUp(object sender, KeyEventArgs e)
        {
            calculateSum();
        }

        private void pricePerUnitTb_KeyUp(object sender, KeyEventArgs e)
        {
            calculateSum();
        }

        private void unitCbb_Loaded(object sender, RoutedEventArgs e)
        {
            var obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 20;
                }
            }
        }

        private void discountTotalTb_KeyUp(object sender, KeyEventArgs e)
        {
            double discount = 0.0;
            if (double.TryParse(discountTb.Text, out discount))
            {
                discountTb.Text = "0.00";
                calculateSum();
            }
        }

        private void discountTotalTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (discountTotalTb.Text == "0.00")
            {
                discountTotalTb.Text = string.Empty;
            }
        }

        private void discountTotalTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (discountTotalTb.Text == string.Empty)
            {
                discountTotalTb.Text = "0.00";
            }
        }

        private void discountTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (discountTb.Text == "0.00")
            {
                discountTb.Text = string.Empty;
            }
        }

        private void discountTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (discountTb.Text == string.Empty)
            {
                discountTb.Text = "0.00";
            }
        }

        private void is_item_Checked(object sender, RoutedEventArgs e)
        {
            unitCbb.IsEditable = true;
            unitCbb.IsEnabled = true;
        }

        private void is_service_Checked(object sender, RoutedEventArgs e)
        {
            unitCbb.IsEditable = false;
            unitCbb.IsEnabled = false;
        }

        private void unitCbb_DropDownOpened(object sender, EventArgs e)
        {
            if (is_service.IsChecked.Value)
            {
                unitCbb.IsDropDownOpen = false;
            }
        }
    }
}
