using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBoats
{
    internal class Board
    {
        public List<Boat> boats = new List<Boat>();
        BoardPosition[,] boardPositions;
        public int height;
        public int width;
        Symbol emptyHitSymbol;
        Symbol emptyRevealedSymbol;
        Symbol emptyHiddenSymbol;

        public Board (int width, int height)
        {
            this.height = height;
            this.width = width;
            this.boardPositions = new BoardPosition[this.height, this.width];
            this.emptyHitSymbol = new Symbol('M', ConsoleColor.White);
            this.emptyRevealedSymbol = new Symbol('.', ConsoleColor.Blue);
            this.emptyHiddenSymbol = new Symbol('.', ConsoleColor.Blue);

            this.UpdateBoard();
        }

        public void RandomlyAddBoats(Boat[] boats)
        {
            Random randomGenerator = new Random();

            foreach (Boat boat in boats)
            {
                do
                {
                    if (randomGenerator.Next(0, 2) == 1) { boat.rotateBoat(); }

                    boat.setBoatPosition((
                        randomGenerator.Next(0, this.height + Boat.MAX_BOAT_SIZE),
                        randomGenerator.Next(0, this.width + Boat.MAX_BOAT_SIZE)
                    ));
                } while (!this.AddBoat(boat));
            }
        }

        public bool BoatsInValidPosition()
        {
            for (int i = 0; i < this.boats.Count; i++)
            {
                for (int j = i + 1; j < this.boats.Count; j++)
                {
                    if (this.boats[i].Overlap(this.boats[j])) { return false;  }
                }
            }

            return true;
        }

        // Updates all the positions on the board
        public void UpdateBoard()
        {
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    boardPositions[i, j] = new BoardPosition(
                        i,
                        j,
                        this.emptyHitSymbol,
                        this.emptyRevealedSymbol,
                        this.emptyHiddenSymbol
                    );
                }
            }

            foreach (Boat boat in this.boats)
            {
                this.PlaceBoatOnBoard(boat);
            }
        }

        // Trys to add new boat to the board boats and if successful return true, if the boat overlaps with another
        // boat then false is returned and the boat is not added
        public bool AddBoat(Boat newBoat)
        {
            foreach (Boat boat in boats)
            {
                if (boat.Overlap(newBoat)) {  return false; }
            }

            if (!newBoat.InsideBounds(this.height, this.width)) {  return false; }

            this.boats.Add(newBoat);

            // Updates Boards position tile to have the properties of the boat
            this.PlaceBoatOnBoard(newBoat);

            return true;
        }

        public bool FireAt(int x, int y)
        {
            boardPositions[x, y].hit = true;

            foreach (Boat boat in boats)
            {
                if (boat.PositionPartOfBoat(x, y)) { return true; }
            }

            return false;
        }

        public void DisplayBoard(bool revealed)
        {
            Console.Write(" |");
            for (int i = 0; i < this.width; i++)
            {
                Console.Write(((i + 1) / 10 == 0 ? " " : Convert.ToString((i + 1) / 10)) + "|");
            }

            Console.Write("\n |");

            for (int i = 0; i < this.width; i++)
            {
                Console.Write(Convert.ToString((i + 1) % 10) + "|");
            }


            for (int i = 0; i < this.height; i++)
            {
                Console.WriteLine();
                Console.Write(Convert.ToChar(65 + i) + "|");
                for (int j = 0; j < this.width; j++)
                {
                    boardPositions[i, j].CurrentSymbol(revealed).ConsoleWriteSymbol();
                    Console.Write(" ");
                }
            }
        }

        private void PlaceBoatOnBoard(Boat newBoat)
        {
            foreach (BoardPosition boatBoardPosition in newBoat.BoatPositionsToBoardPositions())
            {
                boardPositions[boatBoardPosition.x, boatBoardPosition.y] = boatBoardPosition;
            }
        }
    }
}
