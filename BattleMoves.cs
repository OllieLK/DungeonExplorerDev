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
    public enum moveType
    {
        attack,
        defend
    }

    public abstract class BattleMove
    {
        public moveType type { protected set; get; }
        public int healthChange { protected set; get; }
        public string effect { protected set; get; }
        protected string name { set; get; }
        public abstract Creature doMove(Creature target);
        

        public BattleMove(string _name, int _healthChange) {
            name = _name;
            healthChange = _healthChange;
        }
    }
    class AttackingMove : BattleMove
    {
        private int TurnsLasting;
        public AttackingMove(string _name, int _healthChange, string _effect, int _turnsLasting) : base(_name, _healthChange)
        {
            TurnsLasting = _turnsLasting;
            effect = _effect;
            type = moveType.attack;
        }
        
        public AttackingMove(string _name, int _healthChange) : base(_name, _healthChange)
        {
            type = moveType.attack;
            effect = "none";
        }
        public override Creature doMove(Creature target) {
            Console.Write(name + " Damages " + healthChange + " Health. ");
            if (effect != "none")
                Console.Write("Also inflicts " + effect + "\n");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

            target.Health -= healthChange;
            target.BattleEffect.SetEffect(effect, TurnsLasting);
            return target;
        }
    }
    class DefensiveMove : BattleMove
    {
        private string effecttoClear;
        public DefensiveMove(string _name, int _healthChange) : base(_name, _healthChange)
        {
            type = moveType.defend;
            effecttoClear = "none";
        }
        public DefensiveMove(string _name, int _healthChange, string _effectToClear) : base(_name, _healthChange)
        {
            type = moveType.defend;
            effecttoClear = _effectToClear;
        }
        public override Creature doMove(Creature target)
        {
            Console.Write(name + " Recovers " + healthChange + " Health. ");
            if (effecttoClear != "none")
                Console.Write("Also recovered from " + effecttoClear + "\n");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            target.BattleEffect.SetEffect(effecttoClear, 1);
            target.Health += healthChange;
            return target;
        }
    }





}
