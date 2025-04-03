using System;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Media;

namespace Program
{
    public class DungeonExplorer
    {
        private static SoundPlayer p;
        public static Game GameInstance;

        public static void Main(string[] Args)
        {
            p = new SoundPlayer("Legend of Zelda (NES) Intro.wav");
            //p.Play();
            
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
            GameInstance.titleScreen();

            
        }
    }
}

