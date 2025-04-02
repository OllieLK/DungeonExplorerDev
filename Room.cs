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
        public char[,] a;
        int sizeX = 10, sizeY = 20;
        public Room[,] Arr; public Room getRoomFromArr(int x, int y) { return Arr[x, y]; }

        // Initializes each room in the map array with a default character and description
        // Also sets some items in one of the rooms, and gives the 4 bordering rooms a description so that the player can move to them
        // and see the description
        public Map(int startingposX, int startingposY)
        {
            a = new char[sizeX, sizeY];
            Arr = new Room[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Arr[i, j] = new Room('?', '?', "Description Example.");
                }
            }
            Arr[startingposX, startingposY].setFilledIn('⌂');
            Arr[startingposX, startingposY].setDescription("Your house");
            Arr[startingposX, startingposY].FloorItems.Add(new InventoryItem("Weapon", "Sword", 1));

            // Setting up some sample room descriptions
            Arr[(startingposX + 1), startingposY].setDescription("Duelside-Depths");
            Arr[(startingposX - 1), startingposY].setDescription("Green, Grassy Plateu. You feel safe here");
            Arr[(startingposX), startingposY - 1].setDescription("Base of Mount Hylia");
            Arr[(startingposX), startingposY + 1].setDescription("Path To Kakariko");

            // Set the starting position of the player as 'U'
            Arr[startingposX, startingposY].setC('U');
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
            Arr[NposX, NposY].setC('U');          // Put U in new space
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
    public class Room
    {
        public List<InventoryItem> FloorItems = new List<InventoryItem>(); // List for floor items.
        char FilledIn; public char getFilledIn() { return FilledIn;  } public void setFilledIn(char c) { FilledIn = c; }
        char C; public char getC() { return C; } public void setC(char c) { C = c;  }
        private string description;

        public Room(char filledIn, char c, string description)
        {
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
}