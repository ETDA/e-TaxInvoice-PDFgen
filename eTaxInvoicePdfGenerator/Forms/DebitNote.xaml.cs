using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using eTaxInvoicePdfGenerator.Entity;
using eTaxInvoicePdfGenerator.Dao;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ECertificateAPI;
using eTaxInvoicePdfGenerator.Dialogs;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for DebitNote.xaml
    /// </summary>
    public partial class DebitNote : Window
    {
        // ใบเพิ่มหนี้
        private const string PREFIX = "DBN";
        private const string REF_TYPE = "388";
        private const string REF_NAME = "ใบกำกับภาษี";
        private const string INVOICE_NAME = "ใบเพิ่มหนี้";
        private string invoiceID;
        private List<ReferenceObj> refList;
        private SellerObj seller;
        public DebitNote()
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
            Keyboard.Focus(taxIdTb);
            is_main.IsChecked = true;
            init();
        }

        private void init()
        {
            try
            {
                refList = new List<ReferenceObj>();
                List<BuyerObj> buyerList = new BuyerDao().list();
                nameCbb.DisplayMemberPath = "name";
                nameCbb.ItemsSource = buyerList;
                seller = new SellerDao().select();
                vatTb.Text = seller.vat.ToString("N");
                this.invoiceID = PREFIX + seller.dbn_no;

                List<CauseCodeListObj> list = new CauseCodeListDao().list("ใบเพิ่มหนี้");
                purposeCbb.DisplayMemberPath = "description";
                purposeCbb.SelectedValuePath = "code";
                purposeCbb.ItemsSource = list;
                purposeCbb.SelectedIndex = 0;
                otherPurposeTb.IsEnabled = false;
                setProvinceList();
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void setRefDocData()
        {
            if (refList.Count > 0)
            {
                docIdTb.Text = refList[0].documentId;
                docDateTb.Text = refList[0].documentDate;
            }
            else
            {
                docIdTb.Text = "";
                docDateTb.Text = "";
            }
        }

        private void setRef1()
        {
            if (docIdTb.Text != "")
            {
                ReferenceObj ref1 = refList.FirstOrDefault(s => s.number == 1);
                if (ref1 == null)
                {
                    refList.Add(new ReferenceObj(1));
                }
                ref1 = refList.FirstOrDefault(s => s.number == 1);
                ref1.documentId = docIdTb.Text;
                ref1.documentDate = docDateTb.Text;
                ref1.typeCode = REF_TYPE;
                ref1.invoiceId = this.invoiceID;
                ref1.typeCodeObj = new TypeCodeObj(REF_TYPE, REF_NAME);
            }
        }


        private void showData(BuyerObj obj)
        {
            try
            {
                nameCbb.SelectedItem = obj;
                address1Tb.Text = obj.address1;
                houseNoTb.Text = obj.houseNo;
                zipcodeTb.Text = obj.zipCode;
                taxIdTb.Text = obj.taxId;
                if (obj.isBranch)
                {
                    is_branch.IsChecked = true;
                    branchNoTb.Text = obj.branchId;
                }
                else
                {
                    is_main.IsChecked = true;
                }
                emailTb.Text = obj.email;
                contactTb.Text = obj.contactPerson;
                phoneExtTb.Text = ""; phoneNoTb.Text = "";
                phoneNoTb.Text = obj.phoneNo;
                phoneExtTb.Text = obj.phoneExt;
                if (obj.provinceCode != null && obj.provinceCode != "")
                {
                    provinceCbb.SelectedValue = obj.provinceCode + "000000";
                    if (obj.districtCode != null && obj.districtCode != "")
                    {
                        districtCbb.SelectedValue = obj.districtCode + "0000";
                        if (obj.subdistrcitCode != null && obj.subdistrcitCode != "")
                        {
                            subDistrictCbb.SelectedValue = obj.subdistrcitCode + "00";
                        }
                    }
                }
                nameCbb.Focus();
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void setProvinceList()
        {
            provinceCbb.SelectionChanged -= new SelectionChangedEventHandler(provinceCbb_SelectionChanged);
            util.ProvinceCodeList pcl = new util.ProvinceCodeList();
            pcl.SetProvince(provinceCbb);
            provinceCbb.SelectionChanged += new SelectionChangedEventHandler(provinceCbb_SelectionChanged);
        }

        private bool saveData()
        {
            try
            {
                setRef1();
                validateData();
                bool isNew = false;

                InvoiceObj obj = new InvoiceDao().find(this.invoiceID);
                if (obj == null)
                {
                    obj = new InvoiceObj();
                    obj.invoiceId = this.invoiceID;
                    isNew = true;
                }
                obj.invoiceName = INVOICE_NAME;
                if (purposeCbb.SelectedValue.ToString() == "DBNG99" || purposeCbb.SelectedValue.ToString() == "DBNS99")
                {
                    obj.purpose = otherPurposeTb.Text;
                }
                else
                {
                    obj.purpose = purposeCbb.Text;
                }
                obj.purposeCode = purposeCbb.SelectedValue.ToString();
                obj.taxCode = "VAT";
                obj.taxRate = Convert.ToDouble(vatTb.Text);
                obj.lineTotal = Convert.ToDouble(lineTotalTb.Text);
                obj.discount = 0.0;
                obj.taxTotal = Convert.ToDouble(taxTotalTb.Text);
                obj.grandTotal = Convert.ToDouble(grandTotalTb.Text);
                obj.remark = remarkTb.Text;

                obj.difference = Convert.ToDouble(diffValueTb.Text);
                obj.basisAmount = obj.difference;
                obj.original = Convert.ToDouble(originalValueTotal.Text);
                obj.issueDate = DateTime.Now.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));

                BuyerObj buyer = (BuyerObj)nameCbb.SelectedItem;
                if (buyer == null)
                {
                    buyer = new BuyerObj();
                }
                buyer.name = nameCbb.Text;
                buyer.address1 = address1Tb.Text;
                buyer.houseNo = houseNoTb.Text;
                buyer.zipCode = zipcodeTb.Text;
                buyer.taxId = taxIdTb.Text;
                if (is_branch.IsChecked.Value)
                {
                    buyer.isBranch = true;
                    buyer.branchId = branchNoTb.Text;
                }
                else
                {
                    buyer.isBranch = false;
                    buyer.branchId = "00000";
                }
                buyer.email = emailTb.Text;
                buyer.contactPerson = contactTb.Text;
                buyer.phoneNo = phoneNoTb.Text;
                buyer.phoneExt = phoneExtTb.Text;

                buyer.provinceCode = ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2);
                buyer.provinceName = ((AddressCodeListObj)provinceCbb.SelectedItem).changwat_th;
                buyer.districtCode = ((AddressCodeListObj)districtCbb.SelectedItem).code.Substring(0, 4);
                buyer.districtName = ((AddressCodeListObj)districtCbb.SelectedItem).amphoe_th;
                buyer.subdistrcitCode = ((AddressCodeListObj)subDistrictCbb.SelectedItem).code.Substring(0, 6);
                buyer.subdistrictName = ((AddressCodeListObj)subDistrictCbb.SelectedItem).tambon_th;

                obj.sellerId = saveContact(seller);
                obj.buyerId = saveContact(buyer);
                saveReferece();
                saveInvoiceItem();
                new InvoiceDao().save(obj, isNew);
                return true;
            }
            catch (Exception ex)
            {
                createBtn.IsEnabled = true;
                new AlertBox(ex.Message).ShowDialog();
                return false;
            }
        }

        private int saveContact(SellerObj obj)
        {
            ContactObj contact = new ContactObj();
            contact.name = obj.name;
            contact.taxId = obj.taxId;
            contact.branchId = obj.branchId;
            contact.email = obj.email;
            contact.zipCode = obj.zipCode;
            contact.address1 = obj.address1;
            contact.country = "TH";
            contact.phoneNo = obj.phoneNo;
            contact.phoneExt = obj.phoneExt;

            contact.provinceCode = obj.provinceCode;
            contact.provinceName = obj.provinceName;
            contact.districtCode = obj.districtCode;
            contact.districtName = obj.districtName;
            contact.subdistrcitCode = obj.subdistrcitCode;
            contact.subdistrictName = obj.subdistrictName;
            contact.houseNo = obj.houseNo;
            return new ContactDao().save(contact);
        }

        private int saveContact(BuyerObj obj)
        {
            ContactObj contact = new ContactObj();
            contact.name = obj.name;
            contact.taxId = obj.taxId;
            contact.branchId = obj.branchId;
            contact.email = obj.email;
            contact.zipCode = obj.zipCode;
            contact.address1 = obj.address1;
            contact.country = "TH";
            contact.phoneNo = obj.phoneNo;
            contact.phoneExt = obj.phoneExt;
            contact.contactPerson = obj.contactPerson;

            contact.provinceCode = obj.provinceCode;
            contact.provinceName = obj.provinceName;
            contact.districtCode = obj.districtCode;
            contact.districtName = obj.districtName;
            contact.subdistrcitCode = obj.subdistrcitCode;
            contact.subdistrictName = obj.subdistrictName;
            contact.houseNo = obj.houseNo;
            return new ContactDao().save(contact);
        }

        private void saveReferece()
        {
            new ReferenceDao().clear(this.invoiceID);
            foreach (ReferenceObj refObj in refList)
            {
                new ReferenceDao().save(refObj);
            }
        }

        private void saveInvoiceItem()
        {
            new InvoiceItemDao().clear(this.invoiceID);
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            foreach (InvoiceItemObj item in items)
            {
                new InvoiceItemDao().save(item);
            }
        }

        private void validateData()
        {
            util.Validator validator = new util.Validator();
            validator.validateText(docIdTb, "เลขที่ใบกำกับภาษีอ้างถึง", 35, true);
            validator.validateDocDate(docDateTb, "ใบกำกับภาษีอ้างถึง");
            validator.validateTaxID(taxIdTb);
            if (is_branch.IsChecked.Value)
            {
                validator.validateBranchNo(branchNoTb);
                validator.checkBranchID(branchNoTb, is_branch.IsChecked.Value);
            }
            // validate reference
            validator.validateCbb(purposeCbb, "สาเหตุการออกเอกสาร", 256, true);
            if (purposeCbb.SelectedValue.ToString() == "DBNG99" || purposeCbb.SelectedValue.ToString() == "DBNS99")
            {
                validator.validateText(otherPurposeTb, "เหตุอื่น", 256, true);
            }
            validator.validateNameCbb(nameCbb, "ชื่อบริษัท/ผู้ซื้อ", 256, true);
            validator.validateText(address1Tb, "ที่อยู่", 256, false);
            validator.validateText(houseNoTb, "บ้านเลขที่", 256, true);
            validator.validateProviceCodeList(provinceCbb, "จังหวัด");
            validator.validateProviceCodeList(districtCbb, "อำเภอ/เขต");
            validator.validateProviceCodeList(subDistrictCbb, "ตำบล/แขวง");
            validator.validateZipCode(zipcodeTb);
            validator.validateEmail(emailTb);
            validator.validateText(contactTb, "ชื่อผู้ติดต่อ", 140, false);
            validator.validatePhoneNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
            validator.validateDiffValue(diffValueTb.Text);
            
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            if (items.Count < 1)
            {
                throw new Exception("กรุณาเพิ่มรายการสินค้า/บริการ");
            }
            validator.validateDouble(originalValueTotal, "มูลค่าสินค้า/บริการตามใบกำกับภาษีเดิม", 0);
            validator.validateDouble(lineTotalTb, "มูลค่าสินค้า/บริการที่ถูกต้อง", 0);
            validator.validateDoubleRate(vatTb, "อัตราภาษีมูลค่าเพิ่ม", 99.99);
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            createBtn.IsEnabled = false;
            if (saveData())
            {
                try
                {
                    using (Report.InvoiceGenerator invoicegen = new Report.InvoiceGenerator())
                    {
                        invoicegen.create(this.invoiceID);
                        
                        byte[] xmlByte = invoicegen.getByteXml();
                        byte[] pdfByte = invoicegen.getBytePdf();
                        string xmlString = invoicegen.getStringXml();

                        PDFA3Invoice pdf = new PDFA3Invoice();

                        string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
                        string pdfFilePath = base_folder + "in/pdfA3.pdf";
                        System.IO.File.WriteAllBytes(pdfFilePath, pdfByte);
                        string xmlFilePath = base_folder + "in/ContentInformation.xml";
                        string xmlFileName = "ETDA-invoice.xml";

                        System.IO.File.WriteAllText(xmlFilePath, xmlString, System.Text.Encoding.UTF8);

                        string xmlVersion = "1.0";
                        string documentID = this.invoiceID;
                        string documentOID = "";
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.FileName = this.invoiceID + ".pdf";
                        dlg.DefaultExt = ".pdf";
                        dlg.Filter = "Pdf Files|*.pdf";
                        bool result = dlg.ShowDialog().Value;
                        if (result == true)
                        {
                            string outputPath = dlg.FileName;
                            pdf.CreatePDFA3Invoice(pdfFilePath, xmlFilePath, xmlFileName, xmlVersion, documentID, documentOID, outputPath, "Debit Note");
                            updateRunningNumber();
                            this.Close();
                        }
                        else
                        {
                            createBtn.IsEnabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    createBtn.IsEnabled = true;
                    Console.WriteLine("=============================================");
                    Console.WriteLine(ex.StackTrace);
                    new AlertBox(ex.Message).ShowDialog();
                }
            }
        }

        private void updateRunningNumber()
        {
            seller.dbn_no = new util.RunningNumber().updateRunningNumber(seller.dbn_no);
            new SellerDao().save(seller);
        }

        private bool isChange()
        {
            return nameCbb.Text != "" || address1Tb.Text != "" || houseNoTb.Text != "" || zipcodeTb.Text != "" ||
                taxIdTb.Text != "" || is_branch.IsChecked != false || branchNoTb.Text != "" ||
                emailTb.Text != "" || contactTb.Text != "" || vatTb.Text != "7.00" || phoneNoTb.Text != "" ||
                phoneExtTb.Text != "" || refList.Count != 0 || listView.Items.Count != 0 ||
                originalValueTotal.Text != "0.00" || remarkTb.Text != "" || provinceCbb.SelectedIndex != 0 ||
                purposeCbb.SelectedIndex != 0 || docIdTb.Text != "" || docDateTb.Text != "";
        }

        private void addRefBtn_Click(object sender, RoutedEventArgs e)
        {
            RefDoc refDoc = new RefDoc();
            refDoc.invoiceId = this.invoiceID;
            setRef1();
            refDoc.refList = this.refList;
            bool result = refDoc.ShowDialog().Value;
            if (result)
            {
                this.refList = refDoc.refList;
            }
            setRefDocData();
            this.Show();
        }

        private void setItemsSource(List<InvoiceItemObj> items)
        {
            int count = 1;
            foreach (InvoiceItemObj item in items)
            {
                item.number = count++;
            }
            listView.ClearValue(ListView.ItemsSourceProperty);
            listView.Items.Clear();
            listView.ItemsSource = items;
        }

        private void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink button = sender as Hyperlink;
            InvoiceItemObj itemObj = button.DataContext as InvoiceItemObj;
            InvoiceItem config = new InvoiceItem();
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            config.itemObj = itemObj;
            bool result = config.ShowDialog().Value;
            if (result)
            {
                var item = items.Find(x => (x.number == config.itemObj.number));
                item = config.itemObj;
            }
            setItemsSource(items);
            lineTotalTb.Text = "0.00";
            calculate();
            this.Show();
        }

        private void deleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            List<InvoiceItemObj> selectedItems = new List<InvoiceItemObj>();
            foreach (InvoiceItemObj item in items)
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
                            foreach (InvoiceItemObj item in items.ToList())
                            {
                                if (item.isSelected)
                                {
                                    items.Remove(item);
                                }
                            }
                            setItemsSource(items);
                            lineTotalTb.Text = "0.00";
                            calculate();
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
            if (isChange())
            {
                YesNo yn = new YesNo("ท่านต้องการออกจากหน้านี้ ซึ่งระบบจะยังไม่บันทึกข้อมูล ที่ท่านดำเนินการค้างอยู่", "ยืนยันการออกจากการหน้านี้");
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
            this.Close();
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            InvoiceItem item = new InvoiceItem();
            bool result = item.ShowDialog().Value;
            if (result)
            {
                item.itemObj.invoiceId = this.invoiceID;
                items.Add(item.itemObj);
            }
            setItemsSource(items);
            lineTotalTb.Text = "0.00";
            calculate();
            this.Show();
        }

        private void nameCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nameCbb.SelectedIndex > -1)
            {
                showData((BuyerObj)nameCbb.SelectedItem);
            }
        }

        private void calculate()
        {
            double lineTotal;
            double.TryParse(lineTotalTb.Text, out lineTotal);

            double original = 0.0;
            double.TryParse(originalValueTotal.Text, out original);

            double difference = 0.0;
            difference = lineTotal - original;
            diffValueTb.Text = difference.ToString("N");

            double taxRate = 0.0;
            double.TryParse(vatTb.Text, out taxRate);
            double taxTotal = difference * taxRate / 100;
            taxTotalTb.Text = taxTotal.ToString("N");
            double grandTotal = difference + taxTotal;
            grandTotalTb.Text = grandTotal.ToString("N");
        }

        private void vatTb_KeyUp(object sender, KeyEventArgs e)
        {
            double taxRate = 0.0;
            if (double.TryParse(vatTb.Text, out taxRate))
            {
                calculate();
            }
        }

        private void lineTotalTb_KeyUp(object sender, KeyEventArgs e)
        {
            double value = 0.0;
            if (double.TryParse(lineTotalTb.Text, out value))
            {
                calculate();
            }

            if (lineTotalTb.Text == string.Empty)
            {
                lineTotalTb.Text = "0.00";
            }
        }

        private void is_branch_Checked(object sender, RoutedEventArgs e)
        {
            branchNoTb.Text = "";
            branchNoTb.Focus();
        }

        private void branchNoTb_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                new util.Validator().checkBranchID(branchNoTb, is_branch.IsChecked.Value);
            }
            catch (Exception ex)
            {
                new AlertBox(ex.Message).ShowDialog();
            }
        }

        private void is_main_Checked(object sender, RoutedEventArgs e)
        {
            branchNoTb.Text = "";
        }

        private void originalValueTotal_KeyUp(object sender, KeyEventArgs e)
        {
            double value = 0.0;
            if (double.TryParse(originalValueTotal.Text, out value))
            {
                calculate();
            }
        }

        private void phoneNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
        }

        private void phoneExtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
        }

        private void originalValueTotal_GotFocus(object sender, RoutedEventArgs e)
        {
            if (originalValueTotal.Text == "0.00")
            {
                originalValueTotal.Text = string.Empty;
            }
        }

        private void originalValueTotal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (originalValueTotal.Text == string.Empty)
            {
                originalValueTotal.Text = "0.00";
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

        private void provinceCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (provinceCbb.SelectedIndex > 0)
            {
                util.ProvinceCodeList pcl = new util.ProvinceCodeList();
                districtCbb.SelectionChanged -= new SelectionChangedEventHandler(districtCbb_SelectionChanged);
                pcl.SetDistrict(districtCbb, ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2));
                districtCbb.SelectionChanged += new SelectionChangedEventHandler(districtCbb_SelectionChanged);
            }
            else
            {
                districtCbb.ItemsSource = null;
            }
        }

        private void districtCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (districtCbb.SelectedIndex > 0)
            {
                util.ProvinceCodeList pcl = new util.ProvinceCodeList();
                pcl.SetSubDistrict(subDistrictCbb, ((AddressCodeListObj)districtCbb.SelectedItem).code.Substring(0, 4));
            }
            else
            {
                subDistrictCbb.ItemsSource = null;
            }
        }

        private void lineTotalTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lineTotalTb.Text == "0.00")
            {
                lineTotalTb.Text = string.Empty;
            }
        }

        private void lineTotalTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lineTotalTb.Text == string.Empty)
            {
                lineTotalTb.Text = "0.00";
            }
        }

        private void purposeCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (purposeCbb.SelectedValue.ToString() == "DBNG99" || purposeCbb.SelectedValue.ToString() == "DBNS99")
            {
                otherPurposeTb.IsEnabled = true;
            }
            else
            {
                otherPurposeTb.Text = "";
                otherPurposeTb.IsEnabled = false;
            }
        }
    }
}
