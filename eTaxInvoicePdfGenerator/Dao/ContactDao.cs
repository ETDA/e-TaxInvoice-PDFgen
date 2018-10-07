using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqliteConnector;
using eTaxInvoicePdfGenerator.Entity;
using System.Data.SQLite;
using System.Data.SqlClient;
using eTaxInvoicePdfGenerator.util;

namespace eTaxInvoicePdfGenerator.Dao
{
    class ContactDao
    {
        private Sqlite sqlite;
        private string tableName = "contact";
        public ContactDao()
        {
            DatabasePath dbPath = new DatabasePath();
            string base_folder = dbPath.CurrentDBFile(); //System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder);
        }
        internal ContactObj select()
        {
            string txtQuery = string.Format("SELECT * FROM {0} LIMIT 1", this.tableName);
            try
            {
                ContactObj data = new ContactObj();
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
                                data.name = dr["name"].ToString();
                                data.taxId = dr["tax_id"].ToString();
                                data.branchId = dr["branch_id"].ToString();
                                data.email = dr["email"].ToString();
                                data.zipCode = dr["zipcode"].ToString();
                                data.address1 = dr["address1"].ToString();
                                data.country = dr["country"].ToString();
                                data.contactPerson = dr["contact_person"].ToString();
                                data.phoneNo = dr["phone_no"].ToString();
                                data.phoneExt = dr["phone_ext"].ToString();

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

        internal int save(ContactObj obj)
        {
            int id = 0;
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                txtQuery = string.Format("INSERT INTO {0} (name,tax_id,branch_id,email,zipcode,address1,country,contact_person,phone_no,phone_ext"
                    + ",province_name,province_code,district_name,district_code,subdistrict_name,subdistrict_code,house_no,tax_type) VALUES ", this.tableName);
                string values = string.Format("(@name,@tax_id,@branch_id,@email,@zipcode,@address1,@country,@contact_person,@phone_no,@phone_ext"
                    + ",@province_name,@province_code,@district_name,@district_code,@subdistrict_name,@subdistrict_code,@house_no,@tax_type)");
                txtQuery = txtQuery + values;
            }
            else
            {
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("name=@name,tax_id=@tax_id,branch_id=@branch_id,email=@email,zipcode=@zipcode,address1=@address1"
                    + ",country=@country,contact_person=@contact_person,phone_no=@phone_no,phone_ext=@phone_ext"
                    + ",province_name=@province_name,province_code=@province_code,district_name=@district_name,district_code=@district_code,"
                    + ",subdistrict_name=@subdistrict_name,subdistrict_code=@subdistrict_code,house_no=@house_no,tax_type=@tax_type");
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
                    cmd.Parameters.AddWithValue("@tax_type", obj.taxType);
                    cmd.Parameters.AddWithValue("@branch_id", obj.branchId);
                    cmd.Parameters.AddWithValue("@email", obj.email);
                    cmd.Parameters.AddWithValue("@zipcode", obj.zipCode);
                    cmd.Parameters.AddWithValue("@address1", obj.address1);
                    cmd.Parameters.AddWithValue("@country", obj.country);
                    cmd.Parameters.AddWithValue("@contact_person", obj.contactPerson);
                    cmd.Parameters.AddWithValue("@phone_no", obj.phoneNo);
                    cmd.Parameters.AddWithValue("@phone_ext", obj.phoneExt);

                    cmd.Parameters.AddWithValue("@province_name", obj.provinceName);
                    cmd.Parameters.AddWithValue("@province_code", obj.provinceCode);
                    cmd.Parameters.AddWithValue("@district_name", obj.districtName);
                    cmd.Parameters.AddWithValue("@district_code", obj.districtCode);
                    cmd.Parameters.AddWithValue("@subdistrict_name", obj.subdistrictName);
                    cmd.Parameters.AddWithValue("@subdistrict_code", obj.subdistrcitCode);
                    cmd.Parameters.AddWithValue("@house_no",obj.houseNo);
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
                string getLastId = "SELECT last_insert_rowid()";
                using (SQLiteCommand cmd = new SQLiteCommand(getLastId, c))
                {
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return id;
        }

        internal int getLastInserted()
        {
            string txtQuery = string.Format("SELECT last_insert_rowid()");
            using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, c))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
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
