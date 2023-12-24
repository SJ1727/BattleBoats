using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Board
    {
        public List<Boat> boats = new List<Boat>();
        private BoardPosition[,] boardPositions;
        private List<(int x, int y)> highlightedBoardPositions = new List<(int x, int y)>();
        public int height;
        public int width;
        public string title;
        string emptyHitSymbol;
        string emptyRevealedSymbol;
        string emptyHiddenSymbol;
        AnsiColor emptyHitSymbolColor = new AnsiColor(5, 5, 5);
        AnsiColor emptyRevealedSymbolColor = new AnsiColor(0, 0, 4);
        AnsiColor emptyHiddenSymbolColor = new AnsiColor(0, 0, 4);
        AnsiColor HighlightColor = new AnsiColor(5, 5, 5);

        public Board(int width, int height, string title)
        {
            this.height = height;
            this.width = width;
            this.title = title;
            this.boardPositions = new BoardPosition[this.height, this.width];
            this.emptyHitSymbol = emptyHitSymbolColor.ColorCharForeground('M');
            this.emptyRevealedSymbol = emptyRevealedSymbolColor.ColorCharForeground('.');
            this.emptyHiddenSymbol = emptyHiddenSymbolColor.ColorCharForeground('.');

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    this.boardPositions[i, j] = new BoardPosition(
                        i,
                        j,
                        this.emptyHitSymbol,
                        this.emptyRevealedSymbol,
                        this.emptyHiddenSymbol,
                        false
                    );
                }
            }

            this.title = title;
        }

        public void HighlightBoat(int index)
        {
            // Assure index is in range
            Debug.Assert(index >= 0 && index < this.boats.Count);

            foreach (BoardPosition boatBoardPosition in this.boats[index].boatTilesPositions)
            {
                this.HighlightBoardPosition(boatBoardPosition.x, boatBoardPosition.y);
            }
        }

        public void HighlightBoardPosition(int x, int y)
        {
            highlightedBoardPositions.Add((x, y));
        }

        public void ClearBoardPositionsHighlights() { highlightedBoardPositions.Clear(); }

        public bool IsHitPosition(int x, int y)
        {
            return this.boardPositions[x, y].hit;
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
                        this.emptyHiddenSymbol,
                        this.IsHitPosition(i, j)
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
                if (boat.FireAt(x, y)) { return true; }
            }

            return false;
        }

        private void PlaceBoatOnBoard(Boat newBoat)
        {
            foreach (BoardPosition boatBoardPosition in newBoat.boatTilesPositions)
            {
                boardPositions[boatBoardPosition.x, boatBoardPosition.y] = boatBoardPosition;
            }
        }

        public string DisplayBoard(bool revealed)
        {
            string boardDisplay = string.Empty;
            string tileToPlace = string.Empty;

            boardDisplay += $" ---{this.title}---\n";
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    tileToPlace = this.boardPositions[i, j].CurrentSymbol(revealed);
                    if (this.highlightedBoardPositions.Contains((i, j)))
                    {
                        tileToPlace = HighlightColor.ColorStringBackground(tileToPlace);
                    }
                    boardDisplay += "  ";
                    boardDisplay += tileToPlace;
                }
                boardDisplay += "\n\n";
            }

            return boardDisplay;
        }
    }
}
