using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Reporting.WinForms;

namespace eTaxInvoicePdfGenerator.Report
{
    public class InvoiceGenerator : IDisposable
    {
        byte[] invoicePdf;
        byte[] invoiceXmlbyte;
        string invoiceXmlstring;

        /*LOCAL RDLC*/
        //const string absolutepath = "..\\..\\";

        /*DEPLOY RDLC*/
        const string absolutepath = "";
        void IDisposable.Dispose()
        {

        }

        public InvoiceGenerator()
        {

        }
        public void create(string invoice_id)
        {
            InvoiceGen(invoice_id);
        }
        public byte[] getByteXml() { return invoiceXmlbyte; }
        public byte[] getBytePdf() { return invoicePdf; }
        public string getStringXml() { return invoiceXmlstring; }

        private void InvoiceGen(string invoice_id)
        {
            try{
                Dao.ReportDao repDAO = new Dao.ReportDao();

                DataTable buyer = repDAO.getDatatable("select * from contact  where id = " + repDAO.getBuyerID(invoice_id));
                DataTable seller = repDAO.getDatatable("select * from contact where id = " + repDAO.getSellerID(invoice_id));
                
                DataTable reference = repDAO.getDatatable("select * from reference where invoice_id = '" + invoice_id + "'");
                DataTable dt = repDAO.getReportData(invoice_id);
                DataTable item = repDAO.getDatatable_Item_Raw();
                String date = util.ReportUtils.getThaiDate(dt.Rows[0]["issue_date"].ToString());
                String taxType = buyer.Rows[0]["tax_type"].ToString();
                string reftext1 = "";
                string reftext2 = "";
                string reftext3 = "";               

                if (reference.Rows.Count > 0) // yes is reprint , !repDAO.isReprint(invoice_id)
                {
                    //Initial Report Parameter
                    util.ReportUtils utils = new util.ReportUtils();

                    if (dt.Rows[0]["invoice_name"].ToString() == "ใบเพิ่มหนี้")
                    {
                         reftext1 = "สาเหตุการออกใบเพิ่มหนี้";
                         reftext2 = "เลขที่ใบกำกับภาษีอ้างถึง";
                         reftext3 = "วันที่ของใบกำกับภาษีอ้างถึง";
                    }
                    else if (dt.Rows[0]["invoice_name"].ToString() == "ใบลดหนี้")
                    {
                         reftext1 = "สาเหตุการออกใบลดหนี้";
                         reftext2 = "เลขที่ใบกำกับภาษีอ้างถึง";
                         reftext3 = "วันที่ของใบกำกับภาษีอ้างถึง";
                    }
                    else
                    {
                         reftext1 = "สาเหตุในการยกเลิกใบกำกับภาษีเดิม"; 
                        reftext2 = "เลขที่ใบกำกับภาษีเดิม";
                         reftext3 = "วันที่ของใบกำกับภาษีเดิม";
                    }

                    //Assign Report Parameter
                    ReportParameter docNo = new ReportParameter("docNo", invoice_id.ToString());
                    ReportParameter reportDate = new ReportParameter("date", date);

                    ReportParameter sell_name = new ReportParameter("sell_name", seller.Rows[0]["name"].ToString());
                    ReportParameter sell_taxno = new ReportParameter("sell_taxno", seller.Rows[0]["tax_id"].ToString());
                    ReportParameter sell_comno = new ReportParameter("sell_comno", utils.getBranch(seller.Rows[0]["branch_id"].ToString(),"TXID"));
                    ReportParameter sell_email = new ReportParameter("sell_email", seller.Rows[0]["email"].ToString());
                    ReportParameter sell_tellno = new ReportParameter("sell_tellno", util.ReportUtils.getFullThaiMobilePhone(seller.Rows[0]["phone_no"].ToString(), seller.Rows[0]["phone_ext"].ToString()));

                    ReportParameter sell_add1 = new ReportParameter("sell_add1", seller.Rows[0]["address1"].ToString());
                    ReportParameter sell_zipcode = new ReportParameter("sell_zipcode", seller.Rows[0]["zipcode"].ToString());
                    ReportParameter sell_district = new ReportParameter("sell_district", seller.Rows[0]["district_name"].ToString());
                    ReportParameter sell_subdistrict = new ReportParameter("sell_subdistrict", seller.Rows[0]["subdistrict_name"].ToString());
                    ReportParameter sell_province = new ReportParameter("sell_province", seller.Rows[0]["province_name"].ToString());
                    ReportParameter sell_house_no = new ReportParameter("sell_house_no", seller.Rows[0]["house_no"].ToString());

                    ReportParameter buy_name = new ReportParameter("buy_name", buyer.Rows[0]["name"].ToString());
                    ReportParameter buy_taxno = new ReportParameter("buy_taxno", buyer.Rows[0]["tax_id"].ToString());
                    ReportParameter buy_comno = new ReportParameter("buy_comno", utils.getBranch(buyer.Rows[0]["branch_id"].ToString(),taxType));
                    ReportParameter buy_email = new ReportParameter("buy_email", buyer.Rows[0]["email"].ToString());
                    ReportParameter buy_tellno = new ReportParameter("buy_tellno", util.ReportUtils.getFullThaiMobilePhone(buyer.Rows[0]["phone_no"].ToString(), buyer.Rows[0]["phone_ext"].ToString()));
                    ReportParameter buy_refer = new ReportParameter("buy_refer", buyer.Rows[0]["contact_person"].ToString());

                    ReportParameter buy_add1 = new ReportParameter("buy_add1", buyer.Rows[0]["address1"].ToString());
                    ReportParameter buy_zipcode = new ReportParameter("buy_zipcode", buyer.Rows[0]["zipcode"].ToString());
                    ReportParameter buy_district = new ReportParameter("buy_district", buyer.Rows[0]["district_name"].ToString());
                    ReportParameter buy_subdistrict = new ReportParameter("buy_subdistrict", buyer.Rows[0]["subdistrict_name"].ToString());
                    ReportParameter buy_province = new ReportParameter("buy_province", buyer.Rows[0]["province_name"].ToString());
                    ReportParameter buy_house_no = new ReportParameter("buy_house_no", buyer.Rows[0]["house_no"].ToString());

                    ReportParameter testflag = new ReportParameter("testflag", "Y");
                    ReportParameter referText1 = new ReportParameter("referText1", reftext1);
                    ReportParameter referText2 = new ReportParameter("referText2", reftext2);
                    ReportParameter referText3 = new ReportParameter("referText3", reftext3);
                    ReportParameter grand_totalthai = new ReportParameter("grand_totalthai", "( " + util.ReportUtils.getFullThaiBathController(dt.Rows[0]["grand_total"].ToString()) + " )");
                    ReportParameter buy_taxschemeflag = new ReportParameter("buy_taxschemeflag", getSchemeID(taxType));

                    /*FOR REPRINT*/
                    var Ref = util.ReportUtils.getReference(reference);
                    ReportParameter ref_docno = new ReportParameter("ref_docno", Ref[0]);
                    ReportParameter ref_docdate = new ReportParameter("ref_docdate", Ref[1]);

                    ReportDataSource dataSource = new ReportDataSource();

                    dataSource.Name = "Tax";
                    dataSource.Value = dt;

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.DataSources.Add(dataSource);
                        reportViewer.LocalReport.ReportPath = absolutepath+"Report\\tax_multipage_return.rdlc";
                        reportViewer.LocalReport.ReportEmbeddedResource = "eTaxInvoicePdfGenerator.Report.tax_multipage_return.rdlc";                    
                    reportViewer.LocalReport.SetParameters(new ReportParameter[]
                        {
                        docNo,reportDate,
                        sell_name,sell_taxno,sell_comno,sell_email,sell_tellno,
                        buy_name,buy_taxno,buy_comno,buy_email,buy_tellno,buy_refer
                        ,testflag,grand_totalthai,ref_docno,ref_docdate
                        ,sell_add1 ,sell_zipcode,buy_add1,buy_zipcode
                        ,referText1,referText2,referText3,sell_district,sell_subdistrict,sell_province,buy_district,buy_subdistrict,buy_province,sell_house_no,buy_house_no,buy_taxschemeflag
                         });

                    invoicePdf = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    util.getInvoice_Xml invXml = new util.getInvoice_Xml(buyer, seller, reference, item, invoice_id, absolutepath);
                    invoiceXmlstring = invXml.get_Xml();
                    invoiceXmlbyte = Encoding.ASCII.GetBytes(invoiceXmlstring);

                    //System.IO.File.WriteAllBytes("test.pdf", invoicePdf);

                    //Response.Buffer = true;
                    //Response.Clear();
                    //Response.ContentType = mimeType;
                    //Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
                    //Response.BinaryWrite(bytes); // create the file
                    //Response.Flush(); // send it to the client to download

                    /*string[] netpath = path.Split('\\');
                    string newpath = netpath[0] + "\\" + netpath[1] + "\\" + netpath[2] + "\\" + netpath[3] + "\\" + "Export.xml";
                    System.IO.File.WriteAllText(newpath, XML);*/

                    //_reportViewer.RefreshReport();
                    //_isReportViewerLoaded = true;
                }
                else
                {
                    //Initial Report Parameter
                    util.ReportUtils utils = new util.ReportUtils();

                    //Assign Report Parameter
                    ReportParameter docNo = new ReportParameter("docNo", invoice_id.ToString());
                    ReportParameter reportDate = new ReportParameter("date", date);

                    ReportParameter sell_name = new ReportParameter("sell_name", seller.Rows[0]["name"].ToString());
                    ReportParameter sell_taxno = new ReportParameter("sell_taxno", seller.Rows[0]["tax_id"].ToString());
                    ReportParameter sell_comno = new ReportParameter("sell_comno", utils.getBranch(seller.Rows[0]["branch_id"].ToString(),"TXID"));//
                    ReportParameter sell_email = new ReportParameter("sell_email", seller.Rows[0]["email"].ToString());
                    ReportParameter sell_tellno = new ReportParameter("sell_tellno", util.ReportUtils.getFullThaiMobilePhone(seller.Rows[0]["phone_no"].ToString(), seller.Rows[0]["phone_ext"].ToString()));

                    ReportParameter sell_add1 = new ReportParameter("sell_add1", seller.Rows[0]["address1"].ToString());
                    ReportParameter sell_zipcode = new ReportParameter("sell_zipcode", seller.Rows[0]["zipcode"].ToString());
                    ReportParameter sell_district = new ReportParameter("sell_district", seller.Rows[0]["district_name"].ToString());
                    ReportParameter sell_subdistrict = new ReportParameter("sell_subdistrict", seller.Rows[0]["subdistrict_name"].ToString());
                    ReportParameter sell_province = new ReportParameter("sell_province", seller.Rows[0]["province_name"].ToString());
                    ReportParameter sell_house_no = new ReportParameter("sell_house_no", seller.Rows[0]["house_no"].ToString());

                    ReportParameter buy_name = new ReportParameter("buy_name", buyer.Rows[0]["name"].ToString());
                    ReportParameter buy_taxno = new ReportParameter("buy_taxno", buyer.Rows[0]["tax_id"].ToString());
                    ReportParameter buy_comno = new ReportParameter("buy_comno", utils.getBranch(buyer.Rows[0]["branch_id"].ToString(),taxType));
                    ReportParameter buy_email = new ReportParameter("buy_email", buyer.Rows[0]["email"].ToString());
                    ReportParameter buy_tellno = new ReportParameter("buy_tellno", util.ReportUtils.getFullThaiMobilePhone(buyer.Rows[0]["phone_no"].ToString(), buyer.Rows[0]["phone_ext"].ToString()));
                    ReportParameter buy_refer = new ReportParameter("buy_refer", buyer.Rows[0]["contact_person"].ToString());
                    ReportParameter buy_house_no = new ReportParameter("buy_house_no", buyer.Rows[0]["house_no"].ToString());


                    ReportParameter buy_add1 = new ReportParameter("buy_add1", buyer.Rows[0]["address1"].ToString());
                    ReportParameter buy_zipcode = new ReportParameter("buy_zipcode", buyer.Rows[0]["zipcode"].ToString());
                    ReportParameter buy_district = new ReportParameter("buy_district", buyer.Rows[0]["district_name"].ToString());
                    ReportParameter buy_subdistrict = new ReportParameter("buy_subdistrict", buyer.Rows[0]["subdistrict_name"].ToString());
                    ReportParameter buy_province = new ReportParameter("buy_province", buyer.Rows[0]["province_name"].ToString());



                    ReportParameter testflag = new ReportParameter("testflag", "Y");

                    ReportParameter grand_totalthai = new ReportParameter("grand_totalthai", "( " + util.ReportUtils.getFullThaiBathController(dt.Rows[0]["grand_total"].ToString()) + " )");
                    ReportParameter buy_taxschemeflag = new ReportParameter("buy_taxschemeflag", getSchemeID(taxType));

                    ReportDataSource dataSource = new ReportDataSource();

                    dataSource.Name = "Tax";
                    dataSource.Value = dt;

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    ReportViewer reportViewer = new ReportViewer();

                    reportViewer.LocalReport.DataSources.Add(dataSource);

                        reportViewer.LocalReport.ReportPath = absolutepath+"Report\\tax_multipage.rdlc";
                        reportViewer.LocalReport.ReportEmbeddedResource = "eTaxInvoicePdfGenerator.Report.tax_multipage.rdlc";

                    reportViewer.LocalReport.SetParameters(new ReportParameter[]
                        {
                        docNo,reportDate,
                        sell_name,sell_taxno,sell_comno,sell_email,sell_tellno,
                        buy_name,buy_taxno,buy_comno,buy_email,buy_tellno,buy_refer
                        ,testflag,grand_totalthai  ,sell_add1,sell_zipcode,buy_add1 ,buy_zipcode,sell_district,sell_subdistrict,sell_province,buy_district,buy_subdistrict,buy_province,buy_house_no,sell_house_no,buy_taxschemeflag
                        });

                    reportViewer.LocalReport.DisplayName = "Hello";
                    invoicePdf = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    //System.IO.File.WriteAllBytes("test.pdf", invoicePdf);
                    util.getInvoice_Xml invXml = new util.getInvoice_Xml(buyer, seller, reference, item, invoice_id, absolutepath);
                    invoiceXmlstring = invXml.get_Xml();
                    invoiceXmlbyte = Encoding.ASCII.GetBytes(invoiceXmlstring);

                }
            }
            catch(Exception ex)
            {            
                System.Windows.MessageBox.Show(ex.Message);
                System.Windows.MessageBox.Show(ex.InnerException.Message);
                System.Windows.MessageBox.Show(ex.InnerException.StackTrace);
            }
        }

        private string getSchemeID(string schemeID)
        {
            string returnValue = "";

            if(schemeID == "TXID")
            {
                returnValue = "สำนักงานใหญ่/เลขที่สาขา";
            }
            return returnValue;
        }

    }
}

