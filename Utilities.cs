using System;
using System.Collections.Generic;


namespace Program
{
    public class Utils
    {
        public static string Convert2DArrayToString(string[,] array)
        {
            string result = string.Empty;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                var row = new List<string>();
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    row.Add(array[i, j].ToString());
                }
                result += string.Join(" ", row);

                if (i < (array.GetLength(0) - 1))
                    result += "\n";
            }
            result += "\n";
            return result;
        }
    }
    // Class containing static functions for validating userinputs within the game
    public class GameInputs
    {
        public static int G() // Simple function for returning integers from user input
        {
            int UserInput;
            do
            {
                try
                {
                    UserInput = Int16.Parse(Console.ReadLine());
                    return UserInput;
                }
                catch { Console.WriteLine("Invalid Input"); }
            } while (true);
        }

        // For menus, the user inputs integers. This function is for ensuring the users input is valid within the bounds
        // There is one overload for if a minimum is not provided, it would be assumed to be 1.
        public static int V(int max) { return V(max, 1); }
        public static int V(int max, int min)
        {
            bool Valid = false;

            do
            {
                int UserInput = G();
                if (UserInput <= max)
                {
                    if (UserInput >= min)
                    {
                        return UserInput;
                    }
                }
                Console.WriteLine("Invalid Input");
            } while (Valid == false);
            return 0;
        }

        public static Char K() // Overload incase just require yes or no keyboard inputs to save time.
        {
            return K(new List<char> { 'y', 'n' });
        }
        // Some times in the game key inputs are used. This function gets key inputs and checks that they are valid, in the array given to it.
        public static Char K(List<char> ValidKeys)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            
            if (ValidKeys.Contains(Char.ToLower(key.KeyChar))) { return Char.ToLower(key.KeyChar); };
            while (ValidKeys.Contains(Char.ToLower(key.KeyChar)) == false)
            {
                key = Console.ReadKey();
            }
            return Char.ToLower(key.KeyChar);
        }
    }
    
}