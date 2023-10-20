using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor
{
    public class Treasure
    {
        public Treasure()
        {
        }
        public Treasure(int horizontalPosition, int verticalPosition, int amount)
        {
            this.horizontalPosition = horizontalPosition;
            this.verticalPosition = verticalPosition;
            this.amount = amount;
        }
        public int horizontalPosition;
        public int verticalPosition;
        public int amount;
    }
}


