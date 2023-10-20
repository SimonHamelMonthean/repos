using CarteAuTresor;
using CarteAuTresor.Models;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {                
        Helper helper = new Helper();        
        Map map = helper.ExtractData();
        foreach(Adventurer adventurer in map.adventurerList)
        {
            foreach (char move in adventurer.movements)
            {
                helper.StepAdventurer(map, move, adventurer);
            }
        }
        helper.WriteOutputFile(map);

    }


}