using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Entity;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using eTaxInvoicePdfGenerator.Dialogs;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for ItemConfig.xaml
    /// </summary>
    public partial class ItemConfig : Window
    {
        public ItemObj itemObj;
        public bool saveStatus;
        public ItemConfig(bool save)
        {
            this.saveStatus = save;
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (saveStatus)
            {
                new Item().Show();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setCodeList();
            if (this.itemObj != null)
            {
                showData();
            }
            else
            {

                is_item.IsChecked = true;
            }
            Keyboard.Focus(nameTb);
        }

        private void setCodeList()
        {
            try
            {
                List<CodeList> list = new CodeListDao().list();
                unitCbb.DisplayMemberPath = "description";
                unitCbb.SelectedValuePath = "code";
                unitCbb.ItemsSource = list;
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
                nameTb.Text = itemObj.name;
                detailTb.Text = itemObj.detail;
                priceTb.Text = itemObj.pricePerUnit.ToString();
                if (itemObj.isService)
                {
                    is_service.IsChecked = true;
                    unitCbb.IsEditable = false;
                }
                else
                {
                    is_item.IsChecked = true;
                    unitCbb.IsEditable = true;
                }
                unitCbb.Text = itemObj.unit;
                itemCodeTb.Text = itemObj.itemCode;
                itemCodeInterTb.Text = itemObj.itemCodeInter;
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private bool saveData()
        {
            try
            {
                validateData();
                if (itemObj == null)
                {
                    itemObj = new ItemObj();
                }
                itemObj.name = nameTb.Text;
                itemObj.detail = detailTb.Text;
                itemObj.itemCode = itemCodeTb.Text;
                itemObj.itemCodeInter = itemCodeInterTb.Text;
                itemObj.pricePerUnit = Convert.ToDouble(priceTb.Text);
                if (is_service.IsChecked.Value)
                {
                    itemObj.isService = true;
                    itemObj.unit = "";
                    itemObj.unitXml = "";
                }
                else
                {
                    itemObj.isService = false;
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
                        if (is_item.IsChecked.Value)
                        {
                            if (!new CodeListDao().exist(new CodeList(itemObj.unitXml, itemObj.unit)))
                            {
                                new CodeListDao().save(new CodeList(itemObj.unitXml, itemObj.unit));
                            }
                        }
                    }
                    else
                    {
                        itemObj.unit = codelist.description;
                        itemObj.unitXml = codelist.code;
                    }
                }
                if (saveStatus)
                {
                    new ItemDao().save(itemObj);
                }
                new AlertBox("บันทึกข้อมูลสินค้า/บริการเรียบร้อยแล้ว").ShowDialog();
                this.Close();
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
            validator.validateText(nameTb, "ชื่อสินค้า/บริการ", 256, true);
            validator.validatePrice(priceTb);
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
                return nameTb.Text != "" || detailTb.Text != "" || is_item.IsChecked != true || priceTb.Text != "" || unitCbb.Text != "" ||
                    itemCodeTb.Text != "" || itemCodeInterTb.Text != "";
            }
            else
            {
                ItemObj obj = itemObj;
                return nameTb.Text != obj.name || detailTb.Text != obj.detail || obj.isService != is_service.IsChecked ||
                    obj.pricePerUnit.ToString() != priceTb.Text || obj.unit != unitCbb.Text ||
                    itemCodeTb.Text != itemObj.itemCode || itemCodeInterTb.Text != itemObj.itemCodeInter;
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveData();
            }
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

        private void is_service_Checked(object sender, RoutedEventArgs e)
        {
            unitCbb.IsEditable = false;
            unitCbb.IsEnabled = false;
        }

        private void is_item_Checked(object sender, RoutedEventArgs e)
        {
            unitCbb.IsEditable = true;
            unitCbb.IsEnabled = true;
        }

        private void unitCbb_DropDownOpened(object sender, EventArgs e)
        {
            if (is_service.IsChecked.Value)
            {
                unitCbb.IsDropDownOpen = false;
            }
        }

        private void shutdownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isChange())
            {
                YesNo yn = new YesNo("ต้องการปิดโปรแกรมหรือไม่ โดยระบบจะไม่บันทึกข้อมูลไว้", "ยืนยันการออกจากการโปรแกรม");
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
            }
            Application.Current.Shutdown();
        }
    }
}
