using Program;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public abstract class BattleMove
    {
        public static List<BattleMove> BattleMoves;
        public static List<BattleMove> getMoves(int noOfMoves)
        {
            try // Try catch block for getting battle moves. if it fails (Not yet assigned) assigns them and then recalls function
            {
                Random rnd = new Random();
                var items = new List<BattleMove>();
                for (int i = 0; i < noOfMoves; i++)
                    items.Add(BattleMoves[rnd.Next(BattleMoves.Count)]);

                return items;
            }
            catch
            {
                BattleMoves = new List<BattleMove>
                {
                    new PoisionDart(),
                    new Heal()
                };
                getMoves(noOfMoves);
            }
            return null;
        }
        public string type;
        
        public int damage { private set; get; }

        protected string name { private set; get; }
        public virtual Creature doMove(Creature target) { return null;  }
    }
    class AttackingMove : BattleMove
    {
        public AttackingMove()
        {
            type = "attack";
        }
    }
    class DefensiveMove : BattleMove
    {
        public DefensiveMove()
        {
            type = "defensive";
        }
    }

    class PoisionDart : AttackingMove
    {
        public PoisionDart() : base() { }     
        public override Creature doMove(Creature target)
        {
            target.BattleEffect.SetEffect("poison", 2);
            target.Health -= 15;
            AnsiConsole.Render(new Panel("Hit with poison dart. Dealt 15 damage. will deal 10 per turn\nPress enter to continue"));
            Console.ReadLine();
            return target;
        }
    }
    class Heal : DefensiveMove
    {
        public override Creature doMove(Creature target)
        {
            target.Health += 15;
            if (target.Health > target.MaxHealth)
                target.Health = target.MaxHealth;
            AnsiConsole.Render(new Panel("Healed 15 HP!\nPressEnter To Continue!"));
            Console.ReadLine();
            return target;
        }
    }



}
