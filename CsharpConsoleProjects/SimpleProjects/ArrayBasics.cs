using System;

namespace SimpleProject
{
    class M2L4
    {
        public static void ArrayIterationPractice()
        {
            string[] fakeIDs = ["B123", "C234", "A345", "C15", "B177", "G3003", "C235", "B179"];

            foreach (string id in fakeIDs)
            {
                if (id.StartsWith("B"))
                {
                    Console.WriteLine("This ID starts with a B! " +id);
                }
            }
        }
    }
}