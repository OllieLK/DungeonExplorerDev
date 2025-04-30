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
    public enum moveType // Enum for move type
    {
        attack,
        defend
    }

    public abstract class BattleMove
    {
        // Fields with correct access modifiers
        public moveType type { protected set; get; }
        public int healthChange { protected set; get; }
        public string effect { protected set; get; }
        protected string name { set; get; }
        public abstract Creature doMove(Creature target); // Abstract "doMove" - Implemented by attacking move and defensive move differently
        

        public BattleMove(string _name, int _healthChange) {
            name = _name;
            healthChange = _healthChange; // Simple constructor assigning
        }
    }
    class AttackingMove : BattleMove
    {
        private int TurnsLasting; // Turns lasting for status effect

        // Two constructors, one for just damage and one for moves that give a status effect.
        // Also sets type enum
        public AttackingMove(string _name, int _healthChange, string _effect, int _turnsLasting) : base(_name, _healthChange)
        {
            TurnsLasting = _turnsLasting;
            effect = _effect;
            type = moveType.attack;
        }
        
        public AttackingMove(string _name, int _healthChange) : base(_name, _healthChange)
        {
            type = moveType.attack;
            effect = "none"; // set effect to none if no effect
        }

        public override Creature doMove(Creature target) {
            Console.Write(name + " Damages " + healthChange + " Health. ");
            if (effect != "none")
                Console.Write("Also inflicts " + effect + "\n"); // Applies effect if present
            Console.Write("Press enter to continue");
            Console.ReadLine();

            target.Health -= healthChange;
            target.BattleEffect.SetEffect(effect, TurnsLasting); // alter target accordingly
            return target;
        }
    }
    class DefensiveMove : BattleMove
    {
        private string effecttoClear;
        // Similar constructors to above, one for just healing one for moves that remove effect.
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
            if (effecttoClear != "none") // Recovers from effect if present
                Console.Write("Also recovered from " + effecttoClear + "\n");
            Console.Write("Press enter to continue");
            Console.ReadLine();

            target.BattleEffect.SetEffect(effecttoClear, 1);
            target.Health += healthChange; // Alter target accordingly
            return target;
        }
    }





}
