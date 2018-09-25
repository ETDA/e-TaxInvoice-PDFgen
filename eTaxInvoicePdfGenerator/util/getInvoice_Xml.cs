using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;

namespace eTaxInvoicePdfGenerator.util
{
    public class getInvoice_Xml
    {
        Entity.InvoiceXmlObj XmlObj = new Entity.InvoiceXmlObj();
        DataTable buyer;
        DataTable seller;
        DataTable reference;
        DataTable item;
        string invoiceId;
        string absolutepath = "";
        string templatePath = "Resources\\template_debit.xml";
        string typeCode;
        string PurposeCode = "";
        

        public getInvoice_Xml(DataTable buyer, DataTable seller, DataTable reference, DataTable item, string invoiceId,string path)
        {
            Init(buyer, seller, reference, item, invoiceId,path);
        }

        public void Init(DataTable Buyer, DataTable Seller, DataTable Reference, DataTable Item, string InvoiceId,string path)
        {
            absolutepath = path;
            string name = Item.Rows[0]["invoice_name"].ToString();
            string cancleReason = Item.Rows[0]["purpose"].ToString();
            DateTime dateTime = new util.DateHelper().Convert2Date(Item.Rows[0]["issue_date"].ToString());

            if (!string.IsNullOrEmpty(cancleReason))
            {
                //PurposeCode = "TIVC99";
            }
            if (name == "ใบเพิ่มหนี้")
            {
                typeCode = "80";
                //PurposeCode = "DBNG99";

            }
            else if (name == "ใบลดหนี้")
            {
                typeCode = "81";
                //PurposeCode = "CDNG99";
            }
            else
            {
                typeCode = "388";
                templatePath = "Resources\\template.xml";
            }
            

            try
            {
                buyer = Buyer;
                seller = Seller;
                reference = Reference;
                item = Item;
                invoiceId = InvoiceId;

                XmlObj.invoiceId = InvoiceId;
                XmlObj.invoiceName = item.Rows[0]["invoice_name"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["invoice_name"].ToString());
                XmlObj.invoiceTypecode = typeCode ;
                XmlObj.invoiceIssue_date = dateTime.ToString("yyyy-MM-ddT00:00:00.000");
                XmlObj.invoiceName = item.Rows[0]["invoice_name"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["invoice_name"].ToString());
                XmlObj.invoiceCreate_date = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                XmlObj.sellerName = seller.Rows[0]["name"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["name"].ToString());
                XmlObj.sellerTaxid = seller.Rows[0]["tax_id"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["tax_id"].ToString()+seller.Rows[0]["branch_id"].ToString());
                XmlObj.sellerEmail = seller.Rows[0]["email"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["email"].ToString());
                XmlObj.sellerZipcode = seller.Rows[0]["zipcode"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["zipcode"].ToString());
                XmlObj.sellerAddress1 = seller.Rows[0]["address1"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["address1"].ToString());
                XmlObj.sellerAddress2 = seller.Rows[0]["address2"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["address2"].ToString());
                XmlObj.sellerCityname = seller.Rows[0]["district_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["district_code"].ToString());
                XmlObj.sellerCitySubName = seller.Rows[0]["subdistrict_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["subdistrict_code"].ToString());
                XmlObj.sellerCountry = seller.Rows[0]["country"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["country"].ToString());
                XmlObj.sellerCountrySubID = seller.Rows[0]["province_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["province_code"].ToString());
                string seller_phoneno = ReportUtils.getFullThaiMobilePhone(ReportUtils.replaceSpecialChar(seller.Rows[0]["phone_no"].ToString()), ReportUtils.replaceSpecialChar(seller.Rows[0]["phone_ext"].ToString()));
                XmlObj.sellercontactPersonPhoneno = seller.Rows[0]["phone_no"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller_phoneno);
                XmlObj.sellerBuildingName = seller.Rows[0]["house_no"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(seller.Rows[0]["house_no"].ToString());

                XmlObj.buyerName = buyer.Rows[0]["name"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["name"].ToString());
                XmlObj.buyerTaxid = buyer.Rows[0]["tax_id"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["tax_id"].ToString() + buyer.Rows[0]["branch_id"].ToString());
                XmlObj.buyerTaxType = buyer.Rows[0]["tax_type"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["tax_type"].ToString());
                XmlObj.buyereMail = buyer.Rows[0]["email"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["email"].ToString());
                XmlObj.buyerZipcode = buyer.Rows[0]["zipcode"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["zipcode"].ToString());
                XmlObj.buyerAddress1 = buyer.Rows[0]["address1"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["address1"].ToString());
                XmlObj.buyerAddress2 = buyer.Rows[0]["address2"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["address2"].ToString());
                XmlObj.buyerCityname = buyer.Rows[0]["district_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["district_code"].ToString());
                XmlObj.buyerCitySubName = buyer.Rows[0]["subdistrict_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["subdistrict_code"].ToString());
                XmlObj.buyerCountry = buyer.Rows[0]["country"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["country"].ToString());
                XmlObj.buyerCountrySubID = buyer.Rows[0]["province_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["province_code"].ToString());
                XmlObj.buyerContactPerson = buyer.Rows[0]["contact_person"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["contact_person"].ToString());

                string buyer_phoneno = ReportUtils.getFullThaiMobilePhone(ReportUtils.replaceSpecialChar(buyer.Rows[0]["phone_no"].ToString()), ReportUtils.replaceSpecialChar(buyer.Rows[0]["phone_ext"].ToString()));
                XmlObj.buyercontactPersonPhoneno = buyer.Rows[0]["phone_no"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer_phoneno);
                XmlObj.buyerBuildingName = buyer.Rows[0]["house_no"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(buyer.Rows[0]["house_no"].ToString());


                XmlObj.currency = "THB";
                XmlObj.invoiceTaxcode = item.Rows[0]["tax_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["tax_code"].ToString());
                XmlObj.invoiceTaxrate = item.Rows[0]["tax_rate"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["tax_rate"].ToString());
                XmlObj.invoiceChargeindicator = item.Rows[0]["charge_indicator"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["charge_indicator"].ToString());
                XmlObj.invoicePurpose = item.Rows[0]["purpose"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["purpose"].ToString());
                XmlObj.invoicePurposeCode = item.Rows[0]["purpose_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["purpose_code"].ToString());
                XmlObj.invoiceDiscount = item.Rows[0]["invoice_discount"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["invoice_discount"].ToString());
                XmlObj.invoiceService = item.Rows[0]["service_charge"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["service_charge"].ToString());
                XmlObj.invoiceLinetotal = item.Rows[0]["line_total"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["line_total"].ToString());
                XmlObj.invoiceTaxtotal = item.Rows[0]["tax_total"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["tax_total"].ToString());
                XmlObj.invoiceGrandtotal = item.Rows[0]["grand_total"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["grand_total"].ToString());
                XmlObj.invoiceBasisamount = item.Rows[0]["basis_amount"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["basis_amount"].ToString());
                XmlObj.invoiceOriginal = item.Rows[0]["original"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["original"].ToString());
                XmlObj.invoiceDifference = item.Rows[0]["difference"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["difference"].ToString());
                XmlObj.itemCode = item.Rows[0]["item_code"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["item_code"].ToString());
                XmlObj.itemCodeInter = item.Rows[0]["item_code_inter"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["item_code_inter"].ToString());
                XmlObj.remark = item.Rows[0]["remark"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["remark"].ToString());

                XmlObj.invoiceCalculatedAmount = item.Rows[0]["tax_total"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["tax_total"].ToString());
                XmlObj.invoiceTaxBasisTotalAmount = item.Rows[0]["basis_amount"].ToString() == null ? "" : ReportUtils.replaceSpecialChar(item.Rows[0]["basis_amount"].ToString());
                XmlObj.reason = "ส่วนลดจากราคาปกติ";
                XmlObj.reasonCode = "91";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string get_Xml()
        {
            try
            {
                string maintemplate = getMain_template();
                maintemplate = maintemplate.Replace("*item", getItem_template(item));
                maintemplate = maintemplate.Replace("*reference", getReference_template(reference));

                return maintemplate;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getMain_template()
        {
            try
            {
                string result = "";               

                using (StreamReader sr = new StreamReader(absolutepath+templatePath))
                {
                    var param = new Dictionary<string, string>
                {
                {"*invoice_id", XmlObj.invoiceId.ToString()},
                {"*invoice_name", XmlObj.invoiceName },
                {"*invoice_typecode",XmlObj.invoiceTypecode },
                {"*invoice_issue_date", XmlObj.invoiceIssue_date },
                {"*invoice_purpose",(string.IsNullOrWhiteSpace(XmlObj.invoicePurpose))? "":"<ram:Purpose>"+XmlObj.invoicePurpose+"</ram:Purpose >"},
                {"*invoice_Purpose_code",(string.IsNullOrWhiteSpace(XmlObj.invoicePurposeCode))? "":"<ram:PurposeCode>"+XmlObj.invoicePurposeCode+"</ram:PurposeCode >"},
                {"*invoice_create_date",XmlObj.invoiceCreate_date },
                {"*seller_name",XmlObj.sellerName },
                {"*seller_taxid",XmlObj.sellerTaxid  },
                {"*seller_DefinedCITradeContact",getCITradeContact(XmlObj.sellerEmail,XmlObj.sellercontactPersonPhoneno)},
                {"*seller_zipcode",XmlObj.sellerZipcode },
                {"*seller_address1",(string.IsNullOrWhiteSpace(XmlObj.sellerAddress1))? "":"<ram:LineOne>"+XmlObj.sellerAddress1+"</ram:LineOne>"},
                {"*seller_address2",(string.IsNullOrWhiteSpace(XmlObj.sellerAddress2))? "":"<ram:LineTwo>"+XmlObj.sellerAddress2+"</ram:LineTwo>"},
                { "*seller_cityname",(string.IsNullOrWhiteSpace(XmlObj.sellerCityname))? "":"<ram:CityName>"+XmlObj.sellerCityname+"</ram:CityName>"},
                {"*seller_city_subdivision_name",(string.IsNullOrWhiteSpace(XmlObj.sellerCitySubName))? "":"<ram:CitySubDivisionName>"+XmlObj.sellerCitySubName+"</ram:CitySubDivisionName>"},
                { "*seller_country",XmlObj.sellerCountry },
                {"*sellercountry_subdivision_id",(string.IsNullOrWhiteSpace(XmlObj.sellerCountrySubID))? "":"<ram:CountrySubDivisionID>"+XmlObj.sellerCountrySubID+"</ram:CountrySubDivisionID>"},
                {"*seller_building_name",(string.IsNullOrWhiteSpace(XmlObj.sellerBuildingName))? "":"<ram:BuildingNumber>"+XmlObj.sellerBuildingName+"</ram:BuildingNumber>" },

                { "*buyer_name",XmlObj.buyerName},
                {"*buyer_taxid",XmlObj.buyerTaxid },
                {"*buyer_taxtype",XmlObj.buyerTaxid },
                {"*buyer_DefinedCITradeContact",getCITradeContact(XmlObj.buyereMail,XmlObj.buyercontactPersonPhoneno)},
                {"*buyer_zipcode",XmlObj.buyerZipcode },
                {"*buyer_address1",(string.IsNullOrWhiteSpace(XmlObj.buyerAddress1))? "":"<ram:LineOne>"+XmlObj.buyerAddress1+"</ram:LineOne>"},
                {"*buyer_address2",(string.IsNullOrWhiteSpace(XmlObj.buyerAddress2))? "":"<ram:LineTwo>"+XmlObj.buyerAddress2+"</ram:LineTwo>"},
                { "*buyer_cityname",(string.IsNullOrWhiteSpace(XmlObj.buyerCityname))? "":"<ram:CityName>"+XmlObj.buyerCityname+"</ram:CityName>" },
                {"*buyer_city_subdivision_name",(string.IsNullOrWhiteSpace(XmlObj.buyerCitySubName))? "":"<ram:CitySubDivisionName>"+XmlObj.buyerCitySubName+"</ram:CitySubDivisionName>" },
                { "*buyer_country",XmlObj.buyerCountry  },
                {"*buyercountry_subdivision_id",(string.IsNullOrWhiteSpace(XmlObj.buyerCountrySubID))? "":"<ram:CountrySubDivisionID>"+XmlObj.buyerCountrySubID+"</ram:CountrySubDivisionID>" },
                { "*buyer_contact_person",(string.IsNullOrWhiteSpace(XmlObj.buyerContactPerson))? "":"<ram:PersonName>"+XmlObj.buyerContactPerson+"</ram:PersonName>" }, // DefinedCITradeContact is seller ? 
                {"*buyer_person_phoneno_contact",(string.IsNullOrWhiteSpace(XmlObj.buyercontactPersonPhoneno))? "":"<ram:CompleteNumber>"+XmlObj.buyercontactPersonPhoneno+"</ram:CompleteNumber>" },
                {"*buyer_building_name",(string.IsNullOrWhiteSpace(XmlObj.buyerBuildingName))? "":"<ram:BuildingNumber>"+XmlObj.buyerBuildingName+"</ram:BuildingNumber>" },
                { "*currency",XmlObj.currency },
                {"*invoice_tax_code",XmlObj.invoiceTaxcode  },
                {"*invoice_tax_rate",XmlObj.invoiceTaxrate},
                {"*invoice_basis_amount",XmlObj.invoiceBasisamount },
                {"*invoice_discountallowance",(string.IsNullOrWhiteSpace(XmlObj.invoiceDiscount) || XmlObj.invoiceDiscount == "0" )? "":getActualAmount_discount(XmlObj.invoiceDiscount,XmlObj.invoiceChargeindicator)},
                {"*invoice_serviceallowance",(string.IsNullOrWhiteSpace(XmlObj.invoiceService) || XmlObj.invoiceService == "0" )? "":getActualAmount_service(XmlObj.invoiceService)},
                {"*invoice_line_total",XmlObj.invoiceLinetotal},
                {"*invoice_tax_total",XmlObj.invoiceTaxtotal },
                {"*invoice_grand_total",XmlObj.invoiceGrandtotal },
                {"*invoice_original",(string.IsNullOrWhiteSpace(XmlObj.invoiceOriginal))? "":XmlObj.invoiceOriginal},
                {"*invoice_difference",(string.IsNullOrWhiteSpace(XmlObj.invoiceDifference))? "":XmlObj.invoiceDifference},
                {"*invoice_remark",(string.IsNullOrWhiteSpace(XmlObj.remark))? "":getRemark(XmlObj.remark)},
                {"*calculated_amount",(string.IsNullOrWhiteSpace(XmlObj.invoiceCalculatedAmount))? "":XmlObj.invoiceCalculatedAmount},
                {"*tax_basis_total_amount",(string.IsNullOrWhiteSpace(XmlObj.invoiceTaxBasisTotalAmount))? "":XmlObj.invoiceTaxBasisTotalAmount}

            };

                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    foreach (var row in param)
                    {
                        line = line.Replace(row.Key, row.Value);
                    }
                    result = line;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getItem_template(DataTable item)
        {
            try
            {
                string result = "";
                using (StreamReader sr = new StreamReader(absolutepath+"Resources\\item.xml"))
                {
                    int i = 1;
                    string Template = sr.ReadToEnd();
                    foreach (DataRow dr in item.Rows)
                    {
                        var param = new Dictionary<string, string>
                        {
                            {"*item_order",i.ToString()},
                            {"*item_price_per_unit",dr["price_per_unit"].ToString()},
                            {"*item_quantity",dr["quantity"].ToString() },
                            //{"*item_charge_indicator",dr["charge_indicator"].ToString() },
                            //{"*item_discount", dr["item_discount"].ToString()},
                            {"*item_total",dr["item_total"].ToString()},
                            {"*item_including_tax",dr["item_total_including_tax"].ToString()},
                            { "*item_global_id",(string.IsNullOrWhiteSpace(dr["item_code_inter"].ToString()))?"":"<ram:GlobalID schemeID=\"GTIN\" schemeAgencyID=\"GS1\">"+dr["item_code_inter"].ToString()+"</ram:GlobalID>"},
                            {"*item_id",(string.IsNullOrWhiteSpace(dr["item_code"].ToString()))?"":"<ram:ID>"+dr["item_code"].ToString()+"</ram:ID>"},
                            {"*item_name",(string.IsNullOrWhiteSpace(dr["item_name"].ToString()))?"":"<ram:Name>"+dr["item_name"].ToString()+"</ram:Name>"},
                            {"*item_unit_code",(string.IsNullOrWhiteSpace(dr["unit_xml"].ToString()))? "":"unitCode="+'"'+dr["unit_xml"].ToString()+'"'},
                            {"*trade_allowance",(string.IsNullOrWhiteSpace(dr["item_discount"].ToString()) || dr["item_discount"].ToString() == "0" )? "":getActualAmount_discount(dr["item_discount"].ToString(),dr["charge_indicator"].ToString())}
                            //{"*reason",(string.IsNullOrWhiteSpace(XmlObj.reason))? "":XmlObj.reason},
                            //{"*reason_code",(string.IsNullOrWhiteSpace(XmlObj.reasonCode))? "":XmlObj.reasonCode}
                        };

                        // Read the stream to a string, and write the string to the console.
                        String line = Template;
                        foreach (var row in param)
                        {
                            line = line.Replace(row.Key, row.Value);
                        }

                        result = result + line;
                        i++;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getActualAmount_discount(string ActualAmount,string charge_indicator)
        {
            string result = @"
<ram:SpecifiedTradeAllowanceCharge>
	<ram:ChargeIndicator>*item_charge_indicator</ram:ChargeIndicator>
	<ram:ActualAmount>*item_discount</ram:ActualAmount>
	<ram:ReasonCode>95</ram:ReasonCode>
	<ram:Reason>ส่วนลดจากราคาปกติ</ram:Reason>
</ram:SpecifiedTradeAllowanceCharge>";

            if (ActualAmount == "0")
            {
                result = "";
            }
            else
            {
                result = result.Replace("*item_charge_indicator", charge_indicator);
                result = result.Replace("*item_discount", ActualAmount);
            }

            return result;
        }

        private string getActualAmount_service(string ActualAmount)
        {
            string result = @"
<ram:SpecifiedTradeAllowanceCharge>
	<ram:ChargeIndicator>true</ram:ChargeIndicator>
	<ram:ActualAmount>*item_discount</ram:ActualAmount>
	<ram:ReasonCode>57</ram:ReasonCode>
	<ram:Reason>ค่าเบ็ดเตล็ด</ram:Reason>
</ram:SpecifiedTradeAllowanceCharge>";

            if (ActualAmount == "0")
            {
                result = "";
            }
            else
            {
                result = result.Replace("*item_discount", ActualAmount);
            }

            return result;
        }


        public string getReference_template(DataTable reference)
        {
            try
            {
                string result = "";
                using (StreamReader sr = new StreamReader(absolutepath+"Resources\\reference.xml"))
                {
                    string Template = sr.ReadToEnd();
                    foreach (DataRow dr in reference.Rows)
                    {
                        DateTime dt = Convert.ToDateTime(dr["document_date"].ToString());                        

                        var param = new Dictionary<string, string>
                                {
                                    {"*reference_document_id",dr["document_id"].ToString()},
                                    {"*reference_document_date",dt.ToString("yyyy-MM-ddT00:00:00.000") },
                                    {"*reference_document_type_code",dr["type_code"].ToString() }
                                };

                        // Read the stream to a string, and write the string to the console.
                        String line = Template;
                        foreach (var row in param)
                        {
                            line = line.Replace(row.Key, row.Value);
                        }
                        result = result + line;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string removeTag(string tagname)
        {
            string result = "";

            return result;
        }
        public string getRemark(string remark)
        {
            string result = "";
            string remark_template = @"<ram:IncludedNote>
        <ram:Subject>*remark_subject</ram:Subject >
        <ram:Content>*remark_note</ram:Content>
        </ram:IncludedNote>
                                 ";

            if (!string.IsNullOrWhiteSpace(remark))
            {
                remark_template = remark_template.Replace("*remark_subject", "หมายเหตุ");
                result = remark_template.Replace("*remark_note",remark);
            }


            return result;
        }

        public string getCITradeContact(string email,string phone)
        {
            string result = "";
            string contact_template = @"<ram:DefinedTradeContact>
            *email
            *phone
</ram:DefinedTradeContact>
                                 ";
            if (!string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
            {
                contact_template = contact_template.Replace("*email", "<ram:EmailURIUniversalCommunication> \n <ram:URIID>" + email + "</ram:URIID> \n </ram:EmailURIUniversalCommunication> \n");
                result = contact_template.Replace(" *phone", "");
            }
            else if(!string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
            {
                contact_template = contact_template.Replace("*phone", "<ram:TelephoneUniversalCommunication> \n <ram:CompleteNumber>" +phone+ "</ram:CompleteNumber> \n </ram:TelephoneUniversalCommunication> \n");
                result = contact_template.Replace(" *email", "");
            }
            else if(string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(email))
            {
                result = "";
            }
            else
            {
                contact_template = contact_template.Replace("*email", "<ram:EmailURIUniversalCommunication> \n <ram:URIID>" + email + "</ram:URIID> \n </ram:EmailURIUniversalCommunication> \n");
                contact_template = contact_template.Replace("*phone", "<ram:TelephoneUniversalCommunication> \n <ram:CompleteNumber>" + phone + "</ram:CompleteNumber> \n </ram:TelephoneUniversalCommunication> \n");
                result = contact_template;
            }

            result = result.Replace("\n", Environment.NewLine);
            return result;
        }


    }
}
