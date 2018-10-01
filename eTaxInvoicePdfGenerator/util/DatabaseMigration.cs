using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Entity;
using SqliteConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTaxInvoicePdfGenerator.util
{
    class DatabaseMigration
    {
        int status = 0;
        DatabasePath dbPath = new DatabasePath();
        CodeListDao codelistDAO = new CodeListDao();
        private string currentDBFile;
        private string templateDBFile;
        public DatabaseMigration()
        {
            Init();
        }

        public void Init()
        {

            if (!Directory.Exists(dbPath.UserAppDBDir()))
            {
                Directory.CreateDirectory(dbPath.UserAppDBDir());
            }
            if (!System.IO.File.Exists(dbPath.CurrentDBFile()))
            {
                File.Copy(dbPath.TemplateDBFile(),dbPath.CurrentDBFile());
                this.status = 1;
            }

            this.currentDBFile = dbPath.CurrentDBFile();
            this.templateDBFile = dbPath.TemplateDBFile();

        }

        public void MigrationController()
        {
            if(status != 1)
            {
                ExecuteSQLStatement(GetRemoveAllStatement("address_code_list"),currentDBFile);
                ExecuteSQLStatement(GetRemoveAllStatement("cause_code_list"), currentDBFile);

                DataTable dtAddressCodelist = ExcuteDatatable(GetSelectAllStatement("address_code_list"), templateDBFile);
                ExecuteSQLStatement(GetUpdateAddressCodelistStatement(dtAddressCodelist), currentDBFile);

                DataTable dtCauseCodelist = ExcuteDatatable(GetSelectAllStatement("cause_code_list"), templateDBFile);
                ExecuteSQLStatement(GetUpdateCauseCodelistStatement(dtCauseCodelist), currentDBFile);
            }
        }

        private string GetRemoveAllStatement(string tableName)
        {
            return String.Format("DELETE from {0};", tableName);            
        }

        private string GetSelectAllStatement(string tableName)
        {
            return string.Format("Select * from {0};", tableName);
        }

        private string GetUpdateCodelistStatement(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i< dt.Rows.Count; i++)
            {
                string id = dt.Rows[i]["id"].ToString();
                string code = dt.Rows[i]["code"].ToString();
                string description = dt.Rows[i]["description"].ToString();
                sb.Append(string.Format("INSERT into code_list_unit VALUES('{0}','{1}','{2}');",id,code,description));
            }

            return sb.ToString();
        }

        private string GetUpdateCauseCodelistStatement(DataTable dt)
        {

            int limitation = dt.Rows.Count;
            string statement = ("insert into cause_code_list('code', 'description', 'type','case') values");
            for (int i = 0; i < limitation; i++)
            {
                string code = dt.Rows[i]["code"].ToString();
                string description = dt.Rows[i]["description"].ToString();
                string type = dt.Rows[i]["type"].ToString();
                string Case = dt.Rows[i]["case"].ToString();
                statement += string.Format("('{0}','{1}','{2}','{3}')", code, description, type, Case);
                if (i != limitation - 1) statement += ',';
            }
            statement += ';';

            return statement;
        }

        private string GetUpdateAddressCodelistStatement(DataTable dt)
        {
            
            int limitation = dt.Rows.Count;
            string statement = ("insert into address_code_list('Code', 'Changwat_TH', 'Amphoe_TH','Tambon_TH') values");
            for (int i = 0; i < limitation; i++)
            {
                string code = dt.Rows[i]["Code"].ToString();
                string changwat = dt.Rows[i]["Changwat_TH"].ToString();
                string amphoe = dt.Rows[i]["Amphoe_TH"].ToString();
                string tambon = dt.Rows[i]["Tambon_TH"].ToString();
                statement += string.Format("('{0}','{1}','{2}','{3}')",code,changwat,amphoe,tambon);
                if(i != limitation - 1) statement += ',';
            }
            statement += ';';

            return statement;
        }

        internal void ExecuteSQLStatement(string SQLStatement,string dbPath)
        {
            Sqlite sqlite = new Sqlite(dbPath);
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlite.ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(SQLStatement, c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal DataTable ExcuteDatatable(string SQLStatement, string dbPath)
        {
            Sqlite sqlite = new Sqlite(dbPath);
            DataTable dt = new DataTable();
            try
            {
               dt  = sqlite.ExecuteDataTable(SQLStatement);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return dt;
        }
    }
}
