using ECertificateAPI;
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Dialogs;
using eTaxInvoicePdfGenerator.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for CreditNote.xaml
    /// </summary>
    public partial class CreditNote : Window
    {
        // ใบลดหนี้
        private string invoiceID;
        private Collection<TypeCodeObj> typeCodes = new Collection<TypeCodeObj>() { new TypeCodeObj("ALT", "ใบกำกับภาษีเดิม"), new TypeCodeObj("ZZZ", "อื่นๆ") };
        public Collection<TypeCodeObj> TypeCodes { get { return typeCodes; } }
        private List<ReferenceObj> refList;
        private SellerObj seller;
        public CreditNote()
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
            Keyboard.Focus(nameCbb);
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
                this.invoiceID = seller.running_prefix + seller.running_number;
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
                TypeCodeObj typeCode = typeCodes.FirstOrDefault(s => s.code == refList[0].typeCodeObj.description);
                if (typeCode != null)
                {
                    typeCodeTb.SelectedItem = typeCode;
                }
                else
                {
                    typeCodeTb.Text = refList[0].typeCodeObj.description;
                }
            }
            else
            {
                docIdTb.Text = "";
                docDateTb.Text = "";
                typeCodeTb.Text = "";
            }
        }

        private void setRef1()
        {
            if (docIdTb.Text != "")
            {
                ReferenceObj ref1 = refList.FirstOrDefault(s => s.number == 1);
                if (ref1 == null)
                {
                    refList.Add(new ReferenceObj(1, this.invoiceID, docIdTb.Text, docDateTb.Text, typeCodeTb.Text, (TypeCodeObj)typeCodeTb.SelectedItem));
                }
                else
                {
                    ref1.documentId = docIdTb.Text;
                    ref1.documentDate = docDateTb.Text;
                    ref1.typeCode = typeCodeTb.Text;
                    ref1.invoiceId = this.invoiceID;
                    ref1.typeCodeObj = (TypeCodeObj)typeCodeTb.SelectedItem;
                    if (ref1.typeCodeObj == null)
                    {
                        ref1.typeCode = typeCodeTb.Text;
                        ref1.typeCodeObj = new TypeCodeObj("ZZZ", typeCodeTb.Text);
                    }
                    else
                    {
                        ref1.typeCode = refList[0].typeCodeObj.code;
                    }
                }
            }
        }


        private void showData(BuyerObj obj)
        {
            try
            {
                nameCbb.SelectedItem = obj;
                address1Tb.Text = obj.address1;
                address2Tb.Text = obj.address2;
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
                websiteTb.Text = obj.website;
                emailTb.Text = obj.email;
                contactTb.Text = obj.contactPerson;
                phoneNoTb.Text = obj.phoneNo;
                phoneExtTb.Text = obj.phoneExt;
                faxNoTb.Text = obj.faxNo;
                faxExtTb.Text = obj.faxExt;
                if (obj.provinceCode != null && obj.provinceCode != "")
                {
                    provinceCbb.SelectedValue = obj.provinceCode + "000000";
                    if (obj.districtCode != null && obj.districtCode != "")
                    {
                        amphoeCbb.SelectedValue = obj.districtCode + "0000";
                        if (obj.subdistrcitCode != null && obj.subdistrcitCode != "")
                        {
                            tambonCbb.SelectedValue = obj.subdistrcitCode + "00";
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
            pcl.SetChangwat(provinceCbb);
            pcl.SetAmphoe(amphoeCbb, ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2));
            pcl.SetTambon(tambonCbb, ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4));
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
                obj.invoiceName = "ใบลดหนี้";
                obj.purpose = purposeTb.Text;
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
                obj.issueDate = DateTime.Now.ToString();

                BuyerObj buyer = (BuyerObj)nameCbb.SelectedItem;
                if (buyer == null)
                {
                    buyer = new BuyerObj();
                }
                buyer.name = nameCbb.Text;
                buyer.address1 = address1Tb.Text;
                buyer.address2 = address2Tb.Text;
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
                buyer.website = websiteTb.Text;
                buyer.email = emailTb.Text;
                buyer.contactPerson = contactTb.Text;
                buyer.phoneNo = phoneNoTb.Text;
                buyer.phoneExt = phoneExtTb.Text;
                buyer.faxNo = faxNoTb.Text;
                buyer.faxExt = faxExtTb.Text;

                buyer.provinceCode = ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2);
                buyer.provinceName = ((AddressCodeListObj)provinceCbb.SelectedItem).changwat_th;
                buyer.districtCode = ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4);
                buyer.districtName = ((AddressCodeListObj)amphoeCbb.SelectedItem).amphoe_th;
                buyer.subdistrcitCode = ((AddressCodeListObj)tambonCbb.SelectedItem).code.Substring(0, 6);
                buyer.subdistrictName = ((AddressCodeListObj)tambonCbb.SelectedItem).tambon_th;

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
            contact.website = obj.website;
            contact.email = obj.email;
            contact.zipCode = obj.zipCode;
            contact.address1 = obj.address1;
            contact.address2 = obj.address2;
            contact.country = "TH";
            contact.phoneNo = obj.phoneNo;
            contact.phoneExt = obj.phoneExt;
            contact.faxNo = obj.faxNo;
            contact.faxExt = obj.faxExt;

            contact.provinceCode = obj.provinceCode;
            contact.provinceName = obj.provinceName;
            contact.districtCode = obj.districtCode;
            contact.districtName = obj.districtName;
            contact.subdistrcitCode = obj.subdistrcitCode;
            contact.subdistrictName = obj.subdistrictName;
            return new ContactDao().save(contact);
        }

        private int saveContact(BuyerObj obj)
        {
            ContactObj contact = new ContactObj();
            contact.name = obj.name;
            contact.taxId = obj.taxId;
            contact.branchId = obj.branchId;
            contact.website = obj.website;
            contact.email = obj.email;
            contact.zipCode = obj.zipCode;
            contact.address1 = obj.address1;
            contact.address2 = obj.address2;
            contact.country = "TH";
            contact.phoneNo = obj.phoneNo;
            contact.phoneExt = obj.phoneExt;
            contact.faxNo = obj.faxNo;
            contact.faxExt = obj.faxExt;
            contact.contactPerson = obj.contactPerson;

            contact.provinceCode = obj.provinceCode;
            contact.provinceName = obj.provinceName;
            contact.districtCode = obj.districtCode;
            contact.districtName = obj.districtName;
            contact.subdistrcitCode = obj.subdistrcitCode;
            contact.subdistrictName = obj.subdistrictName;
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

            // Validate name
            validator.validateText(nameCbb, "ชื่อผู้ประกอบการ", 256, true);
            // Validate address1
            validator.validateText(address1Tb, "ที่อยู่บรรทัดที่หนึ่ง", 256, true);
            // Validate address2
            validator.validateText(address2Tb, "ที่อยู่บรรทัดที่สอง", 256, true);
            // Validate zip code
            validator.validateZipCode(zipcodeTb);
            // Validate tax id
            validator.validateTaxID(taxIdTb);
            // Validate branch
            if (is_branch.IsChecked == true)
            {
                validator.validateBranchNo(branchNoTb);
                checkBranchID();
            }
            // Validate email
            validator.validateEmail(emailTb);
            // Validate Contact Person
            validator.validateText(contactTb, "ชื่อผู้ติดต่อ", 140, true);
            // Validate Phone no
            validator.validatePhoneNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
            // Validate Fax no
            validator.validatePhoneNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");

            // validate reference
            validator.validateText(purposeTb, "เหตุผลในการออกใบลดหนี้", 256, true);

            // validate doc id
            validator.validateText(docIdTb, "เลขที่ของใบกำกับภาษีเดิม", 35, true);

            validator.validateDocDate(docDateTb);

            validator.validateTypeCode(typeCodeTb);

            // validate vat rate
            validator.validateDouble(vatTb, "อัตราภาษีมูลค่าเพิ่ม", 99.99);

            // validate has item in list
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            if (items.Count < 1)
            {
                throw new Exception("กรุณาเพิ่มรายการสินค้า/บริการ");
            }
        }

        private bool validateDate(DateTime? date)
        {
            if (date == null)
            {
                return false;
            }

            if (date > DateTime.Now)
            {
                return false;
            }
            return true;
        }

        private void checkBranchID()
        {
            if (is_branch.IsChecked == true)
            {
                int intTemp;
                if (int.TryParse(branchNoTb.Text, out intTemp))
                {
                    string temp = branchNoTb.Text;
                    while (temp.Length < 5)
                    {
                        temp = "0" + temp;
                    }
                    branchNoTb.Text = temp;
                }
                else
                {
                    new AlertBox("กรุณาใส่เลขที่สาขา เป็นตัวเลขความยาวไม่เกิน 5 หลัก และไม่ใช่ \"00000\"").ShowDialog();
                }
            }
            else
            {
                branchNoTb.Text = "";
            }
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
                        /*get inv field for test*/
                        invoicegen.create(this.invoiceID);
                        //get xml
                        byte[] xmlByte = invoicegen.getByteXml();
                        //get pdf
                        byte[] pdfByte = invoicegen.getBytePdf();
                        //get xmlString
                        string xmlString = invoicegen.getStringXml();

                        PDFA3Invoice pdf = new PDFA3Invoice();

                        string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
                        string pdfFilePath = base_folder + "in/pdfA3.pdf";
                        System.IO.File.WriteAllBytes(pdfFilePath, pdfByte);
                        string xmlFilePath = base_folder + "in/ContentInformation.xml";
                        string xmlFileName = "ETDA-invoice.xml";
                        //System.IO.File.WriteAllBytes(xmlFileName, xmlByte);
                        System.IO.File.WriteAllText(xmlFilePath, xmlString, System.Text.Encoding.UTF8);

                        string xmlVersion = "1.0";
                        string documentID = this.invoiceID;
                        string documentOID = "";
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.FileName = "report.pdf";
                        dlg.DefaultExt = ".pdf";
                        dlg.Filter = "Pdf Files|*.pdf";
                        bool result = dlg.ShowDialog().Value;
                        if (result == true)
                        {
                            string outputPath = dlg.FileName;
                            pdf.CreatePDFA3Invoice(pdfFilePath, xmlFilePath, xmlFileName, xmlVersion, documentID, documentOID, outputPath, "Credit Note");
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
            int runningNumber = 0;
            int.TryParse(seller.running_number, out runningNumber);
            runningNumber = runningNumber + 1;
            string runningNumberStr = runningNumber.ToString();
            if (seller.running_prefix.Length + runningNumberStr.Length >= 35)
            {
                runningNumberStr = "1";
            }
            while (seller.running_number.Length > runningNumberStr.Length)
            {
                runningNumberStr = "0" + runningNumberStr;
            }
            seller.running_number = runningNumberStr;
            new SellerDao().save(seller);
        }

        private bool isChange()
        {
            return nameCbb.Text != "" || address1Tb.Text != "" || address2Tb.Text != "" || zipcodeTb.Text != "" ||
                 taxIdTb.Text != "" || is_branch.IsChecked != false || branchNoTb.Text != "" || websiteTb.Text != "" ||
                 emailTb.Text != "" || contactTb.Text != "" || vatTb.Text != "7.00" || phoneNoTb.Text != "" ||
                phoneExtTb.Text != "" || faxNoTb.Text != "" || faxExtTb.Text != "" || refList.Count != 0 || listView.Items.Count != 0 ||
                originalValueTotal.Text != "0.00" || remarkTb.Text != "" ||
                purposeTb.Text != "" || docIdTb.Text != "" || docDateTb.Text != "" || typeCodeTb.Text != "";
        }

        private void addRefBtn_Click(object sender, RoutedEventArgs e)
        {
            RefDoc refDoc = new RefDoc();
            refDoc.invoiceId = this.invoiceID;
            setRef1();
            refDoc.refList = this.refList;
            //this.Hide();
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
            //this.Hide();
            bool result = config.ShowDialog().Value;
            if (result)
            {
                var item = items.Find(x => (x.number == config.itemObj.number));
                item = config.itemObj;
            }
            setItemsSource(items);
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
                DelNo dn = new DelNo("ต้องการลบข้อมูลหรือไม่", "ยืนยันการลบรายการ");
                //yn.yesBtn.Content = "ลบ";
                //yn.noBtn.Content = "ยกเลิก";
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
                YesNo yn = new YesNo("ต้องการออกจากหน้านี้หรือไม่ โดยระบบจะไม่บันทึกข้อมูลไว้", "ยืนยันการออกจากการหน้านี้");
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
            List<InvoiceItemObj> items = listView.Items.Cast<InvoiceItemObj>().ToList();
            double lineTotal = 0.0;
            foreach (InvoiceItemObj item in items)
            {
                lineTotal += item.itemTotal;
            }
            lineTotalTb.Text = lineTotal.ToString("N");

            double basisAmount = lineTotal;

            double original = 0.0;
            double.TryParse(originalValueTotal.Text, out original);

            double difference = 0.0;
            difference = original - lineTotal;
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

        private void is_branch_Checked(object sender, RoutedEventArgs e)
        {
            branchNoTb.Text = "";
            branchNoTb.Focus();
        }

        private void branchNoTb_LostFocus(object sender, RoutedEventArgs e)
        {
            checkBranchID();
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

        private void faxNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");
        }

        private void faxExtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");
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
            util.ProvinceCodeList pcl = new util.ProvinceCodeList();
            amphoeCbb.SelectionChanged -= new SelectionChangedEventHandler(amphoeCbb_SelectionChanged);
            pcl.SetAmphoe(amphoeCbb, ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2));
            amphoeCbb.SelectionChanged += new SelectionChangedEventHandler(amphoeCbb_SelectionChanged);
            pcl.SetTambon(tambonCbb, ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4));
        }

        private void amphoeCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            util.ProvinceCodeList pcl = new util.ProvinceCodeList();
            pcl.SetTambon(tambonCbb, ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4));
        }
    }
}
