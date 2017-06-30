using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace eTaxInvoicePdfGenerator.util
{
    public class DateHelper
    {
        public string Convert2En(string dateTime)
        {
            DateTime oDate = DateTime.Parse(dateTime);
            return oDate.ToString("dd/MM/yyyy", new CultureInfo("en-US"));
        }

        public string Convert2Th(string dateTime)
        {
            DateTime oDate = DateTime.Parse(dateTime);
            return oDate.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
        }

        public DateTime Convert2Date(string dateTime)
        {
            // dd/MM/yyyy
            string[] array = dateTime.Split('/');
            string nDateTime = array[2] + '-' + array[1] + '-' +array[0];
            return DateTime.Parse(nDateTime);
        }

        public DateTime Convert2DateFromTH(string dateTime)
        {
            // dd/MM/yyyy TH
            string[] array = dateTime.Split('/');
            string nDateTime = array[2] + '-' + array[1] + '-' + array[0];
            return DateTime.Parse(nDateTime,new CultureInfo("th-TH"));
        }
    }
}
