using iTextSharp.text.xml.xmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECertificateAPI
{
    class ElectronicDocument : XmpSchema
    {
        static string XMLNS = "xmlns:ed=\"http://certificate.teda.th/schema/document/electronic_document#\"";

        static string DocumentFileName = "ed:DocumentFileName";
        static string DocumentVersion = "ed:DocumentVersion";
        static string DocumentReferenceID = "ed:DocumentReferenceID";
        static string DocumentOID = "ed:DocumentOID";

        public ElectronicDocument(string xmlns):base(ElectronicDocument.XMLNS)
        {
            ;
        }

        public static ElectronicDocument generateED(string documentFilename, string documentVersion, string documentReferenceID,
        string documentOID)
        {
            ElectronicDocument ed = new ElectronicDocument("ed");
            ed.AddProperty(DocumentFileName, documentFilename);
            ed.AddProperty(DocumentVersion, documentVersion);
            ed.AddProperty(DocumentReferenceID, documentReferenceID);
            ed.AddProperty(DocumentOID, documentOID);
            return ed;
        }
    }
}
