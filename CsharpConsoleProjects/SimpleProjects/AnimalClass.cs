using System.Drawing;

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
                "mix of breeds.", ""
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
                "with white fur on feet (like socks)", "tabby", "hairless", "short-haired", "long-haired"
            },
            new string[]
            {
                "male", "female"
            },
            new string[]
            {
                "siamese", "persian", "american", "british", "devon-rex", "munchkin"
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


            if (this.Age == (-1))
            {
                // age -1 means age is unknown. assign below
                if (this.Species == "cat") { this.Age = random.Next(0, 21);}

                else if (this.Species == "dog") { this.Age = random.Next(0, 18); }

            }

            // assign descriptions in a seprate method where species is provided?
            if (String.IsNullOrEmpty(this.PhysicalDescription) || this.PhysicalDescription.ToLower() == "unknown")
            {
                
                if (this.Species == "cat")
                {

                    for (int i = 0; i < catPhysicalDescriptions.Length; i++)
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
                            randomCharacteristic = (int)(sizeVariance * 6 * (random.NextDouble() + 1));
                            characteristics += $"weighing about {randomCharacteristic} pounds." + " ";

                        }
                    }
                }
                        

                if (this.Species == "dog"){
                    
                    for (int i = 0; i < dogPhysicalDescriptions.Length; i++) 
                    {
                        
                        randomCharacteristic = random.Next(0, dogPhysicalDescriptions[i].Length);
                        characteristics += dogPhysicalDescriptions[i][randomCharacteristic] + " ";

                        if (i ==0)
                        {
                            sizeVariance = randomCharacteristic + 1;
                        }
                        if (i == 4)
                        {
                            randomCharacteristic = (int)(sizeVariance * 20 * (random.NextDouble() + 1.5));
                            characteristics += $"weighing about {randomCharacteristic} pounds." + " ";

                        }
                    }
                }

                    Console.WriteLine($"VisitTheVet :: Line258 : var characteristics should become the new physcial description: \n\t'{characteristics}");
                    // assign new physical characteristics to parameter
                    this.PhysicalDescription = characteristics;
                
            }else
            {
                // if some data already exists, then only add sterilization, litter box training, & housebroken data

                if (this.Species == "cat")
                {
                    this.PhysicalDescription = this.PhysicalDescription.Trim();
                    if (!this.PhysicalDescription.EndsWith("."))
                     {
                        characteristics = ". ";
                     } else
                    {
                        characteristics = " ";
                    }

                    if (!this.PhysicalDescription.Contains("sterilized"))
                    {

                        randomCharacteristic = random.Next(0, 2);
                        characteristics += catPhysicalDescriptions[5][randomCharacteristic] + " ";
                    }

                    if (!this.PhysicalDescription.Contains("litter box trained"))
                    {
                        randomCharacteristic = random.Next(0, 2);
                        characteristics += catPhysicalDescriptions[6][randomCharacteristic] + " ";
                    }

                    if (!this.PhysicalDescription.Contains("housebroken"))
                    {
                        randomCharacteristic = random.Next(0, 2);
                        characteristics += catPhysicalDescriptions[7][randomCharacteristic];
                    }


                    this.PhysicalDescription += characteristics; 
                }

                if (this.Species == "dog")
                {
                    // if species is dog and some physical description data already exists, only add sterilized and housebroken data
                    if (!this.PhysicalDescription.EndsWith("."))
                    {
                        characteristics = ". ";
                    } else
                    {
                        characteristics = " ";
                    }

                    if (!this.PhysicalDescription.Contains("sterilized"))
                    {
                        randomCharacteristic = random.Next(0, 2);
                        characteristics += dogPhysicalDescriptions[6][randomCharacteristic] + " ";
                    }

                    if (!this.PhysicalDescription.Contains("housebroken"))
                    {
                        randomCharacteristic = random.Next(0, 2);
                        characteristics += dogPhysicalDescriptions[7][randomCharacteristic];
                    }

                    this.PhysicalDescription += characteristics;

                }
            }

            if (!String.IsNullOrEmpty(this.Nickname) && this.Nickname.ToLower() != "unknown")
            {
                Console.WriteLine($"{this.Nickname} (pet id: {this.PetID}) has visited the vet!");
            }else
            {
                Console.WriteLine($"The pet with the id ({this.PetID}) has visited the vet!");
            }

            return this;


        }



        public Animal GetToKnow()
        {
            // check if nickname and/or personality already exists
            if (String.IsNullOrEmpty(this.Nickname) || String.IsNullOrEmpty(this.Personality) || this.Personality.ToLower() == "unknown" || this.Nickname.ToLower() == "unknown") {

                // personality is empty
                if (String.IsNullOrEmpty(this.Nickname) || this.Nickname.ToLower() == "unknown")
                {
                    randomCharacteristic = random.Next(0, nicknameOptions.Length);
                    this.Nickname = nicknameOptions[randomCharacteristic];
                }

                if (String.IsNullOrEmpty(this.Personality) || this.Personality.ToLower() == "unknown")
                {
                    randomCharacteristic = random.Next(0,personalityTypes.Length);
                    this.Personality = personalityTypes[randomCharacteristic];  

                }

                Console.WriteLine($"You've gotten to know that this pet's (pet id: {this.PetID}) nickname is {this.Nickname} and their personality is: {this.Personality}.");
             

                return this;
            }
            else
            {
                Console.WriteLine($"The nickname and personality for {this.Nickname} (pet id: {this.PetID})  are already complete.");
                return this;
            }

        }




        public Animal NewAge(int newAge)
        {
            this.Age = newAge;
            return this;
        }


        public Animal EditPersonality(string newPersonality)
        { 
            this.Personality = newPersonality;
            return this;
        }

        public static void DisplayAnimalsWith(string species, string type, string data, Animal[] animalsGiven) {
            // NOTE: input specicies not currently validated. Make sure it's validated when calling the method

            int ifAge;   // if type is age, string data will parse to this int

            data = data.Trim().ToLower();

            int.TryParse(type, out ifAge);

            switch (type)
            {
                case "age":
                    if (int.TryParse(data, out ifAge) && ifAge >= 0)
                    { // if input type was age, and data is a parsable number, then check if each iteration of cat has a matching age
                        Console.WriteLine($"We'll be showing {species}s with the age of {ifAge}.");
                        break;

                    }
                    else  { // tryparse failed (input data not an int, or age data was less than 0)
                        Console.WriteLine($"The given age is unknown. We'll be showing {species}s with unknown ages.");
                    }
                    break;

                case "physical description":
                    Console.WriteLine($"We're going to be showing {species}s who have a physical description of '{data}'.");
                    break;

                case "personality description":
                    Console.WriteLine($"We're going to be showing {species}s who have '{data}' in their personality description.");
                    break;

                case "nickname":
                    Console.WriteLine($"We're going to be showing {species}s who have the nickname '{data}'.");
                    break;


            }

            for (int i = 0; i < animalsGiven.Length; i++)
            {
                switch (type)
                {
                    case "age":
                        // TODO -- >> test the age portion of this switch statement

                        // at this point if ifAge is a number or defined, then they should have entered a valid number
                        // if/else statement can 

                        if (animalsGiven[i].Species == species)
                        {
                            if (int.TryParse(data, out ifAge) && ifAge >= 0)
                            {
                                Console.WriteLine($"{animalsGiven[i].Nickname} (pet id: {animalsGiven[i].PetID}) is also a {species} with the age of {animalsGiven[i].Age}, just like the age we are checking for, {ifAge}!");
                            } else 
                            {
                                Console.WriteLine($"{animalsGiven[i].Nickname} (pet id: {animalsGiven[i].PetID}) has an unknown age");
                            }
                        }
                        break;


                    case "physical description":
                        if (animalsGiven[i].Species == species)
                        {
                            if (animalsGiven[i].PhysicalDescription.Contains(data)){
                                Console.WriteLine($"{animalsGiven[i].Nickname} (pet id {animalsGiven[i].PetID}) contains the physical description {data}!");
                                Console.WriteLine($"This is the full physical description of {animalsGiven[i].Nickname}: ");
                                Console.WriteLine(animalsGiven[i].PhysicalDescription);
                            }
                        }
                        break;


                    case "personality description":
                        if (animalsGiven[i].Species == species)
                        {
                            if (animalsGiven[i].Personality.Contains(data))
                            {
                                Console.WriteLine($"{animalsGiven[i].Nickname} (pet id {animalsGiven[i].PetID}) has {data} in their personality description!");
                                Console.WriteLine($"This is the full personality description of {animalsGiven[i].Nickname}: ");
                                Console.WriteLine(animalsGiven[i].Personality);
                            }
                        }
                        break;


                    case "nickname":
                        if (animalsGiven[i].Species == species)
                        {
                            if (animalsGiven[i].Nickname == data)
                            {
                                Console.WriteLine($"Animal with ID #: {animalsGiven[i].PetID} has the nickname '{data}'!");
                            }
                        }
                        break;
                }

            }

            Console.WriteLine($"That's all of the {species}s we found with the characteristic '{data}'. \nCome back soon!");
        }

    }
}


    

