using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqliteConnector;
using eTaxInvoicePdfGenerator.Entity;
using System.Data.SQLite;
using eTaxInvoicePdfGenerator.util;

namespace eTaxInvoicePdfGenerator.Dao
{
    class RunningNumberDao
    {
        private Sqlite sqlite;
        private string tableName = "buyer";

        public RunningNumberDao()
        {
            DatabasePath dbPath = new DatabasePath();
            string base_folder = dbPath.CurrentDBFile(); //System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder);
        }

        public RunningNumberObj select(int id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE id = {1}", this.tableName, id);
            try
            {
                RunningNumberObj data = new RunningNumberObj();
                using (SQLiteDataReader dr = sqlite.ExecuteReader(txtQuery))
                {
                    if (dr.Read())
                    {
                        data.id = Convert.ToInt32(dr["id"]);
                        data.prefix = dr["prefix"].ToString();
                        data.number = dr["number"].ToString();
                    }
                }
                sqlite.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
