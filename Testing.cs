using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Program;

namespace DungeonExplorer
{
    public class GameTests
    {

        // Private fields for testing process
        private Game _game;
        private Player _player;
        private string testResults = string.Empty; // string for results global within class - due to simplicity not a problem


        public void RunTests()
        {
            testResults = string.Empty; // Empty results string
            testResults += "----- TESTS STARTING -----";
            Setup();
            TestGameInitialization();
            TestPlayerWrapping();
            TestPlayerPosition();
            TestMapBoundaries();
            TestInventorySystem();
            TestMapUpdate(); // Call all test functions
            testResults += "\n----- ALL TESTS COMPLETED -----";

            StreamWriter w = new StreamWriter("testResults.txt", append:true);            
            w.WriteLine(testResults);
            w.Close(); // Write back results - append so will add on if tests done in the past
        }

        void Setup() // Assign values
        {
            _game = new Game();
            _player = _game.P1;
        }
        void TestGameInitialization()
        {
            Game game = new Game();

            Assert.IsNotNull(game.P1, "Player should be initialized");
            testResults += "\nPlayer Initialized: " + DateTime.Now;
            Assert.IsNotNull(game.P1.GameMap, "GameMap should be initialized");
            testResults += "\nMap Initialized: " + DateTime.Now;
            // Test objects initialized correctly
        }
        void TestPlayerWrapping()
        {
            Player originalPlayer = _game.P1;
            Player newPlayer = new Player(); // assign

            _game.WrapPlayer(newPlayer);
            Assert.AreSame(newPlayer, _game.P1, "Player should be wrapped correctly");
            testResults += "\nPlayer Wrapped: " + DateTime.Now;

            Assert.AreNotSame(originalPlayer, _game.P1, "Original player should be replaced");
            testResults += "\nWrap successful: " + DateTime.Now;
            // Test wrapping

        }
        void TestPlayerPosition()
        {
            int initialX = _player.getPosX();
            int initialY = _player.getPosY();

            Assert.IsTrue(initialX >= 0, "Initial X position should be valid");
            Assert.IsTrue(initialY >= 0, "Initial Y position should be valid");
            testResults += "\nPlayer position valid: " + DateTime.Now;
            // Test map values correct

        }
        void TestMapUpdate()
        {
            int initialX = _player.getPosX();
            int initialY = _player.getPosY();
            try
            {
                _player.GameMap.UpdateMap(initialX, initialY, initialX, initialY);
                testResults += "\nMap updating correctly: " + DateTime.Now;

            }
            catch
            {
                throw new Exception("Map not updating"); // Test map updating
            }
        }
        void TestInventorySystem()
        {
            _player.pInv.PickUpItem(new Weapon("sword", 1));
            Assert.IsNotNull(_player.pInv.GetQueriedList("weapon"), "Inventory sorting not working");
            testResults += "\nInventory sorting working: " + DateTime.Now;
            // Test inventory LINQ works
        }
        void TestMapBoundaries()
        {
            int maxX = Map.sizeX - 1;
            int maxY = Map.sizeY - 1;
            Assert.IsTrue(maxX > 0, "Map should have a positive width");
            Assert.IsTrue(maxY > 0, "Map should have a positive height");
            Assert.IsTrue(_player.getPosX() < maxX, "Player X position should be within map boundaries");
            Assert.IsTrue(_player.getPosY() < maxY, "Player Y position should be within map boundaries");
            testResults += "\nMap bounds working: " + DateTime.Now; // Test map bounds are valid
        }
    }
}