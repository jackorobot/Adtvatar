using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adtvatar
{
    class Drink
    {
        private drinkTypes drinktype;
        private int factor;

        public Drink(int Factor, drinkTypes DrinkType)
        {
            drinktype = DrinkType;
            factor = Factor;
        }
        public drinkTypes DrinkType
        {
            get
            {
                return drinktype;
            }
            set
            {
                drinktype = value;
            }
        }

        public int Factor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

    }
}
