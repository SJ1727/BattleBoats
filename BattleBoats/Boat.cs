using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{

    internal abstract class Boat
    {
        public const int MAX_BOAT_SIZE = 5;
        public BoardPosition[] boatTilesPositions;
        protected BoardPosition headPosition;
        protected string hitSymbol;
        protected string revealedSymbol;
        protected string hiddenSymbol;
        AnsiColor hitSymbolColor;
        AnsiColor revealedSymbolColor;
        AnsiColor hiddenSymbolColor;
        bool destoryed = false;

        // Returns true if provided position if part of the boat, if not return false
        public bool PositionPartOfBoat(int x, int y)
        {
            foreach (BoardPosition position in boatTilesPositions)
            {
                if (position.x == x && position.y == y) { return true; } 
            }

            return false;
        }

        // Returns true if boat overlaps with other boat, if not return false
        public bool Overlap(Boat otherBoat)
        {
            foreach (BoardPosition position in this.boatTilesPositions)
            {
                if (otherBoat.PositionPartOfBoat(position.x, position.y)) { return true; }
            }

            return false;
        } 

        // Return true if position is in bounds, returns false otherwise
        // Bounds are inclusive of the lower bound and are exclusive of the upper bound
        public bool InsideBounds(int widthUpper, int heightUpper, int widthLower=0, int heightLower=0)
        {
            foreach (BoardPosition position in this.boatTilesPositions)
            {
                if (position.x < widthLower || position.x >= widthUpper) { return false; }
                if (position.y < heightLower || position.y >= heightUpper) { return false; }
            }

            return true;
        }

        // Moves boat by x and y shit values
        public void ShiftBoat(int xShift, int yShift)
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                this.boatTilesPositions[i] = new BoardPosition(
                    this.boatTilesPositions[i].x + xShift,
                    this.boatTilesPositions[i].y + yShift,
                    this.hitSymbol,
                    this.revealedSymbol,
                    this.hiddenSymbol,
                    this.boatTilesPositions[i].hit
                    );
            }

            this.headPosition = this.boatTilesPositions[0];
        }


        // Sets the boat to a new position
        public void setBoatPosition((int x, int y) position)
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                this.boatTilesPositions[i] = new BoardPosition(
                    position.x + (this.headPosition.x - this.boatTilesPositions[i].x),
                    position.y + (this.headPosition.y - this.boatTilesPositions[i].y),
                    this.hitSymbol,
                    this.revealedSymbol,
                    this.hiddenSymbol,
                    this.boatTilesPositions[i].hit
                );
            }

            this.headPosition = this.boatTilesPositions[0];
        }

        // Rotatets the boat 90 degrees clockwise
        public void rotateBoat()
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                this.boatTilesPositions[i] = new BoardPosition (
                    this.headPosition.x - (this.headPosition.y - this.boatTilesPositions[i].y),
                    this.headPosition.y + (this.headPosition.x - this.boatTilesPositions[i].x),
                    this.hitSymbol,
                    this.revealedSymbol,
                    this.hiddenSymbol,
                    this.boatTilesPositions[i].hit
                );
            }

            this.headPosition = this.boatTilesPositions[0];
        }

        // Checks if given positon is part of the boat, updates the hit states accordingly, returns true if hits boat otherwise return false
        public bool FireAt(int x, int y)
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                if (this.boatTilesPositions[i].EqualPosition(x, y)) { 
                    this.boatTilesPositions[i].hit = true;
                    return true;
                }
            }
            return false;
        }

        public BoardPosition GetBoatBoardPosition(int x, int y)
        {
            foreach (BoardPosition boatBoardPosition in this.boatTilesPositions)
            {
                if (boatBoardPosition.EqualPosition(x, y)) { return boatBoardPosition; }
            }

            return null;
        }
    }

    internal class Destroyer : Boat
    {
        AnsiColor hitSymbolColor = new AnsiColor(4, 1, 1);
        AnsiColor revealedSymbolColor = new AnsiColor(2, 0, 0);
        AnsiColor hiddenSymbolColor = new AnsiColor(0, 0, 4);

        public Destroyer((int x, int y) headPosition)
        {
            this.hitSymbol = hitSymbolColor.ColorCharForeground('■');
            this.hiddenSymbol = hiddenSymbolColor.ColorCharForeground('.');
            this.revealedSymbol = revealedSymbolColor.ColorCharForeground('■');
            this.boatTilesPositions = new BoardPosition[]{
                new BoardPosition(headPosition.x, headPosition.y, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
            };
            this.headPosition = boatTilesPositions[0];
        }
    }

    internal class Cruiser : Boat
    {
        AnsiColor hitSymbolColor = new AnsiColor(4, 1, 1);
        AnsiColor revealedSymbolColor = new AnsiColor(0, 2, 0);
        AnsiColor hiddenSymbolColor = new AnsiColor(0, 0, 4);

        public Cruiser((int x, int y) headPosition)
        {
            this.hitSymbol = hitSymbolColor.ColorCharForeground('■');
            this.hiddenSymbol = hiddenSymbolColor.ColorCharForeground('.');
            this.revealedSymbol = revealedSymbolColor.ColorCharForeground('■');
            this.boatTilesPositions = new BoardPosition[]{
                new BoardPosition(headPosition.x, headPosition.y, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y - 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
            };
            this.headPosition = boatTilesPositions[0];
        }
    }

    internal class Submarine : Boat
    {
        AnsiColor hitSymbolColor = new AnsiColor(4, 1, 1);
        AnsiColor revealedSymbolColor = new AnsiColor(0, 0, 2);
        AnsiColor hiddenSymbolColor = new AnsiColor(0, 0, 4);

        public Submarine((int x, int y) headPosition)
        {
            this.hitSymbol = hitSymbolColor.ColorCharForeground('■');
            this.hiddenSymbol = hiddenSymbolColor.ColorCharForeground('.');
            this.revealedSymbol = revealedSymbolColor.ColorCharForeground('■');
            this.boatTilesPositions = new BoardPosition[]{
                new BoardPosition(headPosition.x, headPosition.y, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y - 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
            };
            this.headPosition = boatTilesPositions[0];
        }
    }

    internal class BattleShip : Boat
    {
        AnsiColor hitSymbolColor = new AnsiColor(4, 1, 1);
        AnsiColor revealedSymbolColor = new AnsiColor(2, 2, 2);
        AnsiColor hiddenSymbolColor = new AnsiColor(0, 0, 4);

        public BattleShip((int x, int y) headPosition)
        {
            this.hitSymbol = hitSymbolColor.ColorCharForeground('■');
            this.hiddenSymbol = hiddenSymbolColor.ColorCharForeground('.');
            this.revealedSymbol = revealedSymbolColor.ColorCharForeground('■');
            this.boatTilesPositions = new BoardPosition[]{
                new BoardPosition(headPosition.x, headPosition.y, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 2, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y - 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
            };
            this.headPosition = boatTilesPositions[0];
        }
    }

    internal class Carrier : Boat
    {
        AnsiColor hitSymbolColor = new AnsiColor(4, 1, 1);
        AnsiColor revealedSymbolColor = new AnsiColor(3, 1, 3);
        AnsiColor hiddenSymbolColor = new AnsiColor(0, 0, 4);

        public Carrier((int x, int y) headPosition)
        {
            this.hitSymbol = hitSymbolColor.ColorCharForeground('■');
            this.hiddenSymbol = hiddenSymbolColor.ColorCharForeground('.');
            this.revealedSymbol = revealedSymbolColor.ColorCharForeground('■');
            this.boatTilesPositions = new BoardPosition[]{
                new BoardPosition(headPosition.x, headPosition.y, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y + 2, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y - 1, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
                new BoardPosition(headPosition.x, headPosition.y - 2, this.hitSymbol, this.revealedSymbol, this.hiddenSymbol, false),
            };
            this.headPosition = boatTilesPositions[0];
        }
    }
}
