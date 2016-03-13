using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adtvatar
{
    public class Nation
    {
        private string name;
        private int points;

        public Nation(string Name)
        {
            name = Name;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Points
        {
            get
            {
                return points;
            }

            set
            {
                points = value;
            }
        }
    }
}