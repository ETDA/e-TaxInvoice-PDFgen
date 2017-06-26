using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTaxInvoicePdfGenerator.Entity
{
    class AddressCodeListObj
    {
        public string code { get; set; }
        public string changwat_th { get; set; }
        public string amphoe_th { get; set; }
        public string tambon_th { get; set; }

        public AddressCodeListObj()
        {

        }

        public AddressCodeListObj(string code, string changwat_th, string amphoe_th, string tambon_th)
        {
            this.code = code;
            this.changwat_th = changwat_th;
            this.amphoe_th = amphoe_th;
            this.tambon_th = tambon_th;
        }
    }
}
