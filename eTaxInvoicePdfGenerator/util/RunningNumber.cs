using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace eTaxInvoicePdfGenerator.util
{
    class RunningNumber
    {

        internal string updateRunningNumber(string text)
        {
            text = text.Trim();
            string[] result = separatePrefix(text);
            string prefix = result[0];
            string number = result[1];
            int runningNumber;
            int.TryParse(number, out runningNumber);
            runningNumber++;
            string runningNumberStr = runningNumber.ToString();
            if (3 + prefix.Length + runningNumberStr.Length > 35)
            {
                runningNumberStr = "1";
            }
            while (number.Length > runningNumberStr.Length)
            {
                runningNumberStr = "0" + runningNumberStr;
            }
            string rt = prefix + runningNumberStr;
            return prefix + runningNumberStr;

        }

        //internal string updateRunningNumber(string text)
        //{
        //    text = text.Trim();
        //    int runningNumber = 0;
        //    //Regex re = new Regex(@"\d+$");
        //    string regTerm = @"^(?<alphabet>[\D]+)?(?<numeric>[\d]+)$";
        //    Regex re = new Regex(regTerm);
        //    Match match = re.Match(text);
        //    if (match.Success)
        //    {
        //        string number = match.Groups["numeric"].ToString();
        //        int.TryParse(number, out runningNumber);
        //        runningNumber = runningNumber + 1;
        //        string runningNumberStr = runningNumber.ToString();
        //        if (3 + match.Groups["alphabet"].Length + runningNumberStr.Length > 35)
        //        {
        //            runningNumberStr = "1";
        //        }
        //        while (number.Length > runningNumberStr.Length)
        //        {
        //            runningNumberStr = "0" + runningNumberStr;
        //        }
        //        return match.Groups["alphabet"].ToString() + runningNumberStr;
        //    }
        //    else
        //    {
        //        throw new Exception("กรุณาตรวจสอบเลขที่เอกสาร");
        //    }
        //}

        internal string[] separatePrefix(string str)
        {
            string prefix = "";
            string number = "";
            int pointer = str.Length;
            bool Isnumeric = true;
            while (Isnumeric)
            {
                pointer--;
                if (pointer == -1)
                {
                    break;
                }
                int tempInt;
                char c = str[pointer];
                Isnumeric = int.TryParse(c.ToString(), out tempInt);
            }

            prefix = str.Substring(0, pointer + 1);
            number = str.Substring(pointer + 1, str.Length - (pointer + 1));
            return new string[] { prefix, number };
        }
    }
}
