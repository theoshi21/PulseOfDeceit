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
        static void Main(string[] args)
        {
            playGame();
        }

        static void playGame()
        {
            int[] position = { 6, 3 };
            string[] flags = new string[10];
            string[] items = {"holder","holder","holder","holder"};
            bool running = true;
            intro();
            string player = choosePlayer();
            items = perk(player, items);
            string[,] map = {
            {"0","0","0","0","0","0","0","0"},
            {"0","0" ,"0","Morgue","0","0","0","0"},
            {"0","0","Therapy Room","Lobby","0","Room1","0","0"},
            {"0","0","0","Hallway2","Staircase","Second Floor","Room2","0"},
            {"0","0","0","Hallway1","0","Room3","0","0"},
            {"0","Basement3","Left Room","Asylum","Right Room","0","0","0"},
            {"0","Basement2","Basement1","Outside","Tree Right","0","0","0"},
            {"0","0","0","Tree Back","0","0","0","0"},
            {"0","0","0","0","0","0","0","0"},
            };

            Console.Clear();
            intro();
            usedChar(player, items);
            while (running)
            {
                prompt(position, map, flags, player, items);
                string ans = commands(map, position, flags, items);
                if (ans.StartsWith("move")) position = move(position, ans, map);

            }

        }

        //Index Determiner for Items
        static int index(string[] items)
        {
            int index = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == "holder")
                {
                    index = i;
                    i = items.Length;
      }
            }
            return index;
        }

        //Holder Replacer
        static string[] item(string[] items, int index, string replace)
        {
            string[] newItem = items;
            newItem[index] = replace;
            return newItem;
}

        //A function for checking the input for various commands.
        static string commands(string[,] map, int[] position, string[] flags, string[] items)
        {
            while (true)
            {
                //availableDirection(position, map);
                Console.WriteLine();
                string input = action();
                if (input.StartsWith("move") || 
                    input.StartsWith("check") || 
                    input.StartsWith("take") || 
                    input.StartsWith("use") || 
                    input == "CS 103") 
                    return input;
                else if (input == "loc" || input == "location" || input == "map")
                {
                    mapping(map, position);
                }
                else if (input == "clear")
                {
                    Console.Clear();
                    intro();
                }
                else if (input == "items")
                {
                    Console.Write("Your items are: ");
                    foreach (string x in items)
                    {
                        if (x != "holder") Console.Write($"({x}) ");
                    }

                    Console.WriteLine();
                }
                else if (input == "commands" || input == "help")
                {
                    Console.WriteLine("The available command are:\n[1] move [forward / back / left / right]\n[2] location\n[3] items\n[4] check\n[5] clear");
                }
                else Console.WriteLine("Invalid action, try again.");

            }

        }

        // Function for showing available directions.
        /*
        static void availableDirection(int[] position, string[,] map)
        {
            string forward = map[position[0] - 1, position[1]];
            string right = map[position[0], position[1] + 1];
            string back = map[position[0] + 1, position[1]];
            string left = map[position[0], position[1] - 1];
            
            if (forward == "0") forward = "Blocked";
            if (right == "0") right = "Blocked";
            if (back == "0") back = "Blocked";
            if (left == "0") left = "Blocked";

            Console.WriteLine($"Forward: {forward}");
            Console.WriteLine($"Right: {right}");
            Console.WriteLine($"Back: {back}");
            Console.WriteLine($"Left: {left}");
        } */
        
        //A function that displays 2d text map.
        static void mapping(string[,] map, int[] position)
        {
            Console.WriteLine("__________        PULSE OF DECEIT'S MAP        __________");
            for (int i = 1; i < 8; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 7; j++)
                {
                    string loc = map[i, j];
                    if (position[0] == i && position[1] == j)
                    {
                        Console.Write($"\\O/\t");
                    }
                    else if (loc != "0")
                    {
                        Console.Write("[");
                        foreach (char x in loc)
                        {
                            if (char.IsUpper(x) || x == '1' || x == '2' || x == '3')
                            {
                                Console.Write($"{x}");
                            }
                        }
                        Console.Write("]\t");
                    }
                    else Console.Write($"\t");
                }
                Console.Write("|\n");
            }
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("You are currently at " + map[position[0], position[1]] + ".");


        }

        //A function for displaying their chosen character and its item.
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


        //A function for taking in input or action.
        static string action()
        {
            Console.Write("ACTION: ");
            string ans = Console.ReadLine().ToLower();
            Console.WriteLine();
            return ans;
        }


        //Adds perk to the player when chosen.
        static string[] perk(string player, string[] items)
        {
            switch (player)
            {
                case "sheriff": items = item(items,index(items),"Flashlight"); break;
                case "nurse": items = item(items, index(items), "Keycard"); break;
                case "journalist": items = item(items, index(items), "Prehint"); break;
                case "detective": items = item(items, index(items), "Afterhint"); break;
            }
            return items;
        }


        // A function that shows the prompt depending on where they are.
        static void prompt(int[] position, string[,] map, string[] flags, string player, string [] items)
        {
            int[] currPosition = position;
            switch (map[currPosition[0], currPosition[1]])
            {
                case "Tree Right":
                    {
                        righttree(map, position, flags, items, player);
                        break;
                    }
                case "Tree Back": 
                    {
                        backtree(map, position, flags, items, player);
                        break;
                    }

                case "Outside":
                    {
                        outside(map, position, flags, items, player);
                        break;
                    }

                case "Asylum":
                    {
                        asylum(map, position, flags, items, player);
                        break;
                    }

                case "Basement1":
                    {
                        basement1(map, position, flags, items, player);
                        break;
                    }

                case "Basement2":
                    {
                        basement2(map, position, flags, items, player);
                        break;
                    }

                case "Basement3":
                    {
                        basement3(map, position, flags, items, player);
                        break;
                    }

                case "Right Room":
                    {
                        rightroom(map, position, flags, items, player);
                        break;
                    }
                case "Left Room":
                    {
                        leftroom(map, position, flags, items, player); 
                        break;
                    }
                case "Hallway1":
                    {
                        hallway1(map, position, flags, items, player);
                        break;
                    }

                case "Hallway2":
                    {
                        hallway2(map, position, flags, items, player);
                        break;
                    }

                case "Staircase":
                    {
                        staircase(map, position, flags, items, player);
                        break;
                    }

                case "Second Floor":
                    {
                        secondfloor(map, position, flags, items, player);
                        break;
                    }

                case "Room1":
                    {
                        room1(map, position, flags, items, player);
                        break;
                    }

                case "Room2":
                    {
                        room2(map, position, flags, items, player);
                        break;
                    }

                case "Room3":
                    {
                        room3(map, position, flags, items, player);
                        break;
                    }

                case "Lobby":
                    {
                        lobby(map, position, flags, items, player);
                        break;
                    }

                case "Therapy Room":
                    {
                        therapy(map, position, flags, items, player);
                        break;
                    }

                case "Morgue":
                    {
                        morgue(map, position, flags, items, player);
                        break;
                    }

            }

        }

        //Checker if an item is in the array.
        static bool isItem(string match, string[] items)
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

        //Decorative Display / Header
        static void intro()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("|         WELCOME TO          |");
            Console.WriteLine("|       PULSE OF DECEIT!      |");
            Console.WriteLine("-------------------------------");
        }

        // Pre-game prompt that will make you choose a character.
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

        static void rightroom(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.WriteLine("You went into the room to your right. This is a jumbled mess. \nA fallen bookshelf, a broken window, and files and note are on the table.");
            while (true)
            {
                string ans = commands(map, position, flags, items);
                if (ans.StartsWith("move"))
                {
                    if (ans == "move down" || ans == "move back" || ans == "move backward")
                    {
                        Console.WriteLine("This is an enclosed room. You can't go here");
                    }
                    else
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
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
                        if (isItem("Afterhint", items)) Console.WriteLine("Detective Skills: This missing page might lead us to who the killer is.");
                    }
                    else if (ans.ToLower() == "check window" || ans.ToLower() == "check broken window")
                    {
                        if (!isItem("Keycard", items))
                        {
                            Console.WriteLine("You went near the broken window. While walking, you seem to have stepped on something—-a keycard. \nYou took the keycard and looked out the window. All you see outside is the darkness of the forest.");
                            items = item(items, index(items), "Keycard");
                        }
                        else
                        {
                            Console.WriteLine("You went near the broken window. While walking, you seem to have stepped on something—-a keycard. \nBut you already possess a keycard. All you see outside is the darkness of the forest.");
                        }
                    }
                    else Console.WriteLine("Invalid check item, try again.");
                }
            }
        }

        static void asylum(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("A long dark hallway welcomes you. There are room to your left and right.\n");
            string ans = commands(map, position, flags, items);

            if (ans.StartsWith("move"))
            {
                if (ans == "move left" && isItem("Keycard", items))
                {
                    Console.WriteLine("(You used your key to open the door.)");
                    position = move(position, ans, map);
                    prompt(position, map, flags, player, items);
                }
                else if (ans == "move left" && !isItem("Keycard", items))
                {
                    Console.WriteLine("The door seems locked. You need a key, try going back here once you find one.");
                }
                else if (ans.StartsWith("check")){
                    Console.WriteLine("Nothing to check here.");
                }
                else
                {
                    position = move(position, ans, map);
                    prompt(position, map, flags, player, items);
                }
            }
            else if (ans.StartsWith("check")) Console.WriteLine("Nothing to check here.");
        }

        static void leftroom(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.WriteLine("You went inside the cold and dark room and found out that it was a storage room \nthat was almost fully covered with dust and cobwebs. " +
                "It is cramped in here. \nIt was full of shelves containing medicines with a table that had medicines scattered on top of it.");
        }

        static void outside(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in front of the asylum. Cold wind blows -- around the asylum is a dark forest.\n");
        }

        static void righttree(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("There's a tree here.\n");
            while (true)
            {
                string ans = commands(map, position, flags, items);
                if (ans == "move forward" || ans == "move up") Console.WriteLine("There's a building here. You can't go here.");
                else if (ans == "move back" || ans == "move down") Console.WriteLine("There's nothing here.");
                else
                {
                    if (ans.StartsWith("move"))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
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

        static void backtree(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are surrounded by the forest. There seems to be nothing here.\n");
        }

        static void basement1(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in basement 1.\n");
        }

        static void basement2(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in basement 2.\n");
        }

        static void basement3(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in basement 3.\n");
        }

        static void hallway1(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in hallway 1.\n");
        }

        static void hallway2(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in hallway 2.\n");
        }

        static void staircase(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("There's a staircase leading to the 2nd floor to your right.\n");
        }

        static void secondfloor(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You're now in 2nd floor.\n");
        }

        static void room1(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in room 1.\n");
        }

        static void room2(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in room 2.\n");
        }

        static void room3(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in room 3.\n");
        }

        static void lobby(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in hospital lobby.\n");
        }

        static void therapy(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You went inside the therapy room.\n");
        }

        static void morgue(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("It smells bad here, this seems to be the morgue.\n");
        }

    }
}
