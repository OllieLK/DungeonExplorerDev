using System;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Program
{
    public class DungeonExplorer
    {
        public static void Main(string[] Args)
        {
            try
            {
                Testing.RunTests(); // Run tests at start of game.
            }
            catch
            {
                Console.WriteLine("Failed during tests");
            }
            try
            { // Error handling outside of the game class - will halt excecution if there is a problem within the game.
                Game GameInstance = new Game();
            }
            catch
            {
                Console.WriteLine("Error within game: Program crashed.");
            }
            finally
            {
                Console.WriteLine("Game End.");
            }
        }
    }
}

