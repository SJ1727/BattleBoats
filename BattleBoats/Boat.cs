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
        protected (int x, int y)[] boatTilesPositions;
        protected (int x, int y) headPosition;
        protected Symbol hitSymbol;
        protected Symbol revealedSymbol;
        protected Symbol hiddenSymbol;
        bool destoryed = false;

        public List<BoardPosition> BoatPositionsToBoardPositions()
        {
            List<BoardPosition> boatBoardPositions = new List<BoardPosition>();

            foreach ((int x, int y) position in this.boatTilesPositions)
            {
                boatBoardPositions.Add(new BoardPosition(
                    position.x,
                    position.y,
                    this.hitSymbol,
                    this.revealedSymbol,
                    this.hiddenSymbol
                    )
                );
            }

            return boatBoardPositions;
        }

        public void Highlight(bool highligh, ConsoleColor highlightColor=ConsoleColor.White)
        {
            if ( highligh ) { revealedSymbol.backgroundColor = highlightColor; }
            else { revealedSymbol.backgroundColor = ConsoleColor.Black; }
        }


        // Returns true if provided position if part of the boat, if not return false
        public bool PositionPartOfBoat(int x, int y)
        {
            foreach ((int x, int y) position in boatTilesPositions)
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

        public void ShiftBoat(int xShift, int yShift)
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                this.boatTilesPositions[i] = (this.boatTilesPositions[i].x + xShift, this.boatTilesPositions[i].y + yShift);
            }

            this.headPosition = this.boatTilesPositions[0];
        }


        // Sets the boat to a new position
        public void setBoatPosition((int x, int y) position)
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                this.boatTilesPositions[i] = (
                    position.x + (this.headPosition.x - this.boatTilesPositions[i].x),
                    position.y + (this.headPosition.y - this.boatTilesPositions[i].y)
                );
            }

            this.headPosition = this.boatTilesPositions[0];
        }

        public void rotateBoat()
        {
            for (int i = 0; i < this.boatTilesPositions.Length; i++)
            {
                this.boatTilesPositions[i] = (
                    this.headPosition.x - (this.headPosition.y - this.boatTilesPositions[i].y),
                    this.headPosition.y + (this.headPosition.x - this.boatTilesPositions[i].x)
                );
            }

            this.headPosition = this.boatTilesPositions[0];
        }
    }

    internal class Destroyer : Boat
    {
        public const ConsoleColor DESTROYER_COLOR = ConsoleColor.DarkGreen;
        public const int DESTROYER_SIZE = 2;

        public Destroyer((int x, int y) headPosition)
        {
            this.headPosition = headPosition;
            this.hitSymbol = new Symbol('■', ConsoleColor.Red);
            this.hiddenSymbol = new Symbol('.', ConsoleColor.Blue);
            this.revealedSymbol = new Symbol('■', Destroyer.DESTROYER_COLOR);
            this.boatTilesPositions = new (int x, int y)[]{
                (headPosition.x, headPosition.y),
                (headPosition.x, headPosition.y + 1),
            };
        }
    }

    internal class Cruiser : Boat
    {
        public const ConsoleColor CRUSIER_COLOR = ConsoleColor.Gray;
        public const int CRUSIER_SIZE = 3;

        public Cruiser((int x, int y) headPosition)
        {
            this.headPosition = headPosition;
            this.hitSymbol = new Symbol('■', ConsoleColor.Red);
            this.hiddenSymbol = new Symbol('.', ConsoleColor.Blue);
            this.revealedSymbol = new Symbol('■', Cruiser.CRUSIER_COLOR);
            this.boatTilesPositions = new (int x, int y)[]{
                (headPosition.x, headPosition.y),
                (headPosition.x, headPosition.y - 1),
                (headPosition.x, headPosition.y + 1),
            };
        }
    }

    internal class Submarine : Boat
    {
        public const ConsoleColor SUBMARINE_COLOR = ConsoleColor.DarkCyan;
        public const int SUBMARINE_SIZE = 3;

        public Submarine((int x, int y) headPosition)
        {
            this.headPosition = headPosition;
            this.hitSymbol = new Symbol('■', ConsoleColor.Red);
            this.hiddenSymbol = new Symbol('.', ConsoleColor.Blue);
            this.revealedSymbol = new Symbol('■', Submarine.SUBMARINE_COLOR);
            this.boatTilesPositions = new (int x, int y)[]{
                (headPosition.x, headPosition.y),
                (headPosition.x, headPosition.y - 1),
                (headPosition.x, headPosition.y + 1),
            };
        }
    }

    internal class BattleShip : Boat
    {
        public const ConsoleColor BATTLE_SHIP_COLOR = ConsoleColor.DarkBlue;
        public const int BATTLE_SHIP_SIZE = 4;

        public BattleShip((int x, int y) headPosition)
        {
            this.headPosition = headPosition;
            this.hitSymbol = new Symbol('■', ConsoleColor.Red);
            this.hiddenSymbol = new Symbol('.', ConsoleColor.Blue);
            this.revealedSymbol = new Symbol('■', BattleShip.BATTLE_SHIP_COLOR);
            this.boatTilesPositions = new (int x, int y)[]{
                (headPosition.x, headPosition.y),
                (headPosition.x, headPosition.y - 1),
                (headPosition.x, headPosition.y + 1),
                (headPosition.x, headPosition.y + 2),
            };
        }
    }

    internal class Carrier : Boat
    {
        public const ConsoleColor CARRIER_COLOR = ConsoleColor.DarkGray;
        public const int CARRIER_SIZE = 5;

        public Carrier((int x, int y) headPosition)
        {
            this.headPosition = headPosition;
            this.hitSymbol = new Symbol('■', ConsoleColor.Red);
            this.hiddenSymbol = new Symbol('.', ConsoleColor.Blue);
            this.revealedSymbol = new Symbol('■', Carrier.CARRIER_COLOR);
            this.boatTilesPositions = new (int x, int y)[]{
                (headPosition.x, headPosition.y),
                (headPosition.x, headPosition.y - 2),
                (headPosition.x, headPosition.y - 1),
                (headPosition.x, headPosition.y + 1),
                (headPosition.x, headPosition.y + 2),
            };
        }
    }
}
