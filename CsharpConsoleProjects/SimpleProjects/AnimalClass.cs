using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject
{
    internal class Animal
    {
        public string ID { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public string Nickname { get; set; }
        public string PhysicalDescription { get; set; }
        public string Personality { get; set; }


        public Animal()
        {
            ID = string.Empty;
            Species = string.Empty;
            Age = -1;
            Nickname = string.Empty;
            PhysicalDescription = string.Empty;
            Personality = string.Empty;
        }

        public Animal(string id, string species, string nickname)
        {
            ID = id;
            Species = species;
            Nickname = nickname;
            PhysicalDescription = "Unknown";
            Personality ="Unknown";
            Age = -1;
        }







    }
}
