using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Utils
    {
        public static Boat GetBoatFromData(int boatType, int headX, int headY, int orientation)
        {
            Boat returnBoat = null;

            switch (boatType)
            {
                case 0:
                    returnBoat = new Destroyer((headX, headY));
                    break;
                case 1:
                    returnBoat = new Cruiser((headX, headY));
                    break;
                case 2:
                    returnBoat = new Submarine((headX, headY));
                    break;
                case 3:
                    returnBoat = new BattleShip((headX, headY));
                    break;
                case 4:
                    returnBoat = new Carrier((headX, headY));
                    break;
                default:
                    return null;
            }

            for (int i = 0; i < orientation; i++)
            {
                returnBoat.rotateBoat();
            }

            return returnBoat;
        }
    }
}
