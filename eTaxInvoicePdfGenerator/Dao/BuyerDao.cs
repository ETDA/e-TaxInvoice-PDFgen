using System;
using System.Collections.Generic;
using SqliteConnector;
using System.Data.SQLite;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator.Dao
{
    class BuyerDao
    {
        private Sqlite sqlite;
        private string tableName = "buyer";
        public BuyerDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder + "database.db");
        }

        internal BuyerObj select(int id)
        {
            string txtQuery = string.Format("SELECT * FROM {0} WHERE id = @id", this.tableName);
            try
            {
                BuyerObj data = new BuyerObj();
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
                                data.taxId = dr["tax_id"].ToString();
                                data.phoneNo = dr["phone_no"].ToString();
                                data.phoneExt = dr["phone_ext"].ToString();
                                data.zipCode = dr["zipcode"].ToString();
                                data.address1 = dr["address1"].ToString();
                                data.email = dr["email"].ToString();
                                data.contactPerson = dr["contact_person"].ToString();
                                data.isBranch = Convert.ToBoolean(dr["is_branch"]);
                                data.branchId = dr["branch_id"].ToString();

                                data.provinceName = dr["province_name"].ToString();
                                data.provinceCode = dr["province_code"].ToString();
                                data.districtName = dr["district_name"].ToString();
                                data.districtCode = dr["district_code"].ToString();
                                data.subdistrictName = dr["subdistrict_name"].ToString();
                                data.subdistrcitCode = dr["subdistrict_code"].ToString();
                                data.houseNo = dr["house_no"].ToString();
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

        internal List<BuyerList> listView(int page, int pageSize)
        {
            string txtQuery = string.Format("SELECT id,name,tax_id,phone_no,phone_ext FROM {0} LIMIT {1} offset {2}", this.tableName, pageSize, (page - 1) * pageSize);
            try
            {
                List<BuyerList> items = new List<BuyerList>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                items.Add(new BuyerList((int)dr.GetInt64(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4)));
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

        internal List<BuyerList> listView(int page, int pageSize,string sortingColumn,string sortingDirection)
        {
            string txtQuery = string.Format("SELECT id,name,tax_id,phone_no,phone_ext FROM {0} ORDER BY {3} {4} LIMIT {1} offset {2}", this.tableName, pageSize, (page - 1) * pageSize,sortingColumn,sortingDirection);
            try
            {
                List<BuyerList> items = new List<BuyerList>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                items.Add(new BuyerList((int)dr.GetInt64(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4)));
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

        internal List<BuyerObj> list()
        {
            string txtQuery = string.Format("SELECT * FROM {0}", this.tableName);
            try
            {
                List<BuyerObj> items = new List<BuyerObj>();
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BuyerObj obj = new BuyerObj();
                                obj.id = Convert.ToInt32(dr["id"]);
                                obj.name = dr["name"].ToString();
                                obj.address1 = dr["address1"].ToString();
                                obj.zipCode = dr["zipcode"].ToString();
                                obj.taxId = dr["tax_id"].ToString();
                                obj.isBranch = Convert.ToBoolean(dr["is_branch"]);
                                obj.branchId = dr["branch_id"].ToString();
                                obj.email = dr["email"].ToString();
                                obj.contactPerson = dr["contact_person"].ToString();
                                obj.phoneNo = dr["phone_no"].ToString();
                                obj.phoneExt = dr["phone_ext"].ToString();

                                obj.provinceCode = dr["province_code"].ToString();
                                obj.provinceName = dr["province_name"].ToString();
                                obj.districtCode = dr["district_code"].ToString();
                                obj.districtName = dr["district_name"].ToString();
                                obj.subdistrcitCode = dr["subdistrict_code"].ToString();
                                obj.subdistrictName = dr["subdistrict_name"].ToString();
                                obj.houseNo = dr["house_no"].ToString();
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

        internal void save(BuyerObj obj)
        {
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                txtQuery = string.Format("INSERT INTO {0} (name,tax_id,phone_no,phone_ext,zipcode,address1,email,contact_person,is_branch,branch_id"
                    + ",province_name,province_code,district_name,district_code,subdistrict_name,subdistrict_code,house_no) VALUES ", this.tableName);
                string values = string.Format("(@name,@tax_id,@phone_no,@phone_ext,@zipcode,@address1,@email,@contact_person,@is_branch,@branch_id"
                    + ",@province_name,@province_code,@district_name,@district_code,@subdistrict_name,@subdistrict_code,@house_no)");
                txtQuery = txtQuery + values;
            }
            else
            {
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("name=@name,tax_id=@tax_id,phone_no=@phone_no,phone_ext=@phone_ext,zipcode=@zipcode,address1=@address1"
                    + ",email=@email,contact_person=@contact_person,is_branch=@is_branch,branch_id=@branch_id"
                    + ",province_name=@province_name,province_code=@province_code,district_name=@district_name,district_code=@district_code"
                    + ",subdistrict_name=@subdistrict_name,subdistrict_code=@subdistrict_code,house_no=@house_no ");
                string condition = string.Format("WHERE id=@id");
                txtQuery = txtQuery + values + condition;
            }
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    cmd.Parameters.AddWithValue("@name", obj.name);
                    cmd.Parameters.AddWithValue("@tax_id", obj.taxId);
                    cmd.Parameters.AddWithValue("@phone_no", obj.phoneNo);
                    cmd.Parameters.AddWithValue("@phone_ext", obj.phoneExt);
                    cmd.Parameters.AddWithValue("@zipcode", obj.zipCode);
                    cmd.Parameters.AddWithValue("@address1", obj.address1);
                    cmd.Parameters.AddWithValue("@email", obj.email);
                    cmd.Parameters.AddWithValue("@contact_person", obj.contactPerson);
                    cmd.Parameters.AddWithValue("@is_branch", (obj.isBranch) ? 1 : 0);
                    cmd.Parameters.AddWithValue("@branch_id", obj.branchId);

                    cmd.Parameters.AddWithValue("@province_name", obj.provinceName);
                    cmd.Parameters.AddWithValue("@province_code", obj.provinceCode);
                    cmd.Parameters.AddWithValue("@district_name", obj.districtName);
                    cmd.Parameters.AddWithValue("@district_code", obj.districtCode);
                    cmd.Parameters.AddWithValue("@subdistrict_name", obj.subdistrictName);
                    cmd.Parameters.AddWithValue("@subdistrict_code", obj.subdistrcitCode);
                    cmd.Parameters.AddWithValue("@house_no", obj.houseNo);
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
