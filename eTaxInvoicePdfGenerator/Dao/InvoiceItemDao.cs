using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTaxInvoicePdfGenerator.Entity;
using System.Data.SQLite;
using SqliteConnector;

namespace eTaxInvoicePdfGenerator.Dao
{
    class InvoiceItemDao
    {
        private Sqlite sqlite;
        private string tableName = "invoice_item";
        public InvoiceItemDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            //sqlite = new Sqlite(base_folder + Properties.Resources.datasource);
            sqlite = new Sqlite(base_folder + "database.db");
        }

        internal void save(InvoiceItemObj obj)
        {
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                // insert
                txtQuery = string.Format("INSERT INTO {0} (invoice_id,number,price_per_unit,discount,quantity,unit,unit_xml,item_total,item_name,item_code,item_code_inter) VALUES ", this.tableName);
                string values = string.Format("(@invoice_id,@number,@price_per_unit,@discount,@quantity,@unit,@unit_xml,@item_total,@item_name,@item_code,@item_code_inter)");
                txtQuery = txtQuery + values;
            }
            else
            {
                //update 
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("invoice_id=@invoice_id,number=@number,price_per_unit=@price_per_unit,discount=@discount"
                    + ",quantity=@quantity,unit=@unit,unit_xml=@unit_xml,item_total=@item_total,item_name=@item_name,item_code=@item_code,item_code_inter=@item_code_inter ");
                string condition = string.Format("WHERE id=@id");
                txtQuery = txtQuery + values + condition;
            }
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@invoice_id", obj.invoiceId);
                    cmd.Parameters.AddWithValue("@number", obj.number);
                    cmd.Parameters.AddWithValue("@price_per_unit", obj.pricePerUnit);
                    cmd.Parameters.AddWithValue("@discount", obj.discountTotal);
                    cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                    cmd.Parameters.AddWithValue("@unit", obj.unit);
                    cmd.Parameters.AddWithValue("@unit_xml", obj.unti_xml);
                    cmd.Parameters.AddWithValue("@item_total", obj.itemTotal);
                    cmd.Parameters.AddWithValue("@item_name", obj.itemName);
                    cmd.Parameters.AddWithValue("@item_code", obj.itemCode);
                    cmd.Parameters.AddWithValue("@item_code_inter", obj.itemCodeInter);
                    if (obj.id != 0)
                    {
                        cmd.Parameters.AddWithValue("@id", obj.id);
                    }
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

        internal void remove(int id)
        {
            string txtQuery = string.Format("DELETE FROM {0} WHERE id = @id", this.tableName);
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void clear(string invoice_id)
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
