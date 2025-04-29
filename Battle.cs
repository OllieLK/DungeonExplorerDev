using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Program
{
    public class Battle
    {
        private List<InventoryItem> ItemDrops = new List<InventoryItem>();
        private Player Player;
        private List<Enemy> Enemies;
        public Battle(List<Enemy> e)
        {            
            Enemies = e;                     
        }
        public void startBattle(Player p)
        {
            Player = p;
            battleLoop();
        }
        private void battleLoop()
        {            
            while (true)
            {
                foreach (Enemy e in Enemies)
                    e.tickBattleEffect();
                Player.tickBattleEffect(); // Tick all of the status effects (Poision etc) for the player and enemies

                if (Enemies.Count == 0)
                {
                    BattleWon(); // End battle if all enemies defeated
                    return;
                }
                if (Player.Health <= 0)
                    Player.onDeath(); 


                char c = drawBattle(true); // Get player target and draw map
                if (c == 'e')
                    Player.pInv.DrawInventory("", Player); // Showing inventory in a battle counts as a turn // So need to make draw inv return iBattleUsablel;
                else
                    Enemies[c - '0' - 1] = (Enemy)Player.Battleturn(Enemies[c - '0' - 1]); // Perform players turn

                DungeonExplorer.GameInstance.WrapPlayer(Player); // Wrap player back to main instance after each turn

                for (int i = Enemies.Count - 1; i >= 0; i--) // Looping through to see which enemies need removing. have to decrement sicne collection size may change
                {
                    if (Enemies[i].Health <= 0)
                    {
                        Enemies[i].onDeath();
                        Enemies.RemoveAt(i); // Removing the killed enemy.
                    }
                }

                if (Enemies.Count == 0)
                {
                    BattleWon(); // End battle if all enemies defeated
                    return;
                }

                drawBattle(false); // Redraw Battle After the turn;               
                foreach (Creature e in Enemies)
                {
                    Player = (Player)e.Battleturn(Player); // Perform enemy turns
                }
                DungeonExplorer.GameInstance.WrapPlayer(Player); // Wrap player back to main instance after each turn
                
                if (Player.Health <= 0)
                    Player.onDeath(); // Check player still alive and triggering death if player dies.
            }
            
        }
        private char drawBattle(bool playernext)
        {
            AnsiConsole.Clear();
            Player.DrawOverWorld(false); // Clear map and draw world

            List<Char> ValidTargets = new List<Char>
            {
                'e'
            };
            Table tab = new Table();
            string playerhealth = string.Empty; // Create table and strings for displaying 
            string enemystring = string.Empty;

            playerhealth += Player.name + " " + Player.UpdateHealthString() + " " + Player.BattleEffect.Name;
            if (playernext)
            {
                tab.Title = new TableTitle("Select Target, or press e to use an item on yourself. this will use a turn!");
                for (int i = 0; i < Enemies.Count; i++)
                {
                    ValidTargets.Add((char)('0' + i + 1));
                    enemystring += "(" + (i + 1) + ") " + Enemies[i].name + " " + Enemies[i].UpdateHealthString() + " " + Enemies[i].BattleEffect.Name + "\n";
                }
            }
            else
            {
                tab.Title = new TableTitle("Ai Turn");
                foreach (Creature e in Enemies)
                    enemystring = e.name + " " + e.UpdateHealthString() + "\n";
            }

            tab.AddColumn(playerhealth);
            tab.AddColumn(enemystring);
            AnsiConsole.Render(tab);


            if (playernext)
                return GameInputs.K(ValidTargets);
            else
                return 'n';
        }
        private void BattleWon()
        {
            Console.Clear();
            Player.DrawOverWorld(false);
            AnsiConsole.Render(new Panel("Battle Won!\nPress Enter to return."));
        }
    }
}
