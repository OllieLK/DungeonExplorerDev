using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Battle
    {
        private Player Player;
        private List<BattleEntity> Enemies;
        public Battle(Player p, List<BattleEntity> e, string firstTurn)
        {
            Player = p;
            Enemies = e;
            AnsiConsole.Clear();
            Player.DrawOverWorld(false);
            drawBattle(firstTurn);
        }
        private void drawBattle(string nextTurn)
        {
            Table tab = new Table();
            tab.Title = new TableTitle("FIGHT!!!");
            string playerhealth;
            playerhealth = Player.UpdateHealthString();
            tab.AddColumn(playerhealth);
            AnsiConsole.Render(tab);
            Console.ReadLine();
        }
    }

    public class BattleMoves {
        public object Move()
        {
            return null;
        }
    }

    public abstract class BattleEntity
    {
        public int Health;
        public int MaxHealth;
        public List<BattleMoves> moves;

        public abstract void Battleturn();
        public abstract void onDeath();
    }

    

    public class Enemy : BattleEntity
    {
        public override void Battleturn()
        {
            throw new NotImplementedException();
        }
        public override void onDeath()
        {
            throw new NotImplementedException();
        }
    }

}
