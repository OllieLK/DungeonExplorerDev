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
        }

        static void TestMap()
        {
            Map TestMap = new Map(1, 1);
            Room testRoom = TestMap.getRoomFromArr(3, 4); // Makes sure rooms are present and initialized in map
            Debug.Assert(testRoom != null, "Room could not be got from map");
        }
    }
}
