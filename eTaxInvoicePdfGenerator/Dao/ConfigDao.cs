using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTaxInvoicePdfGenerator.Entity;
using System.Data.SQLite;
using SqliteConnector;

namespace eTaxInvoicePdfGenerator.Dao
{
    class ConfigDao
    {
        private Sqlite sqlite;
        private string tableName = "config";
        public ConfigDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder + "database.db");
        }

        internal ConfigObj select(string key)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE key = @key", this.tableName);
            try
            {
                ConfigObj data = new ConfigObj();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@key", key);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.id = Convert.ToInt32(dr["id"]);
                                data.key = dr["key"].ToString();
                                data.value = dr["value"].ToString();
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

        internal void save(ConfigObj obj)
        {
            string txtQuery = string.Empty;
            txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
            string values = string.Format("value=@value ");
            string condition = string.Format("WHERE id=@id");
            txtQuery = txtQuery + values + condition;
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@id", obj.id);
                    cmd.Parameters.AddWithValue("@value", obj.value);
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
    }
}
