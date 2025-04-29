using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Program
{
    public class StatusEffect
    {
        public StatusEffect()
        {
            tick = NoEffect;
            Name = "none";
        }
        public int TurnsLeft;
        public string Name { get; private set; }
        public void SetEffect(string effect, int numberOfTurns)
        {
            TurnsLeft = numberOfTurns;
            Name = effect;
            switch (effect)
            {
                case "fire":
                    tick = Fire;
                    break;
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
        public delegate Creature StatusDelegate(Creature target);
        public Creature DoEffect(Creature target)
        {
            if (TurnsLeft == 0)
            {
                tick = NoEffect;
                return target;
            }
            else
            {
                TurnsLeft--;
                return tick(target);
            }            
        }
        private StatusDelegate tick;

        private static Creature NoEffect(Creature target) {
            return target;
        }

        private static Creature Poison(Creature target) {
            target.Health -= 10;
            return target;
        }
        private static Creature Recovery(Creature target)
        {            
            target.Health += 10;
            return target;
        }

        private static Creature Fire(Creature target)
        {
            target.Health -= 20;
            return target;
        }
    }

    public abstract class Creature
    {
        public void tickBattleEffect()
        {
            BattleEffect.DoEffect(this);
        }
        public StatusEffect BattleEffect;
        public string name;
        public int Health;
        public int MaxHealth;
        
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

        public virtual object Battleturn(Creature target)
        {
            throw new NotImplementedException();
        }
        
    }

    public class Enemy : Creature
    {
        // SECTION
        // Functions and fields for getting enemies (already defined in here)
        // Used for dungeon assignments
        private static Random rnd = new Random();
        public static List<Enemy> getEnemies(int ammount, DungeonDif STRENGTH)
        {
            List<Enemy> m = new List<Enemy>();
            for (int i = 0; i < ammount; i++)
                m.Add(Enemies[rnd.Next(Enemies.Count)]);
            
            return m;
        }
        private static List<BattleMove> GetMoves(int ammount)
        {
            List<BattleMove> m = new List<BattleMove> ();
            for (int i = 0; i <= ammount; i++)
                m.Add(EnemyMoves[rnd.Next(EnemyMoves.Count)]);
            return m;
        }
        private static List<Weapon> EnemyWeapons = new List<Weapon>
        {
            new Weapon("Simple sword", 10),
            new Weapon("Flame sword", 12),
            new Weapon("Poisoned dagger", 8),
            new Weapon("Heavy club", 15),
            new Weapon("Spiked mace", 14)
        };
        private static List<BattleMove> EnemyMoves = new List<BattleMove>
        {
            new AttackingMove("Poison dart", 0, "poison", 3),
            new AttackingMove("Spit", 5),
            new DefensiveMove("Recover", 5),
            new DefensiveMove("Poison ointment", 10, "poison"),
            new AttackingMove("Fireball", 10, "fire", 2),
            new AttackingMove("Blazing Slash", 7, "fire", 3),
            new AttackingMove("Thorn attack", 6, "poison", 2),
            new DefensiveMove("Healing Mist", 8)
        };
        private static List<Enemy> Enemies = new List<Enemy>
        {
            // Using GetMoves(3) to randomly assign 3 moves to each enemy
            new Enemy("Bokoblin", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Fire Imp", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Poisonous Spider", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Goblin Warrior", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Flame Knight", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Venomous Wraith", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Stone Golem", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120)),
            new Enemy("Dark Elf", EnemyWeapons[rnd.Next(EnemyWeapons.Count)], GetMoves(3), rnd.Next(10, 120))
        };

        public Weapon EnemyWeapon;
        public List<BattleMove> Moves;
        public Enemy(string _name, Weapon enemyWeapon, List<BattleMove> _Moves, int maxHealth)
        {
            
            Moves = _Moves;
            BattleEffect = new StatusEffect();
            name = _name;
            MaxHealth = maxHealth;
            Health = rnd.Next(maxHealth / 3, maxHealth);
            EnemyWeapon = enemyWeapon;
        }
        public override object Battleturn(Creature player)
        {
            Random rnd = new Random();
            int choice = rnd.Next(0, Moves.Count + 1);
            Console.Write(name + "'s Move: ");

            if (choice == Moves.Count) // Basic attack
            {
                player.Health -= EnemyWeapon.Damage;
                Console.Write("Used their " + this.EnemyWeapon.sName + ". It dealt " + this.EnemyWeapon.Damage + " Damage. Press Enter to continue");
                Console.ReadLine();         
            }
            else
            {
                if (BattleEffect.Name != "none" || Health <= (MaxHealth / 3))
                {// AI will use restoritive move if low health or has status effect.
                    var defensiveMoves = Moves.Where(move => move.type == moveType.defend).ToList(); // Filter for only defensive moves
                    if (defensiveMoves.Count > 0)
                    {
                        Enemy e = new Enemy(this.name, this.EnemyWeapon, this.Moves, this.MaxHealth);
                        e = defensiveMoves[rnd.Next(defensiveMoves.Count - 1)].doMove(this) as Enemy;
                        this.Health = e.Health;
                        this.BattleEffect = e.BattleEffect; // Makes new instance and wraps back to current instance if move is defensive.
                    }
                }
                else
                {
                    var attackingMoves = Moves.Where(move => move.type == moveType.attack).ToList(); // Filter only attacking moves
                    player = attackingMoves[rnd.Next(attackingMoves.Count - 1)].doMove(player); // attack player
                }
            }
            return player;
        }
        public override void onDeath()
        {
            AnsiConsole.Render(new Panel(name + " has been defeated"));
            System.Threading.Thread.Sleep(300);
        }        
    }

}
