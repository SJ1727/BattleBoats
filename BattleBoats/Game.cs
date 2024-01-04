using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Game
    {
        const int MIN_BOARD_WIDTH = 7;
        const int MIN_BOARD_HEIGHT = 7;
        const int MAX_BOARD_WIDTH = 30;
        const int MAX_BOARD_HEIGHT = 30;

        public static void SetupGame(ref Player? player1, ref Player? player2, ref int startingTurn, ref string savePath)
        {
            bool loadPreviousGame = false;
            string gameType = string.Empty;
            int boardWidth = 0;
            int boardHeight = 0;

            gameType = SelectGameType(ref player1, ref player2, ref savePath);

            switch (gameType)
            {
                case "1":
                    savePath = @"pvp.save";
                    player1 = new HumanPlayer("Player 1", null);
                    player2 = new HumanPlayer("Player 2", null);
                    break;
                case "2":
                    savePath = @"pvc.save";
                    player1 = new HumanPlayer("Player", null);
                    player2 = new ComputerPlayer("Computer", null);
                    break;
                case "3":
                    savePath = @"cvc.save";
                    player1 = new ComputerPlayer("Computer 1", null);
                    player2 = new ComputerPlayer("Computer 2", null);
                    break;
                default:
                    break;
            }

            if (File.Exists(savePath))
            {
                Console.Write("Do you want to load your previous game (Y or N): ");

                loadPreviousGame = Console.ReadLine().ToLower() == "y";
            }

            if (loadPreviousGame)
            {
                Save.LoadGame(ref player1, ref player2, ref startingTurn, savePath);
            }
            else
            {
                Boat[] boats1 = new Boat[5];
                boats1[0] = new Destroyer((0, 0));
                boats1[1] = new Cruiser((0, 0));
                boats1[2] = new Submarine((0, 0));
                boats1[3] = new BattleShip((0, 0));
                boats1[4] = new Carrier((0, 0));

                Boat[] boats2 = new Boat[5];
                boats2[0] = new Destroyer((0, 0));
                boats2[1] = new Cruiser((0, 0));
                boats2[2] = new Submarine((0, 0));
                boats2[3] = new BattleShip((0, 0));
                boats2[4] = new Carrier((0, 0));

                GetNewGameSize(ref boardWidth, ref boardHeight);

                Board player1Board = new Board(boardWidth, boardHeight, player1.name);
                Board plyaer2Board = new Board(boardWidth, boardHeight, player2.name);

                player1.setBoard(player1Board);
                player2.setBoard(plyaer2Board);
                player1.initialiseBoard(boats1);
                player2.initialiseBoard(boats2);
            }
        }

        public static string SelectGameType(ref Player player1, ref Player player2, ref string savePath)
        {
            bool selectingGameType = true;
            string gameType = string.Empty;
            int boardWidth = 0;
            int boardHeight = 0;

            while (gameType != "1" && gameType != "2" && gameType != "3")
            {
                Console.Clear();
                Display.DisplayTitle();
                Console.WriteLine("Select option: ");
                Console.WriteLine("1. Player vs Player game");
                Console.WriteLine("2. Player vs Computer game");
                Console.WriteLine("3. Computer vs Computer game");
                Console.WriteLine("4. Read Instructions");
                Console.WriteLine("5. Quit");
                Console.Write("Enter: ");
                gameType = Console.ReadLine();

                if (gameType == "5") { System.Environment.Exit(0); }

                if (gameType == "4") { Display.DisplayInstructions(); }

                if (gameType != "1" && gameType != "2" && gameType != "3" && gameType != "4")
                {
                    Console.WriteLine("Invalid game type, please re-enter");
                }
            }

            return gameType;
        }

        public static void PlayGame(Player player1, Player player2, int turn, string savePath)
        {
            bool boatHit = false;
            string winner = string.Empty;
            while (!player1.ownBoard.AllBoatsDestroyed() && !player2.ownBoard.AllBoatsDestroyed())
            {
                Save.SaveGame(player1, player2, turn, savePath);

                if (turn % 2 == 0)
                {
                    boatHit = player1.playMove(ref player2);
                }
                else
                {
                    boatHit = player2.playMove(ref player1);
                }

                if (!boatHit)
                {
                    turn++;
                }
            }

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            winner = turn % 2 == 0 ? player1.name : player2.name;

            Console.Write($"\u001b[38;5;127mThe winner is {winner}! Press enter to exit program: \u001b[38;5;231m");
            Console.ReadLine();

        }

        public static void GetNewGameSize(ref int boardWidth, ref int boardHeight)
        {
            string userInput = string.Empty;

            Console.Clear();
            Display.DisplayTitle();

            // Gets the board width
            do
            {
                Console.Write($"Enter the width of the board (must be between {MIN_BOARD_WIDTH} and {MAX_BOARD_WIDTH}): ");
                userInput = Console.ReadLine();
                int.TryParse(userInput, out boardWidth);
                Console.Clear();
                Display.DisplayTitle();

                if (boardWidth < MIN_BOARD_WIDTH || boardWidth > MAX_BOARD_WIDTH)
                {
                    Console.WriteLine("Invalid board width, please re-enter");
                }
            } while (boardWidth < MIN_BOARD_WIDTH || boardWidth > MAX_BOARD_WIDTH);

            // Gets the board height
            do
            {
                Console.Write($"Enter the height of the board (must be between {MIN_BOARD_HEIGHT} and {MAX_BOARD_HEIGHT}): ");
                userInput = Console.ReadLine();
                int.TryParse(userInput, out boardHeight);
                Console.Clear();
                Display.DisplayTitle();

                if (boardHeight < MIN_BOARD_WIDTH || boardHeight > MAX_BOARD_WIDTH)
                {
                    Console.WriteLine("Invalid board height, please re-enter");
                }
            } while (boardHeight < MIN_BOARD_HEIGHT || boardHeight > MAX_BOARD_HEIGHT);
        }
    }
}
