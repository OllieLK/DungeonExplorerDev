using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;
using Spectre.Console;


namespace Program
{
    /// <summary>
    /// Contains all the related classes and functions for the player:
    /// Class Action Menu Action:
    ///     Contains an action and a name, which is used in the player class to list the players currently avaliable actions
    ///     Doing it this way means i can add more player actions as the player progresses
    ///     
    /// Class Player:
    ///     The largest class, Contains all the logic for:
    ///         Moving the player between rooms
    ///         Searching for items
    ///         Displaying the current room information
    ///     Also Contains an instance of the PlayerInventory Class, for the players inventory.
    /// 
    /// Class PlayerInventory:
    ///     Contains the list of inventory items which represents the players current inventory.
    ///     and functions for Adding, Removing and viewing items in the inventory
    ///     
    /// Class Inventory Item
    /// </summary>
    public class ActionMenuAction
    {
        public Action A;
        public string N;
        public ActionMenuAction(Action iA, string iN)
        {
            // Basic constructor assigning values
            A = iA;
            N = iN;
        }
    }
    
    public class Player
    {
        public Map GameMap;
        public Room CurrentRoom; // Current room player is in
        public List<ActionMenuAction> ActionMenuFunctions = new List<ActionMenuAction>();
        int posX; public void setPosX(int i) { posX = i; }
        public int getPosX() { return posX; } // The current position of the player within the map
        int posY; public void setPosY(int i) { posY = i; }
        public int getPosY() { return posY; }
        public int Health;
        int MaxHealth { get; set; }
        public PlayerInventory pInv; // Instance of PlayerInventory class, representing the players inventory

        public void PlayerDebug() // Simple debug functions
        {
            Debug.Assert(Health <= MaxHealth);
            Debug.Assert(Health > 0);
            Debug.Assert(ActionMenuFunctions.Count() > 0);
        }

        public Player()
        {
            // Assigning starting values
            Health = 70;
            MaxHealth = 100;
            pInv = new PlayerInventory(5); // Initializing inventory
            posX = 5;
            posY = 8;
            GameMap = new Map(posX, posY);
            CurrentRoom = GameMap.getRoomFromArr(posX, posY); // Initializing current room
            pInv.PickUpItem(new InventoryItem("weapon", "Sword", 1, 1));


            // Adding the Functions within here to the List of action menu functions
            ActionMenuFunctions.Add(new ActionMenuAction(ShowRoomDescription, "Show Room Description"));
            ActionMenuFunctions.Add(new ActionMenuAction(ScoutForItems, "Scout Around For Items"));
        }


        // Basic function to show the players health, made to look nice using colours and unicode characters.
        public (string, string) UpdateHealthString()
        {
            string red = string.Empty;
            string grey = string.Empty;
            for (int i = 0; i < MaxHealth; i = i + 10)
            {
                if ((Health - i) > 4)              
                    red += "♥";                
                else               
                    grey += "♥";                
            }
            return (red, grey);
        }

        public void OverWorldTurnMenu()
        {
            DrawOverWorld();

            char keyPressed = GameInputs.K(new List<Char> { 'w', 'a', 's', 'd', 'e', 'q' });
            switch (keyPressed)
            {
                case 'q':

                    break;
                case 'e':
                    DungeonExplorer.GameInstance.WrapPlayer(pInv.DrawInventory("", this));                  
                    break;
                default:
                    MoveMenu(keyPressed);
                    break;
            }
        }

