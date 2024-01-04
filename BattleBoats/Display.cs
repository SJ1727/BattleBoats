using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Display
    {
        const string INSTRUCTION_PATH = @"BattleBoatsText\instructions.txt";
        const string TITLE_PATH = @"BattleBoatsText\title.txt";
        const string MOVING_BOATS_INSTRUCTIONS_PATH = @"BattleBoatsText\movingBoatsInstructions.txt";
        const string FIRING_AT_BOARD_INSTRUCTIONS_PATH = @"BattleBoatsText\firingInstructions.txt";

        public static void DisplayBoardFiring(Board board)
        {
            Console.Clear();
            DisplayTitle();
            Console.WriteLine(board.DisplayBoard(false));
            DisplayTextFileContents(FIRING_AT_BOARD_INSTRUCTIONS_PATH);
        }

        public static void DisplayTitle()
        {
            DisplayTextFileContents(TITLE_PATH);
        }

        public static void DisplayTextFileContents(string path)
        {
            // %COLORF is used to indicate the use of an ansi colour to colour the foreground
            // %COLORB is used to indicate the use of an ansi colour to colour the background

            string text = string.Empty;
            using (StreamReader fileReader = new StreamReader(path))
            {
                text = fileReader.ReadToEnd();
                text = text.Replace("%COLORF", "\u001b[38;5;");
                text = text.Replace("%COLORB", "\u001b[48;5;");
                Console.WriteLine(text);
            }
        }

        public static void DisplayBoardPlacingBoats(Board board)
        {
            Console.Clear();
            DisplayTitle();
            Console.WriteLine(board.DisplayBoard(true));
            DisplayTextFileContents(MOVING_BOATS_INSTRUCTIONS_PATH);
        }

        public static void DisplayInstructions()
        {
            Console.Clear();
            DisplayTitle();
            DisplayTextFileContents(INSTRUCTION_PATH);
            Console.Write("Exit: ");
            Console.ReadLine();
        }
    }
}
