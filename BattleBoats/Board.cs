using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Board
    {
        List<Boat> boats = new List<Boat>();
        BoardPosition[,] boardPositions;
        int width;
        int height;

        public Board (int width, int height)
        {
            this.width = width;
            this.height = height;
            this.boardPositions = new BoardPosition[width, height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    boardPositions[i, j] = new BoardPosition(i, j, 'M', '.', '.');
                }
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

            if (!newBoat.InsideBounds(this.width, this.height)) {  return false; }

            this.boats.Add(newBoat);

            // Updates Boards position tile to have the properties of the boat
            foreach (BoardPosition boatBoardPosition in newBoat.BoatPositionsToBoardPositions())
            {
                boardPositions[boatBoardPosition.x, boatBoardPosition.y] = boatBoardPosition;
            }

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


            for (int i = 0; i < height; i++)
            {
                Console.WriteLine();
                Console.Write(Convert.ToChar(65 + i) + "|");
                for (int j = 0; j < width; j++)
                {
                    Console.Write(boardPositions[i, j].currentSymbol(revealed));
                    Console.Write("|");
                }
            }
        }
    }
}
