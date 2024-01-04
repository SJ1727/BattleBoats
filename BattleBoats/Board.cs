using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
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

        public Board(int width, int height, string title = "")
        {
            this.height = height;
            this.width = width;
            this.title = title;
            this.boardPositions = new BoardPosition[this.height, this.width];
            this.emptyHitSymbol = emptyHitSymbolColor.ColorCharForeground('M');
            this.emptyRevealedSymbol = emptyRevealedSymbolColor.ColorCharForeground('.');
            this.emptyHiddenSymbol = emptyHiddenSymbolColor.ColorCharForeground('.');

            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    this.boardPositions[j, i] = new BoardPosition(
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

        public void ClearBoardPositionsHighlights()
        {
            highlightedBoardPositions.Clear();
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
                        randomGenerator.Next(0, this.width + Boat.MAX_BOAT_SIZE),
                        randomGenerator.Next(0, this.height + Boat.MAX_BOAT_SIZE)
                    ));
                } while (!this.AddBoat(boat));
            }
        }

        public bool AddBoats(Boat[] boats)
        {
            foreach (Boat boat in boats)
            {
                if (!this.AddBoat(boat))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AddBoat(Boat newBoat)
        {
            foreach (Boat boat in boats)
            {
                if (boat.Overlap(newBoat)) { return false; }
            }

            if (!newBoat.InsideBounds(this.width, this.height)) { return false; }

            this.boats.Add(newBoat);

            // Updates Boards position tile to have the properties of the boat
            this.PlaceBoatOnBoard(newBoat);

            return true;
        }

        public bool BoatsInValidPosition()
        {
            for (int i = 0; i < this.boats.Count; i++)
            {
                for (int j = i + 1; j < this.boats.Count; j++)
                {
                    if (this.boats[i].Overlap(this.boats[j])) { return false; }
                }
            }

            return true;
        }

        public bool FireAt(int x, int y)
        {
            boardPositions[y, x].hit = true;

            foreach (Boat boat in boats)
            {
                if (boat.FireAt(x, y))
                {
                    boat.IsDestroyed();
                    return true;
                }
            }

            return false;
        }

        public bool IsHitPosition(int x, int y)
        {
            return this.boardPositions[y, x].hit;
        }

        private void PlaceBoatOnBoard(Boat newBoat)
        {
            foreach (BoardPosition boatBoardPosition in newBoat.boatTilesPositions)
            {
                boardPositions[boatBoardPosition.y, boatBoardPosition.x] = boatBoardPosition;
            }
        }

        public bool ShiftBoat(int index, int dx, int dy)
        {
            this.boats[index].ShiftBoat(dx, dy);

            // Check if boat is in valid position
            if (!this.boats[index].InsideBounds(this.width, this.height))
            {
                this.boats[index].ShiftBoat(-dx, -dy);
                return false;
            }

            return true;
        }

        public bool RotateBoat(int index)
        {
            this.boats[index].rotateBoat();

            // Check if boat is in valid position
            if (!this.boats[index].InsideBounds(this.width, this.height))
            {
                for (int i = 0; i < 3; i++)
                {
                    this.boats[index].rotateBoat();
                }
                return false;
            }

            return true;
        }

        public string DisplayBoard(bool revealed)
        {
            string boardDisplay = string.Empty;
            string tileToPlace = string.Empty;

            this.UpdateBoard();

            boardDisplay += $" ---{this.title} Board---\n";
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    tileToPlace = this.boardPositions[j, i].CurrentSymbol(revealed);
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

        public bool AllBoatsDestroyed()
        {
            foreach (Boat boat in this.boats)
            {
                if (!boat.IsDestroyed()) { return false; }
            }

            return true;
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    boardPositions[j, i] = new BoardPosition(
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
    }

    internal class BoardPosition
    {
        public int x;
        public int y;
        string hitSymbol;
        string revealedSymbol;
        string hiddenSymbol;
        public bool hit;

        public BoardPosition(int x, int y, string hitSymbol, string revealedSymbol, string hiddenSymbol, bool hit)
        {
            this.x = x;
            this.y = y;
            this.hitSymbol = hitSymbol;
            this.revealedSymbol = revealedSymbol;
            this.hiddenSymbol = hiddenSymbol;
            this.hit = hit;
        }

        // Checks if the position of the tile is equal to the provided position
        public bool EqualPosition(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        // Return the symbol of the board position based on its hit state and whether the position is supposed
        // to be revealed to the player (e.g. when the player is placing their own boats)
        public string CurrentSymbol(bool revealed)
        {
            if (revealed) { return revealedSymbol; }

            if (this.hit) { return hitSymbol; }

            return this.hiddenSymbol;
        }

        public void ChangeHitSymbol(string hitSymbol)
        {
            this.hitSymbol = hitSymbol;
        }
    }
}
