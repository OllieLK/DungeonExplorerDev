using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Testing
    {
        public static void RunTests()
        {
            TestInventory();
            TestMap();
            Console.WriteLine("All tests passed correctly");
        }

        static void TestInventory() 
        {
            PlayerInventory Inv = new PlayerInventory(5);
            InventoryItem TestSword = new InventoryItem("weapon", "Sword", 1, 1);

            Inv.PickUpItem(TestSword); // Tests the inventory adds items
            Debug.Assert(Inv.IsItemPresent(TestSword), "Inventory failed to add item");

            Inv.DeleteItem(TestSword, true); // tests it removes them
            Debug.Assert(!Inv.IsItemPresent(TestSword), "Inventory item failed to remove");
        }

        static void TestMap()
        {
            Map TestMap = new Map(1, 1);
            Room testRoom = TestMap.getRoomFromArr(3, 4); // Makes sure rooms are present and initialized in map
            Debug.Assert(testRoom != null, "Room could not be got from map");
        }


    }
}
