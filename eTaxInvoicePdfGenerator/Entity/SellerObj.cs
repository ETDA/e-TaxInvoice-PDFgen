namespace eTaxInvoicePdfGenerator.Entity
{
    class SellerObj
    {
        public int id { get; set; }
        public string name { get; set; }
        public string taxId { get; set; }
        public string phoneNo { get; set; }
        public string phoneExt { get; set; }
        public string zipCode { get; set; }
        public string address1 { get; set; }
        //public string address2 { get; set; }
        public string email { get; set;}
        //public string website { get; set; }
        //public string faxNo { get; set; }
        //public string faxExt { get; set; }
        public bool isBranch { get; set; }
        public string branchId { get; set; }
        public double vat { get; set; }
        //public string running_prefix { get; set; }
        //public string running_number { get; set; }

        public string provinceName { get; set; }
        public string provinceCode { get; set; }
        public string districtName { get; set; }
        public string districtCode { get; set; }
        public string subdistrictName { get; set; }
        public string subdistrcitCode { get; set; }
        public string houseNo { get; set; }

        public string inv_no { get; set; }
        public string dbn_no { get; set; }
        public string crn_no { get; set; }
    }
}
