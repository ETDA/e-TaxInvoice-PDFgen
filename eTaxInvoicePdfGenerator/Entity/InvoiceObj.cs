namespace eTaxInvoicePdfGenerator.Entity
{
    class InvoiceObj
    {
        public string invoiceId { get; set; }
        public string invoiceName { get; set; }
        public string issueDate { get; set; }
        public string purpose { get; set; }
        public string purposeCode { get; set; }
        public int sellerId { get; set; }
        public int buyerId { get; set; }
        public string taxCode { get; set; }
        public double taxRate { get; set; }
        public double basisAmount { get; set; }
        public double lineTotal { get; set; }
        public double original { get; set; }
        public double difference { get; set; }
        public double discount_rate { get; set; }
        public double discount { get; set; }
        public double taxTotal { get; set; }
        public double grandTotal { get; set; }
        public string remark { get; set; }
        public double service_charge { get; set; }
        public double service_charge_rate { get; set; }
    }
}
