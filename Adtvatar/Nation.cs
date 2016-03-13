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
        private int id;

        public Nation(int ID, string Name)
        {
            name = Name;
            id = ID;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
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