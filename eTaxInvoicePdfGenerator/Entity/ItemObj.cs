using System.ComponentModel;
namespace eTaxInvoicePdfGenerator.Entity
{
    public class ItemObj
    {
        public int id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public double pricePerUnit { get; set; }
        public string unit { get; set; }
        public string unitXml { get; set; }
        public bool isService { get; set; }
        public string itemCode { get; set; }
        public string itemCodeInter { get; set; }

        public ItemObj()
        {

        }
        public ItemObj(string name, string detail)
        {
            this.name = name;
            this.detail = detail;
        }
    }
}