        // Function to show the user the current FloorItems that are present in the room theyre in. allows them to inspect, pickup or leave them
        private void ScoutForItems()
        {
            if (CurrentRoom.FloorItems.Count == 0) // IF no items present displays quick message and returns
            {
                Console.WriteLine("Theres nothing around here...\nPress enter to return.");
                Console.ReadLine();
                return;
            }
            else
            {
                for (int i = 0; i < CurrentRoom.FloorItems.Count; i++) // Displays floor items present in list
                {
                    Console.WriteLine("[" + (i + 1) + "] " + CurrentRoom.FloorItems[i].sName);
                }
            }

            // User input for what to do about the floor items
            Console.WriteLine("Type the number of the item you would like to inspect / pick up.\nIf you dont want to pick any up, press 0");
            int MChoice = GameInputs.V(CurrentRoom.FloorItems.Count, 0);

            if (MChoice == 0)
            {
                return; // Returns if user presses 0
            }

            // Shows description of floor item
            Console.WriteLine(CurrentRoom.FloorItems[MChoice - 1].sName + ": " + CurrentRoom.FloorItems[MChoice - 1].sDescription + "\n[1] Pickup\n[2] Leave");
            if (GameInputs.V(2) == 1)
            {
                pInv.PickUpItem(CurrentRoom.FloorItems[MChoice - 1]); // Add item to inventory
                CurrentRoom.FloorItems.RemoveAt((MChoice - 1)); // Remove from the floor
            }
            else
            {
                Console.WriteLine("You Return");
            }
        }

        // Simple function to get and show the current room description
        private void ShowRoomDescription()
        {
            Console.WriteLine(CurrentRoom.GetDescription() + "\nPress Enter To Continue");
            Console.ReadLine();
        }



        public void DrawOverWorld()
        {
            string red, grey;
            (red, grey) = this.UpdateHealthString();
            var tab = new Table();
            this.GameMap.UpdateArray();

            string mapstr = "";
            mapstr += Utils.Convert2DArrayToString(this.GameMap.a);
            mapstr += ":pushpin: ";
            mapstr += this.GameMap.getRoomFromArr(this.getPosX(), this.getPosY()).GetDescription();
            
            tab.Title = new TableTitle("THE LEGEND OF ZELDA");
            tab.AddColumn("World Map");
            tab.AddColumn("Controls");


            tab.AddRow("[red]" + red + "[/]" + "[grey]" + grey + "[/]", "WASD - Move around"); // Add
            tab.AddRow(mapstr, "E - Open Inventory\nQ - Forage For Items");
            // Render the table to the console
            AnsiConsole.Render(tab);
        }
    
            // Menu for moving the player between rooms, using the WASD keys for convinience.
        private void MoveMenu(char direction)
        {
            int newX = posX;
            int newY = posY;

            // Handle movement based on the key pressed
            switch (direction)
            {
                case 'a':
                    newY--;
                    break;
                case 'w':
                    newX--;
                    break;
                case 'd':
                    newY++;
                    break;
                case 's':
                    newX++;
                    break;
                default:
                    break;
            }
            // Check if the new position is valid ( Wouldnt send the player off the map )
            if (GameMap.IsValidPosition(newX, newY))
            {
                // Update the map and player position
                GameMap.UpdateMap(posX, posY, newX, newY);
                posX = newX; 
                posY = newY; 

                CurrentRoom = GameMap.getRoomFromArr(posX, posY); // Update current room

                // Clear the screen and show the updated map               
            }
            GameMap.UpdateArray();
        }     
    }


    // Player inventory class, with various functions to manage the inventory
    public class PlayerInventory
    {
        // Simple constructor assigns the capacity
        public PlayerInventory(int inCapacity)
        {
            iCapacity = inCapacity;
           
            Inventory.Add(new Food("food","Sausage Roll", 10, 3)); // Initializes the list of inventory items, adding 3 sausage rolls as a placeholder
            
        }
        private int iCapacity;
        public int getICapacity() { return iCapacity;  }
        public void setICapacity(int setV) { iCapacity = setV; } // Getters and setters for Capacity if it needs to be changed

        public void DebugInv()
        {
            Debug.Assert(Inventory.Count > -1);
            Debug.Assert(Inventory.Count <= iCapacity);
        }
        private List<InventoryItem> Inventory = new List<InventoryItem>(); // The list of inventory items the player currently has
        public int GetInventoryCount() { return Inventory.Count;}

