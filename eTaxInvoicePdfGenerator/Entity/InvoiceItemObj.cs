namespace eTaxInvoicePdfGenerator.Entity
{
    public class InvoiceItemObj
    {
        public int id { get; set; }
        public string invoiceId { get; set; }
        public int number { get; set; }
        public double pricePerUnit { get; set; }
        public string pricePerUnitText { get; set; }
        public double discount { get; set; }
        public double discountTotal { get; set; }
        public int quantity { get; set; }
        public string quantityText { get; set; }
        public string unit { get; set; }
        public string unitXml { get; set; }
        public double itemTotal { get; set; }
        public string itemTotalText { get; set; }
        public string itemName { get; set; }
        public bool is_service { get; set; }
        public bool isSelected { get; set; }
        public string itemCode { get; set; }
        public string itemCodeInter { get; set; }

        public string discountText { get; set; }
    }
}
