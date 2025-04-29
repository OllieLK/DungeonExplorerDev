using Program;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public abstract class Floor
    {
        protected Dungeon dungeon;
        public Floor(Dungeon d)
        {
            dungeon = d;
        }

        public abstract bool DoFLoor(Player p, int floorNum);
        protected virtual void drawFloor(string drawitem, string roomcontrols, int floornum, Player p)
        {
            Console.Clear();
            p.DrawOverWorld(false);
            {
                Table tab = new Table();
                tab.Title = new TableTitle(dungeon.GetDescription() + ": Room " + floornum + "/" + dungeon.numOfFloors);
                tab.AddColumn(drawitem);
                tab.AddColumn(roomcontrols);
                AnsiConsole.Render(tab);
            }
        }
    }
    public class BattleFloor : Floor
    {
        private Battle b { get; }
        public BattleFloor(Dungeon d, Battle _battle) : base(d)
        {
            b = _battle;
        }
        public override bool DoFLoor(Player p, int f)
        {
            Console.SetCursorPosition(0, 23);
            AnsiConsole.Render(new Panel("Battle up ahead. Press enter to start."));
            Console.ReadLine();
            b.startBattle(p);
            return true;
        }
    }
    public class RestFloor : Floor
    {
        public RestFloor(Dungeon d) : base(d)
        {
        }

        public override bool DoFLoor(Player p, int f)
        {
            drawFloor("This is a rest room. you can leave the dungeon at this point", "(w) continue\n(q) leave dungeon\n(e) inventory", f, p);
            switch (GameInputs.K(new List<char> { 'w', 'q', 'e' }))
            {
                case 'w':
                    return false;
                case 'e':
                    p.pInv.DrawInventory("", p);
                    DoFLoor(p, f);
                    break;
                case 'q':
                    return true;
                default: return false;
            }
            return false;
        }
    }
    public class ChestFloor : Floor
    {
        private InventoryItem chestItem { get; }
        public ChestFloor(InventoryItem _chestItem, Dungeon d) : base(d)
        {
            chestItem = _chestItem;
        }
        public override bool DoFLoor(Player p, int f)
        {
            string panelstr;
            panelstr = "This room contains a chest!!!\n" + chestItem.sName + ": " + chestItem.sDescription + "      " + chestItem.noOfItem;
            drawFloor(panelstr, "(y) Pickup\n(n) leave behind", f, p);
            if (GameInputs.K() == 'y')
            {
                p.pInv.PickUpItem(chestItem);
            }
            Program.DungeonExplorer.GameInstance.WrapPlayer(p);
            return false;
        }
    }
}

