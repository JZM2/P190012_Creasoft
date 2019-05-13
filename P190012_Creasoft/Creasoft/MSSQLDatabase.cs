using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace P190012_Creasoft.Creasoft
{
    class MSSQLDatabase : IDataBase
    {
        private DataSetCreasoft _DataSet;
        private SqlConnection _DBconnection;
        private SqlDataAdapter _DataAdapterZbozi;

        public string ConnectionString { set; get; }

        private MSSQLDatabase() { }

        /// <summary>
        /// Vytváří a vrací instanci objektu databáze
        /// </summary>
        /// <param name="ConnectionString"></param>
        public static  MSSQLDatabase CreateDatabase(string ConnectionString)
        {
            MSSQLDatabase db = new MSSQLDatabase();
            db.ConnectionString = ConnectionString;
            db.Open();
            return db;
        }

        /// <summary>
        /// Otevírá spojení s databází a načítá data z databáze do paměťové tabulky aplikace
        /// </summary>
        /// <param name="ConnectionString"></param>
        public void Open()
        {
            this._DBconnection = new SqlConnection(ConnectionString);
            this._DBconnection.Open();

            this._DataSet = new DataSetCreasoft();
            this._DataAdapterZbozi = new SqlDataAdapter("SELECT * FROM zbozi ORDER BY ID", this._DBconnection);
            this._DataAdapterZbozi.Fill(this._DataSet.zbozi);
        }

        /// <summary>
        /// Funkce provede ověření skladových zásob
        /// </summary>
        /// <param name="Goods"></param>
        public void ExpedovatZbozi(List<Zbozi> Goods)
        {
            Console.Write("Zpracování: ");

            int left, top, y = 0;

            foreach (Zbozi Good in Goods)
            {
                try
                {
                    DataSetCreasoft.zboziRow ZboziSklad = this._DataSet.zbozi.FindByID(Good.ID);
                    int expedovat = 0;

                    if (ZboziSklad == null)
                    {
                        throw new Exception(String.Format("Zboží ID {0} nebylo nalezeno ve skladu!", Good.ID));
                    }

                    if (Good.objednano <= ZboziSklad.pocet_kusu_skladem)
                        expedovat = Good.objednano;
                    else
                        expedovat = ZboziSklad.pocet_kusu_skladem;

                    Good.nazev = ZboziSklad.nazev;
                    Good.skladem = ZboziSklad.pocet_kusu_skladem;
                    Good.objednano = expedovat;
                }
                catch(Exception Chyba)
                {
                    left = Console.CursorLeft;
                    top = Console.CursorTop;
                    Console.SetCursorPosition(15, top - y);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CHYBA\n");
                    Console.SetCursorPosition(0, top);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n{0}", Chyba.Message);
                    Console.ResetColor();
                    y++;
                }
            }

            if (y == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.CursorLeft = 15;
                Console.WriteLine("OK");
                Console.ResetColor();
                //return expedovat;
            }
            else
                Console.Write("\n");
        }
    }

    
}
