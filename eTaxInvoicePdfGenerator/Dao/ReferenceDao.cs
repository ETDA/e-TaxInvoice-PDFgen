using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTaxInvoicePdfGenerator.Entity;
using System.Data.SQLite;
using SqliteConnector;
using eTaxInvoicePdfGenerator.util;

namespace eTaxInvoicePdfGenerator.Dao
{
    class ReferenceDao
    {
        private Sqlite sqlite;
        private string tableName = "reference";
        public ReferenceDao()
        {
            DatabasePath dbPath = new DatabasePath();
            string base_folder = dbPath.CurrentDBFile(); 
            sqlite = new Sqlite(base_folder);
        }

        internal ReferenceObj select(int id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE id = @id", this.tableName);
            try
            {
                ReferenceObj data = new ReferenceObj();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.id = Convert.ToInt32(dr["id"]);
                                data.invoiceId = dr["invoice_id"].ToString();
                                data.documentId = dr["document_id"].ToString();
                                data.documentDate = dr["document_date"].ToString();
                                data.typeCode = dr["type_code"].ToString();
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

        internal ReferenceObj find(string invoice_id, string document_id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE invoice_id = @invoice_id and document_id = @document_id limit 1", this.tableName);
            try
            {
                ReferenceObj data = new ReferenceObj();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@invoice_id", invoice_id);
                        cmd.Parameters.AddWithValue("@document_id", document_id);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.id = Convert.ToInt32(dr["id"]);
                                data.invoiceId = dr["invoice_id"].ToString();
                                data.documentId = dr["document_id"].ToString();
                                data.documentDate = dr["document_date"].ToString();
                                data.typeCode = dr["type_code"].ToString();
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

        internal ReferenceObj find(string invoice_id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE invoice_id = @invoice_id order by id limit 1", this.tableName);
            try
            {
                ReferenceObj data = new ReferenceObj();
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
                                data.id = Convert.ToInt32(dr["id"]);
                                data.invoiceId = dr["invoice_id"].ToString();
                                data.documentId = dr["document_id"].ToString();
                                data.documentDate = dr["document_date"].ToString();
                                data.typeCode = dr["type_code"].ToString();
                            }else
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

        internal List<ReferenceObj> list(string invoice_id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE invoice_id = @invoice_id", this.tableName);
            try
            {
                List<ReferenceObj> items = new List<ReferenceObj>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@invoice_id", invoice_id);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ReferenceObj data = new ReferenceObj();
                                data.id = Convert.ToInt32(dr["id"]);
                                data.invoiceId = dr["invoice_id"].ToString();
                                data.documentId = dr["document_id"].ToString();
                                data.documentDate = dr["document_date"].ToString();
                                data.typeCode = dr["type_code"].ToString();
                                items.Add(data);
                            }
                        }
                    }
                }
                return items;
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

        internal void save(ReferenceObj obj)
        {
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                txtQuery = string.Format("INSERT INTO {0} (invoice_id,document_id,document_date,type_code) VALUES ", this.tableName);
                string values = string.Format("(@invoice_id,@document_id,@document_date,@type_code)");
                txtQuery = txtQuery + values;
            }
            else
            {
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("invoice_id=@invoice_id,document_id=@document_id,document_date=@document_date,type_code=@type_code ");
                string condition = string.Format("WHERE id=@id");
                txtQuery = txtQuery + values + condition;
            }
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@invoice_id", obj.invoiceId);
                    cmd.Parameters.AddWithValue("@document_id", obj.documentId);
                    if (obj.documentDate.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@document_date", obj.documentDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@document_date", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss", new System.Globalization.CultureInfo("th-TH")));
                    }
                    cmd.Parameters.AddWithValue("@type_code", obj.typeCode);
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
