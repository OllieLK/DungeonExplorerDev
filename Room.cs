﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
    public class Map
    {
        public string[,] a;
        int sizeX = 10, sizeY = 20;
        private Room[,] Arr; public Room getRoomFromArr(int x, int y) { return Arr[x, y]; }

        // Initializes each room in the map array with a default character and description
        // Also sets some items in one of the rooms, and gives the 4 bordering rooms a description so that the player can move to them
        // and see the description
        public Map(int startingposX, int startingposY)
        {
            Random rnd = new Random();
            a = new string[sizeX, sizeY];
            Arr = new Room[sizeX, sizeY];

            
                
            // Shops
            Arr[1, 2] = new Shop("[blue]S[/]", "?", "Sheik's Shop", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[4, 6] = new Shop("[blue]S[/]", "?", "Impa's Items", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[7, 12] = new Shop("[blue]S[/]", "?", "Nabooru's Nook", InventoryItem.GetRandomItem(rnd.Next(3)));
            Arr[8, 18] = new Shop("[blue]S[/]", "?", "Rauru's Retail", InventoryItem.GetRandomItem(rnd.Next(3)));
            // Dungeons
            Arr[0, 15] = new Dungeon("[red]D[/]", "?", "Forest Temple");
            Arr[2, 5] = new Dungeon("[red]D[/]", "?", "Fire Temple");
            Arr[3, 10] = new Dungeon("[red]D[/]", "?", "Water Temple");
            Arr[5, 4] = new Dungeon("[red]D[/]", "?", "Shadow Temple");
            Arr[5, 15] = new Dungeon("[red]D[/]", "?", "Spirit Temple");
            Arr[6, 2] = new Dungeon("[red]D[/]", "?", "Ice Cavern");
            Arr[6, 10] = new Dungeon("[red]D[/]", "?", "Stone Tower");
            Arr[7, 5] = new Dungeon("[red]D[/]", "?", "Skyward Sword Temple");
            Arr[8, 8] = new Dungeon("[red]D[/]", "?", "Dark Link's Lair");
            Arr[9, 14] = new Dungeon("[red]D[/]", "?", "Temple of Time");

            // NPC Rooms
            Arr[0, 5] = new NPCroom("[yellow]N[/]", "?", "Old Man");
            Arr[3, 3] = new NPCroom("[yellow]N[/]", "?", "Impa");
            Arr[5, 10] = new NPCroom("[yellow]N[/]", "?", "Nabooru");
            Arr[7, 8] = new NPCroom("[yellow]N[/]", "?", "Tingle");
            Arr[9, 12] = new NPCroom("[yellow]N[/]", "?", "The Great Fairy");

            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    if (Arr[i, j] == null)
                    {
                        Arr[i, j] = new Room(" ", "?", "Fields of Hyrule");
                        if (rnd.Next(6) == 1)
                            Arr[i, j].FloorItems.Add(new Coin(rnd.Next(60)));
                    }

            Arr[startingposX, startingposY].setFilledIn("[purple]⌂[/]");           
            Arr[startingposX, startingposY].setDescription("Your house");

            Arr[startingposX, startingposY].FloorItems.Add(new Coin(200));

            
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

    public class Room
    {
        public bool interactable;
        public List<InventoryItem> FloorItems = new List<InventoryItem>(); // List for floor items.
        string FilledIn; public string getFilledIn() { return FilledIn;  } public void setFilledIn(string c) { FilledIn = c; }
        string C; public string getC() { return C; } public void setC(string c) { C = c;  }
        private string description;

        public virtual object Interact(Player p) { return null; }
        

        public Room(string filledIn, string c, string description)
        {
            interactable = false;
            // Basic constructor assigning variables
            FilledIn = filledIn;
            C = c;
            this.description = description;
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


    public class Shop : Room
    {
        public Shop(string filledIn, string c, string description, List<InventoryItem> _saleItems) : base(filledIn, c, description) { 
            interactable = true;
            itemsForSale = _saleItems;
        
        }
        List<InventoryItem> itemsForSale;
        public override object Interact(Player p)
        {
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
        public NPCroom(string filledIn, string c, string description) : base(filledIn, c, description) { interactable = true; }

    }

    public class Dungeon : Room
    {
        public Dungeon(string filledIn, string c, string description) : base(filledIn, c, description) { interactable = true; }
        public override object Interact(Player p)
        {
            Console.WriteLine("Get a load of this ASSHOOOEWLLL");
            Console.ReadLine();
            return null;
        }
    }
}