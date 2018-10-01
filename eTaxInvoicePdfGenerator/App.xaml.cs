using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using eTaxInvoicePdfGenerator.util;

namespace eTaxInvoicePdfGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 


    public partial class App : Application
    {
        App()
        {
            System.Threading.Thread.Sleep(3000);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseMigration dbMigration = new DatabaseMigration();
            dbMigration.MigrationController();


            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            
            if(processes.Count() > 1)
            {
                this.Shutdown();
            }
            string fontFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);
            Shell32.Folder fontFolder = GetShell32NameSpaceFolder(fontFolderPath);
            string base_folder = System.AppDomain.CurrentDomain.BaseDirectory + "in\\";
            string segueUi = "segoeui.ttf";
            string sarabunBold = "THSarabun Bold.ttf";
            string sarabun = "THSarabun.ttf";

            if (fontFolder != null)
            {
                string fontSegoeUI = base_folder + segueUi;
                if (!File.Exists(fontFolderPath + "\\" + segueUi))
                {
                    fontFolder.CopyHere(fontSegoeUI, 4);
                }

                string fontSarabunBold = base_folder + sarabunBold;
                if (!File.Exists(fontFolderPath + "\\" + sarabunBold))
                {
                    fontFolder.CopyHere(fontSarabunBold, 4);
                }


                string fontSarabun = base_folder + sarabun;
                if (!File.Exists(fontFolderPath + "\\" + sarabun))
                {
                    fontFolder.CopyHere(fontSarabun, 4);
                }
            }
        }
        public Shell32.Folder GetShell32NameSpaceFolder(Object folder)
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");

            Object shell = Activator.CreateInstance(shellAppType);
            return (Shell32.Folder)shellAppType.InvokeMember("NameSpace",
            System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { folder });
        }
    }

}
