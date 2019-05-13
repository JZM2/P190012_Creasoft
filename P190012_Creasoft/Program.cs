using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P190012_Creasoft
{
    class Program
    {
        static void Main(string[] args)
        {


            if (args.Count() > 0)
            {
                try
                {
                    Creasoft.MyConsole.PrintToConsole("Start programu CREASOFT TEST\n", ConsoleColor.Gray);
                    Creasoft.ConfigXML Config = Creasoft.ConfigXML.Create(args[0]);
                    Creasoft.MyConsole.PrintToConsole(string.Format("Konfigurační soubor \"{0}\" :", Config.ConfigFile), ConsoleColor.Gray);
                    Creasoft.MyConsole.PrintToConsole("OK\n", ConsoleColor.Green);
                    Creasoft.MyConsole.PrintToConsole("Provádím načítání funkcí a souborů:\n", ConsoleColor.Gray);
                    Config.Execute();
                }
                catch (Exception Chyba)
                {
                    Creasoft.MyConsole.PrintToConsole( String.Format("Došlo k chybě v programu: {0}", Chyba.Message), ConsoleColor.Red);

                }

                Creasoft.MyConsole.PrintToConsole("Konec programu\n", ConsoleColor.DarkYellow);
            }
            else
            {
                Creasoft.MyConsole.PrintToConsole("Nebyl zadán parametr název souboru zpracování!", ConsoleColor.Red);
            }
        }
    }
}
