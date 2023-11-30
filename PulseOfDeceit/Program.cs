using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PulseOfDeceit
{
    internal class Program
    {
        static List<string> items = new List<string>();
        static void Main(string[] args)
        {
            playGame();
        }

        static void playGame()
        {
            int[] position = { 6, 3 };
            string[] flags = new string[10];
            bool running = true;
            intro();
            string player = choosePlayer();
            perk(player);
            string[,] map = {
            {"0","0","0","0","0","0","0","0"},
            {"0","0" ,"0","morgue","0","0","0","0"},
            {"0","0","therapy","lobby","0","room1","0","0"},
            {"0","0","0","hallway2","staircase2f","secondf","room2","0"},
            {"0","0","0","hallway1","0","room3","0","0"},
            {"0","staircase","leftroom","asylum","rightroom","0","0","0"},
            {"0","secret","basement","starting","righttree","0","0","0"},
            {"0","0","0","backtree","0","0","0","0"},
            {"0","0","0","0","0","0","0","0"},
            };

            Console.Clear();
            intro();
            usedChar(player);
            while (running)
            {
                prompt(position, map, flags, player);
                string ans = commands(map, position, flags);
                if(ans.StartsWith("move")) position = move(position, ans, map);

            }

        }


        //A function for checking the input for various commands.
        static string commands(string[,] map, int[] position, string[] flags)
        {
            while (true)
            {
                string input = action();
                if (input.StartsWith("move") || 
                    input.StartsWith("check") || 
                    input.StartsWith("take") || 
                    input.StartsWith("use") || 
                    input == "CS 103") 
                    return input;
                else if (input == "loc" || input == "location")
                {
                    Console.WriteLine($"You're currently at ({map[position[0], position[1]]}).");
                }
                else if (input == "items")
                {
                    Console.Write("Your items are: ");
                    foreach (string x in items) Console.Write($"({x}) ");
                    Console.WriteLine();
                }
                else if (input == "commands" || input == "help")
                {
                    Console.WriteLine("The available command are:\n[1] move [forward / back / left / right]\n[2] location\n[3] items\n[4] check");
                }
                else Console.WriteLine("Invalid action, try again.");

            }

        }

        //A function for displaying their chosen character and its item.
        static void usedChar(string player)
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


        //A function for taking in input or action.
        static string action()
        {
            Console.Write("ACTION: ");
            string ans = Console.ReadLine().ToLower();
            Console.WriteLine();
            return ans;
        }


        static void perk(string player)
        {
            switch (player)
            {
                case "sheriff": items.Add("Flashlight"); break;
                case "nurse": items.Add("Keycard"); break;
                case "journalist": items.Add("Prehint"); break;
                case "detective": items.Add("Afterhint"); break;
            }

        }


        // A function that shows the prompt depending on where they are.
        static void prompt(int[] position, string[,] map, string[] flags, string player)
        {
            int[] currPosition = position;
            switch (map[currPosition[0], currPosition[1]])
            {
                case "righttree":
                    {
                        Console.Write("There's a tree here.\n");
                        while (true)
                        {
                            string ans = commands(map, position, flags);
                            if (ans == "move forward" || ans == "move up") Console.WriteLine("There's a building here. You can't go here.");
                            else if (ans == "move back" || ans == "move down") Console.WriteLine("There's nothing here.");
                            else
                            {
                                if (ans.StartsWith("move"))
                                {
                                    position = move(position, ans, map);
                                    prompt(position, map, flags, player);
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
                        break;
                    }
                case "backtree": Console.Write("You are surrounded by the forest. There seems to be nothing here.\n"); break;

                case "starting": Console.Write("You are in front of the asylum. Cold wind blows -- around the asylum is a dark forest.\n"); break;

                case "asylum":
                    {
                        Console.Write("A long dark hallway welcomes you. There are room to your left and right.\n");
                        string ans = commands(map, position, flags);
         
                        if (ans.StartsWith("move"))
                        {
                            if (ans == "move left" && isItem("Keycard"))
                            {
                                position = move(position, ans, map);
                                prompt(position, map, flags, player);
                            }
                            else if (ans == "move left" && !isItem("Keycard"))
                            {
                                Console.WriteLine("The door seems locked. You need a key, try going back here once you find one.");
                            }
                            else
                            {
                                position = move(position, ans, map);
                                prompt(position, map, flags, player);
                            }
                        }
                        else if (ans.StartsWith("check")) Console.WriteLine("Nothing to check here.");
                        break;
                    }

                case "basement": Console.Write("You're in the basement\n"); break;

                case "secret": Console.Write("You went in the secret room!\n"); break;

                case "rightroom":
                    {
                        Console.WriteLine("You went into the room to your right. This is a jumbled mess. \nA fallen bookshelf, a broken window, and files and note are on the table.");
                        while (true)
                        {
                            string ans = commands(map, position, flags);
                            if (ans.StartsWith("move"))
                            {
                                if (ans == "move down" || ans == "move back" || ans == "move backward")
                                {
                                    Console.WriteLine("This is an enclosed room. You can't go here");
                                }
                                else
                                {
                                    position = move(position, ans, map);
                                    prompt(position, map, flags, player);
                                    break;
                                }
                            }
                            if (ans.StartsWith("check"))
                            {
                                if (ans == "check note")
                                {
                                    Console.WriteLine("You took the note. It reads 'Please go, patients in this asylum are worse than you thought.'");
                                }
                                else if (ans == "check bookshelf" || ans == "check books" || ans == "check book")
                                {
                                    Console.WriteLine("There are books on the floor named ‘Psychology 101’, ‘Comprehensive Overview of Split Personality Disorder’, \nand a file labeled [CONFIDENTIAL]. There is nothing to do here.");
                                }
                                else if (ans == "check file" || ans == "check files")
                                {

                                    Console.WriteLine("You read the file, and you found out that these are the personal records of the patient. " +
                                        "\nWhen reading, you noticed that there was a missing page in the file.");
                                    if (isItem("Afterhint")) Console.WriteLine("Detective Skills: This missing page might lead us to who the killer is.");
                                }
                                else if (ans.ToLower() == "check window" || ans.ToLower() == "check broken window")
                                {
                                    if (!isItem("Keycard"))
                                    {
                                        Console.WriteLine("You went near the broken window. While walking, you seem to have stepped on something—-a keycard. \nYou took the keycard and looked out the window. All you see outside is the darkness of the forest.");
                                        items.Add("Keycard");
                                    }
                                    else
                                    {
                                        Console.WriteLine("You went near the broken window. While walking, you seem to have stepped on something—-a keycard. \nBut you already possess a keycard. All you see outside is the darkness of the forest.");
                                    }
                                }
                                else Console.WriteLine("Invalid check item, try again.");
                            }
                        }
                        break;
                    }
                case "leftroom": Console.WriteLine("You are in the left room.");  break;

            }

        }


        static bool isItem(string match)
        {
            foreach (string x in items)
            {
                if (x == match)
                {
                    return true;
                }
            }
            return false;
        }

        // A function responsible for moving around the map.
        static int[] move(int[] position, string command, string[,] map)
        {
            int[] currPosition = position;

            if ((command == "move forward" || command == "move up") && map[position[0] - 1, position[1]] != "0")
            {
                currPosition[0] -= 1;
            }
            else if ((command == "move back" || command == "move down" || command == "move backward") && map[position[0] + 1, position[1]] != "0")
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
            else Console.WriteLine("Invalid move!");

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
                string ans = action();

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
