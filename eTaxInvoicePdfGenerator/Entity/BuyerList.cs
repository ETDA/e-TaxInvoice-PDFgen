namespace eTaxInvoicePdfGenerator.Entity
{
    class BuyerList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string taxid_no { get; set; }
        public string phone_no { get; set; }
        public bool isSelected { get; set; }

        public BuyerList(int id,string name,string taxid_no,string phone_no,string phone_ext)
        {
            this.id = id;
            this.name = name;
            this.taxid_no = taxid_no;
            if(phone_no.Length > 0)
            {
                phone_no = string.Format("+66-{0}",phone_no);
                if(phone_ext.Length > 0)
                {
                    phone_no += string.Format("-({0})", phone_ext);
                }
            }
            this.phone_no = phone_no;
            this.isSelected = false;
        }
    }
}
