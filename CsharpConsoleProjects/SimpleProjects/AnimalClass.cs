using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject
{
    internal class Animal
    {
        static int totalCats = 0;
        static int totalDogs = 0;
        static int totalUnknownAnimals = 0;

        string petID;
        string species;
        int age;
        string nickname;
        string physicalDescription;
        string personality;

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
            }else if (animalSpecies == "dog")
            {
                totalDogs++;
                return "d" + totalDogs;
            }else
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
            PhysicalDescription= physicalDescription;
            Age = age;
        }







    }
}
