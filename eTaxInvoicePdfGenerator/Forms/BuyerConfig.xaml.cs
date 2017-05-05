using System;
using System.Windows;
using System.Windows.Input;
using eTaxInvoicePdfGenerator.Entity;
using eTaxInvoicePdfGenerator.Dao;
using System.Text.RegularExpressions;
using eTaxInvoicePdfGenerator.Dialogs;
using System.Windows.Controls;

namespace eTaxInvoicePdfGenerator.Forms
{
    /// <summary>
    /// Interaction logic for BuyerConfig.xaml
    /// </summary>
    public partial class BuyerConfig : Window
    {
        public int id = 0;
        public BuyerConfig()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Buyer buyer = new Buyer();
            buyer.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setProvinceList();
            if (this.id != 0)
            {
                showData();
            }
            else
            {
                is_main.IsChecked = true;
            }
            Keyboard.Focus(nameTb);
        }

        private void showData()
        {
            try
            {
                BuyerObj obj = new BuyerDao().select(this.id);
                nameTb.Text = obj.name;
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
                validateData();
                BuyerObj obj = new BuyerObj();
                obj.id = this.id;
                obj.name = nameTb.Text;
                obj.address1 = address1Tb.Text;
                obj.address2 = address2Tb.Text;
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
                obj.website = websiteTb.Text;
                obj.email = emailTb.Text;
                obj.contactPerson = contactTb.Text;
                obj.phoneNo = phoneNoTb.Text;
                obj.phoneExt = phoneExtTb.Text;
                obj.faxNo = faxNoTb.Text;
                obj.faxExt = faxExtTb.Text;

                obj.provinceCode = ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2);
                obj.provinceName = ((AddressCodeListObj)provinceCbb.SelectedItem).changwat_th;
                obj.districtCode = ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4);
                obj.districtName = ((AddressCodeListObj)amphoeCbb.SelectedItem).amphoe_th;
                obj.subdistrcitCode = ((AddressCodeListObj)tambonCbb.SelectedItem).code.Substring(0, 6);
                obj.subdistrictName = ((AddressCodeListObj)tambonCbb.SelectedItem).tambon_th;
                new BuyerDao().save(obj);
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

            // Validate name
            validator.validateText(nameTb, "ชื่อผู้ประกอบการ", 256, true);
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
            if (this.id == 0)
            {
                return nameTb.Text != "" || address1Tb.Text != "" || address2Tb.Text != "" || zipcodeTb.Text != "" ||
                    taxIdTb.Text != "" || taxIdTb.Text != "" || is_branch.IsChecked != false || branchNoTb.Text != "" ||
                    websiteTb.Text != "" || emailTb.Text != "" || contactTb.Text != "" || phoneNoTb.Text != "" ||
                    phoneExtTb.Text != "" || faxNoTb.Text != "" || faxExtTb.Text != "";
            }
            else
            {
                BuyerObj obj = new BuyerDao().select(this.id);
                bool chkBranchNo = false;
                if (is_branch.IsChecked.Value)
                {
                    chkBranchNo = obj.branchId != branchNoTb.Text;
                }
                else
                {
                    chkBranchNo = false;
                }
                return nameTb.Text != obj.name || address1Tb.Text != obj.address1 || address2Tb.Text != obj.address2 || zipcodeTb.Text != obj.zipCode ||
                    taxIdTb.Text != obj.taxId || taxIdTb.Text != obj.taxId || obj.isBranch != is_branch.IsChecked || chkBranchNo ||
                    websiteTb.Text != obj.website || emailTb.Text != obj.email || contactTb.Text != obj.contactPerson || phoneNoTb.Text != obj.phoneNo ||
                    phoneExtTb.Text != obj.phoneExt || faxNoTb.Text != obj.faxNo || faxExtTb.Text != obj.faxExt;
            }

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
                    throw new Exception("กรุณาใส่เลขที่สาขา เป็นตัวเลขความยาวไม่เกิน 5 หลัก และไม่ใช่ \"00000\"");
                }
            }
            else
            {
                branchNoTb.Text = "";
            }
        }

        private void branchNoTb_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                checkBranchID();
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

        private void faxNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");
        }

        private void faxExtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");
        }

        private void is_main_Checked(object sender, RoutedEventArgs e)
        {
            branchNoTb.Text = "";
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveData();
            }
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
