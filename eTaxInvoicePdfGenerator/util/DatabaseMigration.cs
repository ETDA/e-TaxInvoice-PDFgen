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
        private string currentTable;
        private string backupDBFile;

        public DatabaseMigration()
        {
            Init();
        }
        public void Init()
        {
            this.currentDBFile = dbPath.CurrentDBFile();
            this.templateDBFile = dbPath.TemplateDBFile();
            this.backupDBFile = dbPath.BackupDBFile();

            if (!Directory.Exists(dbPath.UserAppDBDir()))
            {
                Directory.CreateDirectory(dbPath.UserAppDBDir());
            }
            if (!File.Exists(dbPath.CurrentDBFile()))
            {
                File.Copy(dbPath.TemplateDBFile(), dbPath.CurrentDBFile());
                this.status = 1;
            }
            if (File.Exists(backupDBFile))
            {
                BackupMigrationController();
                File.Delete(backupDBFile);
            }

        }

        #region Controller 

        public void MigrationController()
        {
            if (status != 1)
            {
                ExecuteSQLStatement(GetRemoveAllStatement("address_code_list"), currentDBFile);
                ExecuteSQLStatement(GetRemoveAllStatement("cause_code_list"), currentDBFile);

                DataTable dtAddressCodelist = ExcuteDatatable(GetSelectAllStatement("address_code_list"), templateDBFile);
                ExecuteSQLStatement(GetUpdateAddressCodelistStatement(dtAddressCodelist), currentDBFile);

                DataTable dtCauseCodelist = ExcuteDatatable(GetSelectAllStatement("cause_code_list"), templateDBFile);
                ExecuteSQLStatement(GetUpdateCauseCodelistStatement(dtCauseCodelist), currentDBFile);
            }
        }

        /*
            Backup Migration for migrate old version database(below than 1.0.4) to new one (version 1.0.5)
            but first.We need to run migration.exe for create backup folder.
                        
        */
        public void BackupMigrationController()
        {
            //Do migration
            
            //clean table
            ExecuteSQLStatement(GetRemoveAllStatement("code_list_unit"), currentDBFile);
            ExecuteSQLStatement(GetRemoveAllStatement("sqlite_sequence"), currentDBFile);
            ExecuteSQLStatement(GetRemoveAllStatement("config"), currentDBFile);

            BackupGetSelectAllStatement(("code_list_unit"));
            BackupGetSelectAllStatement(("invoice"));
            BackupGetSelectAllStatement(("invoice_item"));
            BackupGetSelectAllStatement(("item"));
            BackupGetSelectAllStatement(("reference"));
            BackupGetSelectAllStatement(("seller"));
            BackupGetSelectAllStatement(("sqlite_sequence"));
            BackupGetSelectAllStatement(("config"));

            BackupGetSelectAllStatement(("buyer"));
            BackupGetSelectAllStatement(("contact"));


        }

        #endregion

        #region Old DB Migration function
        private void BackupGetSelectAllStatement(string tableName)
        {
            this.currentTable = tableName;
            BackupExcuteData(string.Format("Select * from {0};", this.currentTable));
        }

        internal void BackupExcuteData(string SQLStatement)
        {
            using (SQLiteConnection con = new SQLiteConnection(string.Format("Data Source={0};", backupDBFile)))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(SQLStatement, con))
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string querystatement = BackupgetData(ref reader);
                        Backupinsert(querystatement);
                    }
                    reader.Close();
                }
                con.Close();
            }
        }
        internal string BackupgetData(ref SQLiteDataReader reader)
        {
            int limit = reader.FieldCount;
            string statement = "";
            for (int i = 0; i < limit; i++)
            {
                Console.WriteLine(reader[i]);
                statement += "'" + reader[i] + "'";
                if (i != limit - 1)
                {
                    statement += ',';
                }
                else
                {
                    if(this.currentTable == "buyer" || (this.currentTable == "contact"))
                    {
                        statement += ","+"'"+"TXID"+"'";
                    }
                }
            }
            return BackupgetQueryStatement(statement);
        }

        public string BackupgetQueryStatement(string statement)
        {
            return string.Format("insert into {0} values ({1});", currentTable, statement);
        }

        internal void Backupinsert(string SQLStatement)
        {
            using (SQLiteConnection con = new SQLiteConnection(string.Format("Data Source={0};", currentDBFile)))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(SQLStatement, con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        #endregion

        #region district and new other field migration function

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

        #endregion
    }
}
