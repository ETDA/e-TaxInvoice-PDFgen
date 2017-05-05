namespace eTaxInvoicePdfGenerator.Entity
{
    class CodeList
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }

        public CodeList()
        {

        }

        public CodeList(string code, string description)
        {
            this.code = code;
            this.description = description;
        }

        public CodeList(int id, string code, string description)
        {
            this.id = id;
            this.code = code;
            this.description = description;
        }
    }


}
