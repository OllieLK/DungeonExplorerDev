using Spectre.Console;
using Spectre.Console.Rendering;
using System;
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
        public static void draw(Map m, Player p)
        {
            var tab = new Table();
            var textbox = new Panel("\n\n");
            textbox.Expand = true;

            p.GameMap.UpdateArray();
            string mapstr = p.GameMap.getRoomFromArr(p.getPosX(), p.getPosY()).GetDescription() + "\n";
            mapstr += Convert2DArrayToString(m.a);
            
            // Create a new table
            tab.Title = new TableTitle("THE LEGEND OF ZELDA");
            tab.AddColumn("MAP");
            tab.AddColumn("ACTIONS (Q)");
            tab.AddColumn("INVENTORY (E)").Centered();
            tab.AddColumn("CONTROLS");
            tab.ShowRowSeparators = false;
            tab.AddRow(mapstr, ActionMenuToString(p), p.pInv.InvString(), "WASD - Move\n").Centered();
            tab.Width = Console.WindowWidth;
            string red, grey;
            (red, grey) = p.UpdateHealthString();
            tab.AddRow("[red]" + red + "[/]" + "[grey]" + grey + "[/]"); // Add
            

            
            // Render the table to the console
            AnsiConsole.Render(tab);
            AnsiConsole.Render(textbox);
            
        }
        
	}


	public class Save
	{
		public static bool saveGame()
		{
			return true;
		}
		public Save()
		{

		}
	}
}
