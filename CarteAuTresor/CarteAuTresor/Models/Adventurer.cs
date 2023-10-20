using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.Models
{
    public class Adventurer
    {
        public Adventurer(string name, int horizontalPosition, int verticalPosition, char orientation, string movements, int treasureCollected)
        {
            this.name = name;
            this.horizontalPosition = horizontalPosition;
            this.verticalPosition = verticalPosition;
            this.orientation = orientation;
            this.movements = movements;
            this.treasureCollected = treasureCollected;
        }

        public string name;
        public int horizontalPosition;
        public int verticalPosition;
        public char orientation;
        public string movements;
        public int treasureCollected;
    }  

}
