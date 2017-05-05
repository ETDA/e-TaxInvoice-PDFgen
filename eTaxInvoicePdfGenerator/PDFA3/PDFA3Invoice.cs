using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml.xmp;
using iTextSharp.xmp;
using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECertificateAPI
{
    class PDFA3Invoice
    {

        //static void Main(string[] args)
        //{
        //    PDFA3Invoice core = new PDFA3Invoice();
        //    String pdfFilePath = "in/test.pdf";
        //    String xmlFilePath = "in/ContentInformation.xml";
        //    String xmlFileName = "content.xml";
        //    String xmlVersion = "1.0";
        //    String documentID = "TIV0000012";
        //    String documentOID = "";
        //    String outputPath = "out/test.pdf";

        //    core.CreatePDFA3Invoice(pdfFilePath, xmlFilePath, xmlFileName, xmlVersion, 
        //        documentID, documentOID, outputPath);

        //    Console.ReadLine();
        //}

        public void CreatePDFA3Invoice(string pdfFilePath, string xmlFilePath, string xmlFileName,
            string xmlVersion, string documentID, string documentOID, string outputPath,string documentType)
        {
            // =========== Create PDF/A-3U Document =================
            // Create PDF/A-3U writer instance from existing document
            PdfReader reader = new PdfReader(pdfFilePath);
            MemoryStream stream = new MemoryStream();
            Document pdfAdocument = new Document();
            PdfAWriter writer = this.CreatePDFAInstance(pdfAdocument, reader, stream);

            // Create Output Intents
            ICC_Profile icc = ICC_Profile.GetInstance("Resources/sRGB Color Space Profile.icm");
            writer.SetOutputIntents("sRGB IEC61966-2.1", "", "http://www.color.org", "sRGB IEC61966-2.1", icc);

            PdfArray array = new PdfArray();
            writer.ExtraCatalog.Put(new PdfName("AF"), array);

            //============= Create Exchange Invoice =================
            // 1 add xml to document
            PdfFileSpecification contentSpec = this.EmbeddedAttachment(xmlFilePath, xmlFileName,
                    "text/xml", new PdfName("Alternative"), writer, "Tax Invoice XML Data");
            array.Add(contentSpec.Reference);

            //byte[] exchangeXMP = File.ReadAllBytes("Resources/EDocument_PDFAExtensionSchema.xml");
            string stringExchangeXMP = File.ReadAllText("Resources/EDocument_PDFAExtensionSchema.xml");
            byte[] exchangeXMP = Encoding.ASCII.GetBytes(stringExchangeXMP.Replace("@DocumentType", documentType)); 
            writer.XmpMetadata = exchangeXMP;

            //// 2 add Electronic Document XMP Metadata
            //ElectronicDocument ed = ElectronicDocument.generateED(xmlFileName, xmlVersion, documentID, documentOID);
            //XmpWriter xmpWriter = writer.XmpWriter;
            //xmpWriter.AddRdfDescription(ed);

            //IXmpMeta edPDFAextension = XmpMetaFactory.Parse(new FileStream("Resources/EDocument_PDFAExtensionSchema.xml", FileMode.Open));
            //IXmpMeta originalXMP = xmpWriter.XmpMeta;

            //XmpUtils.AppendProperties(edPDFAextension, originalXMP, true, true);

            pdfAdocument.Close();
            reader.Close();

            File.WriteAllBytes(outputPath, stream.ToArray());
        }


        public PdfAWriter CreatePDFAInstance(Document targetDocument, PdfReader originalDocument, Stream os)
        {
            PdfAWriter writer = PdfAWriter.GetInstance(targetDocument, os, PdfAConformanceLevel.PDF_A_3U);
            writer.CreateXmpMetadata();

            if (!targetDocument.IsOpen())
                targetDocument.Open();

            PdfContentByte cb = writer.DirectContent; // Holds the PDF data	
            PdfImportedPage page;
            int pageCount = originalDocument.NumberOfPages;
            for (int i = 0; i < pageCount; i++)
            {
                targetDocument.NewPage();
                page = writer.GetImportedPage(originalDocument, i + 1);
                cb.AddTemplate(page, 0, 0);
            }
            return writer;
        }
        public PdfFileSpecification EmbeddedAttachment(string filePath, string fileName, string mimeType,
            PdfName afRelationship, PdfAWriter writer, string description)
        {
            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.MODDATE, new PdfDate(File.GetLastWriteTime(filePath))); 
            PdfFileSpecification fileSpec = PdfFileSpecification.FileEmbedded(writer, filePath, fileName, null, mimeType,
                    parameters, 0);
            fileSpec.Put(new PdfName("AFRelationship"), afRelationship);
            writer.AddFileAttachment(description, fileSpec);
            return fileSpec;
        }
    }
}
