using System;
using System.Collections.Generic;
using System.Linq;
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

    interface iSellable
    {
        
    }

    public abstract class InventoryItem : iSellable
    {
        
        public int SalePrice;
        public virtual Player Use(Player p) { return null;  }
        public string type;
        public int maxNoOfItem { get; set; }
        public int noOfItem { get; set; }
        public string sName { get; set; }
        public string sDescription;

        public static List<InventoryItem> GetRandomItem(int ammount)
        {
            Random rnd = new Random();
            List<InventoryItem> items = new List<InventoryItem>();
            List<InventoryItem> RandomItems = new List<InventoryItem>
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
        int Recovery;
        public Food(string name, int maxNoOfItem, int noOfItem, string desc, int price, int recovery) : base( name, maxNoOfItem, noOfItem, desc, price)
        {
            type = "food";
            Recovery = recovery;
        }
        public override Player Use(Player p)
        {
            p.Health = p.Health + Recovery;
            if (p.Health > p.MaxHealth)
                p.Health = p.MaxHealth;
            return p;
        }       
    }
    public class Weapon : InventoryItem
    {
        public int Damage;
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
