using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTaxInvoicePdfGenerator.Entity
{
    class InvoiceXmlObj
    {
        public string invoiceId { get; set; }
        public string invoiceName { get; set; }
        public string invoiceTypecode { get; set; }
        public string invoiceIssue_date { get; set; }
        public string invoicePurpose { get; set; }
        public string invoicePurposeCode { get; set; }
        public string invoiceCreate_date { get; set; }
        public string sellerName { get; set; }
        public string sellerTaxid { get; set; }
        public string sellerWebsite { get; set; }
        public string sellerEmail { get; set; }
        public string sellerZipcode { get; set; }
        public string sellerAddress1 { get; set; }
        public string sellerAddress2 { get; set; }
        public string sellerCityname { get; set; }
        public string sellerCitySubName { get; set; }        
        public string sellerCountry { get; set; }
        public string sellerCountrySubID { get; set; }
        public string sellercontactPersonPhoneno { get; set; }
        public string sellerBuildingName{ get; set; }
        public string buyerName { get; set; }
        public string buyerTaxid { get; set; }
        public string buyerTaxType { get; set; }        
        public string buyerWebsite { get; set; }
        public string buyereMail { get; set; }
        public string buyerZipcode { get; set; }
        public string buyerAddress1 { get; set; }
        public string buyerAddress2 { get; set; }
        public string buyerCityname { get; set; }
        public string buyerCitySubName { get; set; }
        public string buyerCountry { get; set; }
        public string buyerCountrySubID { get; set; }
        public string buyerContactPerson { get; set; }
        public string buyercontactPersonPhoneno { get; set; }
        public string buyerBuildingName { get; set; }
        public string currency { get; set; }
        public string invoiceTaxcode { get; set; }
        public string invoiceTaxrate { get; set; }
        public string invoiceBasisamount { get; set; }
        public string invoiceChargeindicator { get; set; }
        public string invoiceServiceindicator { get; set; }        
        public string invoiceDiscount { get; set; }
        public string invoiceService { get; set; }
        public string invoiceLinetotal { get; set; }
        public string invoiceTaxtotal { get; set; }
        public string invoiceGrandtotal { get; set; }
        public string invoiceOriginal { get; set; }
        public string invoiceDifference { get; set; }
        public string remark { get; set; }
        public string itemCode { get; set; }
        public string itemCodeInter { get; set; }
        public string reason { get; set; }
        public string reasonCode { get; set; }
        public string invoiceCalculatedAmount { get; set; }
        public string invoiceTaxBasisTotalAmount { get; set; }
    }

}
