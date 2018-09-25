using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace eTaxInvoicePdfGenerator.util
{
    public class Validator
    {
        public void validateText(TextBox obj, string name, int length, bool required)
        {
            if (required && obj.Text.Length == 0 || obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาระบุ{0}", name));
            }
        }

        public void validateCbb(ComboBox obj, string name, int length, bool required)
        {
            if (required && obj.SelectedIndex == 0 || obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณากรอก{0}", name));
            }
        }

        public void validateNameCbb(ComboBox obj, string name, int length, bool required)
        {
            if (required && obj.Text.Length == 0 || obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณากรอก{0}", name));
            }
        }

        public void validateProviceCodeList(ComboBox obj, string name)
        {
            if (obj.SelectedIndex < 1)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณากรอก{0}", name));
            }
        }

        public void validateDoubleRate(TextBox obj, string name, double maxValue)
        {
            double doubleTemp = 0.0;
            if (double.TryParse(obj.Text, out doubleTemp))
            {
                if (doubleTemp < 0.00 || (doubleTemp > maxValue && maxValue > 0))
                {
                    obj.Focus();
                    if (maxValue > 0)
                    {
                        throw new Exception(string.Format("กรุณาระบุ{0} เป็นร้อยละ ไม่เกิน {1}", name, maxValue));
                    }
                    else
                    {
                        throw new Exception(string.Format("ระบุ{0}ไม่ถูกต้อง โปรดตรวจสอบ", name));
                    }
                }
            }
            else
            {
                obj.Focus();
                throw new Exception(string.Format("ระบุ{0}ไม่ถูกต้อง โปรดตรวจสอบ", name, maxValue));
            }
        }

        public void validateDouble(TextBox obj, string name, double maxValue)
        {
            double doubleTemp = 0.0;
            if (double.TryParse(obj.Text, out doubleTemp))
            {
                if (doubleTemp < 0.00 || (doubleTemp > maxValue && maxValue > 0))
                {
                    obj.Focus();
                    if (maxValue > 0)
                    {
                        throw new Exception(string.Format("กรุณาระบุ{0} ไม่เกิน {1}", name, maxValue));
                    }
                    else
                    {
                        throw new Exception(string.Format("ระบุ{0}ไม่ถูกต้อง โปรดตรวจสอบ", name));
                    }
                }
            }
            else
            {
                obj.Focus();
                throw new Exception(string.Format("ระบุ{0}ไม่ถูกต้อง โปรดตรวจสอบ", name));
            }
        }

        public int validateQuantity(TextBox obj, int length)
        {
            int quantity = 0;
            if (obj.Text.Length == 0 || obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาระบุจำนวน เป็นตัวเลขความยาวไม่เกิน {0} หลัก", length));
            }
            else
            {
                if (!int.TryParse(obj.Text, out quantity))
                {
                    obj.Focus();
                    throw new Exception(string.Format("กรุณาระบุจำนวน เป็นตัวเลขความยาวไม่เกิน {0} หลัก", length));
                }
            }
            return quantity;
        }

        public void validateZipCode(TextBox obj)
        {
            if (obj.Text.Length == 0)
            {
                throw new Exception("กรุณาระบุรหัสไปรษณีย์");
            }

            int intTemp = 0;
            if (!int.TryParse(obj.Text, out intTemp) || obj.Text.Length != 5)
            {
                obj.Focus();
                throw new Exception("ระบุรหัสไปรษณีย์ไม่ถูกต้อง โปรดตรวจสอบ");
            }
        }

        public void validateTaxID(TextBox obj)
        {
            if (obj.Text.Length == 0)
            {
                throw new Exception("กรุณาระบุเลขประจําตัวผู้เสียภาษีอากร");
            }

            long longTemp = 0;
            if (!long.TryParse(obj.Text, out longTemp) || obj.Text.Length != 13)
            {
                obj.Focus();
                throw new Exception("ระบุเลขประจำตัวผู้เสียภาษีอากรไม่ถูกต้อง โปรดตรวจสอบ");
            }
        }

        public void validateTaxID(TextBox obj ,int status)
        {
            if (status == 0)
            {
                if (obj.Text.Length == 0)
                {
                    throw new Exception("กรุณาระบุเลขประจําตัวผู้เสียภาษีอากร");
                }

                long longTemp = 0;
                if (!long.TryParse(obj.Text, out longTemp) || obj.Text.Length != 13)
                {
                    obj.Focus();
                    throw new Exception("ระบุเลขประจำตัวผู้เสียภาษีอากรไม่ถูกต้อง โปรดตรวจสอบ");
                }
            }

            if (status == 1)
            {
                if (obj.Text.Length == 0)
                {
                    throw new Exception("กรุณาระบุเลขประจําตัวผู้เสียภาษีอากร");
                }

                long longTemp = 0;
                if (!long.TryParse(obj.Text, out longTemp) || obj.Text.Length != 35)
                {
                    obj.Focus();
                    throw new Exception("ระบุเลขที่หนังสือเดินทางไม่ถูกต้อง โปรดตรวจสอบ");
                }
            }
        }

        public void validateEmail(TextBox obj)
        {
            if (obj.Text.Length > 0)
            {
                bool isEmail = Regex.IsMatch(obj.Text, @"\A(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    obj.Focus();
                    throw new Exception("ระบุอีเมลไม่ถูกต้อง โปรดตรวจสอบ");
                }
            }
            else
            {
                throw new Exception("กรุณาระบุอีเมล");
            }
        }

        public void validatePhoneNumber(TextBox phoneNo, TextBox phoneExt, string name)
        {
            if (phoneNo.Text.Length > 0)
            {
                if (phoneExt.Text.Length > 0)
                {
                    if (!Regex.IsMatch(phoneExt.Text, @"^[0-9\(\)\+\-]{1,10}$"))
                    {
                        phoneExt.Focus();
                        //throw new Exception(string.Format("กรุณาใส่{0}และเบอร์ต่อ ช่องนี้รองรับตัวเลข และอักขระ  \"(\", \")\", \" + \", \" - \" เท่านั้น ความยาว ไม่เกิน 24 ตัวอักษร", name));
                        throw new Exception(string.Format("ระบุเบอร์โทรศัพท์หรือเบอร์ต่อไม่ถูกต้อง โปรดตรวจสอบ", name));
                    }
                }

                if (!Regex.IsMatch(phoneNo.Text, @"^[0-9\(\)\+\-]{1,16}$"))
                {
                    phoneNo.Focus();
                    throw new Exception(string.Format("ระบุเบอร์โทรศัพท์หรือเบอร์ต่อไม่ถูกต้อง โปรดตรวจสอบ", name));
                }

                while (phoneNo.Text.StartsWith("0"))
                {
                    phoneNo.Text = phoneNo.Text.Substring(1);
                }
            }
        }

        public void validateExtNumber(TextBox phoneNo, TextBox phoneExt, string name)
        {
            if (phoneNo.Text.Length == 0 && phoneExt.Text.Length > 0)
            {
                phoneExt.Text = "";
                phoneNo.Focus();
                new Dialogs.AlertBox("ระบุเบอร์โทรศัพท์หรือเบอร์ต่อไม่ถูกต้อง โปรดตรวจสอบ").ShowDialog();
            }
        }

        public double validatePrice(TextBox obj)
        {
            double doubleTemp = 0.00;
            string value = obj.Text.Replace(",", "");
            double.TryParse(value, out doubleTemp);
            if (doubleTemp <= 0)
            {
                throw new Exception("กรุณาระบุราคาต่อหน่วย");
            }
            if (!Regex.IsMatch(value, @"^[0-9]{1,15}(?:\.[0-9]{0,2})?$"))
            {
                obj.Focus();
                throw new Exception("ระบุข้อมูลไม่ถูกต้อง โปรดตรวจสอบ");
            }
            obj.Text = doubleTemp.ToString();
            return doubleTemp;
        }

        public void validateUnit(ComboBox obj)
        {
            if (obj.Text.Length == 0)
            {
                obj.Focus();
                throw new Exception("กรุณาระบุหน่วยสินค้า หรือเพิ่มใหม่หากไม่พบหน่วยที่ต้องการ");
            }
            else
            {
                if (obj.Text.Length > 20)
                {
                    obj.Focus();
                    throw new Exception("กรุณาระบุหน่วยสินค้า เป็นตัวอักษรความยาวไม่เกิน 20 ตัวอักษร");
                }
            }
        }

        public void validateItemCode(TextBox obj)
        {
            if (obj.Text.Length > 0)
            {
                if (!Regex.IsMatch(obj.Text, @"^[0-9a-zA-Z]{1,35}$") || obj.Text.Length > 35)
                {
                    obj.Focus();
                    throw new Exception("ระบุรหัสสินค้าไม่ถูกต้อง โปรดตรวจสอบ");
                }
            }
        }

        public void validateItemCodeInter(TextBox obj)
        {
            if (obj.Text.Length > 0)
            {
                if (!Regex.IsMatch(obj.Text, @"^[0-9]{13,14}$") || obj.Text.Length > 14)
                {
                    obj.Focus();
                    throw new Exception("ท่านระบุรหัสสินค้าสากลไม่ถูกต้อง โปรดตรวจสอบ");
                }
            }
        }

        public double validateDiscount(TextBox obj, double total)
        {
            double doubleTemp = 0.0;
            if (double.TryParse(obj.Text, out doubleTemp))
            {
                if (doubleTemp < 0.00 || doubleTemp > total)
                {
                    obj.Focus();
                    throw new Exception("กรุณาระบุส่วนลดต่อรายการ เป็นบาท ไม่เกินยอดรวมของรายการสินค้า");
                }
            }
            else
            {
                obj.Focus();
                throw new Exception("กรุณาระบุส่วนลดต่อรายการ เป็นบาท ไม่เกินยอดรวมของรายการสินค้า");
            }
            return doubleTemp;
        }

        public void validateDocDate(DatePicker obj, string name)
        {
            if (!validateDate(obj.SelectedDate))
            {
                obj.Focus();
                throw new Exception(string.Format("ระบุวันที่ออก{0}ไม่ถูกต้อง โปรดตรวจสอบ", name));
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

        public void validateTypeCode(ComboBox obj)
        {
            if (obj.Text.Length == 0 || obj.Text.Length > 20)
            {
                obj.Focus();
                throw new Exception("กรุณาระบุประเภทเอกสารอ้างอิง");
            }
        }

        public void validateRunningNumber(TextBox obj, string name, int length)
        {
            if (obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("ระบุ{0}ไม่ถูกต้อง โปรดตรวจสอบ", name, length));
            }

            if (obj.Text.Length == 0)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาระบุ{0}", name));
            }
            else
            {
                string[] result = new RunningNumber().separatePrefix(obj.Text);
                string prefix = result[0];
                string number = result[1];
                int maxLength = Math.Min(prefix.Length, length);
                prefix = prefix.Substring(0, maxLength);
                if (number.Length == 0)
                {
                    number = "00001";
                }
                obj.Text = prefix + number;

            }
        }
        
        public void validateBranchNo(TextBox obj)
        {
            if (obj.Text.Length == 0)
            {
                throw new Exception("กรุณาระบุรหัสสาขา");
            }

            int intTemp = 0;
            if (!int.TryParse(obj.Text, out intTemp) || obj.Text.Length > 5 || obj.Text.Length == 0 || obj.Text == "00000")
            {
                obj.Focus();
                throw new Exception("ระบุเลขที่สาขาไม่ถูกต้อง โปรดตรวจสอบ");
            }
        }

        public void checkBranchID(TextBox obj, bool is_branch)
        {
            if (is_branch)
            {
                int intTemp;
                if (int.TryParse(obj.Text, out intTemp))
                {
                    string temp = obj.Text;
                    while (temp.Length < 5)
                    {
                        temp = "0" + temp;
                    }
                    obj.Text = temp;
                }
                else
                {
                    throw new Exception("ระบุเลขที่สาขาไม่ถูกต้อง โปรดตรวจสอบ");
                }
            }
            else
            {
                obj.Text = "";
            }
        }

        public void validateDiffValue(string value)
        {
            double temp = 0;
            double.TryParse(value, out temp);
            if(temp < 0)
            {
                throw new Exception("กรุณาตรวจสอบ มูลค่าสินค้า/บริการ ตามใบกำกับภาษีเดิม");
            }
        }
    }
}
