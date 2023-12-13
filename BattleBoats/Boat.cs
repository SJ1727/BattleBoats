using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Boat
    {
        public List<(int x, int y)> boatTilesPositions;
        char hitSymbol;
        char revealedSymbol;
        char hiddenSymbol;
        bool destoryed = false;

        public Boat (List<(int x, int y)> boatTilesPositions, char hitSymbol, char revealedSymbol, char hiddenSymbol)
        {
            this.boatTilesPositions = boatTilesPositions;
            this.hitSymbol = hitSymbol;
            this.revealedSymbol = revealedSymbol;
            this.hiddenSymbol = hiddenSymbol;
        }

        public List<BoardPosition> BoatPositionsToBoardPositions()
        {
            List<BoardPosition> boatBoardPositions = new List<BoardPosition>();

            foreach ((int x, int y) position in this.boatTilesPositions)
            {
                boatBoardPositions.Add(new BoardPosition(position.x, position.y, hitSymbol, revealedSymbol, hiddenSymbol));
            }

            return boatBoardPositions;
        }


        // Returns true if provided position if part of the boat, if not return false
        public bool PositionPartOfBoat(int x, int y)
        {
            foreach((int x, int y) position in boatTilesPositions)
            {
                if (position.x == x && position.y == y) { return true; } 
            }

            return false;
        }

        // Returns true if boat overlaps with other boat, if not return false
        public bool Overlap(Boat otherBoat)
        {
            foreach ((int x, int y) position in this.boatTilesPositions)
            {
                if (otherBoat.PositionPartOfBoat(position.x, position.y)) { return true; }
            }

            return false;
        } 

        // Return true if position is in bounds, returns false otherwise
        // Bounds are inclusive of the lower bound and are exclusive of the upper bound
        public bool InsideBounds(int widthUpper, int heightUpper, int widthLower=0, int heightLower=0)
        {
            foreach ((int x, int y) position in this.boatTilesPositions)
            {
                if (position.x < widthLower || position.x >= widthUpper) { return false; }
                if (position.y < heightLower || position.y >= heightUpper) { return false; }
            }

            return true;
        }
    }
}
