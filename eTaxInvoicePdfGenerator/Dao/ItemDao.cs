using System;
using System.Collections.Generic;
using SqliteConnector;
using System.Data.SQLite;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator.Dao
{
    class ItemDao
    {
        private Sqlite sqlite;
        private string tableName = "item";
        public ItemDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder + "database.db");
        }

        internal ItemObj select(int id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE id = @id", this.tableName);
            try
            {
                ItemObj data = new ItemObj();
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
                                data.name = dr["name"].ToString();
                                data.detail = dr["detail"].ToString();
                                data.pricePerUnit = Convert.ToDouble(dr["price_per_unit"]);
                                data.unit = dr["unit"].ToString();
                                data.unitXml = dr["unit_xml"].ToString();
                                data.isService = Convert.ToBoolean(dr["is_service"]);
                                data.itemCode = dr["item_code"].ToString();
                                data.itemCodeInter = dr["item_code_inter"].ToString();
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

        internal List<ItemList> listView(int page, int pageSize)
        {
            string txtQuery = string.Format("SELECT id,name,detail,price_per_unit FROM {0} LIMIT {1} offset {2}", this.tableName, pageSize, (page - 1) * pageSize);
            try
            {
                List<ItemList> items = new List<ItemList>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                items.Add(new ItemList((int)dr.GetInt64(0), dr.GetString(1), dr.GetString(2), dr.GetDouble(3).ToString("N")));
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

        internal List<ItemList> listView(int page, int pageSize,string sortingColumn, string sortingDirection)
        {
            string txtQuery = string.Format("SELECT id,name,detail,price_per_unit FROM {0} ORDER BY {3} {4} LIMIT {1} offset {2} ", this.tableName, pageSize, (page - 1) * pageSize, sortingColumn,sortingDirection);
            try
            {
                List<ItemList> items = new List<ItemList>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                items.Add(new ItemList((int)dr.GetInt64(0), dr.GetString(1), dr.GetString(2), dr.GetDouble(3).ToString("N")));
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

        internal List<ItemObj> list()
        {
            string txtQuery = string.Format("SELECT * FROM {0}", this.tableName);
            try
            {
                List<ItemObj> items = new List<ItemObj>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemObj data = new ItemObj();
                                data.id = Convert.ToInt32(dr["id"]);
                                data.name = dr["name"].ToString();
                                data.detail = dr["detail"].ToString();
                                data.pricePerUnit = Convert.ToDouble(dr["price_per_unit"].ToString());
                                data.unit = dr["unit"].ToString();
                                data.unitXml = dr["unit_xml"].ToString();
                                data.isService = Convert.ToBoolean(dr["is_service"]);
                                data.itemCode = dr["item_code"].ToString();
                                data.itemCodeInter = dr["item_code_inter"].ToString();
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
                return Convert.ToInt16(sqlite.ExecuteScalar(txtQuery));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void save(ItemObj obj)
        {
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                txtQuery = string.Format("INSERT INTO {0} (name,detail,price_per_unit,unit,unit_xml,is_service,item_code,item_code_inter) VALUES ", this.tableName);
                string values = string.Format("(@name,@detail,@price_per_unit,@unit,@unit_xml,@is_service,@item_code,@item_code_inter)");
                txtQuery = txtQuery + values;
            }
            else
            {
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("name=@name,detail=@detail,price_per_unit=@price_per_unit,unit=@unit,unit_xml=@unit_xml,is_service=@is_service,item_code=@item_code,item_code_inter=@item_code_inter ");
                string condition = string.Format("WHERE id=@id");
                txtQuery = txtQuery + values + condition;
            }
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@name", obj.name);
                    cmd.Parameters.AddWithValue("@detail", obj.detail);
                    cmd.Parameters.AddWithValue("@price_per_unit", obj.pricePerUnit);
                    cmd.Parameters.AddWithValue("@unit", obj.unit);
                    cmd.Parameters.AddWithValue("@unit_xml", obj.unitXml);
                    cmd.Parameters.AddWithValue("@is_service", (obj.isService) ? 1 : 0);
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

    }
}
