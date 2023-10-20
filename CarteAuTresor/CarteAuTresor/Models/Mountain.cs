using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor
{
    public class Mountain
    {
        public Mountain()
        {
        }
        public Mountain(int horizontalPosition,int verticalPosition)
        {
            this.horizontalPosition = horizontalPosition;
            this.verticalPosition = verticalPosition;
        }
        public int horizontalPosition;
        public int verticalPosition;
    }
}


