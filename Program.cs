using System;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Media;
using DungeonExplorer;

namespace Program
{
    public class DungeonExplorer
    {
        private static SoundPlayer p;
        public static Game GameInstance;

        public static void Main(string[] Args)
        {
            GameTests g = new GameTests();
            g.RunTests();


            p = new SoundPlayer("Final Fantasy VII - Prelude (Original Soundtrack).wav");
            //p.PlayLooping();
            Console.CursorVisible = false;
            GameInstance = new Game();
            GameInstance.titleScreen();           
        }
    }
}

