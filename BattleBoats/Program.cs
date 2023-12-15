namespace BattleBoats
{
    // http://www.patorjk.com/software/taag/#p=display&h=1&v=1&f=ANSI%20Shadow&t=Battle%20Boats

    internal class Program
    {
        const int MAX_BOARD_WIDTH = 26;
        const int MAX_BOARD_HEIGHT = 26;
        const string INSTRUCTION_PATH = @"instructions.txt";

        static void Main(string[] args)
        {
            Board userBoard = null;
            Board computerBoard = null;

            ConsoleWriteFileContents(INSTRUCTION_PATH);
            ConsoleWriteTypesOfMarkers();

            UserSetBoardSize(ref userBoard, ref computerBoard);

            GetAndPlaceBoats(ref userBoard, ref computerBoard);

            PlaceBoats(ref userBoard);


        }

        static void UsersTurn(ref Board userBoard, ref Board computerBoard) { }

        static void ComputersTurn(ref Board userBoard, ref Board computerBoard) { }

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

        static void PlaceBoats(ref Board board)
        {
            ConsoleKeyInfo keyInput;
            int currentMovingBoatIndex = 0;
            bool placingBoats = true;

            board.boats[currentMovingBoatIndex].Highlight(true);

            DisplayCurrentBoard(board);

            while (placingBoats)
            {
                if (Console.KeyAvailable)
                {
                    keyInput = Console.ReadKey(true);

                    switch (keyInput.Key)
                    {
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
                        case ConsoleKey.R:
                            board.boats[currentMovingBoatIndex].rotateBoat();
                            break;
                        case ConsoleKey.S:
                            if (board.BoatsInValidPosition())
                            {
                                board.boats[currentMovingBoatIndex].Highlight(false);
                                currentMovingBoatIndex++;
                                currentMovingBoatIndex %= board.boats.Count;
                                board.boats[currentMovingBoatIndex].Highlight(true);
                            }
                            break;
                        case ConsoleKey.D:
                            board.boats[currentMovingBoatIndex].Highlight(false);
                            placingBoats = false;
                            break;
                    }


                    // Clears backedup keys
                    while (Console.KeyAvailable) { Console.ReadKey(); }

                    DisplayCurrentBoard(board);
                }
            }
        }

        static void DisplayCurrentBoard(Board board)
        {
            Console.Clear();
            ConsoleWriteFileContents(INSTRUCTION_PATH);
            Console.WriteLine();
            board.UpdateBoard();
            board.DisplayBoard(true);
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

            computerBoard = new Board(width, height);
            userBoard = new Board(width, height);
        }

        static void ConsoleWriteFileContents(string path)
        {
            using (StreamReader fileReader = new StreamReader(path))
            {
                Console.Write(fileReader.ReadToEnd());
            }
        }

        static void ConsoleWriteTypesOfMarkers()
        {
            Symbol exampleTile = new Symbol('■', ConsoleColor.White);

            Console.Write("\nDestroyer: ");
            exampleTile.symbolColor = Destroyer.DESTROYER_COLOR;
            for (int i = 0; i < Destroyer.DESTROYER_SIZE; i++)
            {
                exampleTile.ConsoleWriteSymbol();
            }

            Console.Write("\nCrusier: ");
            exampleTile.symbolColor = Cruiser.CRUSIER_COLOR;
            for (int i = 0; i < Cruiser.CRUSIER_SIZE; i++)
            {
                exampleTile.ConsoleWriteSymbol();
            }

            Console.Write("\nSubmarine: ");
            exampleTile.symbolColor = Submarine.SUBMARINE_COLOR;
            for (int i = 0; i < Submarine.SUBMARINE_SIZE; i++)
            {
                exampleTile.ConsoleWriteSymbol();
            }

            Console.Write("\nBattleship: ");
            exampleTile.symbolColor = BattleShip.BATTLE_SHIP_COLOR;
            for (int i = 0; i < BattleShip.BATTLE_SHIP_SIZE; i++)
            {
                exampleTile.ConsoleWriteSymbol();
            }

            Console.Write("\nCrarrier: ");
            exampleTile.symbolColor = Carrier.CARRIER_COLOR;
            for (int i = 0; i < Carrier.CARRIER_SIZE; i++)
            {
                exampleTile.ConsoleWriteSymbol();
            }

            Console.WriteLine();
        }
    }
}