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
                vatTb.Text = obj.vat.ToString("0.00");
                phoneNoTb.Text = obj.phoneNo;
                phoneExtTb.Text = obj.phoneExt;
                faxNoTb.Text = obj.faxNo;
                faxExtTb.Text = obj.faxExt;
                runningNumberTb.Text = obj.running_prefix + obj.running_number;
                if (runningNumberTb.Text != "")
                {
                    runningNumberTb.IsEnabled = false;
                }
                else
                {
                    runningNumberTb.IsEnabled = true;
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
                // MessageBox.Show(ex.Message);
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
                SellerObj obj = new SellerObj();
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
                obj.vat = Convert.ToDouble(vatTb.Text);
                obj.phoneNo = phoneNoTb.Text;
                obj.phoneExt = phoneExtTb.Text;
                obj.faxNo = faxNoTb.Text;
                obj.faxExt = faxExtTb.Text;

                Regex re = new Regex(@"([\D]+)?([\d]+)?$");
                Match result = re.Match(runningNumberTb.Text.Trim());
                if (result.Success)
                {
                    obj.running_prefix = result.Groups[1].Value;
                    obj.running_number = result.Groups[2].Value;
                }
                else
                {
                    obj.running_prefix = "";
                    obj.running_number = "0001";
                }
                obj.provinceCode = ((AddressCodeListObj)provinceCbb.SelectedItem).code.Substring(0, 2);
                obj.provinceName = ((AddressCodeListObj)provinceCbb.SelectedItem).changwat_th;
                obj.districtCode = ((AddressCodeListObj)amphoeCbb.SelectedItem).code.Substring(0, 4);
                obj.districtName = ((AddressCodeListObj)amphoeCbb.SelectedItem).amphoe_th;
                obj.subdistrcitCode = ((AddressCodeListObj)tambonCbb.SelectedItem).code.Substring(0, 6);
                obj.subdistrictName = ((AddressCodeListObj)tambonCbb.SelectedItem).tambon_th;

                new SellerDao().save(obj);
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
            int intTemp;
            // Validate name
            util.Validator validator = new util.Validator();
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
            if (is_branch.IsChecked.Value)
            {
                validator.validateBranchNo(branchNoTb);
                checkBranchID();
            }

            // Validate email
            validator.validateEmail(emailTb);

            // Validate Phone no
            validator.validatePhoneNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");

            // Validate Fax no
            validator.validatePhoneNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");

            // Validate Running Number
            validator.validateText(runningNumberTb, "ค่าเริ่มต้นของเลขที่เอกสาร", 35, true);

            string temp = runningNumberTb.Text;
            string lastChar = temp.Substring(temp.Length - 1, 1);
            // ตรวจว่ามีตัวเลขต่อท้ายหรือไม่
            if (!int.TryParse(lastChar, out intTemp))
            {
                // ต่อท้ายด้วย 0001 แต่ถ้าตัวอักษรยาวถึง 35 ให้แทนตำแหน่งที่ 32-35
                if (temp.Length > 32)
                {
                    temp = temp.Substring(0, 31) + "0001";
                }
                else
                {
                    temp = temp + "0001";
                }
                runningNumberTb.Text = temp;
            }

            // Validate Vat
            validator.validateDouble(vatTb, "อัตราภาษีมูลค่าเพิ่ม", 99);

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

            //    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("ต้องการบันทึกข้อมูลหรือไม่", "Exit Confirmation", MessageBoxButton.YesNoCancel);
            //    if (messageBoxResult == MessageBoxResult.Yes)
            //    {
            //        if (!saveData())
            //        {
            //            return;
            //        }
            //    }
            //    else if (messageBoxResult == MessageBoxResult.Cancel)
            //    {
            //        return;
            //    }
            //}
            //this.Close();
        }

        private bool isChange()
        {
            SellerObj obj = new SellerDao().select();
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
                 taxIdTb.Text != obj.taxId || obj.isBranch != is_branch.IsChecked || chkBranchNo || websiteTb.Text != obj.website ||
                 emailTb.Text != obj.email || vatTb.Text != obj.vat.ToString("0.00") || phoneNoTb.Text != obj.phoneNo ||
                phoneExtTb.Text != obj.phoneExt || faxNoTb.Text != obj.faxNo || faxExtTb.Text != obj.faxExt || runningNumberTb.Text != obj.running_prefix + obj.running_number;

        }

        private void checkBranchID()
        {
            if (is_branch.IsChecked.Value)
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

        private void branchNoTb_LostFocus(object sender, RoutedEventArgs e)
        {
            checkBranchID();
        }

        private void phoneNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
        }

        private void phoneExtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(phoneNoTb, phoneExtTb, "เบอร์โทรศัพท์");
        }

        private void faxExtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");
        }

        private void faxNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            new util.Validator().validateExtNumber(faxNoTb, faxExtTb, "เบอร์โทรสาร");
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