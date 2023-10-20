using CarteAuTresor;
using CarteAuTresor.Models;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestExtractData()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            Assert.IsNotNull(map);
            Assert.IsNotNull(map.height);
            Assert.IsNotNull(map.width);
            Assert.IsNotNull(map.mountainList);            
            Assert.IsNotNull(map.treasureList);
            Assert.IsNotNull(map.adventurerList);
        }

        [TestMethod]
        public void TestWriteOutputFile()
        {
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Output.txt"));
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            helper.WriteOutputFile(map);
            Assert.IsTrue(File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Output.txt")));
        }

        [TestMethod]
        public void TestChangeDirection()
        {            
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            Adventurer adventurer = new Adventurer("test", 0, 0, 'S', "D",0);
            map.adventurerList.RemoveRange(0,map.adventurerList.Count);
            map.adventurerList.Add(adventurer);
            helper.ChangeDirection(map, adventurer, 'D');
            Assert.AreEqual(map.adventurerList.First().orientation, 'W');
            helper.ChangeDirection(map, adventurer, 'G');
            Assert.AreEqual(map.adventurerList.First().orientation, 'S');
        }

        [TestMethod]
        public void TestGetNextPosition()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            Adventurer adventurer = new Adventurer("test", 0, 0, 'S', "A", 0);
            map.adventurerList.RemoveRange(0, map.adventurerList.Count);
            map.adventurerList.Add(adventurer);
            int nextHorizontalPosition = 0;
            int nextVerticalPosition = 0;
            helper.GetNextPosition(out nextHorizontalPosition,out nextVerticalPosition,map,adventurer);
            Assert.AreEqual(nextHorizontalPosition, 0);
            Assert.AreEqual(nextVerticalPosition, 1);
        }
        
        [TestMethod]
        public void TestNextPositionIsAMountain()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            map.mountainList.RemoveRange(0, map.mountainList.Count);
            Mountain mountain = new Mountain(0,1);
            map.mountainList.Add(mountain);
            Assert.IsTrue(helper.NextPositionIsAMountain(0, 1, map));
            Assert.IsFalse(helper.NextPositionIsAMountain(1,0, map));
        }

        [TestMethod]
        public void TestNextPositionIsATreasure()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            map.treasureList.RemoveRange(0, map.treasureList.Count);
            Treasure treasure = new Treasure(0,1,1);
            map.treasureList.Add(treasure);
            Assert.IsTrue(helper.NextPositionIsATreasure(0, 1, map));
            Assert.IsFalse(helper.NextPositionIsATreasure(1, 0, map));
        }

        [TestMethod]
        public void TestNextPositionIsAnAdventurer()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            Adventurer adventurer = new Adventurer("test", 0, 1, 'S', "A", 0);
            map.adventurerList.RemoveRange(0, map.adventurerList.Count);
            map.adventurerList.Add(adventurer);
            Assert.IsTrue(helper.NextPositionIsAnAdventurer(0, 1, map));
            Assert.IsFalse(helper.NextPositionIsAnAdventurer(1, 0, map));
        }

        [TestMethod]
        public void TestGetATreasure()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            map.treasureList.RemoveRange(0, map.treasureList.Count);
            Treasure treasure = new Treasure(0, 1, 2);
            map.treasureList.Add(treasure);
            helper.GetATresure(0,1,map);
            Assert.AreEqual(map.treasureList.First().amount, 1);
            helper.GetATresure(0, 1, map);
            Assert.AreEqual(map.treasureList.Count, 0);
        }

        [TestMethod]
        public void TestGoForward()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            Adventurer adventurer = new Adventurer("test", 0, 0, 'S', "A", 0);
            map.adventurerList.RemoveRange(0, map.adventurerList.Count);
            map.adventurerList.Add(adventurer);
            helper.GoForward(map, adventurer);
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 0);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 1);
            helper.GoForward(map, adventurer);
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 0);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 2);
            helper.GoForward(map, adventurer);
            helper.GoForward(map, adventurer);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 3);
        }

        [TestMethod]
        public void TestStepAdventurer()
        {
            Helper helper = new Helper();
            Map map = helper.ExtractData();
            Adventurer adventurer = new Adventurer("test", 0, 0, 'S', "A", 0);
            map.mountainList.RemoveRange(0, map.mountainList.Count);
            map.adventurerList.RemoveRange(0, map.adventurerList.Count);
            map.adventurerList.Add(adventurer);
            helper.StepAdventurer(map,'A',map.adventurerList.First());
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 0);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 1);
            helper.StepAdventurer(map, 'G', map.adventurerList.First());
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 0);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 1);
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 1);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 1);
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            helper.StepAdventurer(map, 'D', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 2);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 3);
            helper.StepAdventurer(map, 'G', map.adventurerList.First());
            helper.StepAdventurer(map, 'G', map.adventurerList.First());
            helper.StepAdventurer(map, 'G', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            helper.StepAdventurer(map, 'D', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            helper.StepAdventurer(map, 'A', map.adventurerList.First());
            Assert.AreEqual(map.adventurerList.First().horizontalPosition, 0);
            Assert.AreEqual(map.adventurerList.First().verticalPosition, 0);
        }
    }
}