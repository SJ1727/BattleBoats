using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal abstract class Player
    {
        public string name;
        public Board ownBoard;
        protected const int MILLISECOND_DELAY_AFTER_FIRE = 500;

        public abstract bool playMove(ref Player opponent);

        public abstract void initialiseBoard(Boat[] boats);

        public void setBoard(Board newBoard)
        {
            this.ownBoard = newBoard;
            this.name = newBoard.title;
        }
    }

    internal class ComputerPlayer : Player
    {
        public ComputerPlayer(string name, Board? board)
        {
            this.name = name;
            this.ownBoard = board;
        }

        public override bool playMove(ref Player opponent)
        {
            Random randomGenerator = new Random();
            bool boatHit = false;
            int x;
            int y;

            Display.DisplayBoardFiring(opponent.ownBoard);
            Thread.Sleep(MILLISECOND_DELAY_AFTER_FIRE);

            do
            {
                // Randomly choose a position on the board
                x = randomGenerator.Next(0, opponent.ownBoard.width);
                y = randomGenerator.Next(0, opponent.ownBoard.height);
            } while (opponent.ownBoard.IsHitPosition(x, y));


            boatHit = opponent.ownBoard.FireAt(x, y);

            Display.DisplayBoardFiring(opponent.ownBoard);
            Thread.Sleep(MILLISECOND_DELAY_AFTER_FIRE);

            return boatHit;
        }

        public override void initialiseBoard(Boat[] boats)
        {
            ownBoard.RandomlyAddBoats(boats);
        }
    }

    internal class HumanPlayer : Player
    {
        public HumanPlayer(string name, Board? board)
        {
            this.name = name;
            this.ownBoard = board;
        }

        public override bool playMove(ref Player opponent)
        {
            int x;
            int y;
            bool boatHit = false;
            bool targeting = true;
            ConsoleKeyInfo keyInput;

            x = opponent.ownBoard.width / 2;
            y = opponent.ownBoard.height / 2;
            targeting = true;

            Console.Clear();
            opponent.ownBoard.ClearBoardPositionsHighlights();
            opponent.ownBoard.HighlightBoardPosition(x, y);
            Display.DisplayBoardFiring(opponent.ownBoard);


            while (targeting && !opponent.ownBoard.AllBoatsDestroyed())
            {

                if (Console.KeyAvailable)
                {
                    Console.Clear();

                    keyInput = Console.ReadKey();

                    switch (keyInput.Key)
                    {
                        case ConsoleKey.UpArrow:
                            y = Math.Max(y - 1, 0);
                            break;
                        case ConsoleKey.DownArrow:
                            y = Math.Min(y + 1, opponent.ownBoard.height - 1);
                            break;
                        case ConsoleKey.RightArrow:
                            x = Math.Min(x + 1, opponent.ownBoard.width - 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            x = Math.Max(x - 1, 0);
                            break;
                        case ConsoleKey.F:
                            if (!opponent.ownBoard.IsHitPosition(x, y)) { targeting = false; }
                            break;
                    }

                    Console.Clear();
                    opponent.ownBoard.ClearBoardPositionsHighlights();
                    opponent.ownBoard.HighlightBoardPosition(x, y);
                    Display.DisplayBoardFiring(opponent.ownBoard);

                    // Clears backed up keys
                    while (Console.KeyAvailable) { Console.ReadKey(); }
                }
            }

            Thread.Sleep(MILLISECOND_DELAY_AFTER_FIRE);

            boatHit = opponent.ownBoard.FireAt(x, y);

            opponent.ownBoard.ClearBoardPositionsHighlights();
            Display.DisplayBoardFiring(opponent.ownBoard);

            return boatHit;
        }

        public override void initialiseBoard(Boat[] boats)
        {
            ownBoard.RandomlyAddBoats(boats);

            ConsoleKeyInfo keyInput;
            int currentMovingBoatIndex = 0;
            bool placingBoats = true;

            ownBoard.HighlightBoat(currentMovingBoatIndex);

            Display.DisplayBoardPlacingBoats(ownBoard);

            while (placingBoats)
            {
                if (Console.KeyAvailable)
                {
                    keyInput = Console.ReadKey(true);

                    switch (keyInput.Key)
                    {
                        // Arrow keys used to control the movement of the boats
                        case ConsoleKey.UpArrow:
                            ownBoard.ShiftBoat(currentMovingBoatIndex, 0, -1);
                            break;
                        case ConsoleKey.DownArrow:
                            ownBoard.ShiftBoat(currentMovingBoatIndex, 0, 1);
                            break;
                        case ConsoleKey.RightArrow:
                            ownBoard.ShiftBoat(currentMovingBoatIndex, 1, 0);
                            break;
                        case ConsoleKey.LeftArrow:
                            ownBoard.ShiftBoat(currentMovingBoatIndex, -1, 0);
                            break;
                        // R key used to rotate the current boat
                        case ConsoleKey.R:
                            ownBoard.RotateBoat(currentMovingBoatIndex);
                            break;
                        // If the currently selected boat is in a valid position (not overlapping with another boat) then switch boat
                        case ConsoleKey.S:
                            if (ownBoard.BoatsInValidPosition())
                            {
                                currentMovingBoatIndex++;
                                currentMovingBoatIndex %= ownBoard.boats.Count;
                            }
                            break;
                        case ConsoleKey.A:
                            if (ownBoard.BoatsInValidPosition())
                            {
                                currentMovingBoatIndex--;
                                currentMovingBoatIndex %= ownBoard.boats.Count;
                                // In case the current index becomes negative
                                currentMovingBoatIndex = currentMovingBoatIndex < 0 ? currentMovingBoatIndex + ownBoard.boats.Count : currentMovingBoatIndex;
                            }
                            break;

                        // Finish the process of placing your boats
                        case ConsoleKey.D:
                            if (ownBoard.BoatsInValidPosition()) { placingBoats = false; }
                            break;
                    }

                    // Updates the currently highlighted boat
                    ownBoard.ClearBoardPositionsHighlights();
                    ownBoard.HighlightBoat(currentMovingBoatIndex);

                    // Clears backed up keys
                    while (Console.KeyAvailable) { Console.ReadKey(); }

                    Display.DisplayBoardPlacingBoats(ownBoard);
                }

                ownBoard.ClearBoardPositionsHighlights();
            }
        }
    }
}
