using CarteAuTresor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor
{
    public class Helper
    {
        private static string dash = " - ";
        /// <summary>
        /// Get all the data from the resource file "Input.txt"
        /// and fill the map object with it
        /// </summary>
        /// <returns>The map generated</returns>
        /// <exception cref="BadInputFileException"></exception>
        public Map ExtractData()
        {
            string inputText = Resource.Input;
            Map map = new Map();
            string[] lines = inputText.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith('#'))
                {
                    continue;
                }
                else if (line.StartsWith('C'))
                {
                    string[] mapLine =line.Trim().Split('-');
                    if (mapLine.Length == 3 )
                    {
                        map.width = int.Parse(mapLine[1]);
                        map.height = int.Parse(mapLine[2]);
                    }
                    else
                    {
                        throw new BadInputFileException("map size not defined correctly");
                    }                
                }
                else if (line.StartsWith('M'))
                {
                    string[] mountainLine = line.Trim().Split('-');
                    if (mountainLine.Length == 3)
                    {
                        Mountain mountain = new Mountain(int.Parse(mountainLine[1]), int.Parse(mountainLine[2]));
                        map.mountainList.Add(mountain);
                    }
                    else
                    {
                        throw new BadInputFileException("mountain not defined correctly");
                    }
                }
                else if (line.StartsWith('T'))
                {
                    string[] treasureLine = line.Trim().Split('-');
                    if (treasureLine.Length == 4)
                    {
                        Treasure treasure = new Treasure(int.Parse(treasureLine[1]), int.Parse(treasureLine[2]), int.Parse(treasureLine[3]));                        
                        map.treasureList.Add(treasure);
                    }
                    else
                    {
                        throw new BadInputFileException("treasure not defined correctly");
                    }
                }
                else if (line.StartsWith('A'))
                {
                    string[] adventurerLine = line.Trim().Split('-');
                    if (adventurerLine.Length == 6)
                    {
                        Adventurer adventurer = new Adventurer(adventurerLine[1]
                            , int.Parse(adventurerLine[2])
                            , int.Parse(adventurerLine[3])
                            , char.Parse(adventurerLine[4].Trim())
                            , adventurerLine[5].Trim(),
                            0);                         
                        map.adventurerList.Add(adventurer);
                    }
                    else
                    {
                        throw new BadInputFileException("adventurer not defined correctly");
                    }
                }
            }            
            return map;
        }

        /// <summary>
        /// Depending of the move in parameter, make the adventurer go forward, move right or move left
        /// </summary>
        /// <param name="map"></param>
        /// <param name="move"></param>
        /// <param name="adventurer"></param>
        public void StepAdventurer(Map map, char move, Adventurer adventurer)
        {
            switch (move)
            {
                case 'A':
                    GoForward(map, adventurer);
                    break;
                case 'D':
                    ChangeDirection(map,adventurer, 'D');
                    break;
                case 'G':
                    ChangeDirection(map, adventurer, 'G');
                    break;                
            }
        }

        /// <summary>
        /// Checking if the next position is not out of the map limit, is not a mountain or neither an other adventurer
        /// If it is not one of those cases, allow the adventurer in parameter to move forward
        /// </summary>
        /// <param name="map"></param>
        /// <param name="adventurer"></param>
        public void GoForward(Map map, Adventurer adventurer)
        {
            GetNextPosition(out int nextHorizontalPosition, out int nextVerticalPosition, map, adventurer);
            if(nextHorizontalPosition < 0 || nextVerticalPosition < 0 || nextHorizontalPosition >= map.width || nextVerticalPosition >= map.height)
            {
                return;
            }
            else if (!NextPositionIsAMountain(nextHorizontalPosition,nextVerticalPosition,map) && 
                !NextPositionIsAnAdventurer(nextHorizontalPosition, nextVerticalPosition, map))
            {
                if (NextPositionIsATreasure(nextHorizontalPosition, nextVerticalPosition, map))
                {
                    adventurer.treasureCollected++;
                    GetATresure(nextHorizontalPosition, nextVerticalPosition, map);
                }
                adventurer.verticalPosition = nextVerticalPosition;
                adventurer.horizontalPosition = nextHorizontalPosition;
            }
        }

        /// <summary>
        /// return true if the next position of the current "stepping" is an adventurer, return false otherwise
        /// </summary>
        /// <param name="nextHorizontalPosition"></param>
        /// <param name="nextVerticalPosition"></param>
        /// <param name="map"></param>
        /// <returns>true or false depending of the map context</returns>
        public bool NextPositionIsAnAdventurer(int nextHorizontalPosition, int nextVerticalPosition, Map map)
        {
            return map.adventurerList.Exists(a => a.horizontalPosition == nextHorizontalPosition && a.verticalPosition == nextVerticalPosition);
        }

        /// <summary>
        /// Decrement the amount of the treasure beeing collected and delete it from the list if the amount goes to 0
        /// </summary>
        /// <param name="nextHorizontalPosition"></param>
        /// <param name="nextVerticalPosition"></param>
        /// <param name="map"></param>
        public void GetATresure(int nextHorizontalPosition, int nextVerticalPosition, Map map)
        {
            if(map.treasureList.Find(t => t.horizontalPosition == nextHorizontalPosition && t.verticalPosition == nextVerticalPosition).amount > 0)
            {
                map.treasureList.Find(t => t.horizontalPosition == nextHorizontalPosition && t.verticalPosition == nextVerticalPosition).amount--;
                if(map.treasureList.Find(t => t.horizontalPosition == nextHorizontalPosition && t.verticalPosition == nextVerticalPosition).amount == 0)
                {
                    map.treasureList.Remove(map.treasureList.Find(t => t.horizontalPosition == nextHorizontalPosition && t.verticalPosition == nextVerticalPosition));                   
                }
            }            
        }

        /// <summary>
        /// return true if the next position of the current "stepping" is a Treasure, return false otherwise
        /// </summary>
        /// <param name="nextHorizontalPosition"></param>
        /// <param name="nextVerticalPosition"></param>
        /// <param name="map"></param>
        /// <returns>true or false depending of the map context</returns>
        public bool NextPositionIsATreasure(int nextHorizontalPosition, int nextVerticalPosition, Map map)
        {
            return map.treasureList.Exists(t => t.horizontalPosition == nextHorizontalPosition && t.verticalPosition == nextVerticalPosition);            
        }

        /// <summary>
        /// return true if the next position of the current "stepping" is a Mountain, return false otherwise
        /// </summary>
        /// <param name="nextHorizontalPosition"></param>
        /// <param name="nextVerticalPosition"></param>
        /// <param name="map"></param>
        /// <returns>true or false depending of the map context</returns>
        public bool NextPositionIsAMountain(int nextHorizontalPosition, int nextVerticalPosition, Map map)
        {
            return map.mountainList.Exists(m => m.horizontalPosition == nextHorizontalPosition && m.verticalPosition == nextVerticalPosition);            
        }

        /// <summary>
        /// Deduct the next position of the current adventurer based on his orientation
        /// </summary>
        /// <param name="nextHorizontalPosition"></param>
        /// <param name="nextVerticalPosition"></param>
        /// <param name="map"></param>
        /// <param name="adventurer"></param>
        public void GetNextPosition(out int nextHorizontalPosition, out int nextVerticalPosition, Map map, Adventurer adventurer)
        {
            nextHorizontalPosition = -1;
            nextVerticalPosition = -1;
            switch (adventurer.orientation)
            {
                case 'N':
                    nextVerticalPosition = adventurer.verticalPosition-1;
                    nextHorizontalPosition = adventurer.horizontalPosition;
                    break;
                case 'S':
                    nextVerticalPosition = adventurer.verticalPosition+1;
                    nextHorizontalPosition = adventurer.horizontalPosition;
                    break;
                case 'E':
                    nextHorizontalPosition = adventurer.horizontalPosition+1;
                    nextVerticalPosition= adventurer.verticalPosition;
                    break;
                case 'W':
                    nextHorizontalPosition = adventurer.horizontalPosition-1;
                    nextVerticalPosition= adventurer.verticalPosition;
                    break;
            }
        }

        /// <summary>
        /// Change the orientation of the adventurer in parameter depending of the direction given in parameter
        /// </summary>
        /// <param name="map"></param>
        /// <param name="adventurer"></param>
        /// <param name="direction"></param>
        /// <exception cref="BadInputFileException"></exception>
        public void ChangeDirection(Map map, Adventurer adventurer, char direction)
        {
                switch (adventurer.orientation)
                {
                case 'N':
                    if(direction == 'G')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'W';
                    }
                    else if(direction == 'D')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'E';
                    }
                    else { throw new BadInputFileException("direction not recognized"); }
                    break;
                case 'S':
                    if (direction == 'G')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'E';
                    }
                    else if (direction == 'D')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'W';
                    }
                    else { throw new BadInputFileException("direction not recognized"); }
                    break;
                case 'E':
                    if (direction == 'G')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'N';
                    }
                    else if (direction == 'D')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'S';
                    }
                    else { throw new BadInputFileException("direction not recognized"); }
                    break;
                case 'W':
                    if (direction == 'G')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'S';
                    }
                    else if (direction == 'D')
                    {
                        map.adventurerList.Find(a => a == adventurer).orientation = 'N';
                    }
                    else { throw new BadInputFileException("direction not recognized"); }
                    break;
            }           
        }

        /// <summary>
        /// Write a file "Output.txt" in the document based of the Map object after all the moves of the differents adventurer has been done
        /// </summary>
        /// <param name="map"></param>
        public void WriteOutputFile(Map map)
        {
            List<string> output = new List<string>();
            string mapLocation = "C" + dash + map.width + dash + map.height;
            output.Add(mapLocation);

            foreach(Mountain mountain in map.mountainList)
            {
                string mountainLocation = "M" + dash + mountain.horizontalPosition + dash + mountain.verticalPosition;
                output.Add(mountainLocation);
            }

            foreach (Treasure treasure in map.treasureList)
            {
                string treasureLocation = "T" + dash + treasure.horizontalPosition + dash + treasure.verticalPosition + dash + treasure .amount;
                output.Add(treasureLocation);
            }

            foreach (Adventurer adventurer in map.adventurerList)
            {
                string adventurerLocation = "A" + dash + adventurer.name + dash + adventurer.verticalPosition + dash + adventurer.horizontalPosition
                    + dash + adventurer.orientation + dash + adventurer.treasureCollected;
                output.Add(adventurerLocation);
            }

            // Set a variable to the Documents path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Output.txt")))
            {
                foreach (string line in output)
                    outputFile.WriteLine(line);
            }            
        }
    }
}
