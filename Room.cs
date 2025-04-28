using Program;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Program
{
    /// <summary>
    /// X and Y Inverted: X is from 0 going DOWN,
    /// Y From 0 going Right
    /// 
    /// 
    /// Class Map:
    ///     Has an array of rooms, and functions for displaying the map and updating it after user input
    ///     
    /// Class Room:
    ///     Class for an individual room in the map, has descriptions and characters to display on the map, aswell 
    ///     as floor items to be picked up by the player
    /// </summary>
    
    public enum DungeonDif
    {      
        EASY,
        MEDIUM,
        HARD
    }

    public class Map
    {
        public void openHyruleCastle()
        {
            foreach (Room R in Arr)
                if (R.GetType() == typeof(HyruleCastle))
                    R.interactable = true;
        }

        public const int Easies = 3;
        public const int Mediums = 4;
        public const int Hards = 3;

        public string[,] a;
        int sizeX = 10, sizeY = 20;
        private Room[,] Arr; public Room getRoomFromArr(int x, int y) { return Arr[x, y]; }

        // Initializes each room in the map array with a default character and description
        // Also sets some items in one of the rooms, and gives the 4 bordering rooms a description so that the player can move to them
        // and see the description
        public void UpdateShops() {
            foreach(Room r in Arr)
                if (r.GetType() == typeof(Shop))
                    (r as Shop).UpdateItems();
        }

        public Map(int startingposX, int startingposY)
        {
            Random rnd = new Random();
            a = new string[sizeX, sizeY];
            Arr = new Room[sizeX, sizeY];
            List<Floor> floorList = new List<Floor>();
            
            
                
            // Shops
            Arr[1, 2] = new Shop("Sheik's Shop", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[4, 6] = new Shop("Impa's Items", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[7, 12] = new Shop("Nabooru's Nook", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[8, 18] = new Shop("Rauru's Retail", InventoryItem.GetRandomItem(rnd.Next(3)));
            // Dungeons
            Arr[0, 15] = new Dungeon("Forest Temple", null, DungeonDif.EASY);
            (Arr[0, 15] as Dungeon).setFLoor(new List<Floor>
            {
                new RestFloor(Arr[0, 15] as Dungeon),
                new ChestFloor(new Food("Seared steak", 3, "Finest steak in hyrule", 20), Arr[0, 15] as Dungeon),
                new BattleFloor(Arr[0, 15] as Dungeon, new Battle(new List<Creature>
                {
                    new Enemy("Giant bee", new Weapon("Stinger", "", 15), new List<BattleMove>
                    {
                        new AttackingMove("Poison string", 15, "posion", 3),
                        new DefensiveMove("Regeneration!", 30)
                    }),
                })),
                new ChestFloor(new Weapon("Bee Stinger", "A bee stinger mounted on a stick. sharp, but basic", 20), Arr[0,15] as Dungeon),
            });
            Arr[2, 5] = new Dungeon("Fire Temple", floorList, DungeonDif.EASY);
            (Arr[2, 5] as Dungeon).setFLoor(new List<Floor>
            {
                new RestFloor(Arr[2 , 5] as Dungeon),
                new ChestFloor(new Food("Salty Fish", 5, "Fish from the akalan ocean", 15), Arr[2, 5] as Dungeon),
                new BattleFloor(Arr[2 , 5] as Dungeon, new Battle(new List<Creature>
                {
                    new Enemy("Bokoblin", new Weapon("Horn smash", "", 20), new List<BattleMove>
                    {
                        new AttackingMove("Poison potion!", 15, "posion", 3),
                        new DefensiveMove("Regeneration!", 30)
                    }),
                })),
                new ChestFloor(new Weapon("Bokoblin Horn", "Would make a good weapon", 20), Arr[2 , 5] as Dungeon),
            });
            Arr[3, 10] = new Dungeon("Water Temple", floorList, DungeonDif.EASY);
            
            Arr[5, 4] = new Dungeon("Shadow Temple", floorList, DungeonDif.MEDIUM);
            (Arr[0, 15] as Dungeon).setFLoor(new List<Floor>
            {
                new RestFloor(Arr[0, 15] as Dungeon),
                new ChestFloor(new Food("Seared steak", 3, "Finest steak in hyrule", 20), Arr[0, 15] as Dungeon),
                new BattleFloor(Arr[0, 15] as Dungeon, new Battle(new List<Creature>
                {
                    new Enemy("Giant bee", new Weapon("Stinger", "", 15), new List<BattleMove>
                    {
                        new AttackingMove("Poison string", 15, "posion", 3),
                        new DefensiveMove("Regeneration!", 30)
                    }),
                })),
                new ChestFloor(new Weapon("Bee Stinger", "A bee stinger mounted on a stick. sharp, but basic", 20), Arr[0,15] as Dungeon),
            });
            Arr[5, 15] = new Dungeon("Spirit Temple", floorList, DungeonDif.MEDIUM);
            Arr[6, 2] = new Dungeon("Ice Cavern", floorList, DungeonDif.MEDIUM);
            Arr[6, 10] = new Dungeon("Stone Tower", floorList, DungeonDif.MEDIUM);
            Arr[7, 5] = new Dungeon("Skyward Sword Temple", floorList, DungeonDif.HARD);
            Arr[8, 8] = new Dungeon("Dark Link's Lair", floorList, DungeonDif.HARD);
            Arr[9, 14] = new Dungeon("Temple of Time", floorList, DungeonDif.HARD);

            // NPC Rooms
            Arr[0, 5] = new NPCroom("Old Man");
            Arr[3, 3] = new NPCroom("Impa");
            Arr[5, 10] = new NPCroom("Nabooru");
            Arr[7, 8] = new NPCroom("Tingle");
            Arr[9, 12] = new NPCroom("The Great Fairy");

            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    if (Arr[i, j] == null)
                    {
                        Arr[i, j] = new Field("Fields of Hyrule");
                        if (rnd.Next(3) == 1)
                            Arr[i, j].FloorItems.Add(new Coin(rnd.Next(60)));
                    }

            Arr[startingposX, startingposY].setFilledIn("[purple]H[/]");           
            Arr[startingposX, startingposY].setDescription("Your house");         
            Arr[startingposX, startingposY].setC("U");
            UpdateArray(); 
        }

        // Shows The map after each "Turn"
        public void UpdateArray()
        {
            // Loop through all the rooms on the map and display their key character (set to ? if not seen yet)
            // Currently all but the starting rooms are ? as placeholders.
            for (int i = 0; i < sizeX; i++) 
            {
                for (int j = 0; j < sizeY; j++)
                {
                    a[i, j] = Arr[i, j].getC();
                    //Console.Write(Arr[i, j].getC()); // Display the character of the room
                }
            }           
        }

        // Updates the map after a player moves between rooms
        public void UpdateMap(int posX, int posY, int NposX, int NposY)
        {
            
            Arr[posX, posY].setC(Arr[posX, posY].getFilledIn());    // Reset old room position            
            Arr[NposX, NposY].setC("[blue]U[/]");          // Put U in new space
        }

        // Uses the get length function of arrays to make sure that a movement wouldnt take the player off the edge of the map.
        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Arr.GetLength(0) && y < Arr.GetLength(1);
        }

    }

    /// <summary>
    /// Class for a single "Room". the Array arr, in the map class, is a 2D array of these.
    /// They Contain: Two chars, one for the current char to display on the map, and one for the char when the map has been filled in
    /// A list of inventory Items, FloorItems, which can be picked up and inspected by the user, and a generic room description.
    /// </summary>
    /// 

    interface IInteractable
    {
        object interact();
    }

    public abstract class Room
    {
        
        public bool interactable;
        public List<InventoryItem> FloorItems = new List<InventoryItem>(); // List for floor items.
        protected string FilledIn; public string getFilledIn() { return FilledIn;  } public void setFilledIn(string c) { FilledIn = c; }
        protected string C; public string getC() { return C; } public void setC(string c) { C = c;  }
        protected string description;
        public virtual object Interact(Player p) { return null; }      
        public Room(string Description)
        {
            interactable = false;            
            description = Description;
        }
        // Getters and setters for description
        public void setDescription(string d)
        {
            description = d;
        }
        public string GetDescription()
        {
            return description;
        }
    }

    public class Field : Room
    {
        public Field(string description) : base(description)
        {
            C = "?";
            FilledIn = " ";
        }
    }

    public class Shop : Room
    {
        public void UpdateItems()
        {
            Random rnd = new Random();
            itemsForSale = InventoryItem.GetRandomItem(rnd.Next(5));
        }
        public Shop(string description, List<InventoryItem> _saleItems) : base(description) { 
            interactable = true;
            itemsForSale = _saleItems;
            C = "?";
            FilledIn = "[blue]S[/]";
        }
        List<InventoryItem> itemsForSale;
        public override object Interact(Player p)
        {
            // 7856 378395



            Console.SetCursorPosition(0, 17);
            List<char> valids = new List<char> { 'q' };

            string panelText = "Welcome to " + GetDescription() + "\n";
            for (int i = 0; i < itemsForSale.Count; i++)
            {
                panelText += "(" + (i + 1) + ") " + itemsForSale[i].sName + ": " + itemsForSale[i].sDescription + ": £" + itemsForSale[i].SalePrice +"\n";
                valids.Add((char)('0' + i + 1));
            }
            panelText += "Select the number of the item to purchase. Press Q to return to overworld";
            AnsiConsole.Render(new Panel(panelText));
            char mChoice = GameInputs.K(valids);
            switch (mChoice)
            {
                case 'q':
                    return p;
                default:
                    Console.SetCursorPosition(0, 24);
                    InventoryItem chosen = itemsForSale[mChoice - '0' - 1];
                    if(chosen.SalePrice < p.numberOfCoins)
                    {
                        p.pInv.PickUpItem(chosen);
                        p.numberOfCoins = p.numberOfCoins - chosen.SalePrice;
                        itemsForSale.Remove(chosen);
                        AnsiConsole.Render(new Panel("Item purchased. Press enter to return to main menu"));
                        Console.ReadLine();
                    }
                    else
                    {
                        AnsiConsole.Render(new Panel("You are to poor :c press enter to return to main menu"));
                        Console.ReadLine();
                    }
                    return p;
            }
        }

    }

    public class NPCroom : Room
    {
        public NPCroom(string description) : base(description) { 
            interactable = true;
            C = "?";
            FilledIn = "[yellow]N[/]";
        }
    }

    public class HyruleCastle : Room
    {
        public HyruleCastle(string Description) : base(Description)
        {
        }

        public override object Interact(Player p)
        {
            throw new NotImplementedException();
        }
    }

    public class Dungeon : Room
    {
        public DungeonDif Difficulty;
        int floorsCompleted;
        int currentFloor;
        List<Floor> floors;
        public int numOfFloors { get; private set; }

        public void setFLoor(List<Floor> f)
        {
            floors = f;
            numOfFloors = floors.Count;
        }

        public Dungeon(string description, List<Floor> _floors, DungeonDif _difficulty) : base(description)
        {
            this.Difficulty = _difficulty;
            interactable = true;
            C = "?";
            FilledIn = "[red]D[/]";
            //floors = _floors;
            floorsCompleted = 0;
            currentFloor = 1;
        }
        private void DungeonCompleted()
        {
            interactable = false;
            FilledIn = "[green]D[/]";
        }
        private void DungeonLoop(Player p)
        {
            bool exitDung = floors[currentFloor - 1].DoFLoor(p, currentFloor);
            currentFloor++;
            floorsCompleted++;
            if (floorsCompleted == floors.Count)
                DungeonCompleted();
            if (exitDung) {
                currentFloor--;
                floorsCompleted--;// So will return to the rest room not to combat
                return;
            }
            else
                DungeonLoop(p);
        }
        public override object Interact(Player p)
        {
            Console.Clear();
            p.DrawOverWorld(false);
            AnsiConsole.Render(new Panel(description + "    Floors Completed: " + floorsCompleted + "/" + floors.Count + "\n\n(Y) Go to entrance     (N) Leave"));
            if (GameInputs.K(new List<char> { 'y', 'n' }) == 'y')
            {
                
                DungeonLoop(p);
                return p;
            } else
            {
                return p;
            }
        }
    }
}