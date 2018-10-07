using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Entity;
using System.Text.RegularExpressions;
using System.Linq;
using eTaxInvoicePdfGenerator.Dialogs;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for Seller.xaml
    /// </summary>
    public partial class Seller : Window
    {
        public int id = 0;
        public Seller()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setProvinceList();
            if (new SellerDao().count() > 0)
            {
                showData();
                Keyboard.Focus(nameTb);
            }
            else
            {
                is_main.IsChecked = true;
            }
        }

        private void showData()
        {
            try
            {
                SellerObj obj = new SellerDao().select();
                this.id = obj.id;
                nameTb.Text = obj.name;
                houseNoTb.Text = obj.houseNo;
                address1Tb.Text = obj.address1;
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
                vatTb.Text = obj.vat.ToString("0.00");
                phoneNoTb.Text = obj.phoneNo;
                phoneExtTb.Text = obj.phoneExt;
                if (obj.inv_no.Equals(""))
                {
                    invRunningNumberTb.Text = "00001";
                    invRunningNumberTb.IsEnabled = true;
                }
                else
                {
                    invRunningNumberTb.Text = obj.inv_no;
                    invRunningNumberTb.IsEnabled = false;
                }

                if (obj.dbn_no.Equals(""))
                {
                    dbnRunningNumberTb.Text = "00001";
                    dbnRunningNumberTb.IsEnabled = true;
                }
                else
                {
                    dbnRunningNumberTb.Text = obj.dbn_no;
                    dbnRunningNumberTb.IsEnabled = false;
                }

                if (obj.crn_no.Equals(""))
                {
                    CrnRunningNumberTb.Text = "00001";
                    CrnRunningNumberTb.IsEnabled = true;
                }
                else
                {
                    CrnRunningNumberTb.Text = obj.crn_no;
                    CrnRunningNumberTb.IsEnabled = false;
                }


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
                validateData();
                SellerObj obj = new SellerObj();
                obj.id = this.id;
                obj.name = nameTb.Text;
                obj.houseNo = houseNoTb.Text;
                obj.address1 = address1Tb.Text;
                obj.zipCode = zipcodeTb.Text;
                obj.taxId = taxIdTb.Text;
                if (is_branch.IsChecked.Value)
                {
                    obj.isBranch = true;
                    obj.branchId = branchNoTb.Text;
                }
                else
                {
                    obj.isBranch = false;
                    obj.branchId = "00000";
                }
                obj.email = emailTb.Text;
                obj.vat = Convert.ToDouble(vatTb.Text);
                obj.phoneNo = phoneNoTb.Text;
                obj.phoneExt = phoneExtTb.Text;

                obj.inv_no = invRunningNumberTb.Text;
                obj.dbn_no = dbnRunningNumberTb.Text;
                obj.crn_no = CrnRunningNumberTb.Text;
                obj.provinceCode = ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2);
                obj.provinceName = ((AddressCodeListObj)provinceCbb.SelectedItem).changwat_th;
                obj.districtCode = ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4);
                obj.districtName = ((AddressCodeListObj)amphoeCbb.SelectedItem).amphoe_th;
                obj.subdistrcitCode = ((AddressCodeListObj)tambonCbb.SelectedItem).code.Substring(0, 6);
                obj.subdistrictName = ((AddressCodeListObj)tambonCbb.SelectedItem).tambon_th;

                new SellerDao().save(obj);
                new AlertBox("บันทึกข้อมูลผู้ขายเรียบร้อยแล้ว").ShowDialog();
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
            validator.validateTaxID(taxIdTb);
            if (is_branch.IsChecked.Value)
            {
                validator.validateBranchNo(branchNoTb);
                validator.checkBranchID(branchNoTb, is_branch.IsChecked.Value);
            }
            validator.validateText(nameTb, "ชื่อผู้ประกอบการ", 256, true);
            validator.validateText(address1Tb, "ที่อยู่", 256, false);
            validator.validateText(houseNoTb, "บ้านเลขที่", 256, true);
            validator.validateProviceCodeList(provinceCbb, "จังหวัด");
            validator.validateProviceCodeList(amphoeCbb, "อำเภอ/เขต");
            validator.validateProviceCodeList(tambonCbb, "ตำบล/แขวง");
            validator.validateZipCode(zipcodeTb);
            validator.validateEmail(emailTb);
            validator.validatePhoneNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
            validator.validateRunningNumber(invRunningNumberTb, "ค่าเริ่มต้นของเลขที่เอกสารใบกำกับภาษี", 35 - 3 - 5);
            validator.validateRunningNumber(dbnRunningNumberTb, "ค่าเริ่มต้นของเลขที่เอกสารใบเพิ่มหนี้", 35 - 3 - 5);
            validator.validateRunningNumber(CrnRunningNumberTb, "ค่าเริ่มต้นของเลขที่เอกสารใบลดหนี้ ", 35 - 3 - 5);
            validator.validateDoubleRate(vatTb, "อัตราภาษีมูลค่าเพิ่ม", 99);
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

        private bool isChange()
        {
            SellerObj obj = new SellerDao().select();
            if (this.id == 0)
            {
                return nameTb.Text != "" || houseNoTb.Text != "" || address1Tb.Text != "" || zipcodeTb.Text != "" ||
                    taxIdTb.Text != "" || obj.isBranch != false || branchNoTb.Text != "" || emailTb.Text != "" ||
                    vatTb.Text != "7.00" || phoneNoTb.Text != "" || phoneExtTb.Text != "" ||
                    invRunningNumberTb.Text != "" || dbnRunningNumberTb.Text != "" || CrnRunningNumberTb.Text != "";
            }
            else
            {
                bool chkBranchNo = false;
                if (is_branch.IsChecked.Value)
                {
                    chkBranchNo = obj.branchId != branchNoTb.Text;
                }
                else
                {
                    chkBranchNo = false;
                }

                if (obj.inv_no.Equals(""))
                {
                    obj.inv_no = "00001";
                }

                if (obj.dbn_no.Equals(""))
                {
                    obj.dbn_no = "00001";
                }

                if (obj.crn_no.Equals(""))
                {
                    obj.crn_no = "00001";
                }

                return nameTb.Text != obj.name || houseNoTb.Text != obj.houseNo || address1Tb.Text != obj.address1 || zipcodeTb.Text != obj.zipCode ||
                    taxIdTb.Text != obj.taxId || obj.isBranch != is_branch.IsChecked || chkBranchNo || emailTb.Text != obj.email ||
                    vatTb.Text != obj.vat.ToString("0.00") || phoneNoTb.Text != obj.phoneNo || phoneExtTb.Text != obj.phoneExt ||
                    invRunningNumberTb.Text != obj.inv_no || dbnRunningNumberTb.Text != obj.dbn_no || CrnRunningNumberTb.Text != obj.crn_no;
            }

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

        private void phoneNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
        }

        private void phoneExtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
        }

        private void is_main_Checked(object sender, RoutedEventArgs e)
        {
            branchNoTb.Text = "";
        }

        private void is_branch_Checked(object sender, RoutedEventArgs e)
        {
            branchNoTb.Text = "";
            branchNoTb.Focus();
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveData();
            }
        }

        private void provinceCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (provinceCbb.SelectedIndex > 0)
            {
                util.ProvinceCodeList pcl = new util.ProvinceCodeList();
                amphoeCbb.SelectionChanged -= new SelectionChangedEventHandler(amphoeCbb_SelectionChanged);
                pcl.SetDistrict(amphoeCbb, ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2));
                amphoeCbb.SelectionChanged += new SelectionChangedEventHandler(amphoeCbb_SelectionChanged);
            }
            else
            {
                amphoeCbb.ItemsSource = null;
            }
        }

        private void amphoeCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (amphoeCbb.SelectedIndex > 0)
            {
                util.ProvinceCodeList pcl = new util.ProvinceCodeList();
                pcl.SetSubDistrict(tambonCbb, ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4));
            }
            else
            {
                tambonCbb.ItemsSource = null;
            }
        }

        private void houseNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}