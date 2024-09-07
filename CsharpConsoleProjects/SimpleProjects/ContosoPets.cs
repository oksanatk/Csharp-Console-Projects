using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject
{
    class M3L6
    {
        /* Contoso Pets App
         * - add predefined sample data to pets array
         * Implement code branches corresponding to the user's menu selections
         * Display all info contined in the array used to store pet data (based on user menu selection)
         * Iterate an "add new animal info"code block that lets users add new animals to the pets array
         * 
         * This could also be done with an Object[] or LINQ, and Cat / Dog object classes
         */


        // so this is the entry class that calls the Animal Objects
        // so, first print the menu message to the console. then? verify input?
        public static ContosoPetClinic()
        {
            string? readResult;
            string userMenuSelection = "";

            Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
            Console.WriteLine(" 1. List all of our current pet information");
            Console.WriteLine(" 2. Add a new animal friend to the ourAnimals array");
            Console.WriteLine(" 3. Ensure animal ages and physical descriptions are complete");
            Console.WriteLine(" 4. Ensure animal nicknames and personality descriptions are complete");
            Console.WriteLine(" 5. Edit an animal’s age");
            Console.WriteLine(" 6. Edit an animal’s personality description");
            Console.WriteLine(" 7. Display all cats with a specified characteristic");
            Console.WriteLine(" 8. Display all dogs with a specified characteristic");
            Console.WriteLine("\nEnter your selection number (or type Exit to exit the program)");

            readResult = Console.ReadLine();
            if (readResult != null)
            {
                userMenuSelection = readResult.Trim().ToLower();
            }

            Console.WriteLine($"You picked option number {userMenuSelection}.");
            Console.WriteLine("Press the Enter key to continue");

            
            // per microsoft tutorial, Console.ReadLine on its own can pause a program
            readResult = Console.ReadLine();

        }



    }
}
