using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class AnsiColor
    {
        private const int LOWER_BOUND = 16;
        private int red;
        private int green;
        private int blue;
        private int val;

        // red, green, and blue should be in range [0,5]
        public AnsiColor (int red, int green, int blue)
        {
            // Assertion to check range of provided colors
            Debug.Assert(red >= 0 && red <= 5  && green >= 0 && green <= 5 && blue >= 0 && blue <= 5);

            this.red = red;
            this.green = green;
            this.blue = blue;
            this.val = LOWER_BOUND + red * 36 + green * 6 + blue;
        }

        public string ColorStringForeground(string uncoloredString)
        {
            return $"\u001b[38;5;{this.val}m" + uncoloredString + "\u001b[38;5;231m";
        }
        public string ColorStringBackground(string uncoloredString)
        {
            return $"\u001b[48;5;{this.val}m" + uncoloredString + "\u001b[48;5;0m";
        }
        public string ColorCharForeground(char uncoloredChar)
        {
            return $"\u001b[38;5;{this.val}m" + uncoloredChar + "\u001b[38;5;231m";
        }
        public string ColorCharBackground(char uncoloredChar)
        {
            return $"\u001b[48;5;{this.val}m" + uncoloredChar + "\u001b[48;5;0m";
        }
    }
}
