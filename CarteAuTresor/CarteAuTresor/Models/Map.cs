using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarteAuTresor.Models;

namespace CarteAuTresor
{
    public class Map
    {
        public Map(int width, int height, List<Mountain> mountainList, List<Treasure> treasureList, List<Adventurer> adventurerList)
        {
            this.width = width;
            this.height = height;
            this.mountainList = mountainList;
            this.treasureList = treasureList;
            this.adventurerList = adventurerList;
        }
        public Map()
        {
            mountainList = new List<Mountain>();
            treasureList = new List<Treasure>();
            adventurerList = new List<Adventurer>();
        }

        public int width;
        public int height;
        public List<Mountain> mountainList;
        public List<Treasure> treasureList;
        public List<Adventurer> adventurerList;
    }
}

