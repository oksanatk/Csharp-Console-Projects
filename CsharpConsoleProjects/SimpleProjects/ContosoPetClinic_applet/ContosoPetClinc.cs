using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject.ContosoPetClinic_applet
{
    class MainClinic
    {
        /* Contoso Pets App
         *   - requires class AnimalClass.cs
         *   allows user to choose between menu options to modify stored Animal Objects
         */


        public static void Main(string[] args)
        {

            Animal[] storedAnimals = new Animal[8];

            string? readResult;
            string userMenuSelection = "";
            string userPetIdSelection = "";
            string userPetAgeSelection = "";
            int userSelectedAge;


            // initialize the given default data from microsoft tutorials

            storedAnimals[0] = new Animal("dog", 2, "medium sized cream colored female golden retriever weighing about 65 pounds. housebroken.", "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.", "lola");

            storedAnimals[1] = new Animal("dog", 9, "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.", "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.", "loki");

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

                if (userMenuSelection == "exit")
                {
                    Console.WriteLine("You are choosing to exit the Contoso Pet Clinic app.");
                    Console.WriteLine("Press the Enter key to continue.");

                }
                else
                {
                    Console.WriteLine($"You picked option number {userMenuSelection}.");
                    Console.WriteLine("Press the Enter key to continue");
                }


                readResult = Console.ReadLine();

                switch (userMenuSelection)
                {
                    case "1":
                    case "one":

                        ListAllPetInfo(storedAnimals);

                        break;

                    case "2":
                    case "two":

                        Console.Clear();
                        Console.WriteLine("You've selected to add a new animal to our clinc.");

                        storedAnimals = AddNewAnimal(storedAnimals);

                        break;



                    case "3":
                    case "three":

                        Console.WriteLine("You've selected for all pets to visit the vet.");

                        for (int i = 0; i < storedAnimals.Length; i++)
                        {
                            if (storedAnimals[i] != null)
                            {
                                storedAnimals[i] = storedAnimals[i].VisitTheVet();
                            }
                        }
                        Console.WriteLine("\nAll pets have visited the vet!");
                        Console.WriteLine("Their ages and physical descriptions are now complete.\n");

                        break;



                    case "4":
                    case "four":
                        Console.WriteLine("You've selected to get to know all of our animals.");

                        for (int i = 0; i < storedAnimals.Length; i++)
                        {
                            if (storedAnimals[i] != null)
                            {
                                storedAnimals[i] = storedAnimals[i].GetToKnow();
                            }

                        }

                        Console.WriteLine("\nYou've gotten to know all of our animals!");

                        break;

                    case "5":
                    case "five":

                        do
                        {
                            Console.Clear();
                            Console.WriteLine("You've selected to edit an animal's age.\n");
                            foreach (Animal a in storedAnimals)
                            {
                                if (a != null)
                                {

                                    if (String.IsNullOrEmpty(a.Nickname) || a.Nickname.ToLower() == "unknown")
                                    {
                                        // if both nickname and age are unknown
                                        if (a.Age < 0)
                                        {
                                            Console.WriteLine($"We have a {a.Species} (pet id: {a.PetID}) with an unknown age.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"We have a {a.Species} (pet id: {a.PetID}) with the age of {a.Age}.");
                                        }
                                    }
                                    else if (a.Age < 0)
                                    {
                                        // if nickname is known, but age is unknown
                                        Console.WriteLine($"{a.Nickname} is a {a.Species} (pet id: {a.PetID} with an unknown age.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"{a.Nickname} is a {a.Species} (pet id: {a.PetID}) with the age of {a.Age}.");

                                    }

                                }

                            }

                            Console.WriteLine("\nPlease enter the pet id of the pet whose age you'd like to edit.");
                            Console.WriteLine("Or type exit to return to the main menu.");

                            readResult = Console.ReadLine();
                            if (readResult != null)
                            {
                                userPetIdSelection = readResult.Trim().ToLower();
                            }

                            if (userPetIdSelection == "exit")
                            {
                                Console.WriteLine("\nYou're choosing to go back to the main menu.");

                            }
                            else
                            {
                                for (int i = 0; i < storedAnimals.Length; i++)
                                {

                                    if (storedAnimals[i] != null)
                                    {

                                        if (storedAnimals[i].PetID == userPetIdSelection)
                                        {

                                            // if matching pet id, print current age
                                            if (String.IsNullOrEmpty(storedAnimals[i].Nickname) || storedAnimals[i].Nickname.ToLower() == "unknown")
                                            {
                                                if (storedAnimals[i].Age < 0)
                                                {
                                                    Console.WriteLine($"\nYou've selected our {storedAnimals[i].Species} with the pet id: ({storedAnimals[i].PetID}). They currently have an unknown age.");
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"\nYou've selected our {storedAnimals[i].Species} with the pet id: ({storedAnimals[i].PetID}). They currently have an age of {storedAnimals[i].Age}.");
                                                }
                                            }
                                            else if (storedAnimals[i].Age < 0)
                                            {
                                                Console.WriteLine($"\nYou've selected {storedAnimals[i].Nickname} (pet id: {storedAnimals[i].PetID}). They currently have an unknown age.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"\nYou've selected {storedAnimals[i].Nickname} (pet id: {storedAnimals[i].PetID}). They currently have an age of {storedAnimals[i].Age}");
                                            }

                                            do
                                            {
                                                Console.WriteLine("\nPlease enter tne new age (00) that you'd like to set. Type -1 for an unknown age.");
                                                Console.WriteLine("Or type exit to return to the previous menu.");


                                                readResult = Console.ReadLine();
                                                if (readResult != null)
                                                {
                                                    userPetAgeSelection = readResult.Trim().ToLower();

                                                    if (userPetAgeSelection == "exit")
                                                    {
                                                        Console.WriteLine("\nYou're choosing to go back to the previous menu.");
                                                    }
                                                }

                                                if (int.TryParse(userPetAgeSelection, out userSelectedAge))
                                                {
                                                    if (userSelectedAge > 30)
                                                    {
                                                        Console.WriteLine("I'm sorry. That doesn't seem right. The oldest cat or dog we've ever heard of is only 30 years old.");
                                                        Console.WriteLine("Press the Enter key to try again.");
                                                        Console.ReadLine();

                                                    }
                                                    else
                                                    {
                                                        if (userSelectedAge < -1)
                                                        {
                                                            userSelectedAge = -1;
                                                        }
                                                        // age is a valid number
                                                        if (String.IsNullOrEmpty(storedAnimals[i].Nickname) || storedAnimals[i].Nickname.ToLower() == "unknown")
                                                        {
                                                            if (userSelectedAge == -1)
                                                            {
                                                                Console.WriteLine($"Now changing the age of our pet with the id ({storedAnimals[i].PetID}) to {userSelectedAge} (unknown age).");

                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine($"Now changing the age of our pet with the id ({storedAnimals[i].PetID}) to {userSelectedAge}.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (userSelectedAge == -1)
                                                            {
                                                                Console.WriteLine($"Now changing {storedAnimals[i].Nickname}'s (pet id: {storedAnimals[i].PetID}) age to {userSelectedAge} (unknown age).");
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine($"Now changing {storedAnimals[i].Nickname}'s (pet id: {storedAnimals[i].PetID}) age to {userSelectedAge}.");

                                                            }
                                                        }

                                                        storedAnimals[i] = storedAnimals[i].NewAge(userSelectedAge);

                                                        userPetAgeSelection = "exit";
                                                        userPetIdSelection = "exit";

                                                    }



                                                }
                                                else if (userPetAgeSelection != "exit")
                                                {
                                                    Console.WriteLine("Sorry, we couldn't recognize that age.");
                                                    Console.WriteLine("Press the Enter key to continue.");
                                                    Console.ReadLine();
                                                }




                                            } while (userPetAgeSelection != "exit");

                                            i = storedAnimals.Length + 1;

                                        }
                                    }
                                    if (i == storedAnimals.Length - 1)
                                    {
                                        // for loop over, pet id didn't match to anything
                                        Console.WriteLine("\nI'm sorry. I didn't recognize that pet id. Are you sure it's one of ours?");
                                    }
                                }
                                if (userPetIdSelection != "exit" && userPetAgeSelection != "exit")
                                {
                                    // don't run this confirmation if a new age was just set
                                    Console.WriteLine("Press Enter to continue.");
                                    Console.ReadLine();

                                }

                            }


                        } while (userPetIdSelection != "exit");


                        break;



                    case "6":
                    case "six":

                        string userSelectedPersonality = "";

                        do
                        {
                            Console.Clear();
                            Console.WriteLine("You've selected to edit an animal's personality description.");
                            Console.WriteLine("These are our pets and their personalities:\n");

                            foreach (Animal a in storedAnimals)
                            {
                                if (a != null)
                                {
                                    if (String.IsNullOrEmpty(a.Nickname) || a.Nickname.ToLower() == "unknown")
                                    {
                                        if (String.IsNullOrEmpty(a.Personality) || a.Personality.ToLower() == "unknown")
                                        {
                                            Console.WriteLine($"\nWe have a {a.Species} with no nickname and the pet id: {a.PetID}. They have an unknown personality.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\nWe have a {a.Species} with no nickname and the pet id: {a.PetID}.");
                                            Console.WriteLine($"They have the following personality: \n\r'{a.Personality}'");
                                        }
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(a.Personality) || a.Personality.ToLower() == "unknown")
                                        {
                                            Console.WriteLine($"\nWe have a {a.Species} nicknamed {a.Nickname} with the id: {a.PetID}. They have an unknown personality.");

                                        }
                                        else
                                        {
                                            Console.WriteLine($"\nWe have a {a.Species} nicknamed {a.Nickname} with the id: {a.PetID}. \n{a.Nickname} has the following personality:\n\r'{a.Personality}'");

                                        }

                                    }
                                }

                            }


                            Console.WriteLine("\nPlease enter a valid Pet ID to continue. Or type exit to return to the previous menu.");

                            readResult = Console.ReadLine();
                            if (readResult != null)
                            {
                                userPetIdSelection = readResult.Trim().ToLower();
                            }

                            for (int i = 0; i < storedAnimals.Length; i++)
                            {
                                if (storedAnimals[i] != null)
                                {
                                    if (storedAnimals[i].PetID == userPetIdSelection)
                                    {
                                        // if valid pet id, continue to ask user for personality input.
                                        if (String.IsNullOrEmpty(storedAnimals[i].Nickname) || storedAnimals[i].Nickname.ToLower() == "unknown")
                                        {
                                            Console.WriteLine($"\nOkay, cool. We'll edit the personality of the pet with id {storedAnimals[i].PetID}.");
                                            if (String.IsNullOrEmpty(storedAnimals[i].Personality) || storedAnimals[i].Personality.ToLower() == "unknown")
                                            {
                                                Console.WriteLine("They currently have an unknown personality.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Their current personality is:\n\r'{storedAnimals[i].Personality}'");

                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\nOkay, cool. We'll edit the personality of the pet ({storedAnimals[i].PetID}) nicknamed {storedAnimals[i].Nickname}.");
                                        }

                                        Console.WriteLine("\nPlease enter the new personality.");
                                        Console.WriteLine("Or type exit to return to the previous menu");

                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            userSelectedPersonality = readResult.Trim().ToLower();
                                        }

                                        if (userSelectedPersonality != "exit")
                                        {
                                            storedAnimals[i] = storedAnimals[i].EditPersonality(userSelectedPersonality);

                                            if (String.IsNullOrEmpty(storedAnimals[i].Nickname) || storedAnimals[i].Nickname.ToLower() == "unknown")
                                            {
                                                Console.WriteLine($"\nSuccess! We've changed the personality of the {storedAnimals[i].Species} with id {storedAnimals[i].PetID} to:\n'{storedAnimals[i].Personality}'");

                                            }
                                            else
                                            {
                                                Console.WriteLine($"\nSuccess! We've changed the personality of {storedAnimals[i].Nickname}, a {storedAnimals[i].Species} with the pet id {storedAnimals[i].PetID} to: \n'{storedAnimals[i].Personality}'");
                                            }

                                            userSelectedPersonality = "exit";
                                            userPetIdSelection = "exit";

                                        }

                                        i = storedAnimals.Length + 1;

                                    }
                                }
                                if (i == storedAnimals.Length - 1 && userPetIdSelection != "exit")
                                {
                                    // for loop over, no match was found.
                                    Console.WriteLine("I'm sorry, I didn't recognize that pet id. Are you sure it's one of ours?");
                                }

                            }
                            if (userPetIdSelection != "exit" && userSelectedPersonality != "exit")
                            {
                                //pause code execution before starting loop over.
                                //don't run confirmation if personality has just been set
                                Console.WriteLine();
                                Console.WriteLine("Press the Enter key to continue.");
                                Console.ReadLine();

                            }


                        } while (userPetIdSelection != "exit");

                        break;



                    case "7":
                    case "seven":

                        UserSearchForCharacteristic("cat", storedAnimals);

                        break;

                    case "8":
                    case "eight":

                        UserSearchForCharacteristic("dog", storedAnimals);

                        break;


                    case "exit":
                    case "Exit":
                        break;
                    // need case exit to avoid printing error message


                    default:
                        Console.WriteLine("I'm sorry. I didn't recognize that input. Please try again.");
                        break;
                }

                if (userMenuSelection != "exit")
                {
                    // pause code execution, then start loop over again
                    Console.WriteLine("Press the Enter key to continue.");
                    Console.ReadLine();

                }

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
                    if (pet.Age == -1)
                    {
                        Console.WriteLine("Age: Unknown");
                    }
                    else
                    {
                        Console.WriteLine("Age: " + pet.Age);
                    }
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
            for (int i = (currentAnimals.Length - 1); i >= 0; i--)
            {
                if (currentAnimals[i] == null)
                {
                    isOpen = true;
                    openSpot = i;
                }
            }

            if (isOpen)
            {
                Console.WriteLine();
                Console.WriteLine("Congrats, we have an open spot available!");
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
                Console.Clear();
                Console.WriteLine("You've selected to add a new animal to our clinic.");
                Console.WriteLine("\nCongrats, we have an open spot available!");

                Console.WriteLine("Is the new animal a cat or a dog?");
                readResult = Console.ReadLine();
                if (readResult != null) { userAnimalInput = readResult.Trim().ToLower(); }

                if (userAnimalInput == "cat" || userAnimalInput == "dog")
                {
                    animalSpecies = userAnimalInput;
                    Console.WriteLine($"\nThanks for telling us that the new animal will be a {userAnimalInput}. Knowing helps us prepare for them.");

                }
                else
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

                    currentAnimals[openSpot] = new Animal(animalSpecies, -1, "Unknown", "Unknown", "Unknown");
                    return currentAnimals;

                }
                else if (userAnimalInput == "yes" || userAnimalInput == "y")
                {
                    Console.WriteLine("That's great! We'll just ask a few more questions then...\n");
                }
                else
                {
                    Console.WriteLine("\nI'm sorry, I didn't recognize that input. Please try again.");
                }

            } while (userAnimalInput != "yes" && userAnimalInput != "y" && userAnimalInput != "no" && userAnimalInput != "n");


            // default assign empty strings to be unknown characteristics
            string animalNickname = "";
            string animalPhysicalDescription = "";
            string animalPersonality = "";
            int animalAge = -1;

            string[] askingOptions = new string[] { "age", "physical description", "personality", "nickname" };


            // for loop - accept user input and save accordingly for each detail of animal class
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
                        Console.WriteLine("\nI'm sorry, I didn't recognize that input.\nPlease press Enter to try again.\n");
                        Console.ReadLine();
                    }
                } while (userAnimalInput != "yes" && userAnimalInput != "y" && userAnimalInput != "n" && userAnimalInput != "no");



                // if they do know the characteristic, ask to input it 
                if (userAnimalInput == "yes" || userAnimalInput == "y")
                {
                    Console.WriteLine($"That's great! Please type in this {animalSpecies}'s {askingOptions[i]}.");
                    readResult = Console.ReadLine();


                    // if the input is the nickname, keep input case sensitive
                    if (askingOptions[i] == "nickname")
                    {
                        if (readResult != null)
                        {
                            userAnimalInput = readResult.Trim();
                        }

                    }
                    else
                    {
                        if (readResult != null)
                        {
                            userAnimalInput = readResult.Trim().ToLower();
                        }

                    }

                    switch (i)
                    {

                        case 0:
                            if (int.TryParse(userAnimalInput, out animalAge))
                            {

                                while (animalAge > 30 || animalAge < 0)
                                {

                                    Console.WriteLine("\nI'm sorry, that didn't look right. The oldest cat or dog we've ever heard of was only 30 years old.");
                                    Console.WriteLine($"Please type in this {animalSpecies}'s age as a number (00) between 0 and 30");

                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        userAnimalInput = readResult.Trim().ToLower();
                                    }

                                    int.TryParse(userAnimalInput, out animalAge);

                                }

                            }
                            else
                            {
                                while (!int.TryParse(userAnimalInput, out animalAge))
                                {
                                    Console.WriteLine("\nI'm sorry. That input didn't look like a number. We need a number to represent their age.\n");
                                    Console.WriteLine("Please try again.");
                                    Console.WriteLine($"Please type in this {animalSpecies}'s age as a number (0):");

                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        userAnimalInput = readResult.Trim().ToLower();
                                    }

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
                            // nickname should remain case sensitive
                            animalNickname = userAnimalInput;
                            break;

                    }

                }
                else if (userAnimalInput == "n" || userAnimalInput == "no")
                {
                    Console.WriteLine("Got it! Thanks for letting me know.\n");
                }
            }

            //initialize in the index spot that was marked as open
            currentAnimals[openSpot] = new Animal(animalSpecies, animalAge, animalPhysicalDescription, animalPersonality, animalNickname);

            return currentAnimals;
        }


        private static void UserSearchForCharacteristic(string species, Animal[] storedAnimals)
        {
            string userCharacteristicSelection = "";
            string userSelectedComparison = "";
            string? readResult;
            int userSelectedAge;


            do
            {
                Console.Clear();
                Console.WriteLine("You've selected to display all {species}s with a specific characteristic.");
                Console.WriteLine("Please enter one of the following characterstics to search for: \n - age \n - physical description \n - personality \n - nickname \n\nOR type exit to return to the main menu.");

                string[] checkCharacteristicOptions = new string[] { "age", "physical description", "personality", "nickname" };


                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    userCharacteristicSelection = readResult.Trim().ToLower();
                }

                if (userCharacteristicSelection != "exit")
                {
                    Console.WriteLine($"\nYou've selected the following option: {userCharacteristicSelection}");
                    Console.WriteLine("Press the Enter key to continue.");

                    Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("\nYou've selected to go back to the main menu.");
                }


                // if the user input is a valid characteristic to display, continue to specifics
                if (userCharacteristicSelection != "exit")
                {
                    if (checkCharacteristicOptions.Contains(userCharacteristicSelection))
                    {

                        do
                        {
                            Console.WriteLine($"Great! Please enter the {userCharacteristicSelection} you want us to look for.");

                            if (userCharacteristicSelection == "age")
                            {
                                Console.WriteLine("Please enter a valid age number (00) or we will display only {species}s with unknown ages.");
                            }

                            Console.WriteLine("\nOR type Exit to return to the previous menu.");

                            readResult = Console.ReadLine();
                            if (readResult != null)
                            {
                                // don't accept empty inputs, convert to unknown 
                                if (String.IsNullOrEmpty(readResult))
                                {
                                    if (userCharacteristicSelection == "age")
                                    {
                                        userSelectedComparison = "-1";
                                    }
                                    else
                                    {
                                        userSelectedComparison = "unknown";
                                    }
                                }
                                else
                                {
                                    userSelectedComparison = readResult.Trim().ToLower();
                                }

                            }


                            if (userCharacteristicSelection == "age")
                            {

                                do
                                {

                                    if (int.TryParse(userSelectedComparison, out userSelectedAge) && userSelectedAge >= 0)
                                    {

                                        Console.WriteLine($"You've selected to display all {species}s with the age of {userSelectedAge}.");
                                        Console.WriteLine("Press the Enter key to continue.");


                                        Console.ReadLine();


                                        Animal.DisplayAnimalsWith(species, userCharacteristicSelection, userSelectedComparison, storedAnimals);

                                        userSelectedComparison = "exit";
                                        userCharacteristicSelection = "exit";

                                    }
                                    else if (int.TryParse(userSelectedComparison, out userSelectedAge) && userSelectedAge < 0)
                                    {
                                        // age is negative (unknown)

                                        Console.WriteLine($"You've selected to display all {species}s with an unknown age. Is this correct (y/n)?");

                                        string userYesOrNo = "";

                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            userYesOrNo = readResult.Trim().ToLower();
                                        }

                                        if (userYesOrNo == "y" || userYesOrNo == "yes")
                                        {
                                            Animal.DisplayAnimalsWith(species, userCharacteristicSelection, userSelectedComparison, storedAnimals);
                                            userSelectedComparison = "exit";
                                            userCharacteristicSelection = "exit";

                                        }
                                        else
                                        {
                                            Console.WriteLine($"It seems like you didn't want to display {species}s with unknown ages. Please try again.\n");
                                            break;
                                        }

                                    }
                                    else if (userSelectedComparison == "exit")
                                    {
                                        Console.WriteLine("\nYou've selected to go back to the previous screen.");
                                        Console.WriteLine("Press the Enter key to contine.");
                                        Console.ReadLine();

                                    }
                                    else
                                    {
                                        Console.WriteLine($"\nI'm sorry, but I didn't recognize that input. Please try again.");
                                        Console.WriteLine("Press the Enter key to continue.");
                                        Console.ReadLine();
                                        break; //only exits to the option to re-enter comparison age

                                    }
                                } while (userSelectedComparison != "exit");

                            }
                            else
                            {
                                // if characteristic is NOT age, then no need to verify input. can directly compare string to string w/o errors
                                if (userSelectedComparison != "exit")
                                {
                                    Animal.DisplayAnimalsWith(species, userCharacteristicSelection, userSelectedComparison, storedAnimals);
                                    userSelectedComparison = "exit";
                                    userCharacteristicSelection = "exit";

                                }
                                else
                                {
                                    Console.WriteLine("\nYou've selected to go back to the previous screen.");
                                    Console.WriteLine("Press the Enter key to continue.");
                                    Console.ReadLine();
                                }

                            }

                        } while (userSelectedComparison != "exit");



                    }
                    else if (userCharacteristicSelection != "exit")
                    {
                        Console.WriteLine("You didn't seem to select a valid characteristic. Please try again.");
                        Console.WriteLine("Press the Enter key to continue.");

                        Console.ReadLine();
                    }
                }


            } while (userCharacteristicSelection != "exit");


        }



    }
}
