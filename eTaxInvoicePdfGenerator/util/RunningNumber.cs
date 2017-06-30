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