        public bool IsItemPresent(InventoryItem item)
        {
            return (Inventory.Contains(item));
        }
        public List<InventoryItem> GetQueriedList(string Query)
        {
            List<InventoryItem> Queried = new List<InventoryItem>();
            if (Query == "" || Query == "a")
                return (Queried = Inventory);                       
            Queried = Inventory.Where(InventoryItem => InventoryItem.type == Query).ToList();
            return Queried;
        }
        public Player ShowInventoryItem(InventoryItem item, Player p) {
            Console.SetCursorPosition(0, 22);
            List<char> valids = new List<char> { 'd', 'q' };
            Panel showPanel;
            if (item.GetType() == typeof(Food))
            {
                showPanel = new Panel(item.sName + ": " + item.sDescription + "\nType: " + item.type + "\n(D) Remove  (Q) Return to menu (U) Use");
                valids.Add('u');
            }
            else
                showPanel = new Panel(item.sName + ": " + item.sDescription + "\nType: " + item.type + "\n(D) Remove  (Q) Return to menu");
            

            showPanel.Header = new PanelHeader("Item:");
            AnsiConsole.Render(showPanel);
            switch (GameInputs.K(valids))
            {
                case 'u':
                    p = item.Use(p);
                    this.DeleteItem(item, false);
                    break;
                case 'd':
                    Console.SetCursorPosition(0, 27);
                    AnsiConsole.Render(new Panel("(1) Remove one\n(A) Remove all\n(C) Cancel") { Header = new PanelHeader("Remove?") });
                    switch (GameInputs.K(new List<char> { '1', 'c', 'a' }))
                    {
                        case '1':
                            DeleteItem(item, false); break;
                        case 'a':
                            DeleteItem(item, true); break;
                        case 'c':
                            return p;
                    }
                    break;
                case 'q':
                    return p;
            }
            return p;
        }
        public Player DrawInventory(string Query, Player p)
        {
            Console.Clear();
            p.DrawOverWorld();
            Console.SetCursorPosition(0, 17);
            List<InventoryItem> displayItems = GetQueriedList(Query);
            Panel invPanel, queryPanel;
            queryPanel = new Panel("Sorting Options:\n- (A) All\n- (W) Weapons\n- (F) Foods\n- (K) Key Items\nOr:\n- (Q) Leave Menu");
            if (Query == "")
                invPanel = new Panel(InvString(displayItems));
            else
                invPanel = new Panel(InvString(displayItems));

            invPanel.Header = new PanelHeader("Inventory");
            queryPanel.Header = new PanelHeader("Options");
            
            AnsiConsole.Write(new Columns(invPanel, queryPanel).Collapse());
            List<char> valids = new List<char> { 'a', 'w', 'f', 'k', 'q' };
            for (int i = 0; i < displayItems.Count; i++)
            {
                valids.Add((char)('0' + i + 1));
            }
            char keyPressed = GameInputs.K(valids);
            int k = keyPressed - '0';
            if (k < 10)
            {
                p = ShowInventoryItem(displayItems[k - 1], p);
                DrawInventory("", p);
            }
            switch (keyPressed)
            {
                case 'a':
                    DrawInventory("", p);
                    break;
                case 'w':
                    DrawInventory("weapon", p);
                    break;
                case 'f':
                    DrawInventory("food", p);
                    break;
                case 'k':
                    DrawInventory("keyitems", p);
                    break;
                case 'q':
                    return p;                    
            }
            return p;
        }
        
        private static string InvString(List<InventoryItem> l)
        {
            if (l.Count == 0)
                return "Empty       \n";

            string s = String.Empty;
            for (int i = 0; i < l.Count; i++) // Loops through list displaying the item, number they have and the max number they can hold
            {
                s += "(" + (i + 1).ToString() + ") " + l[i].sName + ": " + l[i].noOfItem.ToString() + "of" + l[i].maxNoOfItem.ToString();
                s += "\n";
            }
            return s;
        }

        // Adds one inventory item to the list, checking not full first
        public void PickUpItem(InventoryItem ItemToAdd) 
        {
            if (Inventory.Count == iCapacity) // Checking inventory not full
            {
                Console.WriteLine( "Your inventory is full! You must drop something first");
            }
            Inventory.Add(ItemToAdd);
            return;
        }

        
        // Simple function to delete item. does need a quick linear search to find the index to remove at if removing one.
        public void DeleteItem(InventoryItem ItemToRemove, bool All) // Remove item from the inventory
        {
            if (All)
                Inventory.Remove(ItemToRemove);
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i] == ItemToRemove)
                {
                    Inventory[i].noOfItem--; // quick search to find index to remove at
                }
            }
        }
    }
}