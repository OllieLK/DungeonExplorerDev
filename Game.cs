using Spectre.Console;
using System;

namespace Program
{
    


    // Main game class, main game loop present in here and entry point.
    public class Game
    {

        public void WrapPlayer(Player player) // Wrapper function so that P1 can be updated from another assembly (After modified by enemy for instance)
        {
            P1 = player;
        }
        // Player instance
        public Player P1 { get; private set; }
        // Entry point, sets encoding to support unicode and initializes the player and the map, then starts the titlescreen.
        public Game()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            InventoryItem.initRandomItems();
            P1 = new Player();
            P1.GameMap.UpdateMap(P1.getPosX(), P1.getPosY(), P1.getPosX(), P1.getPosY());
        }

        // Simple game title screen. will have saving at some point
        public void titleScreen()
        {
            Console.Clear();
            AnsiConsole.Render(new Panel("THE LEGEND OF ZELDA\nPress enter to start\n\n\nBy Oliver Lazarus-Keene 29218390\nMusic not made by myself"));
            Console.ReadLine();
            gameLoop();           
        }
        
        private void gameLoop()
        {
            do { 
                           
                Console.Clear(); // Clears console at start of new turn
                P1.OverWorldTurnMenu();
                
            } while (true);
        }

        public void EndCredits()
        {
            Console.Clear();
            Console.WriteLine("Thanks for playing my dungeon explorer game!\nMusic used was not made by myself.\nPress enter to return to title screen.");
            Console.ReadLine();
            titleScreen();
        }
    }
}
