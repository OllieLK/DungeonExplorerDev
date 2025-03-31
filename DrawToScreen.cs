using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;


namespace Program
{
	public class DrawScreen
	{
        public static string Convert2DArrayToString(char[,] array)
        {
            // Initialize an empty string to hold the result
            string result = string.Empty;

            // Loop through each row of the 2D array
            for (int i = 0; i < array.GetLength(0); i++)
            {
                // Create a list to hold the elements of the current row
                var row = new List<string>();

                // Loop through each column of the current row
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    // Add the element to the row list as a string
                    row.Add(array[i, j].ToString());
                }

                // Join the row elements with a space (or other separator if needed)
                result += string.Join(" ", row);

                if (i < (array.GetLength(0) - 1))
                    result += "\n";
            }
            result += "\n";
            // Return the final result string
            return result;
        }      
        public static string ActionMenuToString(Player p)
        {
            var a = p.ActionMenuFunctions;
            string s = string.Empty;
            for (int i = 0; i < a.Count; i++)
            {
                s += "{" + (i+1).ToString() + "} " + a[i].N;
                s += "\n";
            }
            return s;
        }

        

        public static void drawOverWorld(Map m, Player p)
        {
            string red, grey;
            (red, grey) = p.UpdateHealthString();
            var tab = new Table();
            p.GameMap.UpdateArray();

            string mapstr = "";
            mapstr += Convert2DArrayToString(m.a);
            mapstr += ":pushpin: ";
            mapstr += p.GameMap.getRoomFromArr(p.getPosX(), p.getPosY()).GetDescription();

            tab.Title = new TableTitle("THE LEGEND OF ZELDA");
            tab.AddColumn("World Map");
            tab.AddColumn("Controls");
            
            
            tab.AddRow("[red]" + red + "[/]" + "[grey]" + grey + "[/]", "WASD - Move around"); // Add
            tab.AddRow(mapstr, "E - Open Inventory");
            // Render the table to the console
            AnsiConsole.Render(tab);
                        
        }
        public static void DrawInventory(Player p)
        {
            Console.Clear();
            var invPanel = new Panel(p.pInv.InvString());
            invPanel.Header = new PanelHeader("Inventory");
            AnsiConsole.Render(invPanel);
            Console.ReadLine();
        }
	}


	public class Save
	{
		public static bool saveGame()
		{
			return true;
		}
        public static bool loadGame() { 
            throw new NotImplementedException();
        }
	}
}
