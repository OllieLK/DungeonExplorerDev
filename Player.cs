using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;
using Spectre.Console;
using System.Diagnostics.Eventing.Reader;
using System.Net;


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
    
    public class Player : Creature
    {
        
        public Map GameMap;
        public int numberOfCoins;
        public Room CurrentRoom; // Current room player is in
        public List<ActionMenuAction> ActionMenuFunctions = new List<ActionMenuAction>();
        int posX; public void setPosX(int i) { posX = i; }
        public int getPosX() { return posX; } // The current position of the player within the map
        int posY; public void setPosY(int i) { posY = i; }
        public int getPosY() { return posY; }
        
        public PlayerInventory pInv; // Instance of PlayerInventory class, representing the players inventory

        public void PlayerDebug() // Simple debug functions
        {
            Debug.Assert(Health <= MaxHealth);
            Debug.Assert(Health > 0);
            Debug.Assert(ActionMenuFunctions.Count() > 0);
        }

        // Battle related functions for player
        public override object Battleturn(Creature target)
        {
            Console.SetCursorPosition(0, 24);
            string displayString = "Press E to use an item on an enemy on an enemy\n";
            List<char> ValidInputs = new List<char> { 'e' };
            var FilteredInventory = pInv.GetQueriedList("weapon");
            List<Weapon> Weapons = new List<Weapon>();
            for (int i = 0; i < FilteredInventory.Count; i++)
            {
                Weapons.Add(FilteredInventory[i] as Weapon);
                ValidInputs.Add((char)('0' + i + 1));
                displayString += "(" + (i + 1) + ") " + FilteredInventory[i].sName + "\n";
            }
            AnsiConsole.Render(new Panel(displayString));
            char choice = GameInputs.K(ValidInputs);
            if (choice == 'e')
            {
                var itemToUse = pInv.GetItemInBattle();
                if (itemToUse != null)
                    target = itemToUse.UseInBattle(target);
                else
                {
                    Console.SetCursorPosition(0, 26);
                    AnsiConsole.Render(new Panel("No Valid Items To Use!\nPress enter to return"));
                    Console.ReadLine(); 
                    return target;
                }
            }                
            else
            {
                Weapon SelectedWeapon = Weapons[choice - '0' - 1];
                target.Health -= SelectedWeapon.Damage;
            }
            return target;
        }
        public override void onDeath()
        {
            Console.Clear();
            AnsiConsole.Render(new Panel("YOU HAVE DIED.\nPRESS ENTER TO RETURN TO TITLE SCREEN"));
            Console.ReadLine();
            DungeonExplorer.Main(null);
        }

        public Player()
        {

            // Assigning starting values
            BattleEffect = new StatusEffect();
            name = "Link";
            Health = 70;
            numberOfCoins = 10;
            MaxHealth = 100;
            pInv = new PlayerInventory(5); // Initializing inventory
            posX = 0;
            posY = 0;
            GameMap = new Map(posX, posY);
            CurrentRoom = GameMap.getRoomFromArr(posX, posY); // Initializing current room

            // Adding the Functions within here to the List of action menu functions
            
            ActionMenuFunctions.Add(new ActionMenuAction(ScoutForItems, "Scout Around For Items"));
        }


        

        public void OverWorldTurnMenu()
        {
            DrawOverWorld(true);
            List<char> Valids = new List<char> { 'w', 'a', 's', 'd', 'e', 'q' };
            if (CurrentRoom.interactable == true)            
                Valids.Add('r');
            
            char keyPressed = GameInputs.K(Valids);
            switch (keyPressed)
            {
                case 'r':
                    this.CurrentRoom.Interact(this);
                    break;
                case 'q':
                    ScoutForItems();
                    break;
                case 'e':
                    /*DungeonExplorer.GameInstance.WrapPlayer(*/pInv.DrawInventory("", this)/*)*/;                  
                    break;
                default:
                    MoveMenu(keyPressed);
                    break;
            }
        }

        // Function to show the user the current FloorItems that are present in the room theyre in. allows them to inspect, pickup or leave them
        private void ScoutForItems()
        {
            Console.Clear();
            DrawOverWorld(false);
            string scoutString = "";
            List<char> valids = new List<char> { 'q' };
            if (CurrentRoom.FloorItems.Count == 0) // IF no items present displays quick message and returns
            {
                AnsiConsole.Write(new Panel("There is nothing around here to collect.\nPress enter to return to main menu"));
                Console.ReadLine();
                return;
            }
            else
            {
                for (int i = 0; i < CurrentRoom.FloorItems.Count; i++) // Displays floor items present in list
                {
                    scoutString += ("(" + (i + 1) + ") " + CurrentRoom.FloorItems[i].sName + "\n");
                    valids.Add((char)('0' + i + 1));
                }
            }

            // User input for what to do about the floor items
            scoutString += ("Type the number of the item you would like to inspect / pick up.\nIf you dont want to pick any up, press Q");
            AnsiConsole.Render(new Panel(scoutString));
            char MChoice = GameInputs.K(valids);
            
            switch (MChoice)
            {
                case 'q':
                    return;
                default:
                    int k = MChoice - '0';
                    Console.SetCursorPosition(0, 22);
                    scoutString = "";
                    scoutString += CurrentRoom.FloorItems[k - 1].sName + ": " + CurrentRoom.FloorItems[k - 1].sDescription + "\n(E) Pickup\n(Q) Leave";
                    AnsiConsole.Render(new Panel(scoutString));
                    switch(GameInputs.K(new List<char> { 'e', 'q' }))
                    {
                        case 'e':
                            if (CurrentRoom.FloorItems[k - 1].type == "coin")
                                this.numberOfCoins += CurrentRoom.FloorItems[k - 1].noOfItem;
                            else
                                pInv.PickUpItem(CurrentRoom.FloorItems[k - 1]); // Add item to inventory
                            CurrentRoom.FloorItems.RemoveAt((k - 1)); // Remove from the floor
                            return;
                        case 'q':
                            return;
                    }
                    break;
            }
        }

        

        public void DrawOverWorld(bool showControls)
        {
            string controls;
            string healthstr = this.UpdateHealthString();
            var tab = new Table();
            this.GameMap.UpdateArray();
            controls = "WASD - Move around\nE - Open Inventory\nQ - Forage For Items";
            if (CurrentRoom.interactable == true)
            {
                controls += "\nR - Interact";
            }
            string mapstr = "";
            mapstr += Utils.Convert2DArrayToString(this.GameMap.a);
            mapstr += ":pushpin: ";
            mapstr += this.GameMap.getRoomFromArr(this.getPosX(), this.getPosY()).GetDescription();
            
            tab.Title = new TableTitle("THE LEGEND OF ZELDA");
            tab.AddColumn("World Map");
            


            if (showControls)
            {
                tab.AddColumn("Controls");
                tab.AddRow(healthstr + "                   [olive]" + numberOfCoins + " Coins[/]"); // Add
                tab.AddRow(mapstr, controls);
            }
            else
            {
                tab.AddRow(healthstr + "                   [olive]" + numberOfCoins + " Coins[/]"); // Add
                tab.AddRow(mapstr);
            }
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

        public IBattleUsable GetItemInBattle()
        {
            string printString = string.Empty;
            List<char> valids = new List<char>();
            var validItems = Inventory.OfType<IBattleUsable>().ToList();
            if (validItems.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < validItems.Count; i++) {
                valids.Add((char)('0' + i + 1));
                printString += "(" + (i + 1) + ") " + (validItems[i] as InventoryItem).sName;
            }
            AnsiConsole.Render(new Panel(printString));
            var chosenItem = validItems[GameInputs.K(valids) - '0' - 1];
            this.DeleteItem((chosenItem as InventoryItem), false);

            return chosenItem;
        }

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
        // Checks for duplicate items and stacks them neatly
        private void UpdateInv()
        {
            if (Inventory.Count <= 1)
                return;
            for (int i = 0; i < Inventory.Count - 1; i++)
            {
                for (int j = i + 1; j < Inventory.Count; j++)
                {
                    if (Inventory[i].sName == Inventory[j].sName)
                    {
                        int spaceLeft = Inventory[i].maxNoOfItem - Inventory[i].noOfItem;
                        if (spaceLeft > 0)
                        {
                            int amountToMove = Math.Min(spaceLeft, Inventory[j].noOfItem);
                            Inventory[i].noOfItem += amountToMove;
                            Inventory[j].noOfItem -= amountToMove;
                            if (Inventory[j].noOfItem == 0)
                            {
                                Inventory.RemoveAt(j);
                                j--; // stay on same index since list just shifted
                            }
                        }
                    }
                }
            }
        }

        public Player DrawInventory(string Query, Player p)
        {
            UpdateInv();
            Console.Clear();
            p.DrawOverWorld(false);
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
                AnsiConsole.Render(new Panel("Inventory is full\nPress enter to return"));
                Console.ReadLine();
            }
            foreach(InventoryItem i in Inventory)
            {
                if (i.sName == ItemToAdd.sName)
                {
                    i.noOfItem += ItemToAdd.noOfItem;
                    if (i.noOfItem > i.maxNoOfItem)
                        i.noOfItem = i.maxNoOfItem;
                }
            }
            Inventory.Add(ItemToAdd);
            return;
        }

        public void DeleteItem(InventoryItem ItemToRemove, bool All, bool Keyitem)
        {
            if (All)
                Inventory.Remove(ItemToRemove);
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i] == ItemToRemove)
                {
                    if (Inventory[i].noOfItem != 1)
                        Inventory[i].noOfItem--; // quick search to find index to remove at
                    else
                        Inventory.Remove(ItemToRemove);
                }
            }
        }
        // Simple function to delete item. does need a quick linear search to find the index to remove at if removing one.
        public void DeleteItem(InventoryItem ItemToRemove, bool All) // Remove item from the inventory
        {
            if (ItemToRemove.type != "keyitems")
            {
                if (All)
                    Inventory.Remove(ItemToRemove);
                for (int i = 0; i < Inventory.Count; i++)
                {
                    if (Inventory[i] == ItemToRemove)
                    {
                        if (Inventory[i].noOfItem != 1)
                            Inventory[i].noOfItem--; // quick search to find index to remove at
                        else
                            Inventory.Remove(ItemToRemove);
                    }
                }
            }
            else
            {
                Console.SetCursorPosition(0, 29);
                AnsiConsole.Render(new Panel("Cannot remove key items\nPress Enter to return"));
                Console.ReadLine();
            }
        }
    }
}