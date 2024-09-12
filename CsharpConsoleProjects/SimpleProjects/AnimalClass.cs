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

        private string petID;
        private string species;
        private int age;
        private string nickname;
        private string physicalDescription;
        private string personality;

        private Random random = new Random();


        private static string[][] dogPhysicalDescriptions = new string[8][];
        private string[][] catPhysicalDescriptions;

        dogPhysicalDescriptions[0]  = new string[4] {"small", "medium sized", "large", "huge"};

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
            } else if (animalSpecies == "dog")
            {
                totalDogs++;
                return "d" + totalDogs;
            } else
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
        Random random = new Random();


        if (age != -1)
        {
            if (species == "cat") { age = random.Next(1, 20); }
            else if (species == "dog") { age = random.Next(1, 18); }
        }

        // assign descriptions in a seprate method where species is provided?
        if (!String.IsNullOrEmpty(PhysicalDescription))
        {
            if (species == "cat")
            {
                for (int i = 0; i < catPhysicalDescriptions.Length; i++)
                {

                }
            }
            if (species == "dog")
            {
                for (int i = 0; i < dogPhysicalDescriptions.Length; i++) { }

            }

        }


    }


}


    

