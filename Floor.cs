using Program;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public abstract class Floor
    {
        protected Dungeon dungeon;
        public Floor(Dungeon d)
        {
            dungeon = d;
        }
        
        public abstract void DoFLoor(Player p, int floorNum);
        protected virtual void drawFloor(string drawitem, string roomcontrols, int floornum) {
            Table tab = new Table();
            tab.Title = new TableTitle(dungeon.GetDescription() + ": Room " + floornum + "/" + dungeon.numOfFloors);
            tab.AddColumn(drawitem);
            tab.AddColumn(roomcontrols);
            AnsiConsole.Render(tab);
        }       
    }

    public class BattleFloor : Floor {
        public BattleFloor(Dungeon d) : base(d)
        {

        }
        Battle b;
        public override void DoFLoor(Player p, int f)
        {
            
        }
    }
    public class RestFloor : Floor {
        public RestFloor(Dungeon d) : base(d)
        {
        }

        public override void DoFLoor(Player p, int f)
        {
            throw new NotImplementedException();
        }
    }
    public class ChestFloor : Floor { 
        public InventoryItem chestItem { get; }
        public ChestFloor(InventoryItem _chestItem, Dungeon d) : base (d) {
            chestItem = _chestItem;
        }
        public override void DoFLoor(Player p, int f)
        {            
            string panelstr;
            panelstr = "This room contains a chest!!!\n" + chestItem.sName + ": " + chestItem.sDescription + "      " + chestItem.noOfItem;
            drawFloor(panelstr, "(y) Pickup\n(n) leave behind", f);
            if (GameInputs.K() == 'y')
            {
                p.pInv.PickUpItem(chestItem);
            }
            Program.DungeonExplorer.GameInstance.WrapPlayer(p);           
        }
    }
    public class SpecialFloor : Floor {
        public SpecialFloor(Dungeon d) : base(d)
        {
        }

        public override void DoFLoor(Player p, int f)
        {
            throw new NotImplementedException();
        }
    }

}
