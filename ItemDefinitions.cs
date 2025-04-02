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

    interface iUsable
    {
    }

    public class InventoryItem
    {
        public virtual Player Use(Player p) { return null;  }
        public string type;
        public int maxNoOfItem { get; set; }
        public int noOfItem { get; set; }
        public string sName { get; set; }
        public string sDescription;
        public InventoryItem(string ntype, string name, int maxNoOfItem)
        {
            type = ntype;
            sDescription = "A common food item.";
            this.sName = name;
            this.maxNoOfItem = maxNoOfItem;
            this.noOfItem = 1;
        }
        public InventoryItem(string ntype, string name, int maxNoOfItem, int noOfItem)
        {
            type = ntype;
            sDescription = "A common food item in hyrule. eating will restore health!";
            this.sName = name;
            this.maxNoOfItem = maxNoOfItem;
            this.noOfItem = noOfItem;
        }
    }

    public class Food : InventoryItem
    {
        public Food(string ntype, string name, int maxNoOfItem) : base(ntype, name, maxNoOfItem)
        {
        }

        public Food(string ntype, string name, int maxNoOfItem, int noOfItem) : base(ntype, name, maxNoOfItem, noOfItem)
        {
        }
        public override Player Use(Player p)
        {
            p.Health = p.Health + 20;
            return p;
        }
        
    }

}
