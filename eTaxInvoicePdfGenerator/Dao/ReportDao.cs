using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SqliteConnector;
using System.Data.SQLite;

namespace eTaxInvoicePdfGenerator.Dao
{
    class ReportDao
    {
        DataTable dt_item_raw;
        private string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;        
        private Sqlite sqlite = new Sqlite("database.db"); 

        public ReportDao()
        {
            sqlite = new Sqlite(base_folder + "database.db");
        }

        public DataTable getReportData ( string invoiceid)
        {
            try
            {
                string id = invoiceid.ToString();
                string Query = @"
               select   item.item_name , item.quantity ,
               item.price_per_unit,item.unit,
                case when(item.discount isnull) then 0 else   item.discount end as item_discount , 
               item.item_total as item_total , 
                invoice.line_total as line_total ,
                invoice.discount as invoice_discount,
                invoice.basis_amount as basis_amount ,
                invoice.tax_total as tax_total,
                invoice.grand_total as grand_total ,
                invoice.invoice_name , invoice.purpose ,
                invoice.tax_code,
                invoice.tax_rate,
                 ""false"" as charge_indicator ,
                item.unit_xml ,
                item.item_code,
                item.item_code_inter,
                invoice.original,
                invoice.difference,
                invoice.issue_date,
                invoice.remark,
                case when(invoice.invoice_name like ""%ภาษี%"") then 'Y' else  'N' end as  invoice_debit_flag,
                case when(invoice.purpose = """") then 'Y' else  'N' end as purpose_flag,
                (item.item_total+(item.item_total * (tax_rate/100))) as item_total_including_tax
                from invoice_item item
                left
                join invoice invoice  on (invoice.invoice_id = item.invoice_id)
               where invoice.invoice_id = ""*id""
";
                #region backup qry
                /*string Query = @"
                select   item.item_name , item.quantity ,
                item.price_per_unit,item.unit,
                 case when(item.discount isnull) then 0 else   item.discount end as item_discount , 
                item.item_total as item_total , 
                 invoice.line_total as line_total ,
                 invoice.discount as invoice_discount,
                 invoice.basis_amount as basis_amount ,
                 invoice.tax_total as tax_total,
                 invoice.grand_total as grand_total ,
                 invoice.invoice_name , invoice.purpose ,
                 invoice.tax_code,
                 invoice.tax_rate,
                  ""false"" as charge_indicator ,
                 item.unit_xml ,
                 item.item_code,
                 item.item_code_inter,
                 invoice.original,
				 invoice.difference,
                 invoice.issue_date,
                 invoice.remark,
                 case when(invoice.invoice_name like ""%ภาษี%"") then 'Y' else  'N' end as  invoice_debit_flag,
			     case when(invoice.purpose = """") then 'Y' else  'N' end as purpose_flag,
                 (item.item_total+(item.item_total * (tax_rate/100))) as item_total_including_tax
                 from invoice_item item
                 left
                 join invoice invoice  on (invoice.invoice_id = item.invoice_id)
                where invoice.invoice_id = ""*id""
";*/
                #endregion
                Query = Query.Replace("*id", id);
                DataTable dt = new System.Data.DataTable();
                dt = sqlite.ExecuteDataTable(Query);
                dt_item_raw = dt.Copy();

                int limit = 19;
                int group = 0;
                int seq = 0;
                int total_seq = 0;
                dt.Columns.Add("seq");
                dt.Columns.Add("grouping");
                dt.Columns.Add("Appearance");

                DataRow[] Editrows = dt.Select();

                //Run Sequence
                for (int i = 0; i < Editrows.Length; i++)
                {
                    if (seq < limit)
                    {
                        Editrows[i]["grouping"] = group;
                        Editrows[i]["seq"] = seq;
                    }
                    else
                        seq = 0;

                    seq++;
                    if (seq == limit)
                    {
                        seq = 0; group++;
                    }
                    total_seq++;
                }
                dt.AcceptChanges();

                //Add Rows Dummy for full Table 
                if ((dt.Rows.Count % limit) != 0)
                {
                    DataRow[] Rows_count = dt.Select(" grouping =" + group);
                    while (total_seq < (group + 1) * limit)
                    {
                        dt.Rows.Add();
                        DataRow[] Addrow = dt.Select();
                        Addrow[total_seq]["grouping"] = group;
                        dt.AcceptChanges();
                        total_seq++;
                    }
                }
                // Assign Flag Y fro appearance
                var max = dt.Compute("max(grouping)", string.Empty);
                DataRow[] Appearance = dt.Select("grouping =" + max);

                foreach (var dr in Appearance)
                {
                    dr["Appearance"] = "Y";
                }
                dt.AcceptChanges();

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getDatatable(String query)
        {
            try
            {
                //sqlite = new Sqlite("");
                string Query = query;
                DataTable dt = new System.Data.DataTable();
                dt = sqlite.ExecuteDataTable(Query);
                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getDatatable_Item_Raw()
        {
            try
            {
                return dt_item_raw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool isReprint(string invoice_id)
        {

            bool flag = true;
            string Query = @"
                select  id as FLAG  from reference  where  invoice_id =  ""*id""
                ";
            Query = Query.Replace("*id", invoice_id);
            DataTable dt = new System.Data.DataTable();
            dt = sqlite.ExecuteDataTable(Query);
            
            return flag;
        }
        public string getSellerID(string invoice_id)
        {
            try
            {
                string id = "";
                string Query = @"
                select  seller_id  from invoice  where invoice_id = ""*id"";
                ";
                Query = Query.Replace("*id", invoice_id);
                DataTable dt = new System.Data.DataTable();
                dt = sqlite.ExecuteDataTable(Query);
                id = dt.Rows[0][0].ToString();
                return id;
            }
            catch(Exception ex) {
                throw ex;
            }
        }
        public string getBuyerID(string invoice_id)
        {
            try { 
            string id = "";
            string Query = @"
                    select  buyer_id  from invoice  where invoice_id =  ""*id"";
                    ";
            Query = Query.Replace("*id", invoice_id);
            DataTable dt = new System.Data.DataTable();
            dt = sqlite.ExecuteDataTable(Query);

            id = dt.Rows[0][0].ToString();
            return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
