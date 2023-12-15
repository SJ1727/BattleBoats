using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class BoardPosition
    {
        public int x;
        public int y;
        Symbol hitSymbol;
        Symbol revealedSymbol;
        Symbol hiddenSymbol;
        public bool hit;

        public BoardPosition (int x, int y, Symbol hitSymbol, Symbol revealedSymbol, Symbol hiddenSymbol)
        {
            this.x = x;
            this.y = y;
            this.hitSymbol = hitSymbol;
            this.revealedSymbol = revealedSymbol;
            this.hiddenSymbol = hiddenSymbol;
        }

        // Checks if the position of the tile is equal to the provided position
        public bool EqualPosition(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        // Return the symbol of the board position based on its hit state and whether the position is supposed
        // to be revealed to the player (e.g. when the player is placing their own boats)
        public Symbol CurrentSymbol(bool revealed)
        {
            if (revealed) { return revealedSymbol; }

            if (this.hit) { return hitSymbol; }

            return this.hiddenSymbol;
        }

    }
}
