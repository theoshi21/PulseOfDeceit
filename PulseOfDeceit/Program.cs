using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
            int[] position = { 6, 3 };
            int[] prevPos;
            string[] flags = { };
            bool running = true;
            intro();
            string player = choosePlayer();
            items = perk(player);
            string[,] map = {
            {"0","0","0","0","0","0","0"},
            {"0","0" ,"0","morgue","0","0","0"},
            {"0","0","therapy","lobby","0","room1","0"},
            {"0","0","0","hallway2","staircase2f","secondf","room2"},
            {"0","0","0","hallway1","0","room3","0"},
            {"0","staircase","leftroom","asylum","rightroom","0","0"},
            {"0","secret","basement","starting","righttree","0","0"},
            {"0","0","0","backtree","0","0","0"},
            {"0","0","0","0","0","0","0"},
            };

            Console.Clear();
            intro();
            usedChar(player, items);
            while (running)
            {
                prompt(position, map, items, flags);
                string ans = commands(map, position, items, flags);
                if(ans.StartsWith("move")) position = move(position, ans, map);

            }

        }

        static string commands(string[,] map, int[] position, string[] items, string[] flags)
        {
            while (true)
            {
                string input = action();
                if (input.StartsWith("move") || input.StartsWith("check") || input == "CS 103") return input; // if starts with go, just skip because the position changer is in the play game function
                else if (input == "loc")
                {
                    Console.WriteLine($"You're currently at ({map[position[0], position[1]]}).");
                }
                else if (input == "items")
                {
                    Console.Write("Your items are: ");
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (i == items.Length - 1) Console.WriteLine(items[i]);
                        else Console.WriteLine(items[i] + ",");
                        break;
                    }
                }
                else if (input.StartsWith("check")) return input;
                else if (input == "commands" || input == "help")
                {
                    Console.WriteLine("The available command are: [1] move [forward / back / left / right]\n[2] location\n[3] items\n[4] check");
                }
                else Console.WriteLine("Invalid action, try again.");

            }

        }

        static void usedChar(string player, string[] items)
        {
            switch (player)
            {
                case "sheriff": Console.WriteLine("You are now playing as the Marshall Batumbakal (Sheriff)"); break;
                case "nurse": Console.WriteLine("You are now playing as Anastasia Propaganda Maria Dela Cruz (Nurse)"); break;
                case "journalist": Console.WriteLine("You are now playing as Kim Magtanggol (Journalist)"); break;
                case "detective": Console.WriteLine("You are now playing as Rey P. Nyoco (Detective)"); break;
            }

            Console.WriteLine($"Item: {items[0]}\n");
        }

        static string action()
        {
            Console.Write("ACTION: ");
            string ans = Console.ReadLine();
            Console.WriteLine();
            return ans;

        }

        static string[] perk(string player)
        {
            string[] item = new string[1];
            switch (player)
            {
                case "sheriff": item[0] = "Flashlight"; break;
                case "nurse": item[0] = "Keycard"; break;
                case "journalist": item[0] = "Prehint"; break;
                case "detective": item[0] = "Afterhint"; break;
            }
            return item;
        }

        static void prompt(int[] position, string[,] map, string[] items, string[] flags)
        {
            int[] currPosition = position;
            switch (map[currPosition[0], currPosition[1]])
            {
                case "righttree":
                    {
                        Console.Write("There's a tree here.\n");
                        while (true)
                        {
                            string ans = commands(map, position, items, flags);
                            if (ans == "move forward" || ans == "move up") Console.WriteLine("There's a building here. You can't go here.");
                            else if (ans == "move back" || ans == "move down") Console.WriteLine("There's nothing here.");
                            else
                            {
                                if (ans.StartsWith("move"))
                                {
                                    position = move(position, ans, map);
                                    prompt(position, map, items, flags);
                                    break;
                                }
                                else if (ans == "CS 103")
                                {
                                    Console.WriteLine("That is the section that made this game!"); 
                                    break;
                                }
                                else Console.WriteLine("Invalid action, try again.");
                            }
                        }

                    }
                    break;
                case "backtree": Console.Write("There's a tree here.\n"); break;
                case "starting": Console.Write("You're in the starting point\n"); break;
                case "asylum": Console.Write("You're in the asylum.\n"); break;
                case "basement": Console.Write("You're in the basement\n"); break;
                case "secret": Console.Write("You went in the secret room!\n"); break;
                case "tree": Console.Write("There's a tree here.\n"); break;
            }

        }

        static int[] move(int[] position, string command, string[,] map)
        {
            int[] currPosition = position;

            if ((command == "move forward" || command == "move up") && map[position[0] - 1, position[1]] != "0")
            {
                currPosition[0] -= 1;
            }
            else if ((command == "move back" || command == "move down") && map[position[0] + 1, position[1]] != "0")
            {
                currPosition[0] += 1;
            }
            else if (command == "move left" && map[position[0], position[1] - 1] != "0")
            {
                currPosition[1] -= 1;
            }
            else if (command == "move right" && map[position[0], position[1] + 1] != "0")
            {
                currPosition[1] += 1;
            }
            else if (
                map[position[0]+1, position[1]] == "0" ||
                map[position[0]-1, position[1]] == "0" ||
                map[position[0], position[1]+1] == "0" ||
                map[position[0], position[1]-1] == "0"
                ) Console.Write("You can't go here!\nAgain. ");
            else Console.Write("Invalid move!");

            return currPosition;
        }
        static void intro()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("|         WELCOME TO          |");
            Console.WriteLine("|       PULSE OF DECEIT!      |");
            Console.WriteLine("-------------------------------");
        }

        static string choosePlayer()
        {
            string role = "";
            Console.WriteLine("WHO WOULD YOU LIKE TO PLAY AS?");
            Console.WriteLine("");
            Console.WriteLine(@"[1] Marshall Batumbakal (Sheriff)
Perks: Flashlight
Character Description: Marshall Batumbakal is the local sheriff of their town, known for his strong sense of justice.

[2] Anastasia Propaganda Maria Dela Cruz (Nurse)
Perks: Keycard
Character Description: Anastasia Propaganda Maria Dela Cruz is a former nurse in the asylum, concealing a dark secret from her past that haunted her, driving her actions and decisions.

[3] Kim Magtanggol (Journalist)
Perks: Hint ( at the start )
Character Description: Kim Magtanggol, is a fearless journalist, who wants to know the truth behind the chilling murders that surrounded the asylum. 

[4] Rey P. Nyoco (Detective)
Perks: Hint ( along the way )
Character Description: Rey P. Nyoco, a seasoned detective, was called to investigate the series of perplexing murders haunting the asylum. 
");

            while (true)
            {
                Console.Write("ACTION: ");
                string ans = Console.ReadLine();

                if (ans == "1" || ans == "2" || ans == "3" || ans == "4")
                {
                    switch (ans)
                    {
                        case "1": role = "sheriff"; break;
                        case "2": role = "nurse"; break;
                        case "3": role = "journalist"; break;
                        case "4": role = "detective"; break;
                    }
                    break;

                }
                else
                {
                    Console.WriteLine("Invalid action, try again.");
                    continue;
                }
            }
            return role;
        }

    }
}
