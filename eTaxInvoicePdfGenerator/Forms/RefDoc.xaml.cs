using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using eTaxInvoicePdfGenerator.Entity;
using System.Collections.ObjectModel;
using eTaxInvoicePdfGenerator.Dialogs;
using System.Windows.Controls;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for RefDoc.xaml
    /// </summary>
    public partial class RefDoc : Window
    {
        public string invoiceId;
        public List<ReferenceObj> refList { get; set; }
        private Collection<TypeCodeObj> typeCodes = new Collection<TypeCodeObj>() {
            new TypeCodeObj("", "ใบกำกับภาษี"),
            new TypeCodeObj("T03","ใบเสร็จรับเงิน/ใบกำกับภาษี"),
            new TypeCodeObj("81", "ใบลดหนี้"),
            new TypeCodeObj("80", "ใบเพิ่มหนี้")            
        };
        public Collection<TypeCodeObj> TypeCodes { get { return typeCodes; } }
        public RefDoc()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                showData();
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void showData()
        {
            // set 1st doc
            resetData();
            if (refList.Count > 0)
            {
                documentId1.Text = refList[0].documentId;
                documentDate1.Text = refList[0].documentDate;
                TypeCodeObj typeCode = typeCodes.FirstOrDefault(s => s.code == refList[0].typeCodeObj.description);
                if (typeCode == null)
                {
                    typeCode1.Text = refList[0].typeCodeObj.description;
                }
                else
                {
                    typeCode1.SelectedItem = typeCode;
                }
                delCb1.IsEnabled = true;
            }

            // set 2nd doc
            if (refList.Count > 1)
            {
                documentId2.Text = refList[1].documentId;
                documentDate2.Text = refList[1].documentDate;
                TypeCodeObj typeCode = typeCodes.FirstOrDefault(s => s.code == refList[1].typeCodeObj.description);
                if (typeCode == null)
                {
                    typeCode2.Text = refList[1].typeCodeObj.description;
                }
                else
                {
                    typeCode2.SelectedItem = typeCode;
                }
                delCb2.IsEnabled = true;
            }

            // set 3rd doc
            if (refList.Count > 2)
            {
                documentId3.Text = refList[2].documentId;
                documentDate3.Text = refList[2].documentDate;
                TypeCodeObj typeCode = typeCodes.FirstOrDefault(s => s.code == refList[2].typeCodeObj.description);
                if (typeCode == null)
                {
                    typeCode3.Text = refList[2].typeCodeObj.description;
                }
                else
                {
                    typeCode3.SelectedItem = typeCode;
                }
                delCb3.IsEnabled = true;
            }

            // set 4th doc
            if (refList.Count > 3)
            {
                documentId4.Text = refList[3].documentId;
                documentDate4.Text = refList[3].documentDate;
                TypeCodeObj typeCode = typeCodes.FirstOrDefault(s => s.code == refList[3].typeCodeObj.description);
                if (typeCode == null)
                {
                    typeCode4.Text = refList[3].typeCodeObj.description;
                }
                else
                {
                    typeCode4.SelectedItem = typeCode;
                }
                delCb4.IsEnabled = true;
            }

            // set 5th doc
            if (refList.Count > 4)
            {
                documentId5.Text = refList[4].documentId;
                documentDate5.Text = refList[4].documentDate;
                TypeCodeObj typeCode = typeCodes.FirstOrDefault(s => s.code == refList[4].typeCodeObj.description);
                if (typeCode == null)
                {
                    typeCode5.Text = refList[4].typeCodeObj.description;
                }
                else
                {
                    typeCode5.SelectedItem = typeCode;
                }
                delCb5.IsEnabled = true;
            }

        }

        private void resetData()
        {
            documentId1.Text = "";
            documentDate1.Text = "";
            //typeCode1.SelectedItem = null;
            typeCode1.Text = TypeCodes[0].description;
            delCb1.IsChecked = false;
            delCb1.IsEnabled = false;

            documentId2.Text = "";
            documentDate2.Text = "";
            typeCode2.SelectedItem = null;
            typeCode2.Text = "";
            delCb2.IsChecked = false;
            delCb2.IsEnabled = false;

            documentId3.Text = "";
            documentDate3.Text = "";
            typeCode3.SelectedItem = null;
            typeCode3.Text = "";
            delCb3.IsChecked = false;
            delCb3.IsEnabled = false;

            documentId4.Text = "";
            documentDate4.Text = "";
            typeCode4.SelectedItem = null;
            typeCode4.Text = "";
            delCb4.IsChecked = false;
            delCb4.IsEnabled = false;

            documentId5.Text = "";
            documentDate5.Text = "";
            typeCode5.SelectedItem = null;
            typeCode5.Text = "";
            delCb5.IsChecked = false;
            delCb5.IsEnabled = false;
        }

        private bool saveData()
        {
            try
            {
                validateData();
                refList = new List<ReferenceObj>();
                int number = 1;
                if (documentId1.Text.Length > 0)
                {
                    refList.Add(new ReferenceObj(number++, this.invoiceId, documentId1.Text, documentDate1.Text, typeCode1.Text, (TypeCodeObj)typeCode1.SelectedItem));
                }

                if (documentId2.Text.Length > 0)
                {
                    refList.Add(new ReferenceObj(number++, this.invoiceId, documentId2.Text, documentDate2.Text, typeCode2.Text, (TypeCodeObj)typeCode2.SelectedItem));
                }

                if (documentId3.Text.Length > 0)
                {
                    refList.Add(new ReferenceObj(number++, this.invoiceId, documentId3.Text, documentDate3.Text, typeCode3.Text, (TypeCodeObj)typeCode3.SelectedItem));
                }

                if (documentId4.Text.Length > 0)
                {
                    refList.Add(new ReferenceObj(number++, this.invoiceId, documentId4.Text, documentDate4.Text, typeCode4.Text, (TypeCodeObj)typeCode4.SelectedItem));
                }

                if (documentId5.Text.Length > 0)
                {
                    refList.Add(new ReferenceObj(number++, this.invoiceId, documentId5.Text, documentDate5.Text, typeCode5.Text, (TypeCodeObj)typeCode5.SelectedItem));
                }
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
            validateRef(documentId1, documentDate1, typeCode1);
            validateRef(documentId2, documentDate2, typeCode2);
            validateRef(documentId3, documentDate3, typeCode3);
            validateRef(documentId4, documentDate4, typeCode4);
            validateRef(documentId5, documentDate5, typeCode5);
        }

        private void validateRef(TextBox id,DatePicker date,ComboBox typeCode)
        {
            if (id.Text.Length > 0)
            {
                util.Validator validator = new util.Validator();
                validator.validateText(id,"เลขที่ของใบกำกับภาษีเดิม",35,false);

                validator.validateTypeCode(typeCode);

                validator.validateDocDate(date,"เอกสารอ้างถึง");
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (saveData())
            {
                this.DialogResult = true;
                this.Close();
            }
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
                        DialogResult = true;
                        break;
                    case YesNoCancel.RESULT_NO:
                        DialogResult = false;
                        break;
                    case YesNoCancel.RESULT_CANCEL:
                        return;
                    default:
                        return;
                }
            }
            this.Close();
        }

        private bool isChange()
        {
            ReferenceObj ref1 = refList.FirstOrDefault(s => s.number == 1);
            if (documentId1.Text.Length > 0 != (ref1 != null))
            {
                return true;
            }
            else
            {
                if (ref1 != null)
                {
                    if (!ref1.Equals(new ReferenceObj(1, this.invoiceId, documentId1.Text, documentDate1.Text, typeCode1.Text, (TypeCodeObj)typeCode1.SelectedItem)))
                    {
                        return true;
                    }
                }
            }

            ReferenceObj ref2 = refList.FirstOrDefault(s => s.number == 2);
            if (documentId2.Text.Length > 0 != (ref2 != null))
            {
                return true;
            }
            else
            {
                if (ref2 != null)
                {
                    if (!ref2.Equals(new ReferenceObj(2, this.invoiceId, documentId2.Text, documentDate2.Text, typeCode2.Text, (TypeCodeObj)typeCode2.SelectedItem)))
                    {
                        return true;
                    }
                }
            }

            ReferenceObj ref3 = refList.FirstOrDefault(s => s.number == 3);
            if (documentId3.Text.Length > 0 != (ref3 != null))
            {
                return true;
            }
            else
            {
                if (ref3 != null)
                {
                    if (!ref3.Equals(new ReferenceObj(3, this.invoiceId, documentId3.Text, documentDate3.Text, typeCode3.Text, (TypeCodeObj)typeCode3.SelectedItem)))
                    {
                        return true;
                    }
                }
            }

            ReferenceObj ref4 = refList.FirstOrDefault(s => s.number == 4);
            if (documentId4.Text.Length > 0 != (ref4 != null))
            {
                return true;
            }
            else
            {
                if (ref4 != null)
                {
                    if (!ref4.Equals(new ReferenceObj(4, this.invoiceId, documentId4.Text, documentDate4.Text, typeCode4.Text, (TypeCodeObj)typeCode4.SelectedItem)))
                    {
                        return true;
                    }
                }
            }

            ReferenceObj ref5 = refList.FirstOrDefault(s => s.number == 5);
            if (documentId5.Text.Length > 0 != (ref5 != null))
            {
                return true;
            }
            else
            {
                if (ref5 != null)
                {
                    if (!ref5.Equals(new ReferenceObj(5, this.invoiceId, documentId5.Text, documentDate5.Text, typeCode5.Text, (TypeCodeObj)typeCode5.SelectedItem)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveData();
            }
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            DelNo dn = new DelNo("ต้องการลบข้อมูลหรือไม่", "ยืนยันการลบรายการ");
            dn.ShowDialog();
            switch (dn.response)
            {
                case DelNo.RESULT_YES:
                    try
                    {
                        if (delCb5.IsChecked.Value)
                        {
                            refList.Remove(refList.FirstOrDefault(s => s.number == 5));
                        }

                        if (delCb4.IsChecked.Value)
                        {
                            refList.Remove(refList.FirstOrDefault(s => s.number == 4));
                        }

                        if (delCb3.IsChecked.Value)
                        {
                            refList.Remove(refList.FirstOrDefault(s => s.number == 3));
                        }

                        if (delCb2.IsChecked.Value)
                        {
                            refList.Remove(refList.FirstOrDefault(s => s.number == 2));
                        }

                        if (delCb1.IsChecked.Value)
                        {
                            refList.Remove(refList.FirstOrDefault(s => s.number == 1));
                        }
                        resetIndex();
                        showData();
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

        private void resetIndex()
        {
            int i = 1;
            foreach (ReferenceObj refObj in refList)
            {
                refObj.number = i++;
            }
        }

        private void typeCode1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
