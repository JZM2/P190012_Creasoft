using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P190012_Creasoft.Creasoft
{
    static class MyConsole
    {
        public static void PrintToConsole(string Text, ConsoleColor Barva)
        {
            Console.ForegroundColor = Barva;
            Console.Write(Text);
            Console.ResetColor();
        }
    }
}
