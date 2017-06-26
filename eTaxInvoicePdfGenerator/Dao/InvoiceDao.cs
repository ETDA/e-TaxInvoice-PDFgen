using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqliteConnector;
using eTaxInvoicePdfGenerator.Entity;
using System.Data.SQLite;

namespace eTaxInvoicePdfGenerator.Dao
{
    class InvoiceDao
    {
        private Sqlite sqlite;
        private string tableName = "invoice";
        public InvoiceDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            //sqlite = new Sqlite(base_folder + Properties.Resources.datasource);
            sqlite = new Sqlite(base_folder + "database.db");
        }
        internal InvoiceObj select()
        {
            string txtQuery = string.Format("SELECT * FROM {0} LIMIT 1", this.tableName);
            try
            {
                InvoiceObj data = new InvoiceObj();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.invoiceId = dr["invoice_id"].ToString();
                                data.invoiceName = dr["invoice_name"].ToString();
                                data.purpose = dr["purpose"].ToString();
                                data.purposeCode = dr["purpose_code"].ToString();
                                data.sellerId = Convert.ToInt32(dr["seller_id"]);
                                data.buyerId = Convert.ToInt32(dr["buyer_id"]);
                                data.taxCode = dr["tax_code"].ToString();
                                data.taxRate = Convert.ToDouble(dr["tax_rate"]);
                                data.basisAmount = Convert.ToDouble(dr["basis_amount"]);
                                data.lineTotal = Convert.ToDouble(dr["line_total"]);
                                data.original = Convert.ToDouble(dr["original"]);
                                data.difference = Convert.ToDouble(dr["difference"]);
                                data.discount = Convert.ToDouble(dr["discount"]);
                                data.taxTotal = Convert.ToDouble(dr["tax_total"]);
                                data.grandTotal = Convert.ToDouble(dr["grand_total"]);
                                data.remark = dr["remark"].ToString();
                                data.discount_rate = Convert.ToDouble(dr["discount_rate"]);
                                data.service_charge = Convert.ToDouble(dr["service_charge"]);
                                data.service_charge_rate = Convert.ToDouble(dr["service_charge_rate"]);
                            }
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal InvoiceObj find(string invoice_id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE invoice_id = @invoice_id LIMIT 1", this.tableName);
            try
            {
                InvoiceObj data = new InvoiceObj();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@invoice_id", invoice_id);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.invoiceId = dr["invoice_id"].ToString();
                                data.invoiceName = dr["invoice_name"].ToString();
                                data.purpose = dr["purpose"].ToString();
                                data.purposeCode = dr["purpose_code"].ToString();
                                data.issueDate = dr["issue_date"].ToString();
                                data.sellerId = Convert.ToInt32(dr["seller_id"]);
                                data.buyerId = Convert.ToInt32(dr["buyer_id"]);
                                data.taxCode = dr["tax_code"].ToString();
                                data.taxRate = Convert.ToDouble(dr["tax_rate"]);
                                data.basisAmount = Convert.ToDouble(dr["basis_amount"]);
                                data.lineTotal = Convert.ToDouble(dr["line_total"]);
                                data.original = Convert.ToDouble(dr["original"]);
                                data.difference = Convert.ToDouble(dr["difference"]);
                                data.discount = Convert.ToDouble(dr["discount"]);
                                data.taxTotal = Convert.ToDouble(dr["tax_total"]);
                                data.grandTotal = Convert.ToDouble(dr["grand_total"]);
                                data.remark = dr["remark"].ToString();
                                data.discount_rate = Convert.ToDouble(dr["discount_rate"]);
                                data.service_charge = Convert.ToDouble(dr["service_charge"]);
                                data.service_charge_rate = Convert.ToDouble(dr["service_charge_rate"]);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal int count()
        {
            string txtQuery = string.Format("SELECT COUNT(*) FROM {0}", this.tableName);
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void save(InvoiceObj obj,bool insert)
        {
            string txtQuery = string.Empty;
            if (insert)
            {
                // insert
                txtQuery = string.Format("INSERT INTO {0} (invoice_id,invoice_name,purpose,purpose_code,issue_date,seller_id,buyer_id,tax_code,tax_rate,basis_amount,line_total"
                    + ",original,difference,discount,discount_rate,tax_total,grand_total,remark,service_charge,service_charge_rate) VALUES ", this.tableName);
                string values = string.Format("(@invoice_id,@invoice_name,@purpose,@purpose_code,@issue_date,@seller_id,@buyer_id,@tax_code,@tax_rate,@basis_amount,@line_total"
                    + ",@original,@difference,@discount,@discount_rate,@tax_total,@grand_total,@remark,@service_charge,@service_charge_rate)");
                txtQuery = txtQuery + values;
            }
            else
            {
                //update 
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("invoice_name=@invoice_name,purpose=@purpose,purpose_code=@purpose_code,issue_date=@issue_date,seller_id=@seller_id"
                    + ",buyer_id=@buyer_id,tax_code=@tax_code,tax_rate=@tax_rate,basis_amount=@basis_amount,line_total=@line_total"
                    + ",original=@original,difference=@difference,discount=@discount,discount_rate=@discount_rate,tax_total=@tax_total,grand_total=@grand_total,remark=@remark"
                    + ",service_charge=@service_charge,service_charge_rate=@service_charge_rate ");
                string condition = string.Format("WHERE invoice_id=@invoice_id");
                txtQuery = txtQuery + values + condition;
            }
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@invoice_id", obj.invoiceId);
                    cmd.Parameters.AddWithValue("@invoice_name", obj.invoiceName);
                    cmd.Parameters.AddWithValue("@purpose", obj.purpose);
                    cmd.Parameters.AddWithValue("@purpose_code", obj.purposeCode);
                    cmd.Parameters.AddWithValue("@issue_date", obj.issueDate);
                    cmd.Parameters.AddWithValue("@seller_id", obj.sellerId);
                    cmd.Parameters.AddWithValue("@buyer_id", obj.buyerId);
                    cmd.Parameters.AddWithValue("@tax_code", obj.taxCode);
                    cmd.Parameters.AddWithValue("@tax_rate", obj.taxRate);
                    cmd.Parameters.AddWithValue("@basis_amount", obj.basisAmount);
                    cmd.Parameters.AddWithValue("@line_total", obj.lineTotal);
                    cmd.Parameters.AddWithValue("@original", obj.original);
                    cmd.Parameters.AddWithValue("@difference", obj.difference);
                    cmd.Parameters.AddWithValue("@discount", obj.discount);
                    cmd.Parameters.AddWithValue("@discount_rate", obj.discount_rate);
                    cmd.Parameters.AddWithValue("@tax_total", obj.taxTotal);
                    cmd.Parameters.AddWithValue("@grand_total", obj.grandTotal);
                    cmd.Parameters.AddWithValue("@remark",obj.remark);
                    cmd.Parameters.AddWithValue("@service_charge", obj.service_charge);
                    cmd.Parameters.AddWithValue("@service_charge_rate", obj.service_charge_rate);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void remove(string invoice_id)
        {
            string txtQuery = string.Format("DELETE FROM {0} WHERE invoice_id = @invoice_id", this.tableName);
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@invoice_id", invoice_id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
