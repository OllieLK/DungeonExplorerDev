using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    /// <summary>
    /// Simple class for inventory items, which are kept in the players inventory or as 'FloorItems' in rooms.
    /// Has simple overloads for if there is no items present but they want to remain in the inventory, for instance key items.
    /// 
    /// </summary>
    /// 

    public interface IBattleUsable
    {
        Creature UseInBattle(Creature Target);
    }



    public abstract class InventoryItem
    {
        public int SalePrice { get; protected set; }
        public virtual Player Use(Player p) { return null;  }
        public string type { get; protected set; }
        public int maxNoOfItem { get; protected set; }
        public int noOfItem { get; set; }
        public string sName { get; protected set; }

        public string sDescription { get; protected set; }

        private static List<InventoryItem> RandomItems;

        public static void initRandomItems()
        {
            RandomItems = new List<InventoryItem>
            {
                new Food("Hearty Apple", 20, 20, "Common Hyrulian Apple. Eating +10", 20, 10),
                new Food("Hylian Mushroom", 30, 30, "Common mushroom. Eating +5", 15, 5),
                new Food("Hylian Bass", 10, 10, "Premium fish from the Zora. Eating +25", 40, 25),
                new Food("Prime Steak", 5, 5, "Finest meat in hyrulde. Eating +50", 65, 50),

                new Weapon("Royal Broardsword", 1, 1, "Sword of the knights of the princess. 80 Damage", 200, 80),
                new Weapon("Flameblade", 1, 1, "A magical sword that burns with fire. 65 Damage", 180, 65),
                new Weapon("Thunderfury", 1, 1, "A blade infused with lightning, striking with 90 Damage", 250, 90),
                new Weapon("Shadowfang", 1, 1, "A cursed dagger that drains life. 50 Damage", 150, 50),
                new Weapon("Frostmourne", 1, 1, "A legendary sword of ice, chilling foes with 100 Damage", 300, 100),
            };
        }

        public static InventoryItem GetRandomItem()
        {
            Random rnd = new Random();
            return RandomItems[rnd.Next(RandomItems.Count)];
        }
        public static List<InventoryItem> GetRandomItem(int ammount)
        {
            Random rnd = new Random();
            List<InventoryItem> items = new List<InventoryItem>();
            
            for (int i = 0; i < ammount; i++)
                items.Add(RandomItems[rnd.Next(RandomItems.Count)]);

            return items;
        }
        public InventoryItem() { }
        public InventoryItem(string name, int maxNoOfItem, int noOfItem, string desc, int price)
        {           
            SalePrice = price;
            sDescription = desc;
            this.sName = name;
            this.maxNoOfItem = maxNoOfItem;
            this.noOfItem = noOfItem;
        }
    }

    public class Food : InventoryItem
    {
        string effectToClear;
        int Recovery;
        
        public Food(string name, int maxnoOfItem, string desc, int recovery)
        {
            noOfItem = maxnoOfItem;
            type = "food";
            sName = name;
            sDescription = desc;
            maxNoOfItem = maxnoOfItem;
            Recovery = recovery;
        }
        public Food(string name, int maxnoOfItem, string desc, int recovery, string effect)
        {
            noOfItem = maxnoOfItem;
            type = "food";
            sName = name;
            sDescription = desc;
            maxNoOfItem = maxnoOfItem;
            Recovery = recovery;
            effectToClear = effect;
        }
        public Food(string name, int maxNoOfItem, int noOfItem, string desc, int price, int recovery, string effect) : base(name, maxNoOfItem, noOfItem, desc, price)
        {
            effectToClear = effect;
            type = "food";
            Recovery = recovery;
        }
        public Food(string name, int maxNoOfItem, int noOfItem, string desc, int price, int recovery) : base( name, maxNoOfItem, noOfItem, desc, price)
        {
            type = "food";
            Recovery = recovery;
        }
        public override Player Use(Player p)
        {
            if (effectToClear != "none")
                if (p.BattleEffect.Name == effectToClear)
                    p.BattleEffect.SetEffect("none", 1);
            p.Health = p.Health + Recovery;
            if (p.Health > p.MaxHealth)
                p.Health = p.MaxHealth;
            return p;
        }       
    }

    public class AttackingItem : InventoryItem, IBattleUsable
    {
        public int numberofTurns { private set; get; }
        public int damage { private set; get; }
        public string battleEffect { private set; get; }
        public AttackingItem(string name, int _damage)
        {
            type = "battle";
            sName = name;
            damage = _damage;
            battleEffect = "none";
        }
        public AttackingItem(string name, string _effect, int _damage, int numberOfTurns)
        {
            type = "battle";
            sName = name;
            damage = _damage;
            battleEffect = _effect;
            numberofTurns = numberOfTurns;
        }
        public Creature UseInBattle(Creature Target)
        {
            Console.WriteLine("Used " + sName + ".\npress enter");
            Target.BattleEffect.SetEffect(battleEffect, numberofTurns);
            Target.Health -= damage;
            return Target;
        }        
    }

    

    public class Weapon : InventoryItem
    {
        public int Damage { get; private set; }
        public Weapon(string name, int damage)
        {
            Damage = damage;
            sName = name;
            type = "weapon";
        }
        public Weapon(string name, string desc, int damage)
        {
            noOfItem = 1;
            maxNoOfItem = 1;
            sName = name;
            Damage = damage;
            sDescription = desc;
            type = "weapon";
        }
        public Weapon(string name, int maxNoOfItem, int noOfItem, string desc, int price, int damage) : base(name, maxNoOfItem, noOfItem, desc, price)
        {
            type = "weapon";
            Damage = damage;
        }
        
    }
    public class Coin : InventoryItem
    {        
        public Coin(int ammount)
        {
            noOfItem = ammount;
            type = "coin";
            sName = "Coin stash";
            sDescription = ammount.ToString();
        }
    }
}

