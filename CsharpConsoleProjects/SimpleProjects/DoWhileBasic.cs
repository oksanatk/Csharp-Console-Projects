using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class M3L5
    {
        public static void HeroMonsterBattle()
        {
            int heroHealth = 10;
            int monsterHealth = 10;
            Random randomPower = new Random();
            int attackPower = 0;
            Console.WriteLine($"Starting Health is.... Hero Health: {heroHealth} and Monster Health: {monsterHealth}");
            
            do
            {

                attackPower = randomPower.Next(1, 11);
                monsterHealth -= attackPower;
                Console.WriteLine($"The monster was damaged and lost {attackPower} health. The monster now has {monsterHealth} health.");

                if (monsterHealth <= 0) break;


                attackPower = randomPower.Next(1, 11);
                heroHealth -= attackPower;
                Console.WriteLine($"The hero was damaged and lost {attackPower} health. The hero now has {heroHealth} health.");
            } while (heroHealth > 0 && monsterHealth > 0);

            if (heroHealth <= 0)
            {
                //Console.WriteLine($"The final health is Hero Health:{heroHealth} and Monster Health: {monsterHealth}.");
                Console.WriteLine("The monster won.");
            }

            if (monsterHealth <= 0)
            {
                //Console.WriteLine($"The final health is Hero Healh:{heroHealth} and Monster Health:{monsterHealth}");
                Console.WriteLine("The hero won.");
            }

            if (heroHealth > 0 && monsterHealth > 0)
            {
                Console.WriteLine(".... is this thing working?");
            }

        }  
        }
    }
