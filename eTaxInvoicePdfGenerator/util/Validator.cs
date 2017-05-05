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
            if (required)
            {
                if (obj.Text.Length == 0)
                {
                    obj.Focus();
                    throw new Exception(string.Format("กรุณาใส่{0} ความยาวไม่เกิน {1} ตัวอักษร", name, length));
                }
            }

            if (obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาใส่{0} ความยาวไม่เกิน {1} ตัวอักษร", name, length));
            }
        }

        public void validateText(ComboBox obj, string name, int length, bool required)
        {
            if (required)
            {
                if (obj.Text.Length == 0)
                {
                    obj.Focus();
                    throw new Exception(string.Format("กรุณาเลือก หรือใส่{0} ความยาวไม่เกิน {1} ตัวอักษร", name, length));
                }
            }

            if (obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาเลือก หรือใส่{0} ความยาวไม่เกิน {1} ตัวอักษร", name, length));
            }
        }

        public void validateDouble(TextBox obj, string name, double maxValue)
        {
            double doubleTemp = 0.0;
            if (double.TryParse(obj.Text, out doubleTemp))
            {
                if (doubleTemp < 0.00 || doubleTemp > maxValue)
                {
                    obj.Focus();
                    throw new Exception(string.Format("กรุณาใส่{0} เป็นร้อยละ ไม่เกิน {1}", name, maxValue));
                }
            }
            else
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาใส่{0} เป็นร้อยละ ไม่เกิน {1}", name, maxValue));
            }
        }

        public int validateQuantity(TextBox obj, int length)
        {
            int quantity = 0;
            if (obj.Text.Length == 0 || obj.Text.Length > length)
            {
                obj.Focus();
                throw new Exception(string.Format("กรุณาใส่จำนวน เป็นตัวเลขความยาวไม่เกิน {0} หลัก", length));
            }
            else
            {
                if (!int.TryParse(obj.Text, out quantity))
                {
                    obj.Focus();
                    throw new Exception(string.Format("กรุณาใส่จำนวน เป็นตัวเลขความยาวไม่เกิน {0} หลัก", length));
                }
            }
            return quantity;
        }

        public void validateZipCode(TextBox obj)
        {
            int intTemp = 0;
            if (!int.TryParse(obj.Text, out intTemp) || obj.Text.Length == 0 || obj.Text.Length != 5)
            {
                obj.Focus();
                throw new Exception("กรุณาใส่รหัสไปรษณีย์ เป็นตัวเลขความยาว 5 หลัก");
            }
        }

        public void validateTaxID(TextBox obj)
        {
            long longTemp = 0;
            if (!long.TryParse(obj.Text, out longTemp) || obj.Text.Length != 13)
            {
                obj.Focus();
                throw new Exception("กรุณาใส่เลขประจําตัวผู้เสียภาษีอากร เป็นตัวเลขความยาว 13 หลัก");
            }
        }

        public void validateBranchNo(TextBox obj)
        {
            int intTemp = 0;
            if (!int.TryParse(obj.Text, out intTemp) || obj.Text.Length > 5 || obj.Text.Length == 0 || obj.Text == "00000")
            {
                obj.Focus();
                throw new Exception("กรุณาใส่เลขที่สาขา เป็นตัวเลขความยาวไม่เกิน 5 หลัก และไม่ใช่ \"00000\"");
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
                    throw new Exception("กรุณาใส่อีเมลที่ถูกต้อง");
                }
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
                        throw new Exception(string.Format("กรุณาใส่{0}และเบอร์ต่อ ช่องนี้รองรับตัวเลข และอักขระ  \"(\", \")\", \" + \", \" - \" เท่านั้น ความยาว ไม่เกิน 24 ตัวอักษร", name));
                    }
                }

                if (!Regex.IsMatch(phoneNo.Text, @"^[0-9\(\)\+\-]{1,16}$"))
                {
                    phoneNo.Focus();
                    throw new Exception(string.Format("กรุณาใส่{0} ช่องนี้รองรับตัวเลข และอักขระ  \"(\", \")\", \" + \", \" - \" เท่านั้น ความยาว ไม่เกิน 26 ตัวอักษร", name));
                }

                while (phoneNo.Text.StartsWith("0"))
                {
                    phoneNo.Text = phoneNo.Text.Substring(1);
                }
            }
        }

        public void validateExtNumber(TextBox phoneNo,TextBox phoneExt,string name)
        {
            if (phoneNo.Text.Length == 0 && phoneExt.Text.Length > 0)
            {
                new Dialogs.AlertBox(string.Format("กรุณาใส่{0} ช่องนี้รองรับตัวเลข และอักขระ  \"(\", \")\", \" + \", \" - \" เท่านั้น ความยาว ไม่เกิน 26 ตัวอักษร",name)).ShowDialog();
                phoneExt.Text = "";
                phoneNo.Focus();
            }
        }

        public double validatePrice(TextBox obj)
        {
            double doubleTemp = 0.00;
            if (!Regex.IsMatch(obj.Text, @"^[0-9]{1,15}(?:\.[0-9]{0,2})?$") || !double.TryParse(obj.Text, out doubleTemp))
            {
                //if (doubleTemp < 0)
                //{
                //    Math.Abs(doubleTemp);
                //}
                obj.Text = doubleTemp.ToString("N");
                obj.Focus();
                throw new Exception("กรุณาใส่ราคาต่อหน่วย ที่ยังไม่รวมภาษีมูลค่าเพิ่ม เป็นตัวเลขความยาวไม่เกิน 18 หลักรวมจุด");
            }
            return doubleTemp;
        }

        public void validateUnit(ComboBox obj)
        {
            if (obj.Text.Length == 0)
            {
                obj.Focus();
                throw new Exception("กรุณาเลือกหน่วยสินค้า หรือเพิ่มใหม่หากไม่พบหน่วยที่ต้องการ");
            }
            else
            {
                if (obj.Text.Length > 20)
                {
                    obj.Focus();
                    throw new Exception("กรุณาใส่หน่วยสินค้า เป็นตัวอักษรความยาวไม่เกิน 20 ตัวอักษร");
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
                    throw new Exception("กรุณาใส่รหัสสินค้า ช่องนี้รองรับตัวอักษร A-Z และตัวเลข 0-9 เท่านั้น ความยาวไม่เกิน 35 ตัวอักษร");
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
                    throw new Exception("กรุณาใส่รหัสสินค้าสากล ช่องนี้รองรับตัวเลข 0-9 เท่านั้น ความยาว 13 หรือ 14 ตัวอักษร");
                }
            }
        }

        public double validateDiscount(TextBox obj,double total)
        {
            double doubleTemp = 0.0;
            if (double.TryParse(obj.Text, out doubleTemp))
            {
                if (doubleTemp < 0.00 || doubleTemp > total)
                {
                    obj.Focus();
                    throw new Exception("กรุณาใส่ส่วนลดต่อรายการ เป็นบาท ไม่เกินยอดรวมของรายการสินค้า");
                }
            }
            else
            {
                obj.Focus();
                throw new Exception("กรุณาใส่ส่วนลดต่อรายการ เป็นบาท ไม่เกินยอดรวมของรายการสินค้า");
            }
            return doubleTemp;
        }

        public void validateDocDate(DatePicker obj)
        {
            if (!validateDate(obj.SelectedDate))
            {
                obj.Focus();
                throw new Exception("กรุณาใส่วันที่ออกใบกำกับภาษีเดิมเป็นวันที่ ที่ไม่เกินวันที่ปัจจุบัน");
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
                throw new Exception("กรุณาเลือกประเภทเอกสารอ้างอิง");
            }
        }
    }
}
