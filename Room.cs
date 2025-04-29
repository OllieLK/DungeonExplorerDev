using Program;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
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

        public string[,] a { get; private set; }
        private const int sizeX = 10, sizeY = 20;
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


            Arr[5, 17] = new HyruleCastle();
            
                
            // Shops
            Arr[1, 2] = new Shop("Sheik's Shop", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[4, 6] = new Shop("Impa's Items", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[7, 12] = new Shop("Nabooru's Nook", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[8, 18] = new Shop("Rauru's Retail", InventoryItem.GetRandomItem(rnd.Next(3)));
            // Dungeons
            // Forest Temple (EASY) - 4 rooms
            Arr[0, 15] = new Dungeon("Forest Temple", null, DungeonDif.EASY);
            (Arr[0, 15] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[0, 15] as Dungeon),
                new ChestFloor(new Food("Seared steak", 3, "Finest steak in Hyrule", 20), Arr[0, 15] as Dungeon),
                new BattleFloor(Arr[0, 15] as Dungeon, new Battle(Enemy.getEnemies(2, DungeonDif.EASY))),
                new ChestFloor(new Weapon("Bee Stinger", "A bee stinger mounted on a stick. Sharp, but basic", 20), Arr[0, 15] as Dungeon),
            });

            // Fire Temple (EASY) - 4 rooms
            Arr[2, 5] = new Dungeon("Fire Temple", null, DungeonDif.EASY);
            (Arr[2, 5] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[2, 5] as Dungeon),
                new ChestFloor(new Food("Mystic Herb", 2, "A magical herb that restores health", 30), Arr[2, 5] as Dungeon),
                new BattleFloor(Arr[2, 5] as Dungeon, new Battle(Enemy.getEnemies(3, DungeonDif.EASY))),
                new ChestFloor(new Weapon("Fire Sword", "A sword imbued with the power of fire", 25), Arr[2, 5] as Dungeon),
            });

            // Water Temple (EASY) - 4 rooms
            Arr[3, 10] = new Dungeon("Water Temple", null, DungeonDif.EASY);
            (Arr[3, 10] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[3, 10] as Dungeon),
                new ChestFloor(new Food("Watermelon", 5, "Fresh and hydrating", 15), Arr[3, 10] as Dungeon),
                new BattleFloor(Arr[3, 10] as Dungeon, new Battle(Enemy.getEnemies(3, DungeonDif.EASY))),
                new ChestFloor(new Weapon("Water Staff", "A staff that controls water currents", 18), Arr[3, 10] as Dungeon),
            });

            // Shadow Temple (MEDIUM) - 5 rooms
            Arr[5, 4] = new Dungeon("Shadow Temple", null, DungeonDif.MEDIUM);
            (Arr[5, 4] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[5, 4] as Dungeon),
                new ChestFloor(new Food("Shadow Soup", 4, "A dark dish that boosts your stealth", 25), Arr[5, 4] as Dungeon),
                new BattleFloor(Arr[5, 4] as Dungeon, new Battle(Enemy.getEnemies(3, DungeonDif.MEDIUM))),
                new ChestFloor(new Weapon("Shadow Blade", "A cursed sword that deals heavy damage", 30), Arr[5, 4] as Dungeon),
                new ChestFloor(new Food("Dark Elixir", 1, "An elixir that increases dark magic power", 50), Arr[5, 4] as Dungeon),
            });

            // Spirit Temple (MEDIUM) - 5 rooms
            Arr[5, 15] = new Dungeon("Spirit Temple", null, DungeonDif.MEDIUM);
            (Arr[5, 15] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[5, 15] as Dungeon),
                new ChestFloor(new Food("Spirit Fruit", 2, "A mysterious fruit with ethereal power", 35), Arr[5, 15] as Dungeon),
                new BattleFloor(Arr[5, 15] as Dungeon, new Battle(Enemy.getEnemies(3, DungeonDif.MEDIUM))),
                new ChestFloor(new Weapon("Spirit Bow", "A bow that fires arrows of light", 22), Arr[5, 15] as Dungeon),
                new ChestFloor(new Weapon("Spirit Staff", "A magical staff that channels ethereal power", 25), Arr[5, 15] as Dungeon),
            });

            // Ice Cavern (MEDIUM) - 5 rooms
            Arr[6, 2] = new Dungeon("Ice Cavern", null, DungeonDif.MEDIUM);
            (Arr[6, 2] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[6, 2] as Dungeon),
                new ChestFloor(new Food("Frost Berries", 3, "Berries that chill and heal", 20), Arr[6, 2] as Dungeon),
                new BattleFloor(Arr[6, 2] as Dungeon, new Battle(Enemy.getEnemies(2, DungeonDif.MEDIUM))),
                new ChestFloor(new Weapon("Ice Sword", "A sword that freezes enemies on contact", 28), Arr[6, 2] as Dungeon),
                new ChestFloor(new Food("Iced Nectar", 2, "A chilling nectar that restores health", 30), Arr[6, 2] as Dungeon),
            });

            // Stone Tower (MEDIUM) - 5 rooms
            Arr[6, 10] = new Dungeon("Stone Tower", null, DungeonDif.MEDIUM);
            (Arr[6, 10] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[6, 10] as Dungeon),
                new ChestFloor(new Food("Granite Bread", 2, "Sturdy bread that restores stamina", 18), Arr[6, 10] as Dungeon),
                new BattleFloor(Arr[6, 10] as Dungeon, new Battle(Enemy.getEnemies(3, DungeonDif.MEDIUM))),
                new ChestFloor(new Weapon("Stone Hammer", "A heavy hammer that crushes rocks", 35), Arr[6, 10] as Dungeon),
                new ChestFloor(new Weapon("Stone Shield", "A shield made from solid stone", 12), Arr[6, 10] as Dungeon),
            });

            // Skyward Sword Temple (HARD) - 6 rooms (added more battle floors)
            Arr[7, 5] = new Dungeon("Skyward Sword Temple", null, DungeonDif.HARD);
            (Arr[7, 5] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[7, 5] as Dungeon),
                new ChestFloor(new Food("Skyfruit", 2, "A mystical fruit that boosts agility", 40), Arr[7, 5] as Dungeon),
                new BattleFloor(Arr[7, 5] as Dungeon, new Battle(Enemy.getEnemies(4, DungeonDif.HARD))),
                new ChestFloor(new Weapon("Skyward Sword", "A legendary sword said to pierce through the heavens", 45), Arr[7, 5] as Dungeon),
                new ChestFloor(new Food("Cloud Nectar", 3, "A rare nectar that restores full health", 80), Arr[7, 5] as Dungeon),
                new ChestFloor(new Weapon("Sky Shield", "A shield forged in the sky, light yet strong", 20), Arr[7, 5] as Dungeon),
                new BattleFloor(Arr[7, 5] as Dungeon, new Battle(Enemy.getEnemies(5, DungeonDif.HARD))),  // Additional BattleFloor
                new BattleFloor(Arr[7, 5] as Dungeon, new Battle(Enemy.getEnemies(5, DungeonDif.HARD))),  // Additional BattleFloor
            });

            // Dark Link's Lair (HARD) - 6 rooms (added more battle floors)
            Arr[8, 8] = new Dungeon("Dark Link's Lair", null, DungeonDif.HARD);
            (Arr[8, 8] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[8, 8] as Dungeon),
                new ChestFloor(new Food("Dark Elixir", 1, "An elixir that increases dark magic power", 50), Arr[8, 8] as Dungeon),
                new BattleFloor(Arr[8, 8] as Dungeon, new Battle(Enemy.getEnemies(4, DungeonDif.HARD))),
                new ChestFloor(new Weapon("Dark Link's Blade", "A blade imbued with dark magic", 40), Arr[8, 8] as Dungeon),
                new ChestFloor(new Weapon("Dark Shield", "A shield infused with dark energy", 30), Arr[8, 8] as Dungeon),
                new ChestFloor(new Food("Shadow Nectar", 2, "A dark nectar that restores health", 60), Arr[8, 8] as Dungeon),
                new BattleFloor(Arr[8, 8] as Dungeon, new Battle(Enemy.getEnemies(5, DungeonDif.HARD))),  // Additional BattleFloor
                new BattleFloor(Arr[8, 8] as Dungeon, new Battle(Enemy.getEnemies(5, DungeonDif.HARD))),  // Additional BattleFloor
            });

            // Temple of Time (HARD) - 6 rooms (added more battle floors)
            Arr[9, 14] = new Dungeon("Temple of Time", null, DungeonDif.HARD);
            (Arr[9, 14] as Dungeon).setFloor(new List<Floor>
            {
                new RestFloor(Arr[9, 14] as Dungeon),
                new ChestFloor(new Food("Timeless Nectar", 5, "A nectar that restores full health", 100), Arr[9, 14] as Dungeon),
                new BattleFloor(Arr[9, 14] as Dungeon, new Battle(Enemy.getEnemies(4, DungeonDif.HARD))),
                new ChestFloor(new Weapon("Master Sword", "The legendary sword that seals evil", 50), Arr[9, 14] as Dungeon),
                new ChestFloor(new Weapon("Time Shield", "A shield that bends time itself", 35), Arr[9, 14] as Dungeon),
                new ChestFloor(new Food("Chrono Fruit", 3, "A fruit that slows down time for a brief period", 75), Arr[9, 14] as Dungeon),
                new BattleFloor(Arr[9, 14] as Dungeon, new Battle(Enemy.getEnemies(5, DungeonDif.HARD))),  // Additional BattleFloor
                new BattleFloor(Arr[9, 14] as Dungeon, new Battle(Enemy.getEnemies(5, DungeonDif.HARD))),  // Additional BattleFloor
            });


            // NPC Rooms
            Arr[0, 5] = new NPCroom("Old Man", "Its dangerous to go alone! with ganon back! you must find a weapon!");
            Arr[3, 3] = new NPCroom("Impa", "Have you ever heard of the triforce? its said its ten pieces\ncould be used to defeat ancient evil!");
            Arr[5, 10] = new NPCroom("Nabooru", "Have you seen the dungeons scattered around the map?\nI wonder if they hide treasure!");
            Arr[7, 8] = new NPCroom("Tingle", "Stop at some of the shops to restock gear!");
            Arr[9, 12] = new NPCroom("The Great Fairy", "Always scout around for things...\nwho knows what u might find!");

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


    public abstract class Room
    {

        public bool interactable { get; set; }

        public List<InventoryItem> FloorItems = new List<InventoryItem>(); // List for floor items.
        protected string FilledIn; public string getFilledIn() { return FilledIn;  } public void setFilledIn(string c) { FilledIn = c; }
        protected string C; public string getC() { return C; } public void setC(string c) { C = c;  }
        protected string description;
        public virtual object Interact(Player p) { return null; }
        public Room() { }
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
        private List<InventoryItem> itemsForSale;
        public override object Interact(Player p)
        {
            // 7856 378395 - mazda birmingham

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
        private string dialogue;
        public NPCroom(string description, string _dialogue) : base(description) { 
            interactable = true;
            dialogue = _dialogue;
            C = "?";
            FilledIn = "[yellow]N[/]";
        }

        
        public override object Interact(Player p)
        {
            Console.Clear();
            p.DrawOverWorld(false);
            Panel txt = new Panel(dialogue + "\nPress enter to continue");
            txt.Header = new PanelHeader(description);
            AnsiConsole.Render(txt);
            Console.Read();
            interactable = false;
            return p;
        }
    }

    public class HyruleCastle : Room
    {
        public HyruleCastle()
        {
            C = "?";
            FilledIn = "[purple]H[/]";
            description = "Hyrule Castle";
            interactable = false;
        }


        public override object Interact(Player p)
        {
            AnsiConsole.Clear();
            Panel txt = new Panel("The time to strike is now! Press enter to use the triforce\n to seal away gannondorf!");
            txt.Header = new PanelHeader("Hyrule Castle: Sanctum");
            AnsiConsole.Render(txt);
            Console.ReadLine();
            System.Threading.Thread.Sleep(1000);
            DungeonExplorer.GameInstance.EndCredits();
            return null;
        }

        
    }

    public class Dungeon : Room
    {
        public DungeonDif Difficulty { get; private set;  }
        private int floorsCompleted { get; set; } = 0;
        private int currentFloor { get; set; }
        private List<Floor> floors { get; set; }
        public int numOfFloors { get; private set; }

        public void setFloor(List<Floor> f)
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

        private static readonly Dictionary<DungeonDif, string> DifficultyStrings = new Dictionary<DungeonDif, string>
        {
        { DungeonDif.EASY, "Easy" },
        { DungeonDif.MEDIUM, "Medium" },
        { DungeonDif.HARD, "Hard" }
        };

        public override object Interact(Player p)
        {
            Console.Clear();
            p.DrawOverWorld(false);
            switch (Difficulty)
            {
                case DungeonDif.EASY:
                    break;
                case DungeonDif.MEDIUM:
                    if (p.easyCompleted == false)
                    {
                        AnsiConsole.Render(new Panel("Difficulty: Medium\nComplete easy dungeons first!!\nPress Enter"));
                        Console.ReadLine();
                        return p;
                    }
                    break;
                case DungeonDif.HARD:
                    if (p.mediumCompleted != true)
                    {
                        AnsiConsole.Render(new Panel("Difficulty: Hard\nComplete medium dungeons first!!\nPress Enter"));
                        Console.ReadLine();
                        return p;
                    }
                    break;
            }
            AnsiConsole.Render(new Panel("Difficulty: " + DifficultyStrings[Difficulty] + "\n" + description + "    Floors Completed: " + floorsCompleted + "/" + floors.Count + "\n\n(Y) Go to entrance     (N) Leave"));
            if (GameInputs.K(new List<char> { 'y', 'n' }) == 'y')
            {

                DungeonLoop(p);
                return p;
            }
            else
            {
                return p;
            }
        }

        
    }
}
