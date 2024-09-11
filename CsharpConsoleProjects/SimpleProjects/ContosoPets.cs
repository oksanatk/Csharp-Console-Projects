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
         */


        // so this is the entry class that calls the Animal Objects
        // so, first print the menu message to the console. then? verify input?
        public static void ContosoPetClinic()
        {

            Animal[] storedAnimals = new Animal[8];

            string? readResult;
            string userMenuSelection = "";

            // initialize the given default data

            storedAnimals[0] = new Animal("dog", 2, "medium sized cream colored female golden retriever weighing about 65 pounds. housebroken.", "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.", "lola");

            storedAnimals[1] = new Animal("dog",9, "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.", "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.","loki");

            storedAnimals[2] = new Animal("cat", 1, "small white female weighing about 8 pounds. litter box trained.", "friendly", "Puss");

            storedAnimals[3] = new Animal("cat");

            do
            {

                Console.Clear();

                Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
                Console.WriteLine(" 1. List all of our current pet information");
                Console.WriteLine(" 2. Add a new animal friend to the storedAnimals array");
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


                // per microsoft tutorial, Console.ReadLine on its own can pause code execution
                readResult = Console.ReadLine();

                switch (userMenuSelection)
                {
                    case "1":
                    case "one":
                        ListAllPetInfo(storedAnimals);
                        break;

                    case "2":
                    case "two":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        //AddNewAnimal();
                        break;

                    case "3":
                    case "three":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        break;

                    case "4":
                    case "four":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        break;

                    case "5":
                    case "five":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        break;

                    case "6":
                    case "six":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        break;

                    case "7":
                    case "seven":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        break;

                    case "8":
                    case "eight":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        break;


                    default:
                        Console.WriteLine("I'm sorry. I didn't recognize that input. Please try again.");
                        break;
                }

                // pause code execution, then start loop over again
                Console.WriteLine("Press the Enter key to continue.");
                Console.ReadLine();



            } while (userMenuSelection != "exit");

        }

        private static void ListAllPetInfo(Animal[] currentPets)
        {
            foreach (Animal pet in currentPets)
            {
                if (pet != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("ID #: " + pet.PetID);
                    Console.WriteLine("Species: " + pet.Species);
                    Console.WriteLine("Age: " + pet.Age);
                    Console.WriteLine("Nickname; " + pet.Nickname);
                    Console.WriteLine("Physical description: " + pet.PhysicalDescription);
                    Console.WriteLine("Personality: " + pet.Personality);

                }
              

            }
        }
        private static Animal[] AddNewAnimal(Animal[] currentAnimals)
        {
            //check if there's an open spot
            int openSpot = 0;
            bool isOpen = false;

            //loop backwards so that new animal is always put in the first available spot in the array
            for (int i = (currentAnimals.Length - 1); i >= 0; i--) {
                if (currentAnimals[i] == null)
                {
                    isOpen = true;
                    openSpot = i;
                }
            }

            if (isOpen)
            {
                Console.WriteLine();
                Console.WriteLine("Congrats, we have an open spot available.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Sorry, but it looks like we don't have any open spots right now. \nHave you tried adopting?");

                Console.WriteLine("\nNo new animals were added.");
                Console.WriteLine("Press Enter to continue back to the main menu.");
                Console.ReadLine();

                return currentAnimals;
            }

            //get user input to initialize the new animal
            string? readResult;
            string userAnimalInput = "";
            string animalSpecies = "";

            // MUST know if animal is a cat or dog to initialize
            do
            {
                Console.WriteLine("Is the new animal a cat or a dog?");
                readResult = Console.ReadLine();
                if (readResult != null) { userAnimalInput = readResult.Trim().ToLower(); }

                if (userAnimalInput == "cat" || userAnimalInput == "dog")
                {
                    animalSpecies = userAnimalInput;
                    Console.WriteLine($"Thanks for telling us that the new animal will be a {userAnimalInput}. Knowing helps us prepare for them.");

                }else
                {
                    Console.WriteLine("I'm sorry, I didn't understand that input. Our space is only set up for cats or dogs.");

                }
                Console.WriteLine("Press the Enter key to continue.\n");
                Console.ReadLine();
            } while (userAnimalInput != "cat" && userAnimalInput != "dog");


            //ask user if they know any more details - determines which Animal Class constructor to use
            do
            {
                Console.WriteLine($"Do you know anything else about this {animalSpecies}? We would like to know their nickname, personality, physical description, and/or age.");
                Console.WriteLine("Please answer Yes or No (y/n):");

                readResult = Console.ReadLine();
                if (readResult != null) { userAnimalInput = readResult.Trim().ToLower(); }

                if (userAnimalInput == "no" || userAnimalInput == "n")
                {
                    Console.WriteLine("Got it! Thanks for letting us know.");
                    Console.WriteLine($"We're preparing a space for the new {animalSpecies} right now!");

                    currentAnimals[openSpot] = new Animal(animalSpecies);
                    return currentAnimals;

                } else if (userAnimalInput == "yes" || userAnimalInput == "y")
                {
                    Console.WriteLine("That's great! We'll just ask a few more questions then...\n");
                } else
                {
                    Console.WriteLine("I'm sorry, I didn't recognize that input. Please try again.");
                }

            } while (userAnimalInput != "yes" && userAnimalInput != "y" && userAnimalInput != "no" && userAnimalInput != "n");

            // if the user only knows one other characteristic, then i don't have enough constructors. 
            // making more is simple, just time-consuming.
            // and potentially not too useful for the amount of time it'll take
            // orrr..... i could leave the number of constructors, but pass along empty string arguments 
            // and handle the exception in the actual constructor?

            //TODO:: make 2 total constructors in Animal class. 1 for species only, the other for all characteristics. 
            //    assign empty strings to be unknown characteristics.

            string animalNickname = "";
            string animalPhysicalDescription = "";
            string animalPersonality = "";
            int animalAge = -1;

            string[] askingOptions = new string[] { "age", "physical description", "personality", "nickname" };

            for (int i = 0; i < 4; i++)
            {
                // ask if they know each characteristic
                do
                {
                    Console.WriteLine($"Do you know this {animalSpecies}'s {askingOptions[i]}?");
                    Console.WriteLine("Please answer Yes or No (y/n): ");

                    readResult = Console.ReadLine();
                    if (readResult != null) { userAnimalInput = readResult.Trim().ToLower(); }
                    
                    if (userAnimalInput != "yes" && userAnimalInput != "y" && userAnimalInput != "n" && userAnimalInput != "no")
                    {
                        Console.WriteLine("I'm sorry, I didn't recognize that input. Please try again.\n");
                    }
                } while (userAnimalInput != "yes" && userAnimalInput != "y" && userAnimalInput != "n" && userAnimalInput != "no");



                // if they do know the characteristic, ask to input it 
                if (userAnimalInput == "yes" || userAnimalInput =="y")
                    {
                    Console.WriteLine($"That's great! Please type in this {animalSpecies}'s {askingOptions[i]}.");
                    readResult = Console.ReadLine();
                    if (readResult != null) { userAnimalInput = readResult.Trim().ToLower(); }

                    switch (i)
                    {

// TODO - not sure if the following while loop works correctly
                        case 0:
                            if (int.TryParse(userAnimalInput, out animalAge)) { }
                            else 
                            {
                                while (!int.TryParse(userAnimalInput, out animalAge))
                                {
                                    Console.WriteLine("I'm sorry. That input didn't look like a number. We need a number to represent their age.\n");
                                    Console.WriteLine("Please try again.");
                                    Console.WriteLine($"Please type in this {animalSpecies}'s age as a number (0):");

                                    readResult= Console.ReadLine();
                                    if (readResult != null ) { userAnimalInput= readResult.Trim().ToLower(); } 
                                }
                            }
                            Console.WriteLine($"Thanks! We've recorded that this {animalSpecies}'s age is {animalAge}.");

                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadLine();
                            break;

                        case 1:
                            animalPhysicalDescription = userAnimalInput;
                            break;

                        case 2:
                            animalPersonality = userAnimalInput;
                            break;

                        case 3:
                            animalNickname = userAnimalInput;
                            break;

                    }

                }else if (userAnimalInput == "n" || userAnimalInput == "no")
                {
                    Console.WriteLine("Got it! Thanks for letting me know.\n");
                }
            }

            //initialize in the index spot that was marked as open
            currentAnimals[openSpot] = new Animal(animalSpecies, animalAge, animalPhysicalDescription, animalPersonality, animalNickname);

            return currentAnimals;
        }



    }
}
