﻿namespace eTaxInvoicePdfGenerator.Entity
{
    class ContactObj
    {
        public int id { get; set; }
        public string name { get; set; }
        public string taxId { get; set; }
        public string taxType { get; set; }
        public string branchId { get; set; }
        //public string website { get; set; }
        public string email { get; set; }
        public string zipCode { get; set; }
        public string address1 { get; set; }
        //public string address2 { get; set; }
        public string country { get; set; }
        public string contactPerson { get; set; }
        public string phoneNo { get; set; }
        public string phoneExt { get; set; }
        //public string faxNo { get; set; }
        //public string faxExt { get; set; }

        public string provinceName { get; set; }
        public string provinceCode { get; set; }
        public string districtName { get; set; }
        public string districtCode { get; set; }
        public string subdistrictName { get; set; }
        public string subdistrcitCode { get; set; }
        public string houseNo { get; set; }
    }
}
