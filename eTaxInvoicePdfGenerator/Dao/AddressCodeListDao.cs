using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using SqliteConnector;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator.Dao
{
    class AddressCodeListDao
    {
        private Sqlite sqlite;
        private string tableName = "address_code_list";

        public AddressCodeListDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder + "database.db");
        }

        internal AddressCodeListObj select(string code)
        {
            while(code.Length < 8)
            {
                code = code + "0";
            }
            string txtQuery = string.Format("SELECT * FROM {0} WHERE Code = @code", this.tableName);
            try
            {
                AddressCodeListObj data = new AddressCodeListObj();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                data.code = dr["Code"].ToString();
                                data.changwat_th = dr["Changwat_TH"].ToString();
                                data.amphoe_th = dr["Amphoe_TH"].ToString();
                                data.tambon_th = dr["Tambon_TH"].ToString();
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

        internal List<AddressCodeListObj> ProvinceList()
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE Amphoe_th = '' and Tambon_th = '' order by Changwat_TH", this.tableName);
            try
            {
                List<AddressCodeListObj> items = new List<AddressCodeListObj>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            items.Add(new AddressCodeListObj("0","กรุณาเลือกจังหวัด","กรุณาเลือกอำเภอ","กรุณาเลือกตำบล"));
                            while (dr.Read())
                            {
                                AddressCodeListObj obj = new AddressCodeListObj();
                                obj.code = dr["Code"].ToString();
                                obj.changwat_th = dr["Changwat_TH"].ToString();
                                obj.amphoe_th = dr["Amphoe_TH"].ToString();
                                obj.tambon_th = dr["Tambon_TH"].ToString();
                                items.Add(obj);
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

        internal List<AddressCodeListObj> DistrictList(string code)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE (Code LIKE @code) and Amphoe_TH != '' and Tambon_TH = '' order by Amphoe_TH", this.tableName);
            try
            {
                List<AddressCodeListObj> items = new List<AddressCodeListObj>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@code", code + "%");
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            items.Add(new AddressCodeListObj("0", "กรุณาเลือกจังหวัด", "กรุณาเลือกอำเภอ", "กรุณาเลือกตำบล"));
                            while (dr.Read())
                            {
                                AddressCodeListObj obj = new AddressCodeListObj();
                                obj.code = dr["Code"].ToString();
                                obj.changwat_th = dr["Changwat_TH"].ToString();
                                obj.amphoe_th = dr["Amphoe_TH"].ToString();
                                obj.tambon_th = dr["Tambon_TH"].ToString();
                                items.Add(obj);
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

        internal List<AddressCodeListObj> SubDistrictList(string code)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE (Code LIKE @code) and Tambon_TH != '' order by Tambon_TH", this.tableName);
            try
            {
                List<AddressCodeListObj> items = new List<AddressCodeListObj>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        cmd.Parameters.AddWithValue("@code", code + "%");
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            items.Add(new AddressCodeListObj("0", "กรุณาเลือกจังหวัด", "กรุณาเลือกอำเภอ", "กรุณาเลือกตำบล"));
                            while (dr.Read())
                            {
                                AddressCodeListObj obj = new AddressCodeListObj();
                                obj.code = dr["Code"].ToString();
                                obj.changwat_th = dr["Changwat_TH"].ToString();
                                obj.amphoe_th = dr["Amphoe_TH"].ToString();
                                obj.tambon_th = dr["Tambon_TH"].ToString();
                                items.Add(obj);
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
    }
}
