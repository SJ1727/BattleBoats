using System.Formats.Asn1;
using System.Reflection;
using System.Text;

namespace BattleBoats
{
    // http://www.patorjk.com/software/taag/#p=display&h=1&v=1&f=ANSI%20Shadow&t=Battle%20Boats
    // TODO: Add a selection for difficulty
    // TODO: Finish users firing
    // TODO: Write intructions to be displayed below board
    // TODO: Make it look better
    // TODO: Add more to the inititial intructions

    internal class Program
    {
        const int MAX_BOARD_WIDTH = 26;
        const int MAX_BOARD_HEIGHT = 26;
        const string INSTRUCTION_PATH = @"instructions.txt";
        const string TITLE_PATH = @"title.txt";
        const string MOVING_BOATS_CONTROL_PATH = @"movingShipsControls.txt";
        const int MILLISECOND_DELAY_AFTER_FIRE = 500;

        static void Main(string[] args)
        {
            Board userBoard = null;
            Board computerBoard = null;
            int difficulty = 5;

            // Make text color white and background black
            Console.Write("\u001b[38;5;231m");
            Console.Write("\u001b[48;5;0m");

            ConsoleWriteFileContents(TITLE_PATH);
            ConsoleWriteFileContents(INSTRUCTION_PATH);

            // Set the boad size for both the users board and comouters board
            UserSetBoardSize(ref userBoard, ref computerBoard);

            // Initialise the boards
            GetAndPlaceBoats(ref userBoard, ref computerBoard);

            // User places their boats
            PlaceBoats(ref userBoard);

            while (true)
            {
                // The computer gets additional turns based on the difficulty
                for (int i = 0; i <= difficulty; i++)
                {
                    ComputersTurn(ref userBoard);
                }

                System.Environment.Exit(0);

                UsersTurn(ref computerBoard);
            }
        }

        static void UsersTurn(ref Board computerBoard)
        {
            int x;
            int y;
            bool targetting = true;
            ConsoleKeyInfo keyInput;

            Console.Clear();
            computerBoard.DisplayBoard(false);

            do
            {
                x = computerBoard.width / 2;
                y = computerBoard.height / 2;
                targetting = true;

                Console.Clear();
                computerBoard.UpdateBoard();
                //computerBoard.boardPositions[x, y].Highlight(true);
                computerBoard.DisplayBoard(false);

                while (targetting)
                {

                    if (Console.KeyAvailable)
                    {
                        Console.Clear();

                        keyInput = Console.ReadKey();

                        switch (keyInput.Key)
                        {
                            case ConsoleKey.UpArrow:
                                y = Math.Min(y + 1, computerBoard.height - 1);
                                break;
                            case ConsoleKey.DownArrow:
                                y = Math.Max(y - 1, 0);
                                break;
                            case ConsoleKey.RightArrow:
                                x = Math.Min(x + 1, computerBoard.width - 1);
                                break;
                            case ConsoleKey.LeftArrow:
                                x = Math.Max(x - 1 , 0);
                                break;
                            case ConsoleKey.F:
                                if (!computerBoard.IsHitPosition(x, y)) { targetting = false; }
                                break;
                        }

                        Console.Clear();
                        computerBoard.UpdateBoard();
                        //computerBoard.boardPositions[x, y].Highlight(true);
                        computerBoard.DisplayBoard(false);

                        // Clears backedup keys
                        while (Console.KeyAvailable) { Console.ReadKey(); }
                    }
                }

                Thread.Sleep(MILLISECOND_DELAY_AFTER_FIRE);

                Console.Clear();
                computerBoard.UpdateBoard();
                //computerBoard.boardPositions[x, y].Highlight(true);
                computerBoard.DisplayBoard(false);

            } while (computerBoard.FireAt(x, y));
        }

        static void ComputersTurn(ref Board userBoard)
        {
            Random randomGenerator = new Random();
            int x;
            int y;

            // Keeps looping aslong as the computer keeps hitting boats
            do
            {
                // Loops until a random non-hit position is chosen
                // TODO: Change so it does do so many redundent loops
                do
                {
                    // Randomly choose a position on the board
                    x = randomGenerator.Next(0, userBoard.width);
                    y = randomGenerator.Next(0, userBoard.height);
                } while (userBoard.IsHitPosition(x, y));

                // Displays the board after the board has been hit
                DisplayCurrentBoard(userBoard, false);
                Thread.Sleep(MILLISECOND_DELAY_AFTER_FIRE);

            } while (userBoard.FireAt(x, y));
        }

