using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseOfDeceit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            playGame();


        }

        static void playGame()
        {
            string[] items;
            int[] position = { 5, 3 };
            string[] flags;
            bool running = true;
            intro();
            string player = choosePlayer();
            items = perk(player);
            string[,] map = {
            {"0","0" ,"0","morgue","0","0","0"},
            {"0","0","therapy","lobby","0","room1","0"},
            {"0","0","0","hallway2","staircase","secondf","room2"},
            {"0","0","0","hallway1","0","room3","0"},
            {"0","staircase","leftroom","asylum","rightroom","0","0"},
            {"0","secret","basement","starting","tree","0","0"},
            {"0","0","0","tree","0","0","0"},
            };

            while (running)
            {
                prompt(position, map, items);
            }

        }


        static string[] perk(string player)
        {
            string[] item = new string[1];
            switch (player)
            {
                case "sheriff": item[0] = "flashlight"; break;
                case "nurse": item[0] = "keycard"; break;
                case "journalsit": item[0] = "prehint"; break;
                case "detective": item[0] = "afterhint"; break;
            }
            return item;
        }

        static void prompt(int[] position, string[,] map, string[] items)
        {

        }

        static void intro() {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("|         WELCOME TO          |");
            Console.WriteLine("|       PULSE OF DECEIT!      |");
            Console.WriteLine("-------------------------------");
        }

        static string choosePlayer(){
            string role;
            Console.WriteLine("WHO WOULD YOU LIKE TO PLAY AS?");
            Console.WriteLine("");
            Console.WriteLine(@"[1] Marshall Batumbakal (Sheriff)
Perks: Flashlight
Character Description: Marshall Batumbakal is the local sheriff of their town, known for his strong sense of justice.

[2] Anastasia Propaganda Maria Dela Cruz (Nurse)
Perks: Keycard
Character Description: Anastasia Propaganda Maria Dela Cruz is a former nurse in the asylum, concealing a dark secret from her past that haunted her, driving her actions and decisions.

[3] Kim Magtanggol - (Journalist)
Perks: Hint ( at the start )
Character Description: Kim Magtanggol, is a fearless journalist, who wants to know the truth behind the chilling murders that surrounded the asylum. 

[4] Rey P. Nyoco (Detective)
Perks: Hint ( along the way )
Character Description: Rey P. Nyoco, a seasoned detective, was called to investigate the series of perplexing murders haunting the asylum. 
");
            Console.Write("ACTION: ");
            role = Console.ReadLine();

            switch (role)
            {
                case "1": role = "sheriff"; break;
                case "2": role = "nurse"; break;
                case "3": role = "journalist"; break;
                case "4": role = "detective"; break;
            }

            return role;
            }
    }
}
