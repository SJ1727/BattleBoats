using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Symbol
    {
        char symbol;
        public ConsoleColor symbolColor;
        public ConsoleColor backgroundColor;

        public Symbol (char symbol, ConsoleColor symbolColor, ConsoleColor backgroundColor=ConsoleColor.Black)
        {
            this.symbol = symbol;
            this.symbolColor = symbolColor;
            this.backgroundColor = backgroundColor;
        }

        public void ConsoleWriteSymbol()
        {
            Console.ForegroundColor = this.symbolColor;
            Console.BackgroundColor = this.backgroundColor;
            Console.Write(this.symbol);
            Console.ResetColor();
        }
    }
}
