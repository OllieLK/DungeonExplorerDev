using System;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Program
{
    public class DungeonExplorer
    {
        public static Game GameInstance;

        public static void Main(string[] Args)
        {
            Console.CursorVisible = false;
            
            try
            {
                Testing.RunTests(); // Run tests at start of game.
            }
            catch
            {
                Console.WriteLine("Failed during tests");
            }
            
            GameInstance = new Game();
            
            
        }
    }
}

