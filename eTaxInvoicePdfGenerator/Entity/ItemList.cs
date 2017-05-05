namespace eTaxInvoicePdfGenerator.Entity
{
    class ItemList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public string price_per_unit { get; set; }        
        public bool isSelected { get; set; }

        public ItemList(int id, string name, string detail, string price_per_unit)
        {
            this.id = id;
            this.name = name;
            this.detail = detail;
            this.price_per_unit = price_per_unit;
            this.isSelected = false;
        }
    }
}
