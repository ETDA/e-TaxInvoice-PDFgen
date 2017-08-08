using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SqliteConnector;
using System.Data.SQLite;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator.Dao
{
    class SellerDao
    {
        private Sqlite sqlite;
        private string tableName = "seller";
        public SellerDao()
        {
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder + "database.db");
        }
        internal SellerObj select()
        {
            string txtQuery = string.Format("SELECT * FROM {0} LIMIT 1", this.tableName);
            try
            {
                SellerObj data = new SellerObj();
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
                                data.phoneNo = dr["phone_no"].ToString();
                                data.phoneExt = dr["phone_ext"].ToString();
                                data.zipCode = dr["zipcode"].ToString();
                                data.address1 = dr["address1"].ToString();
                                data.houseNo = dr["house_no"].ToString();
                                data.email = dr["email"].ToString();
                                data.isBranch = Convert.ToBoolean(dr["is_branch"]);
                                data.branchId = dr["branch_id"].ToString();
                                data.vat = Convert.ToDouble(dr["vat"]);

                                data.provinceCode = dr["province_code"].ToString();
                                data.provinceName = dr["province_name"].ToString();
                                data.districtCode = dr["district_code"].ToString();
                                data.districtName = dr["district_name"].ToString();
                                data.subdistrcitCode = dr["subdistrict_code"].ToString();
                                data.subdistrictName = dr["subdistrict_name"].ToString();
                                data.houseNo = dr["house_no"].ToString();
                                data.inv_no = dr["inv_no"].ToString();
                                data.dbn_no = dr["dbn_no"].ToString();
                                data.crn_no = dr["crn_no"].ToString();
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

        internal void save(SellerObj obj)
        {
            string txtQuery = string.Empty;
            if (obj.id == 0)
            {
                txtQuery = string.Format("INSERT INTO {0} (name,tax_id,phone_no,phone_ext,zipcode,address1,email,vat,is_branch,branch_id"
                    + ",province_name,province_code,district_name,district_code,subdistrict_name,subdistrict_code,house_no,inv_no,dbn_no,crn_no) VALUES ", this.tableName);
                string values = string.Format("(@name,@tax_id,@phone_no,@phone_ext,@zipcode,@address1,@email,@vat,@is_branch,@branch_id"
                    + ",@province_name,@province_code,@district_name,@district_code,@subdistrict_name,@subdistrict_code,@house_no,@inv_no,@dbn_no,@crn_no)");
                txtQuery = txtQuery + values;
            }
            else
            {
                txtQuery = string.Format("UPDATE {0} SET ", this.tableName);
                string values = string.Format("name=@name,tax_id=@tax_id,phone_no=@phone_no,phone_ext=@phone_ext,zipcode=@zipcode,address1=@address1"
                    + ",email=@email,vat=@vat,is_branch=@is_branch,branch_id=@branch_id,province_name=@province_name,province_code=@province_code"
                    + ",district_name=@district_name,district_code=@district_code,subdistrict_name=@subdistrict_name,subdistrict_code=@subdistrict_code"
                    + ",house_no=@house_no,inv_no=@inv_no,dbn_no=@dbn_no,crn_no=@crn_no ");
                //string condition = string.Format("WHERE id={0}", obj.id);
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
                    cmd.Parameters.AddWithValue("@vat", obj.vat);
                    cmd.Parameters.AddWithValue("@is_branch", (obj.isBranch) ? 1 : 0);
                    cmd.Parameters.AddWithValue("@branch_id", obj.branchId);

                    cmd.Parameters.AddWithValue("@province_name", obj.provinceName);
                    cmd.Parameters.AddWithValue("@province_code", obj.provinceCode);
                    cmd.Parameters.AddWithValue("@district_name", obj.districtName);
                    cmd.Parameters.AddWithValue("@district_code", obj.districtCode);
                    cmd.Parameters.AddWithValue("@subdistrict_name", obj.subdistrictName);
                    cmd.Parameters.AddWithValue("@subdistrict_code", obj.subdistrcitCode);
                    cmd.Parameters.AddWithValue("@house_no", obj.houseNo);
                    cmd.Parameters.AddWithValue("@inv_no", obj.inv_no);
                    cmd.Parameters.AddWithValue("@dbn_no", obj.dbn_no);
                    cmd.Parameters.AddWithValue("@crn_no", obj.crn_no);
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
