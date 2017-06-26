namespace eTaxInvoicePdfGenerator.Entity
{
    public class ReferenceObj
    {
        public int id { get; set; }
        public int number { get; set; }
        public string invoiceId { get; set; }
        public string documentId { get; set; }
        public string documentDate { get; set; }
        public TypeCodeObj typeCodeObj { get; set; }
        public string typeCode { get; set; }

        public ReferenceObj()
        {

        }

        public ReferenceObj(int number)
        {
            this.number = number;
        }


        public ReferenceObj(int number, string invoiceId, string documentId, string documentDate, string typeCode, TypeCodeObj typeCodeObj)
        {
            this.number = number;
            this.invoiceId = invoiceId;
            this.documentId = documentId;
            this.documentDate = documentDate;
            this.typeCode = typeCode;
            if (typeCodeObj == null)
            {
                this.typeCode = typeCode;
                this.typeCodeObj = new TypeCodeObj("ZZZ", typeCode);
            }
            else
            {
                this.typeCode = typeCodeObj.code;
                this.typeCodeObj = typeCodeObj;
            }
        }

        public bool Equals(ReferenceObj obj)
        {
            if(this.number != obj.number)
            {
                return false;
            }
            if(this.documentId != obj.documentId)
            {
                return false;
            }
            if(this.documentDate != obj.documentDate)
            {
                return false;
            }
            if(this.typeCode != obj.typeCode)
            {
                return false;
            }
            if (this.typeCodeObj.code != obj.typeCodeObj.code)
            {
                return false;
            }
            if (this.typeCodeObj.description != obj.typeCodeObj.description)
            {
                return false;
            }
            return true;
        }
    }
}
