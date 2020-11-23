using System;
using System.Collections.Generic;
using SqliteConnector;
using System.Data.SQLite;
using eTaxInvoicePdfGenerator.Entity;
using eTaxInvoicePdfGenerator.util;

namespace eTaxInvoicePdfGenerator.Dao
{
    class CodeListDao
    {
        private Sqlite sqlite;
        private string tableName = "code_list_unit";

        public CodeListDao()
        {
            DatabasePath dbPath = new DatabasePath();
            string base_folder = dbPath.CurrentDBFile(); 
            sqlite = new Sqlite(base_folder);
        }

        internal CodeList select(int id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE id = @id", this.tableName);
            try
            {
                CodeList data = new CodeList();
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
                                data.code = dr["code"].ToString(); ;
                                data.description = dr["description"].ToString();
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

        internal bool exist(CodeList codeList)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE code = @code and description = @description", this.tableName);
            try
            {
                CodeList data = new CodeList();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@code", codeList.code);
                        cmd.Parameters.AddWithValue("@description", codeList.description);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                return true;
                            }else
                            {
                                return false;
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal CodeList select(string code)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE code = '{1}'", this.tableName, code);
            try
            {
                CodeList data = new CodeList();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.id = Convert.ToInt32(dr["id"]);
                                data.code = dr["code"].ToString(); ;
                                data.description = dr["description"].ToString();
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

        internal List<CodeList> list()
        {
            string txtQuery = string.Format("SELECT id,code,description FROM {0}", this.tableName);
            try
            {
                List<CodeList> items = new List<CodeList>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                items.Add(new CodeList((int)dr.GetInt64(0), dr.GetString(1), dr.GetString(2)));
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

        internal void save(CodeList obj)
        {
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                txtQuery = string.Format("INSERT INTO {0} (code,description) VALUES ", this.tableName);
                string values = string.Format("(@code,@description)");
                txtQuery = txtQuery + values;
            }
            else
            {
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("code=@code,description=@description ");
                string condition = string.Format("WHERE id=@id");
                txtQuery = txtQuery + values + condition;
            }
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@code", obj.code);
                    cmd.Parameters.AddWithValue("@description", obj.description);
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
    }
}
