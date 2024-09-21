﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
            string userPetIdSelection = "";
            string userSelectedComparison = "";
            int userSelectedAge;

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

                        Console.WriteLine("You've selected to add a new animal to our clinc.");

                        storedAnimals = AddNewAnimal(storedAnimals);

                        break;



                    case "3":
                    case "three":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        // animalInstance.VisitTheVet();
                        //     done!
                        //         --> methinks, anyway. not currently tested.

                        for (int i = 0; i< storedAnimals.Length; i++)
                        {
                            storedAnimals[i] = storedAnimals[i].VisitTheVet();
                        }

                        Console.WriteLine("All animals have visited the vet. Age and physical descriptions are now complete.");

                        break;



                    case "4":
                    case "four":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        // GetToKnow()
                        //     -->> done!

                        for (int i = 0; i < storedAnimals.Length; i++)
                        {
                            storedAnimals[i] = storedAnimals[i].GetToKnow();
                        }

                        Console.WriteLine("We've gotten to know all of our animals!");

                        break;

                    case "5":
                    case "five":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        // call animal.EditAge(int newAge) function

                        do
                        {
                            Console.WriteLine("You've selected to edit an animal's age.");
                            foreach (Animal a in storedAnimals)
                            {
                                Console.WriteLine($"{a.Nickname} is a {a.Species} (pet id: {a.PetID}) with the age of {a.Age}.");
                            }

                            Console.WriteLine("Please enter the pet id of the whose age you'd like to edit.");
                            Console.WriteLine("Or type exit to return to the previous menu.");

                            readResult = Console.ReadLine();
                            if(readResult != null)
                            {
                                userPetIdSelection = readResult.Trim().ToLower();
                            }

                            for (int i = 0; i < storedAnimals.Length; i++)
                            {
                                if (storedAnimals[i].PetID == userPetIdSelection)
                                {
                                    Console.WriteLine($"{storedAnimals[i].Nickname} (pet id: {storedAnimals[i].PetID}) currently has an age of {storedAnimals[i].Age}");

                                    do
                                    {
                                        Console.WriteLine("Please enter tne new age (00) that you'd like to set. Type -1 for an unknown age.");
                                        Console.WriteLine("Or type exit to return to the previous menu.");

                                        // TODO --> set max age? 

                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            userMenuSelection = readResult.Trim().ToLower();
                                        }

                                        if (int.TryParse(userMenuSelection, out userSelectedAge))
                                        {
                                            Console.WriteLine($"Now changing {storedAnimals[i].Nickname}'s (pet id: {storedAnimals[i].PetID}) age to {userSelectedAge}.");

                                            storedAnimals[i] = storedAnimals[i].NewAge(userSelectedAge);


                                        } else
                                        {
                                            Console.WriteLine("Sorry, we couldn't recognize that age.");
                                        }

                                        i = storedAnimals.Length + 1; //end the for loop



                                    } while (userMenuSelection != "exit");
                                }
                            }

                        } while (userPetIdSelection != "exit");
                            

                        break;



                    case "6":
                    case "six":


                        string userSelectedPersonality = "";

                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        // call animal.EditPersonality(string newPersonality) function
                        do
                        {
                            Console.WriteLine("You've selected to edit an animal's personality description.");
                            
                            foreach (Animal a in storedAnimals)
                            {
                                Console.WriteLine($"Pet ID: ({a.PetID}) is a {a.Species} with the nickname {a.Nickname}. \n{a.Nickname} has the following personality:\n\t'{a.Personality}'.");
                            }

                            
                            Console.WriteLine("Please enter a valid Pet ID to continue. Or type exit to return to the previous menu.");

                            readResult = Console.ReadLine();
                            if (readResult != null)
                            {
                                userPetIdSelection = readResult.Trim().ToLower();
                            }

                            for (int i = 0;i < storedAnimals.Length; i++)
                            {
                                if (storedAnimals[i].PetID == userPetIdSelection)
                                {
                                    // if valid pet id, continue to ask user for personality input.
                                    Console.WriteLine($"Okay, cool. We'll edit the personality of the pet ({storedAnimals[i].PetID}) and nickname {storedAnimals[i].Nickname}.");
                                    Console.WriteLine("Please enter the new personality.");
                                    Console.WriteLine("Or, type exit to return to the previous menu");
                                    
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        userSelectedPersonality = readResult.Trim().ToLower();
                                    }

                                    if (userSelectedPersonality != "exit")
                                    {
                                        storedAnimals[i] = storedAnimals[i].EditPersonality(userSelectedPersonality);

                                    }

                                    i = storedAnimals.Length + 1;

                                }
                            }

                        } while (userPetIdSelection != "exit");

                        break;



                    case "7":
                    case "seven":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");

                        do
                        {

                            Console.WriteLine("You've selected to display all cats with a specific characteristic.");
                            Console.WriteLine("Please enter one of the following characterstics to search for: 'age', 'physical description', 'personality', or 'nickname' \nOR type exit to return to the main menu.");

                            string[] checkCharacteristicOptions = new string[] { "age", "physical description", "personality", "nickname"};
                            string userCharacteristicSelection = "";


                            readResult = Console.ReadLine();
                            if (readResult != null)
                            {
                                userCharacteristicSelection = readResult.Trim().ToLower();
                            }

                            Console.WriteLine($"You've selected the following option: {userMenuSelection}");
                            Console.WriteLine("Press the Enter key to continue.");

                            Console.ReadLine();

                            // if the user input is a valid characteristic to display, continue to specifics
                            if (checkCharacteristicOptions.Contains(userCharacteristicSelection))
                            {

                                do
                                {
                                    Console.WriteLine($"Great! Please enter the {userCharacteristicSelection} you want us to look for. \nOR type exit to return to the previous menu.");

                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        userSelectedComparison = readResult.Trim().ToLower();
                                    }

                                    if (userCharacteristicSelection == "age")
                                    {

                                        do
                                        {

                                            Console.WriteLine("Please enter a valid age number (00) or we will display only cats with unkonwn ages. \nOR type exit to return to the previous menu.");

                                            if (int.TryParse(userSelectedComparison, out userSelectedAge) && userSelectedAge >= 0)
                                            {

                                                Console.WriteLine($"You've selected to display all cats with the age of {userSelectedAge}.");
                                                Console.WriteLine("Press the Enter key to continue.");


                                                Console.ReadLine();


                                                Animal.DisplayAnimalsWith("cat", userCharacteristicSelection, userSelectedComparison, storedAnimals);
                                                break;

                                            }
                                            else
                                            {
                                                // can't parse integer from user-inputted age or age is negative (unknown)

                                                Console.WriteLine($"You've selected to display all cats with an unknown age. Is this correct (y/n)?");

                                                readResult = Console.ReadLine();
                                                if (readResult != null)
                                                {
                                                    userMenuSelection = readResult.Trim().ToLower();
                                                }

                                                if (userMenuSelection == "y" || userMenuSelection == "yes")
                                                {
                                                    Animal.DisplayAnimalsWith("cat", userCharacteristicSelection, userSelectedComparison, storedAnimals);
                                                    break;

                                                    // TODO : for Testing: should i put a userMenuSelection = "exit"; here? to escape out of the next loop or couple of loops?
                                                }

                                            }
                                        } while (userMenuSelection != "exit");
                                    }else
                                    {
                                        // if characteristic is NOT age, then no need to verify input. can directly compare string to string w/o errors

                                        Animal.DisplayAnimalsWith("cat", userCharacteristicSelection, userMenuSelection, storedAnimals);
                                        break;

                                    }

                                } while (userMenuSelection != "exit");



                            }else
                            {
                                Console.WriteLine("You didn't seem to select a valid characteristic. Please try again.");
                                Console.WriteLine("Press the Enter key to continue.");

                                Console.ReadLine();
                            }


                        } while (userMenuSelection != "exit");

                        break;

                    case "8":
                    case "eight":
                        Console.WriteLine($"this feature ({userMenuSelection}) is under construction. check back soon.");
                        // call DisplayAnimalsWith("dog", user input string type, user input string data, storedAnimals);

                        do
                        {

                            Console.WriteLine("You've selected to display all dogs with a specific characteristic.");
                            Console.WriteLine("Please enter one of the following characterstics to search for: 'age', 'physical description', 'personality', or 'nickname' \nOR type exit to return to the main menu.");

                            string[] checkCharacteristicOptions = new string[] { "age", "physical description", "personality", "nickname" };
                            string userCharacteristicSelection = "";




                            readResult = Console.ReadLine();
                            if (readResult != null)
                            {
                                userCharacteristicSelection = readResult.Trim().ToLower();
                            }

                            Console.WriteLine($"You've selected the following option: {userMenuSelection}");
                            Console.WriteLine("Press the Enter key to continue.");

                            Console.ReadLine();

                            // if the user input is a valid characteristic to display, continue to specifics
                            if (checkCharacteristicOptions.Contains(userCharacteristicSelection))
                            {

                                do
                                {
                                    Console.WriteLine($"Great! Please enter the {userCharacteristicSelection} you want us to look for. \nOR type exit to return to the previous menu.");

                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        userSelectedComparison = readResult.Trim().ToLower();
                                    }

                                    if (userCharacteristicSelection == "age")
                                    {

                                        do
                                        {

                                            Console.WriteLine("Please enter a valid age number (00) or we will display only dogs with unkonwn ages. \nOR type exit to return to the previous menu.");

                                            if (int.TryParse(userSelectedComparison, out userSelectedAge) && userSelectedAge >= 0)
                                            {

                                                Console.WriteLine($"You've selected to display all dogs with the age of {userSelectedAge}.");
                                                Console.WriteLine("Press the Enter key to continue.");


                                                Console.ReadLine();


                                                Animal.DisplayAnimalsWith("dog", userCharacteristicSelection, userSelectedComparison, storedAnimals);
                                                break;

                                            }
                                            else
                                            {
                                                // can't parse integer from user-inputted age or age is negative (unknown)

                                                Console.WriteLine($"You've selected to display all dogs with an unknown age. Is this correct (y/n)?");

                                                readResult = Console.ReadLine();
                                                if (readResult != null)
                                                {
                                                    userMenuSelection = readResult.Trim().ToLower();
                                                }

                                                if (userMenuSelection == "y" || userMenuSelection == "yes")
                                                {
                                                    Animal.DisplayAnimalsWith("dog", userCharacteristicSelection, userSelectedComparison, storedAnimals);
                                                    break;

                                                    // TODO : for Testing: should i put a userMenuSelection = "exit"; here? to escape out of the next loop or couple of loops?
                                                }

                                            }
                                        } while (userMenuSelection != "exit");
                                    }
                                    else
                                    {
                                        // if characteristic is NOT age, then no need to verify input. can directly compare string to string w/o errors

                                        Animal.DisplayAnimalsWith("dog", userCharacteristicSelection, userMenuSelection, storedAnimals);
                                        break;

                                    }

                                } while (userMenuSelection != "exit");



                            }
                            else
                            {
                                Console.WriteLine("You didn't seem to select a valid characteristic. Please try again.");
                                Console.WriteLine("Press the Enter key to continue.");

                                Console.ReadLine();
                            }


                        } while (userMenuSelection != "exit");

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
                    Console.WriteLine("Nickname: " + pet.Nickname);
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
                    Console.WriteLine($"\nThanks for telling us that the new animal will be a {userAnimalInput}. Knowing helps us prepare for them.");

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
