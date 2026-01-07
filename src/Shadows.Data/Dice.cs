using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data
{
    public class Dice
    {
        private Random rnd;

        public Dice()
        {
            rnd = new Random();
        }

        public int Get3k6()
        {
            //hod hodnota se urci jako 3x 1k6 - 5 
            int finalVal = 0;

            //kostka 1
            int randomVal1 = rnd.Next(1, 7);
            if (randomVal1 > 6) randomVal1 = 6;

            //kostka 2
            int randomVal2 = rnd.Next(1, 7);
            if (randomVal2 > 6) randomVal2 = 6;

            //kostka 3
            int randomVal3 = rnd.Next(1, 7);
            if (randomVal3 > 6) randomVal3 = 6;

            finalVal = randomVal1 + randomVal2 + randomVal3 - 5;

            return finalVal;
        }
    }
}
