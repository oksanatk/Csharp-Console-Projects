﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject
{
    internal class Animal
    {
        static int totalCats = 0;
        static int totalDogs = 0;
        static int totalUnknownAnimals = 0;

        private string petID;
        private string species;
        private int age;
        private string nickname;
        private string physicalDescription;
        private string personality;

        // characteristics chosen at random from vars below
        private static Random random = new Random();
        int randomCharacteristic;
        string characteristics = "";


        private string[][] dogPhysicalDescriptions = {
            new string[]
            {
                "small", "medium","large","huge"
            },
            new string[]
            {
                "white", "reddish-brown", "brindle", "cream-colored", "black", "tan", "black and tan", "black and white"
            },
            new string[]
            {
                "with spots", "long-haired", "short-haired","curly-tailed", "hypoallergenic", "fluffy", "curly-furred"
            },
            new string[]
            {
                "male", "female"
            },
            new string[]
            {
                "chihuahua", "beagle", "border collie", "corgie", "husky", "poodle", "golden retreiever", "terrier"
            },
            new string[]
            {
                "mix", ""
            },
            new string[]
            {
                "sterilized.", "not sterilized."
            },
            new string[]
            {
                "housebroken.", "not housebroken"
            }

        };


        private string[][] catPhysicalDescriptions = {
            new string[]
            {
                "small", "medium sized", "large"
            },
            new string[]
            {
                "white", "reddish-brown", "ginger", "brindle", "cream colored", "black", "grey", "black and grey", "black and white"
            },
            new string[]
            {
                "white fur on (socked) feet", "tabby", "hairless", "short-haired", "long-haired"
            },
            new string[]
            {
                "siamese", "persian", "american", "british", "devon-rex", "munchkin"
            },
            new string[]
            {
                "male", "female"
            },
            new string[]
            {
                "sterilized.", "not sterilized."
            },
            new string[]
            {
                "litter box trained.", "not litter box trained."
            },
            new string[]
            {
                "housebroken.", "not housebroken."
            }


        };


        private string[] personalityTypes = new string[]
        {
            "loyal", "confident", "affectionate", "playful", "friendly", "anxious", "unconfident and submissive", "independent", "friendly", "nurturing", "the peacekeeper", "guarded / the guardian of the family"
        };

        private string[] nicknameOptions = new string[]
        {
            "Abby" , "Ace", "Adele", "Ajax", "Angel", "Annie", "Apollo","Archie", "Baer", "Baguette", "Bailey", "Balto", "Banjo", "Baxter", "Baya",
            "Beau", "Cannoli", "Capri", "Carey", "Casper", "Charlie", "Chaser", "Coco", "Dakota", "Darcy", "Demi", "Diego", "Domino", "Dobby", "Dublin",
            "Echo", "Ellie", "Ember", "Ember", "Espresso", "Fawkes", "Fawkes", "Fitz", "Fleur","Frankie", "Fudge", "Fuji", "Ginger", "Gizmo", "Goofy", "Gracie","Gunner",
            "Harley", "Harper", "Hazel", "Hershey", "Hilo", "Hiroko", "Hunter", "Indy", "Inu", "Izzy", "Jabba","Jett", "Jax", "Jinx", "Jupiter","Kahana", "Kahlo", "Kai", "Kuchen",
            "Kyoto", "Lassie", "Lavender","Leo", "Levi", "Lexi", "Lilo" , "Loki", "Lucky", "Lupo", "Maddie","Malia", "Maru", "Marley", "Milo", "Mitsuko", "Mochi", "Monet", "Monet", "Montana", "Mushu",
            "Nacho", "Nala", "Nemo", "Nero", "Nova", "Oakley", "Obi", "Odie", "Ollie", "Oreo", "Otto", "Paco", "Paris", "Peanut", "Pepper", "Picasso", "Pickles",
            "Piper", "Pluto", "Polenta", "Posie", "Quincey", "Quinn", "Rambo", "Ranger", "Raven", "Reese", "Remy", "Rhett", "Riley", "Rocco", "Rogue", "Roo", "Ruby",
            "Sake", "Sawyer", "Scarlett", "Scout", "Sequoia", "Shadow", "Shanti","Sirius", "Sparky", "Stella", "Sushi", "Tahoe", "Taylor", "Tippi", "Toto", "Trapper", "Tupelo",
            "Weasley", "Whiskey", "Winn-Dixie", "Winnie", "Wiggum", "Xena", "Xavier", "Zelda", "Ziggy", "Yara", "Yoda", "Zeus"

        };



        public string PetID { get; private set; }
        public string Species { get; private set; }
        public int Age { get; private set; }
        public string Nickname { get; set; }
        public string PhysicalDescription { get; set; }
        public string Personality { get; set; }

        public string GeneratePetID(string animalSpecies)
        {
            if (animalSpecies == "cat")
            {
                totalCats++;
                return "c" + totalCats;
            }
            else if (animalSpecies == "dog")
            {
                totalDogs++;
                return "d" + totalDogs;
            }
            else
            {
                totalUnknownAnimals++;
                return "u" + totalUnknownAnimals;
            }
        }


        // species is required to be entered when adding to clinic
        public Animal(string animalSpecies)
        //TODO: trusted input for species here, need to validate when accepting input
        {

            Species = animalSpecies;
            PetID = GeneratePetID(animalSpecies);

            Age = -1;
            Nickname = string.Empty;
            PhysicalDescription = string.Empty;
            Personality = string.Empty;
        }

        public Animal(string animalSpecies, string nickname, string personalityDescription)
        {
            PetID = GeneratePetID(animalSpecies);
            Species = animalSpecies;
            Nickname = nickname;
            PhysicalDescription = "Unknown";
            Personality = personalityDescription;
            Age = -1;
        }

        public Animal(string animalSpecies, int age, string physicalDescription)
        {
            //TODO: need to verify positive age upon accepting the input
            PetID = GeneratePetID(animalSpecies);
            Species = animalSpecies;
            Nickname = "Unknown";
            Personality = "Unknown";
            PhysicalDescription = physicalDescription;
            Age = age;
        }
        public Animal(string animalSpecies, int age, string physicalDescription, string personalityDescription, string nickname)
        {
            PetID = GeneratePetID(animalSpecies);
            Species = animalSpecies;
            if (string.IsNullOrEmpty(nickname)) { Nickname = "Unknown"; }
            else { Nickname = nickname; }

            if (string.IsNullOrEmpty(physicalDescription)) { PhysicalDescription = "Unknown"; }
            else { PhysicalDescription = physicalDescription; }

            if (string.IsNullOrEmpty(personalityDescription)) { Personality = "Unknown"; }
            else { Personality = personalityDescription; }

            Age = age;
        }

        public Animal VisitTheVet()
        {
            // check if age and physical descriptions already exist

            int sizeVariance = 0;

            if (age != -1)
            {
                if (species == "cat") { this.age = random.Next(0, 21); }
                else if (species == "dog") { this.age = random.Next(0, 18); }
            }

            // assign descriptions in a seprate method where species is provided?
            if (!String.IsNullOrEmpty(PhysicalDescription) || this.PhysicalDescription.ToLower() == "unknown")
            {
                if (species == "cat")
                {
                    for (int i = 0; i < catPhysicalDescriptions.Length; i++)
                    {
                        if (species == "cat")
                        {
                            randomCharacteristic = random.Next(0, catPhysicalDescriptions[i].Length);
                            characteristics += catPhysicalDescriptions[i][randomCharacteristic] + " ";

                            
                            // small / med / large used to determine number weight later
                            if (i == 0)
                            {
                                sizeVariance = randomCharacteristic + 1;
                            }

                            // append string 'weighing about xx pounds.' before sterilization
                            if (i == 4)
                            {
                                randomCharacteristic = (int)(sizeVariance * 6 * (random.NextDouble() * 1.5));
                                characteristics += $"weighing about {randomCharacteristic} pounds." + " ";

                            }
                        }

                        if (species == "dog"){
                            randomCharacteristic = random.Next(0, dogPhysicalDescriptions.Length);
                            characteristics += dogPhysicalDescriptions[i][randomCharacteristic] + " ";

                            if (i ==0)
                            {
                                sizeVariance = randomCharacteristic + 1;
                            }
                            if (i == 4)
                            {
                                randomCharacteristic = (int)(sizeVariance * 20 * (random.NextDouble() * 1.5));
                                characteristics += $"weighing about {randomCharacteristic} pounds." + " ";

                            }
                        }
                    }
                    // assign new physical characteristics to parameter
                    this.PhysicalDescription = characteristics;
                }
            }else
            {
                // if some data already exists, then only add sterilization, litter box training, & housebroken data
                if (species == "cat")
                {
                    characteristics = ". ";
                    randomCharacteristic = random.Next(0, 2);
                    characteristics += catPhysicalDescriptions[5][randomCharacteristic];

                    randomCharacteristic = random.Next(0, 2);
                    characteristics += catPhysicalDescriptions[6][randomCharacteristic];

                    randomCharacteristic = random.Next(0, 2);
                    characteristics += catPhysicalDescriptions[7][randomCharacteristic];

                    this.PhysicalDescription += characteristics; 
                }

                if (species == "dog")
                {
                    characteristics = ". ";

                    randomCharacteristic = random.Next(0, 2);
                    characteristics += catPhysicalDescriptions[6][randomCharacteristic];

                    randomCharacteristic = random.Next(0, 2);
                    characteristics += catPhysicalDescriptions[7][randomCharacteristic];

                    this.PhysicalDescription += characteristics;

                }
            }

            return this;


        }



        public Animal GetToKnow()
        {
            // check if nickname and/or personality already exists
            if (!String.IsNullOrEmpty(this.nickname) || (!String.IsNullOrEmpty(this.personality))) {

                // personality is empty
                if (!String.IsNullOrEmpty(this.nickname))
                {
                    randomCharacteristic = random.Next(0, nicknameOptions.Length);
                    nickname = nicknameOptions[randomCharacteristic];
                }

                if (!String.IsNullOrEmpty(this.personality))
                {
                    randomCharacteristic = random.Next(0,personalityTypes.Length);
                    personality = personalityTypes[randomCharacteristic];  

                }

                else
                {
                    Console.WriteLine("The nickname and personality are already complete.");
                }

                return this;
            }
            else
            {
                Console.WriteLine("The NickName and personality are already complete.");
                return this;
            }

            return this;
        }




        public Animal NewAge(int newAge)
        {
            this.age = newAge;
            return this;
        }


        public Animal EditPersonality(string newPersonality)
        { 
            this.Personality = newPersonality;
            return this;
        }

        public void DisplayCatsWith(string type, string? data, string[] catsGiven)
        {

            int ifAge;
            string newData;

            data = data.Trim().ToLower();


            for (int i = 0; i < catsGiven.Length; i++)
            {
                switch (type)
                {
                    case "age":
                        if (int.TryParse(data))
                        {
                            ifAge = data;
                            break;
                        }
                        break;

                    case "physical description":
                        if catsGiven[i].species == "cat"{

                        }
                }
            }
        }


        public void DisplayDogsWith(string type, string data, string[] dogsGiven)
        {

        }

    }
}


    

