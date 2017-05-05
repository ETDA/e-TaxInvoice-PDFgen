using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqliteConnector;
using System.Data;
using System.Data.SQLite;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator
{
    class Modifier
    {
        private Sqlite sqlite;
        private string tableName;
        public const string BUYER = "buyer";
        public const string SELLER = "seller";
        public const string ITEM = "item";
        public Modifier(string tableName)
        {
            this.tableName = tableName;
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory;
            sqlite = new Sqlite(base_folder + Properties.Resources.datasource);
        }

        public DataTable list()
        {
            string txtQuery = string.Format("SELECT * FROM {0}", this.tableName);
            try
            {
                return sqlite.ExecuteDataTable(txtQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SQLiteDataReader drList()
        {
            string txtQuery = string.Format("SELECT * FROM {0}", this.tableName);
            try
            {
                return sqlite.ExecuteReader(txtQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void insert(object data)
        {

        }

        public void insert(string txtQuery)
        {
            try
            {
                sqlite.ExecuteNonQuery(txtQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void update(int pk, string column, object value)
        {
            string txtQuery = string.Format("UPDATE {0} set {1}='{2}' WHERE id={3}", this.tableName, column, value, pk);
            try
            {
                sqlite.ExecuteNonQuery(txtQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void insertBuyer(BuyerObj buyer)
        //{
        //    string txtQuery = "INSERT INTO buyer (name,taxid_no,phone_no,phone_ext,zip_code,address1,address2,email,website,contact_person,fax_no,is_branch,branch_no) VALUES ";
        //    string values = string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})",
        //        buyer.name, buyer.taxId, buyer.phoneNo, buyer.phoneExt, buyer.zipCode, buyer.address1, buyer.address2, 
        //        buyer.email, buyer.website,buyer.contactPerson,buyer.faxNo,buyer.faxExt,buyer.isBranch,buyer.branchId
        //        );
        //    txtQuery = txtQuery + values;
        //    try
        //    {
        //        sqlite.ExecuteNonQuery(txtQuery);
        //    }catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
