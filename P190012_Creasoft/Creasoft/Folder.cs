using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P190012_Creasoft.Creasoft
{
    class Folder
    {
        public string Filter { set; get; }
        public string Archive { set; get; }
        public string Output { set; get; }
        public string Path { set; get; }

        private Creasoft.MSSQLDatabase _Database;

        public List<ImportFile> ImportFiles;

        public Folder()
        {
            this._Database = Creasoft.MSSQLDatabase.CreateDatabase("Server=HONZA-PC\\SQLEXPRESS;Database=creasoft;Trusted_Connection=True;");
        }

        /// <summary>
        /// Spouští vyhládáví souborů v adresáři
        /// </summary>
        public void ExecuteFiles()
        {

            try
            {
                this.ImportFiles = new List<ImportFile>();

                //List<String> files = Directory.EnumerateFiles(this.Path);
                var files = Directory.EnumerateFiles(this.Path, this.Filter , SearchOption.TopDirectoryOnly);

                foreach (string filename in files)
                {
                    try
                    {
                        Creasoft.ImportFile ImFile = new ImportFile(filename, this.Archive, this.Output);
                        this.ImportFiles.Add(ImFile);

                        List<Zbozi> Zbozi = ImFile.ExecuteFiles();
                        this._Database.ExpedovatZbozi(Zbozi);
                        ImFile.OutputData();
                    }
                    catch(Exception Chyba)
                    {
                        MyConsole.PrintToConsole(String.Format("Položka nebyla nalezena! {0}\n", Chyba.Message), ConsoleColor.Red);
                    }
                }

            }
            catch (Exception Chyba)
            {
                MyConsole.PrintToConsole(String.Format("Chyba při importu dat ze souborů! {0}\n", Chyba.Message), ConsoleColor.Red);
            }
        }
    }
}
