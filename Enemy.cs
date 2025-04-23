using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class StatusEffect
    {
        public StatusEffect()
        {
            tick = NoEffect;
            Name = "none";
        }
        public string Name { get; private set; }
        public void SetEffect(string effect)
        {
            Name = effect;
            switch (effect)
            {
                case "posion":
                    tick = Poison;
                    break;
                case "recovery":
                    tick = Recovery;
                    break;
                case "none":
                    tick = NoEffect; 
                    break;
            }
        }
        public delegate BattleEntity StatusDelegate(BattleEntity target);
        public StatusDelegate tick;

        private static BattleEntity NoEffect(BattleEntity target) {
            return target;
        }

        private static BattleEntity Poison(BattleEntity target) {
            target.Health -= 10;
            return target;
        }
        private static BattleEntity Recovery(BattleEntity target)
        {
            target.Health += 10;
            return target;
        }
    }

    public abstract class BattleEntity
    {
        public void tickBattleEffect()
        {
            BattleEffect.tick(this);
        }
        public StatusEffect BattleEffect;
        public string name;
        public int Health;
        public int MaxHealth;
        public List<BattleMoves> moves;
        public abstract void onDeath();

        // Basic function to show the players health, made to look nice using colours and unicode characters.
        public string UpdateHealthString()
        {
            string red = string.Empty;
            string grey = string.Empty;
            for (int i = 0; i < MaxHealth; i = i + 10)
            {
                if ((Health - i) > 4)
                    red += "[red]♥[/]";
                else
                    grey += "[grey]♥[/]";
            }
            return (red + grey);
        }

        public virtual Object Battleturn(BattleEntity target)
        {
            throw new NotImplementedException();
        }
    }


    public class Battle
    {      
        private Player Player;
        private List<BattleEntity> Enemies;
        public Battle(Player p, List<BattleEntity> e, string firstTurn)
        {
            Player = p;
            Enemies = e;        
            Enemies = new List<BattleEntity>();
            Enemies.Add(new Enemy("troll"));
            battleLoop();
        }
        private void battleLoop()
        {           
            bool continueBattle = true;
            while (continueBattle)
            {
                foreach (Enemy e in Enemies)
                    e.tickBattleEffect();
                Player.tickBattleEffect(); // Tick all of the status effects (Poision etc) for the player and enemies


                char c = drawBattle(true); // Get player target and draw map
                if (c == 'e')
                    Player.pInv.DrawInventory("", Player); // Showing inventory in a battle counts as a turn
                else
                    Enemies[c - '0' - 1] = (Enemy)Player.Battleturn(Enemies[c - '0' - 1]); // Perform players turn
                DungeonExplorer.GameInstance.WrapPlayer(Player); // Wrap player back to main instance after each turn

                drawBattle(false); // Redraw Battle After the turn;               
                foreach(BattleEntity e in Enemies)
                {
                    Player = (Player)e.Battleturn(Player); // Perform enemy turns
                }
                DungeonExplorer.GameInstance.WrapPlayer(Player); // Wrap player back to main instance after each turn


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

            playerhealth += Player.name + "     " + Player.UpdateHealthString();
            if (playernext)
            {
                tab.Title = new TableTitle("Select Target, or press e to open inventory. this will use a turn!");
                for (int i = 0; i < Enemies.Count; i++)
                {
                    ValidTargets.Add((char)('0' + i + 1));
                    enemystring += "(" + (i + 1) + ") " + Enemies[i].name + "       " + Enemies[i].UpdateHealthString() + "\n";
                }
            }
            else
            {
                tab.Title = new TableTitle("Ai Turn");
                foreach (BattleEntity e in Enemies)
                    enemystring = e.name + "        " + e.UpdateHealthString() + "\n";
            }

            tab.AddColumn(playerhealth);
            tab.AddColumn(enemystring);
            AnsiConsole.Render(tab);


            if (playernext)                            
                return GameInputs.K(ValidTargets);           
            else
                return 'n';           
        }
    }

    public class BattleMoves {
        public object Move()
        {
            return null;
        }
    }


    

    public class Enemy : BattleEntity
    {
        public Enemy(string _name)
        {
            BattleEffect = new StatusEffect();
            name = _name;
            MaxHealth = 150;
            Health = 50;
        }
        public override object Battleturn(BattleEntity player)
        {
            throw new NotImplementedException();
        }
        public override void onDeath()
        {
            throw new NotImplementedException();
        }
    }

}
