using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTaxInvoicePdfGenerator.util
{


    class DatabasePath
    {
        private string userAppDataDir = "";
        private string appDataDir = "";
        private string dbFileName = "PDFgenDatabase.db";
        private string dbNameDir = "etaxEtda";
        private string templateFileDBName = "database.db"; 

        public DatabasePath()
        {
            Init();
        }

        public void Init()
        {
            this.userAppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.appDataDir = System.AppDomain.CurrentDomain.BaseDirectory;
        }
        public string TemplateDBFile()
        {
            return Path.Combine(appDataDir, templateFileDBName);
        }
        public string CurrentDBFile()
        {
            return Path.Combine(UserAppDBDir(), dbFileName);
        }
        public string BackupDBFile()
        {
            return Path.Combine(UserAppDBDir(), templateFileDBName);
        }
        public string UserAppDataDir()
        {
            return  userAppDataDir;
        }
        public string UserAppDBDir()
        {
            return Path.Combine(userAppDataDir, dbNameDir);
        }
        public string AppDataDir()
        {
            return appDataDir;
        }
    }
}
