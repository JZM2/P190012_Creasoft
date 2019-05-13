using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P190012_Creasoft.Creasoft
{
    class ImportFile
    {
        public string FilePath;
        public string Archive;
        public string Output;

        public List<Zbozi> Zbozi;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="PathFile"></param>
        /// <param name="ArchivePath"></param>
        /// <param name="OutputPath"></param>
        public ImportFile(string PathFile, string ArchivePath, string OutputPath)
        {
            this.Zbozi = new List<Zbozi>();

            this.FilePath = PathFile;

            FileInfo file = new FileInfo(PathFile);

            if (ArchivePath != "no") 
                this.Archive = ArchivePath + "\\" + file.Name;
            else
                this.Archive = "";

            try
            {
                if (OutputPath != "no")
                {
                    FileInfo FileOutput = new FileInfo(OutputPath);

                    if (!FileOutput.Exists)
                    {
                        if (!FileOutput.Directory.Exists)
                            FileOutput.Directory.Create();
                        FileOutput.Create();
                    }
                    this.Output = FileOutput.FullName;
                }
                else
                    this.Output = "";
            }
            catch(Exception Chyba)
            {

            }
        }

        /// <summary>
        /// Spouští vlákno s archivací
        /// </summary>
        private void ArchiveFile()
        {
            if (this.Archive != "")
            {
                Thread thread1 = new Thread(this.ThreadArchive);
                //thread1.Resume += this.ThreadArchiveEnded;
                thread1.Start();

                int i = 0;
                //Console.CursorLeft = 20;
                string znak = "-";
                //Console.Write(znak);

                while (thread1.IsAlive)
                {
                    switch(i)
                    {
                        case 0:
                            znak = "-";
                            i++;
                            break;
                        case 1:
                            znak = "\\";
                            i++;
                            break;
                        case 2:
                            znak = "|";
                            i++;
                            break;
                        case 3:
                            znak = "/";
                            i = 0;
                            break;
                    }

                    //Console.CursorLeft = 20;
                    //Console.Write(znak);
                    i++;
                    Thread.Sleep(100);
                }
            }
            else
                return;
        }

        /// <summary>
        /// Vlákno pro zpracování archivu
        /// </summary>
        private void ThreadArchive()
        {
            Console.Write("\nArchivace: ");
            try
            {
                FileInfo Archiv = new FileInfo(this.Archive);

                if (!Archiv.Directory.Exists)
                    Archiv.Directory.Create();
                File.Move(this.FilePath, this.Archive);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.CursorLeft = 15;
                Console.Write("OK\n");
                Console.ResetColor();
            }
            catch (Exception Chyba)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.CursorLeft = 15;
                Console.Write("CHYBA\n{0}\n", Chyba.Message);
                Console.ResetColor();
            }
            finally
            {

            }
        }

        /// <summary>
        /// Souští zpracování souboru
        /// </summary>
        /// <returns></returns>
        public List<Zbozi> ExecuteFiles()
        {
            MyConsole.PrintToConsole(String.Format("\nZpracovávám soubor {0} : \n", FilePath), ConsoleColor.Gray);
            this.ImportData();
            this.ArchiveFile();

            return this.Zbozi;
        }

        /// <summary>
        /// Importuje data ze souboru do paměti aplikace
        /// </summary>
        /// <returns></returns>
        private List<Zbozi> ImportData()
        {
            using (StreamReader sr = new StreamReader(this.FilePath))
            {
                Console.Write("Import dat: ");

                try
                {

                    string zbozi;
                    int i = 0;
                    bool error = false;

                    while ((zbozi = sr.ReadLine()) != null)
                    {
                        try
                        {
                            i++;
                            string[] radek = zbozi.Split(char.Parse(";"));

                            Zbozi zbz = new Creasoft.Zbozi();

                            if (!int.TryParse(radek[0], out zbz.ID))
                                throw new Exception(String.Format("Item ({0}) - ID ({1}) is not number!", i, radek[0]));
                            
                            if (!int.TryParse(radek[1], out zbz.objednano))
                                throw new Exception(String.Format("Položka č. {0} - Počet kusů ({1}) není číslo!", i, radek[1]));

                            this.Zbozi.Add(zbz);
                        }
                        catch(Exception Chyba)
                        {
                            error = true;
                            Console.CursorLeft = 0;
                            //Console.Write("VÝSTRAHA\n");
                            MyConsole.PrintToConsole(String.Format("\n{0}", Chyba.Message), ConsoleColor.Yellow);
                        }
                    }

                    if (error)
                        throw new FormatException("CHYBA");

                    Console.CursorLeft = 15;
                    MyConsole.PrintToConsole("OK", ConsoleColor.Green);

               }
                catch (FormatException Chyba)
                {
                    int left, top;
                    left = Console.CursorLeft;
                    top = Console.CursorTop;
                    Console.SetCursorPosition(15, 8);
                    MyConsole.PrintToConsole(Chyba.Message, ConsoleColor.Red);
                    Console.SetCursorPosition(left, top);
                }
                catch (Exception Chyba)
                {
                    Console.CursorLeft = 15;
                    MyConsole.PrintToConsole(String.Format("CHYBA\n{0}",Chyba.Message), ConsoleColor.Red);
                }
            }

            return this.Zbozi;
        }

        /// <summary>
        /// Generuje výstup
        /// </summary>
        /// <returns></returns>
        public string OutputData()
        {
            Console.Write("Výstup: ");

            if (this.Output == "")
            {
                MyConsole.PrintToConsole(String.Format("Nepožadováno\n"), ConsoleColor.Blue);
                return "";
            }

            string Data = "";

            foreach (Zbozi Item in this.Zbozi)
            {
                Data += String.Format("{0};{1};{2}\n", Item.ID, Item.nazev, Item.objednano);
            }

            using (TextWriter twWriter = new StreamWriter(this.Output, true ))
            {
                twWriter.Write(Data);
                //foreach (string line in lines)
                //    twWriter.WriteLine(line);
            }

            Console.CursorLeft = 15;
            MyConsole.PrintToConsole("OK\n", ConsoleColor.Green);
            return Data;
        }
    }


}