        // TODO: Redo
        static void GetAndPlaceBoats(ref Board userBoard, ref Board computerBoard)
        {
            int[] numberOfShipType = new int[5];
            Console.Write("How many crusiers do you want: ");
            numberOfShipType[0] = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many destroyers do you want: ");
            numberOfShipType[1] = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many submarines do you want: ");
            numberOfShipType[2] = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many battle ships do you want: ");
            numberOfShipType[3] = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many carriers do you want: ");
            numberOfShipType[4] = Convert.ToInt32(Console.ReadLine());

            Boat[] userBoats = new Boat[numberOfShipType.Sum()];
            Boat[] computerBoats = new Boat[numberOfShipType.Sum()];

            int currNum = -1;

            for (int i = 0; i < numberOfShipType[0]; i++)
            {
                currNum++;
                userBoats[currNum] = new Cruiser((0,0));
                computerBoats[currNum] = new Cruiser((0, 0));
            }
            for (int i = 0; i < numberOfShipType[1]; i++)
            {
                currNum++;
                userBoats[currNum] = new Destroyer((0, 0));
                computerBoats[currNum] = new Destroyer((0, 0));
            }
            for (int i = 0; i < numberOfShipType[2]; i++)
            {
                currNum++;
                userBoats[currNum] = new Submarine((0, 0));
                computerBoats[currNum] = new Submarine((0, 0));
            }
            for (int i = 0; i < numberOfShipType[3]; i++)
            {
                currNum++;
                userBoats[currNum] = new BattleShip((0, 0));
                computerBoats[currNum] = new BattleShip((0, 0));
            }
            for (int i = 0; i < numberOfShipType[4]; i++)
            {
                currNum++;
                userBoats[currNum] = new Carrier((0, 0));
                computerBoats[currNum] = new Carrier((0, 0));
            }

            userBoard.RandomlyAddBoats(userBoats);
            computerBoard.RandomlyAddBoats(computerBoats);
        }

        // TODO: Redo the higlighting system because it is shit
        static void PlaceBoats(ref Board board)
        {
            ConsoleKeyInfo keyInput;
            int currentMovingBoatIndex = 0;
            bool placingBoats = true;

            board.HighlightBoat(currentMovingBoatIndex);

            DisplayCurrentBoard(board, true);

            while (placingBoats)
            {
                if (Console.KeyAvailable)
                {
                    keyInput = Console.ReadKey(true);

                    switch (keyInput.Key)
                    {
                        // Arrow keys used to control the movement of the boats
                        case ConsoleKey.UpArrow:
                            board.boats[currentMovingBoatIndex].ShiftBoat(-1, 0);
                            break;
                        case ConsoleKey.DownArrow:
                            board.boats[currentMovingBoatIndex].ShiftBoat(1, 0);
                            break;
                        case ConsoleKey.RightArrow:
                            board.boats[currentMovingBoatIndex].ShiftBoat(0, 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            board.boats[currentMovingBoatIndex].ShiftBoat(0, -1);
                            break;
                        // R key used to rotate the current boat
                        case ConsoleKey.R:
                            board.boats[currentMovingBoatIndex].rotateBoat();
                            break;
                        // If the currently selected boat is in a valid position (not overlapping with another boat) then switch boat
                        case ConsoleKey.S:
                            if (board.BoatsInValidPosition())
                            {
                                currentMovingBoatIndex++;
                                currentMovingBoatIndex %= board.boats.Count;
                            }
                            break;
                        case ConsoleKey.A:
                            if (board.BoatsInValidPosition())
                            {
                                currentMovingBoatIndex--;
                                currentMovingBoatIndex %= board.boats.Count;
                                // In case the current index becomes negative
                                currentMovingBoatIndex = currentMovingBoatIndex < 0 ? currentMovingBoatIndex + board.boats.Count : currentMovingBoatIndex;
                            }
                            break;

                        // Finish the process of placing your boats
                        case ConsoleKey.D:
                            placingBoats = false;
                            break;
                    }

                    // Updates the currently highlighted boat
                    board.ClearBoardPositionsHighlights();
                    board.HighlightBoat(currentMovingBoatIndex);

                    // Clears backedup keys
                    while (Console.KeyAvailable) { Console.ReadKey(); }

                    DisplayCurrentBoard(board, true);
                }

                board.ClearBoardPositionsHighlights();
            }
        }

        // TODO: Only here for debugging purposes
        static void DisplayCurrentBoard(Board board, bool revealed)
        {
            Console.Clear();
            ConsoleWriteFileContents(TITLE_PATH);
            Console.WriteLine();
            board.UpdateBoard();
            Console.WriteLine(board.DisplayBoard(revealed));
            ConsoleWriteFileContents(INSTRUCTION_PATH);
            Console.WriteLine();
        }

        static void UserSetBoardSize(ref Board userBoard, ref Board computerBoard)
        {
            int width = 0;
            int height = 0;

            Console.Write("Enter the width of the board: ");
            width = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the height of the board: ");
            height = Convert.ToInt32(Console.ReadLine());

            computerBoard = new Board(width, height, "Computers board");
            userBoard = new Board(width, height, "Your board");
        }

        static void ConsoleWriteFileContents(string path)
        {
            // !COLORF is used to indicate the use of an ansi colour to colour the foreground
            // !COLORB is used to indicate the use of an ansi colour to colour the background

            string text = string.Empty;
            using (StreamReader fileReader = new StreamReader(path))
            {
                text = fileReader.ReadToEnd();
                text = text.Replace("!COLORF", "\u001b[38;5;");
                text = text.Replace("!COLORB", "\u001b[48;5;");
                Console.WriteLine(text);
            }
        }
    }
}