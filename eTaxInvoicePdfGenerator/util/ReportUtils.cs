using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Data;
namespace eTaxInvoicePdfGenerator.util
{
    public class ReportUtils
    {
        public static string getThaiDate(string date)
        {
            string dateThai = "";
            if (!string.IsNullOrEmpty(date))
            {

                DateTime datetime = new util.DateHelper().Convert2Date(date);

                ThaiBuddhistCalendar cal = new ThaiBuddhistCalendar();
                string[] thaiMonth = { "มกราคม","กุมภาพันธ์","มีนาคม","เมษายน","พฤษภาคม"
                                    ,"มิถุนายน","กรกฏาคม","สิงหาคม","กันยายน","ตุลาคม","พฤศจิกายน","ธันวาคม" };
                DateTime thai = new DateTime(cal.GetYear(datetime), cal.GetMonth(datetime), datetime.Day);
                string culture_th = System.Globalization.CultureInfo.CurrentCulture.DisplayName;
                if (culture_th=="Thai (Thailand)")
                {
                    string year = thai.ToString("yyyy", new System.Globalization.CultureInfo("th-TH"));
                    dateThai = thai.Day + " " + thaiMonth[thai.Month - 1] + " " + year;
                }
                else
                {
                    dateThai = thai.Day + " " + thaiMonth[thai.Month - 1] + " " + thai.Year;
                }
            }
            else { dateThai = null; }

            return dateThai;
        }
        public static string getFullThaiMobilePhone(string number, string ext)
        {
            var result = " ";
            if (!string.IsNullOrEmpty(number))
            {
                if (string.IsNullOrEmpty(ext))
                {
                    result = number.Substring(0, number.Length);
                    result = "+66-" + result;
                }
                else if (!string.IsNullOrEmpty(ext))
                {
                    result = number.Substring(0, number.Length);
                    result = "+66-" + result + "(" + ext + ")";
                }
            }
            
            return result;
        }
        public string getBranch(string id,string taxScheme)
        {
            string text_result = "";

            if (taxScheme == "TXID")
            {
                text_result = "(" + id + ")";

                if (id.IndexOf("00000") != -1)
                    text_result = "สำนักงานใหญ่(" + id + ")";
                else
                    text_result = "สาขา(" + id + ")";
            }
                return text_result;
        }
        public static string getFullThaiBathController(string txt)
        {
            string result = "";
            string[] bathTxt = txt.Split('.');
            string second = "";
            string first = "";

            if (bathTxt[0].Count() > 7)
            {
                var first_part = bathTxt[0].Substring(0, (bathTxt[0].Count() - 6));
                first = getFullThaiBaht(first_part) + "ล้าน";
                first = first.Replace("ถ้วน", "");
                first = first.Replace("บาท", "");

                second = bathTxt[0].Replace(first_part, "");
                if (bathTxt.Count() > 1)
                {
                    second = second + "." + bathTxt[1];
                    second = getFullThaiBaht(second);
                }
                else if (bathTxt.Count() < 2)
                {
                    second = "";
                    second = "บาทถ้วน";
                }
                result = first + second;
            }
            else
            {
                result = getFullThaiBaht(txt);
            }

            return result;

        }
        public static string getFullThaiBaht(string txt)
        {

            string bahtTxt, n, bahtTH = "";
            double amount;
            try { amount = Convert.ToDouble(txt); }
            catch { amount = 0; }
            bahtTxt = amount.ToString("####.00");
            string[] num = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] rank = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
            string[] temp = bahtTxt.Split('.');
            string intVal = temp[0];
            string decVal = temp[1];
            if (Convert.ToDouble(bahtTxt) == 0)
                bahtTH = "ศูนย์บาทถ้วน";
            else if (Convert.ToDouble(bahtTxt) == 1)
                bahtTH = "หนึ่งบาทถ้วน";
            else
            {
                if (intVal.Length > 0)
                {
                    if (intVal.Substring(0, 1) == "-")
                    {
                        bahtTH += "ลบ";
                        intVal = intVal.Substring(1, intVal.Length - 1);
                    }
                }

                for (int i = 0; i < intVal.Length; i++)
                {
                    n = intVal.Substring(i, 1);
                    if (n != "0")
                    {
                        if ((i == (intVal.Length - 1)) && (n == "1"))
                            bahtTH += "เอ็ด";
                        else if ((i == (intVal.Length - 2)) && (n == "2"))
                            bahtTH += "ยี่";
                        else if ((i == (intVal.Length - 2)) && (n == "1"))
                            bahtTH += "";
                        else
                            bahtTH += num[Convert.ToInt32(n)];
                        bahtTH += rank[(intVal.Length - i) - 1];
                    }
                }
                bahtTH += "บาท";
                if (decVal == "00")
                    bahtTH += "ถ้วน";
                else
                {
                    for (int i = 0; i < decVal.Length; i++)
                    {
                        n = decVal.Substring(i, 1);
                        if (n != "0")
                        {
                            if ((i == decVal.Length - 1) && (n == "1"))
                                bahtTH += "เอ็ด";
                            else if ((i == (decVal.Length - 2)) && (n == "2"))
                                bahtTH += "ยี่";
                            else if ((i == (decVal.Length - 2)) && (n == "1"))
                                bahtTH += "";
                            else
                                bahtTH += num[Convert.ToInt32(n)];
                            bahtTH += rank[(decVal.Length - i) - 1];
                        }
                    }
                    bahtTH += "สตางค์";
                }
            }
            return bahtTH;
        }

        public string getTaxNO(string v, string taxType)
        {
            String textReturn = v;

            if (taxType == "OTHR")
            {
                textReturn = "N/A";
            }

            return textReturn;
        }

        public static List<string> getReference (DataTable reference)
        {
            List<string> doc_reference = new List<string>();
            string doc_id = "";
            string doc_date = "";

            foreach(DataRow dr in reference.Rows)
            {
                doc_id = doc_id + dr["document_id"].ToString()+",";
                doc_date = doc_date + getThaiDate(dr["document_date"].ToString())+",";
            }

            doc_reference.Add(doc_id.Substring(0,doc_id.Length-1));
            doc_reference.Add(doc_date.Substring(0, doc_date.Length - 1));

            return doc_reference;
        }
        public static string replaceSpecialChar(string input)
        {
            String temp = input;

            char[] specialChar = { '&', '<', '>', '\'', '"' };
            IDictionary<char, string> dict = new Dictionary<char, string>()
                                            {
                                                {'&',"&amp;"},
                                                {'<', "&lt;"},
                                                {'>', "&gt;"},
                                                {'\'', "&apos;"},
                                                {'"', "&quot;"}
                                            };

            foreach (char n in specialChar)
            {
                while (input.IndexOf(n) != -1)
                {
                    temp = temp.Replace(n.ToString(), dict[n]);
                    break;
                }
            }

            return temp;
        }
    }
}
