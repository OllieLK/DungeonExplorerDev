using System;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Media;
using DungeonExplorer;

namespace Program
{
    public class DungeonExplorer
    {
        private static SoundPlayer p; // Just for a bit of fun - background music.
        public static Game GameInstance; // Instance

        public static void Main()
        {
            GameTests g = new GameTests();
            g.RunTests(); // Run test class



            Console.CursorVisible = false; // Set curser to be invisible
            GameInstance = new Game();
            GameInstance.titleScreen();           // Start game
        }
    }
}

