using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;

namespace BattleBoats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player? player1 = null;
            Player? player2 = null;
            string savePath = string.Empty;
            int startingTurn = 0;

            Console.Title = "Battle Boats";

            // Make text color white and background black
            Console.Write("\u001b[38;5;231m");
            Console.Write("\u001b[48;5;0m");

            Game.SetupGame(ref player1, ref player2, ref startingTurn, ref savePath);
            Game.PlayGame(player1, player2, startingTurn, savePath);
        }
    }
}