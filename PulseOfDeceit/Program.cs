using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
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
            string[] flags = { "holder", "holder", "holder", "holder", "holder", "holder", "holder" , "holder" , "holder" , "holder" };
            string[] items = { "holder", "holder", "holder", "holder", "holder" , "holder" , "holder" , "holder" , "holder" , "holder" };
            bool running = true;
            string[,] map = {
            {"0","0","0","0","0","0","0","0"},
            {"0","0" ,"0","Morgue","0","0","0","0"},
            {"0","0","Security Room","Lobby","0","Patient Ward","0","0"},
            {"0","0","0","Hallway2","Staircase","Second Floor","Director's Office","0"},
            {"0","0","0","Hallway1","0","Private Ward","0","0"},
            {"0","Ground Basement","Laboratory","Asylum","Storage Room","0","0","0"},
            {"0","Upper Basement","Tree Left","Outside","Tree Right","0","0","0"},
            {"0","Lower Basement","0","Gate","0","0","0","0"},
            {"0","0","0","0","0","0","0","0"},
            };

            string player = playerName();
            prologue();
            instructions();
            bgstory();

            while (running)
            {
                prompt(position, map, flags, player, items);
                string ans = commands(map, position, flags, items, player);
                if (ans.StartsWith("move")) position = move(position, ans, map);

            }

        }

        //Player / Username
        static string playerName()
        {
            intro();
            Console.WriteLine("\nWelcome! Before we start, may we know what is your name?");

            while (true)
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();

                if (name != "")
                {
                    Console.Write($"\nThank you, {name}. To start the game, press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    return name;
                }
                else
                {
                    Console.WriteLine("Please don't use a blank name!\n");
                }
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

        //Item Breaker
        static string[] remove(string[] items, string remove)
        {
            string[] newItem = items;
            for(int i = 0; i < newItem.Length; i++)
            {
                if (newItem[i] == remove)
                {
                    newItem[i] = "holder";
                    break;
                }
            }
            return newItem;
        }

        //A function for checking the input for various commands.
        static string commands(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            while (true)
            {
                //availableDirection(position, map);
                Console.WriteLine();
                string input = action();
                if (input.StartsWith("move") ||
                    input.StartsWith("check") ||
                    input.StartsWith("take") ||
                    input == "CS 103")
                    return input;
                else if (input == "where")
                {
                    Console.WriteLine($"You are currently at {map[position[0], position[1]]}");
                }
                else if (input == "clear")
                {
                    Console.Clear();
                    mapping(map, position);

                }
                else if (input.StartsWith("use"))
                {
                    if(input == "use map")
                    {
                        if (isItem("map", flags) && isItem("Map", items)) Console.WriteLine("Your map is already displayed!");
                        else if (!isItem("map", flags) && !isItem("Map", items)) Console.WriteLine("You don't have a map to display.");
                        else
                        {
                            Console.WriteLine("Map is now displayed.");
                            flags = item(flags, index(flags), "map");
                        }
                    }
                    else
                    {
                        return input;
                    }


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
                    Console.WriteLine("The available commands are:");
                    Console.WriteLine(@"> help - display the list of commands.
> move (up/left/down/right) - move in the direction of your choice.
> check (object) - check the items. [Objects] enclosed in brackets can be typed in.
> take (object) - take the item.
> items - list the items you currently have.
");
                }
                else Console.WriteLine("Invalid action, try again.");

            }

        }

        //A function that displays 2d text map.
        static void mapping(string[,] map, int[] position)
        {
            Console.WriteLine("----------        PULSE OF DECEIT'S MAP        ----------");
            for (int i = 1; i < 8; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 7; j++)
                {
                    string loc = map[i, j];
                    if (position[0] == i && position[1] == j)
                    {
                        Console.Write($"(XX)\t");
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
            Console.WriteLine("---------------------------------------------------------\n");

        }


        //A function for taking in input or action.
        static string action()
        {
            Console.Write("> ");
            string ans = Console.ReadLine().ToLower();
            Console.WriteLine();
            return ans;
        }


        // A function that shows the prompt depending on where they are.
        static void prompt(int[] position, string[,] map, string[] flags, string player, string [] items)
        {
            int[] currPosition = position;
            Console.Clear();

            header(flags, position, map);

            switch (map[currPosition[0], currPosition[1]])
            {
                case "Gate":
                    {
                        break;
                    }
                case "Tree Right":
                    {
                        righttree(map, position, flags, items, player);
                        break;
                    }
                case "Tree Left": 
                    {
                        lefttree(map, position, flags, items, player);
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

                case "Ground Basement":
                    {
                        basement1(map, position, flags, items, player);
                        break;
                    }

                case "Upper Basement":
                    {
                        basement2(map, position, flags, items, player);
                        break;
                    }

                case "Lower Basement":
                    {
                        basement3(map, position, flags, items, player);
                        break;
                    }

                case "Storage Room":
                    {
                        storageroom(map, position, flags, items, player);
                        break;
                    }
                case "Laboratory":
                    {
                        laboratory(map, position, flags, items, player); 
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

                case "Patient Ward":
                    {
                        patientward(map, position, flags, items, player);
                        break;
                    }

                case "Director's Office":
                    {
                        director(map, position, flags, items, player);
                        break;
                    }

                case "Private Ward":
                    {
                        privateward(map, position, flags, items, player);
                        break;
                    }

                case "Lobby":
                    {
                        lobby(map, position, flags, items, player);
                        break;
                    }

                case "Security Room":
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

            return currPosition;
        }


        static void prologue()
        {
            Console.WriteLine(@"______          _                        
| ___ \        | |                       
| |_/ / __ ___ | | ___   __ _ _   _  ___ 
|  __/ '__/ _ \| |/ _ \ / _` | | | |/ _ \
| |  | | | (_) | | (_) | (_| | |_| |  __/
\_|  |_|  \___/|_|\___/ \__, |\__,_|\___|
                         __/ |           
                        |___/         ");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        //Decorative Display / Header
        static void intro()
        {
            Console.WriteLine(@"  ____   _   _  _      ____   _____     ___   _____  
 |  _ \ | | | || |    / ___| | ____|   / _ \ |  ___| 
 | |_) || | | || |    \___ \ |  _|    | | | || |_    
 |  __/ | |_| || |___  ___) || |___   | |_| ||  _|   
 |_|     \___/ |_____||____/ |_____|   \___/ |_|     
                                                     
  ____   _____  ____  _____  ___  _____              
 |  _ \ | ____|/ ___|| ____||_ _||_   _|             
 | | | ||  _| | |    |  _|   | |   | |               
 | |_| || |___| |___ | |___  | |   | |               
 |____/ |_____|\____||_____||___|  |_|");
        }

        static void bgstory()
        {

            Console.WriteLine(@"______            _                                   _   _____ _                   
| ___ \          | |                                 | | /  ___| |                  
| |_/ / __ _  ___| | ____ _ _ __ ___  _   _ _ __   __| | \ `--.| |_ ___  _ __ _   _ 
| ___ \/ _` |/ __| |/ / _` | '__/ _ \| | | | '_ \ / _` |  `--. \ __/ _ \| '__| | | |
| |_/ / (_| | (__|   < (_| | | | (_) | |_| | | | | (_| | /\__/ / || (_) | |  | |_| |
\____/ \__,_|\___|_|\_\__, |_|  \___/ \__,_|_| |_|\__,_| \____/ \__\___/|_|   \__, |
                       __/ |                                                   __/ |
                      |___/                                                   |___/ 
        __   _   _            _   _                                                 
       / _| | | | |          | \ | |                                                
  ___ | |_  | |_| |__   ___  |  \| |_   _ _ __ ___  ___                             
 / _ \|  _| | __| '_ \ / _ \ | . ` | | | | '__/ __|/ _ \                            
| (_) | |   | |_| | | |  __/ | |\  | |_| | |  \__ \  __/                            
 \___/|_|    \__|_| |_|\___| \_| \_/\__,_|_|  |___/\___|    

Anastacia Propaganda Maria Dela Cruz, a nurse driven by compassion and a desire to make a difference, dedicated her 
life to nursing. Her journey into mental health care led her to an abandoned asylum, where she hoped to offer 
solace to the forgotten. Without her knowledge, she received an anonymous letter hinting at mysterious 
deaths within the asylum, setting her on a path of investigation. Little did she know, the 
shadows she sought that the dark secrets in the asylum were connected to her. 

The story is about her journey to uncover this truth, 
facing darkness and the scary reality of the asylum.");
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        

        static void pod()
        {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("|       PULSE OF DECEIT       |");
                Console.WriteLine("-------------------------------");
        }

        static void instructions()
        {
            Console.WriteLine(@"
 _____          _                   _   _                 
|_   _|        | |                 | | (_)                
  | | _ __  ___| |_ _ __ _   _  ___| |_ _  ___  _ __  ___ 
  | || '_ \/ __| __| '__| | | |/ __| __| |/ _ \| '_ \/ __|
 _| || | | \__ \ |_| |  | |_| | (__| |_| | (_) | | | \__ \
 \___/_| |_|___/\__|_|   \__,_|\___|\__|_|\___/|_| |_|___/

In this game, you have the following commands at your perusal. These commands will be helpful for you to interact with your surroundings and finish the game. 

> help - display the list of commands.
> move (up/left/down/right) - move in the direction of your choice.
> check (item) - check the items. [Items] enclosed in brackets can be typed in.
> take (item) - take the item
> items - list the items you currently have.
");

            Console.Write("Press any key continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void header(string[] flags, int[] position, string[,] map)
        {
            if(isItem("map", flags)){
                mapping(map, position);
            }
            else
            {
                pod();
            }
        }

        static void storageroom(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("storage", flags))
            {
                Console.WriteLine("\"Oh, this room is open. What can I find here?\" Anastacia said curiously.\n");
                flags = item(flags, index(flags), "storage");

                Console.ReadKey();
                Console.WriteLine("HTE STRAOGE OROM, TRPAPED NI YACDE, FETAURDE BRKONE SWWINDO, A YTMUS LLMSE, DAN FROGTTEN SERLIC. \n" +
                   "A BOOLD-TENWRTI MSSEAEG, \"RUN IF YOU WANT TO LIVE.\" DAORDNE ETH LLAW. HTE ERIEE CILENSE THINED TA NUSTTLEING \n" +
                   "CESRETS, WTHI LOCASSIONA RSUTELS DAN TANTDIS ECHOSE, RINGFFERO A CHILNGIL WRNAING OT THESO HOW REDAD OT ETNER.");

                Console.ReadKey();
                items = item(items, index(items), "Flashlight");
                Console.WriteLine("\n\"Despite the unflattering confines of this room, at least they have a [Flashlight] as my only hope, \n" +
                    "lighting up my surroundings sufficiently to let me see comple– who is that!?\"");
                Console.WriteLine("\nYou got a [Flashlight].\n");

                Console.ReadKey();
                Console.WriteLine("The wind intensified crawling through the hallway, and amplifying the unsettling atmosphere. \n" +
                    "Doors were abruptly and violently shut by an unknown power, revealing after the faint echo of receding footsteps \n" +
                    "resounding the hallway, inciting a halt in Anastacia’s spoken expression.\r\n");

                Console.ReadKey();
                Console.WriteLine("You can't leave the room, what would you like to do?\n" +
                    "[1] Kick the door open or\n" +
                    "[2] Use the crowbar to break the doorknob.");
                while (true) { 
                string ans = action();
                    if (ans == "1")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("\"Frick! That hurt and still it won't budge?\"\n");
                        Console.WriteLine("Anastacia gruntly shouted as she hurt herself trying to break the door. \n" +
                            "Now resorting to her next option.\n");
                        Console.ReadKey();
                        break;
                    }
                    else if (ans == "2")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        break;
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }

                Console.WriteLine("\"May this thing bear fruit\"");
                Console.ReadKey();
                Console.WriteLine("With a hint of optimism, Anastacia uttered. Her tone was covered with tension as she \n" +
                    "skillfully handled the crowbar in an attempt to remove the doorknob—a tool that she used to break in, \n" +
                    "which caused it to smash to smithereens. Fortunately, the doorknob falls to bits.\r\n");
                Console.WriteLine("Your [Crowbar] broked.\n");
                remove(items, "Crowbar");

                Console.ReadKey();
                Console.WriteLine("“Who was that? Was that just the wind? Didn’t I hear footsteps?”\r\n");

                Console.WriteLine("Anastacia said with a voice mixed with uncertainty. Ignoring the thing, \n" +
                    "she continued and was committed to solving the mystery that was clinging to the air.");
            }
            else
            {
                if (isItem("Flashlight", items))
                {
                    Console.WriteLine("The storage room, trapped in decay, featured broken windows, a musty smell, and forgotten relics. \n" +
                        "A blood-written message, \"RUN IF YOU WANT TO LIVE,\" adorned the wall. The eerie silence hinted at unsettling \n" +
                        "secrets, with occasional rustles and distant echoes, offering a chilling warning to those who dared to enter.");
                }

            }

            while (true)
            {
                string ans = commands(map, position, flags, items, player);
                if (ans.StartsWith("move"))
                {
                    if ((ans == "move down" || ans == "move backward") || (ans == "move up" || ans == "move forward") || ans == "move right")
                    {
                        Console.WriteLine("You can't go here. This is just a wall.");
                    }
                    else if (ans == "move left")
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("check"))
                {
                    Console.WriteLine("Nothing to check here.");
                }
                else if (ans.StartsWith("take"))
                {
                    Console.WriteLine("Nothing to take here.");
                }
                else Console.WriteLine("Invalid action, try again.");
            }
        }

        static void asylum(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("asylum", flags))
            {
                Console.WriteLine("You used your crowbar to break the chains and is now inside the asylum.\n");
                flags = item(flags, index(flags), "asylum");
                Console.WriteLine($"READ CAREFULLY, {player}.\n");
                Console.Write("Press any key to continue...");
                Console.ReadKey();

                Console.WriteLine("\"This place is truly disordered, I can't even see a thing.\"");
                Console.WriteLine("\nAnastacia said, breaking the terrifying silence as her voice trailed\n" +
                    "reflecting the unease that lingered over the room.\n");
            }

            if (!isItem("Flashlight", items))
            {
                Console.WriteLine("LKOSO LIEK SI TI YVER DRAK IN HREE. UYO MTIHG WNTA TO FNDI A FLSHALGHIT TISRF.");

                while (true)
                {
                    string ans = commands(map, position, flags, items, player);
                    if (ans.StartsWith("move"))
                    {
                        if ((ans == "move up" || ans == "move forward"))
                        {
                            Console.WriteLine("The hallway is very dark, it's best to find something that can luminate these halls.");
                        }

                        else if (ans == "move down" || ans == "move backward")
                        {
                            Console.WriteLine("That's outside, there's nothing to do there. You should start inspecting the inside.");
                        }
                        else if (ans == "move left")
                        {
                            Console.WriteLine("OYU CNA TON TERNE HET LBAROTRYAO OROM TYE UNLTI UOY DNFI YROUESLF A KYACDRE.");
                        }
                        else if (ans == "move right")
                        {
                            position = move(position, ans, map);
                            Console.Clear();
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
            }
            else
            {
                Console.WriteLine("You are back at the main entrance of the asylum.");
                while (true)
                {
                    string ans = commands(map, position, flags, items, player);
                    if (ans.StartsWith("move"))
                    {

                        if (ans == "move down" || ans == "move backward")
                        {
                            Console.WriteLine("That's outside, there's nothing to do there. You should start inspecting the inside.");
                        }
                        else if (ans == "move left")
                        {
                            if (!isItem("Keycard", items)) Console.WriteLine("You can not enter the laboratory room yet until you find yourself a keycard.");
                            else
                            {
                                position = move(position, ans, map);
                                Console.Clear();
                                prompt(position, map, flags, player, items);
                                break;
                            }
                        }
                        else if (ans == "move right" || ans == "move up" || ans == "move forward")
                        {
                            position = move(position, ans, map);
                            Console.Clear();
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
            }
               
          
             
        }

        static void laboratory(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.WriteLine("You went inside the cold and dark room and found out that it was a storage room \nthat was almost fully covered with dust and cobwebs. " +
                "It is cramped in here. \nIt was full of shelves containing medicines with a table that had medicines scattered on top of it.");
        }

        static void outside(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("outside",flags))
            {
                Console.WriteLine("You entered the gate, a huge building welcomes you.\n" +
                    "As you are standing outside the asylum, feeling the chilling wind whisper through the overgrown vines \n" +
                    "and dilapidated windows, carrying a sense of foreboding, you feel the weight of the impending truth \n" +
                    "pressing down on your shoulders.\n");

                Console.WriteLine("Would you like to inspect your surroundings? [Yes] or [No] ?\n");
                while (true)
                {
                    string answer = action();
                    if (answer.ToLower() == "yes")
                    {
                        Console.Clear();
                        pod();
                        Console.WriteLine("You are outside the asylum with a door blocking your path; as it is surrounded by chains, rejecting the curiosity. " +
                            "\nYou will need a crowbar to break the chains to conceal the mysterious secrets. " +
                            "\nAs you inspect your surroundings, you notice that you are surrounded by trees. " +
                            "\nThe tree on your left stands like a sentinel, its gnarled branches reaching out for secrets buried in the soil. " +
                            "\nTo your right, another tree glows mysteriously, its leaves whispering a way in a haunting melody.\n");
                        Console.ReadKey();
                        Console.WriteLine("\"Why am I entangled in this labyrinth of mysteries?  It was a naive move to go " +
                            "\nthrough these gloomy depths in the first place.\"\r\n");
                        Console.ReadKey();
                        Console.WriteLine("Anastacia said and let out a deep sigh, wondering why she was pursuing the unknown so eagerly.\n");
                        Console.ReadKey();
                        Console.WriteLine("Could these trees be something? Take a push to discover, the floor is yours, {player}, to unravel behind" +
                            $"\nthe creaking leaves and shady branches.");
                        break;
                    }
                    else if (answer.ToLower() == "no")
                    {
                        Console.Clear();
                        pod();
                        Console.WriteLine("You are outside the asylum with a door blocking your path; as it is surrounded by \n" +
                            "chains, rejecting the curiosity. You will need a crowbar to break the chains to conceal the mysterious secrets.");
                        break;
                    }
                    else Console.WriteLine("Invalid answer, try again.");
                }
                item(flags, index(flags), "outside");
            }
            else
            {
                Console.WriteLine("You are outside the asylum with a door blocking your path; as it is surrounded by \n" +
                            "chains, rejecting the curiosity. You will need a crowbar to break the chains to conceal the mysterious secrets.");
            }

            while (true)
            {
            string ans = commands(map, position, flags, items, player);
                if (ans.StartsWith("move"))
                {
                    if ((ans == "move up" || ans == "move forward"))
                    {
                        if (!isItem("Crowbar", items) && !isItem("Map", items))
                        {
                            Console.WriteLine("The door in front of you chained prohibiting you from entering. You still need to explore your surroundings before entering.");
                        }
                        else if (isItem("Crowbar", items) && !isItem("Map", items))
                        {
                            Console.WriteLine("You have a crowbar but it may be hard navigating through the asylum. You still need to explore further.");
                        }
                        else if (!isItem("Crowbar", items) && isItem("Map", items))
                        {
                            Console.WriteLine("You have your navigation tool but you still can't break the chains. You still need to explore further.");
                        }
                        else
                        {
                            position = move(position, ans, map);
                            prompt(position, map, flags, player, items);
                            break;
                        }
                    }

                    else if (ans == "move down" || ans == "move backward")
                    {
                        Console.WriteLine("That's the gate you went into, you cannot go there.");
                    }
                    else if ((ans == "move up" || ans == "move forward") && isItem("Map", items) && isItem("Crowbar", items)) {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else if (ans == "move left" || ans == "move right")
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("check"))
                {
                    Console.WriteLine("Nothing to check here.");
                }
                else Console.WriteLine("Invalid action, try again.");
            }
        }

        static void righttree(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("Crowbar", items) && !isItem("Map", items))
            {
                Console.WriteLine("As you approach the dark, oak tree, you notice a rusted metal object lying flat \n" +
                "at the base of the trunk. You approach and pick up the metal object. A [Crowbar]?\n");
                Console.ReadKey();
                Console.WriteLine("You got a [Crowbar]!\n");
                Console.ReadKey();
                Console.WriteLine("Right, the key will be a crowbar—to release the chains that are blocking the main doors' entry.\n");
                Console.ReadKey();
                Console.WriteLine("\"Well, I guess, this could be a weapon, just in case.\" Anastacia said.\n");
                Console.ReadKey();
                Console.WriteLine("Amidst the huge void, how could you know your location without the indispensable help of a map?");
                items = item(items, index(items), "Crowbar");
            }
            else if (!isItem("Crowbar", items) && isItem("Map", items))
            {
                Console.WriteLine("As you approach the dark, oak tree, you notice a rusted metal object lying flat \n" +
                "at the base of the trunk. You approach and pick up the metal object. A [Crowbar]?\n");
                Console.ReadKey();
                Console.WriteLine("You got a [Crowbar]!\n");
                Console.ReadKey();
                Console.WriteLine("Right, the key will be a crowbar—to release the chains that are blocking the main doors' entry.");
                items = item(items, index(items), "Crowbar");
            }
            else if (isItem("Crowbar", items) && isItem("Map", items))
            {
                Console.WriteLine("A long dark, oak tree welcomes you. There's nothing to do here now.");
            }
            else if (isItem("Crowbar", items) && !isItem("Map", items))
            {
                Console.WriteLine("A long dark, oak tree welcomes you. There's nothing to do here now.\n" +
                    "You might want to find the map now.");
            }

            while (true)
            {
                string ans = commands(map, position, flags, items, player);
                if (ans.StartsWith("move"))
                {
                    if (ans == "move up")
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else if (ans == "move down" || ans == "move right")
                    {
                        Console.WriteLine("There's nothing to do out there, it is best not to go there.");
                    }
                    else if(ans == "move left")
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("check"))
                {
                    Console.WriteLine("Nothing to check here.");
                }
                else Console.WriteLine("Invalid action, try again.");
            }
        }

        static void lefttree(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("Map", items) && !isItem("Crowbar", items))
            {
                Console.WriteLine("An old, eerie tree with dead leaves, welcomes you with a cozy atmosphere, " +
               "\nthe leaves dancing in the breeze, as you find a piece of paper tucked in between the trunk and a branch. \n");
                Console.ReadKey();
                Console.WriteLine("Great start, you found a [Map].");
                items = item(items, index(items), "Map");

                Console.ReadKey();
                while (true)
                {
                    Console.Write("\nWould you like to display your map? [Yes] or [No]: ");
                    string displayMap = Console.ReadLine().ToLower();
                    if (displayMap == "yes")
                    {
                        flags = item(flags, index(flags), "map");
                        Console.WriteLine("\n\"A map, huh?\" Anastacia said.");
                        Console.ReadKey();
                        Console.Clear();
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else if (displayMap == "no")
                    {
                        Console.WriteLine("\nThe map won't be displayed, this might make your game harder. If your mind changes,\n" +
                            "just type \"use map\"");
                        break;
                    }
                    else Console.WriteLine("Invalid answer, try again.");
                }
            }
            else if (!isItem("Map", items) && isItem("Crowbar", items))
            {
                Console.WriteLine("An old, eerie tree with dead leaves, welcomes you with a cozy atmosphere, " +
               "\nthe leaves dancing in the breeze, as you find a piece of paper tucked in between the trunk and a branch. \n");
                Console.ReadKey();
                Console.WriteLine("Great, you found the [Map].");
                items = item(items, index(items), "Map");

                Console.ReadKey();
                while (true) {
                Console.Write("\nThis map can help you keep track of where you are.\nWould you like to display your map? [Yes] or [No]: ");
                string displayMap = Console.ReadLine().ToLower();
                if (displayMap == "yes")
                {
                        flags = item(flags, index(flags), "map");
                        Console.WriteLine("\n\"A map, huh?\" Anastacia said.");
                        Console.ReadKey();
                        Console.Clear();
                        prompt(position, map, flags, player, items);
                        break;
                }
                else if (displayMap == "no")
                {
                        Console.WriteLine("\nThe map won't be displayed, this might make your game harder. If your mind changes,\n" +
                            "just type \"use map\"");
                        break;
                }
                else Console.WriteLine("Invalid answer, try again.");
                }

            }
            else if(isItem("Map", items)) Console.WriteLine("An old, eerie tree with dead leves, welcomes you with a cozy atmosphere, " +
                "\nand the leaves dancing in the breeze.");

            while (true)
            {
                string ans = commands(map, position, flags, items, player);
                if (ans.StartsWith("move"))
                {
                    if (ans == "move up" || ans == "move left")
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else if (ans == "move down")
                    {
                        Console.WriteLine("There's nothing to do out there, it is best not to go there.");
                    }
                    else if (ans == "move right")
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("check"))
                {
                    Console.WriteLine("Nothing to check here.");
                }
                else Console.WriteLine("Invalid action, try again.");
            }



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
            Console.WriteLine("You walked through the long hallway. It seems like you need to continue moving forward.");
            while (true)
            {
                string ans = commands(map, position, flags, items, player);
                if (ans.StartsWith("move"))
                {
                    if (ans == "move right" || ans == "move left")
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else if (ans == "move up")
                    {
                        position = move(position, ans, map);
                        prompt(position, map, flags, player, items);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("check"))
                {
                    Console.WriteLine("Nothing to check here.");
                }

                else if (ans.StartsWith("take"))
                {
                    Console.WriteLine("Nothing to take here.");
                }
                else Console.WriteLine("Invalid action, try again.");
            }
        }
        static void hallway2(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("hide", flags))
            {
                Console.WriteLine("You finished the end of the walkway. There is a lobby in front of you, and a staircase on your right");
                Console.WriteLine("\nWhere would you like to go?");
                while (true)
                {
                    string ans = commands(map, position, flags, items, player);
                    if (ans.StartsWith("move"))
                    {
                        if (ans == "move left")
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (ans == "move down")
                        {
                            Console.WriteLine("That is where you were, it's probably a waste of time to go back there for now.");
                        }
                        else if (ans == "move up" || ans == "move right")
                        {
                            position = move(position, ans, map);
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("Nothing to take here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
            }
            else Console.WriteLine("You're in the end of the hallway.");
        }

        static void staircase(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("hide", flags))
            {
                items = item(flags, index(flags), "stairFirst");
                Console.WriteLine("As you walked to the staircase, the faint mumble of someone having a conversation \n" +
                    "on the second floor reached your ears. An ensemble of voices that spoke to stories hidden and yearning to be discovered.");
                Console.ReadKey();
                Console.WriteLine("\nIt's probably best not to go up for now.");
                while (true)
                {
                    string ans = commands(map, position, flags, items, player);
                    if (ans.StartsWith("move"))
                    {
                        if (ans == "move up" || ans == "move forward")
                        {
                            Console.WriteLine("It's probably best not to go upstairs...");
                        }
                        else if (ans == "move down" || ans == "move backward")
                        {
                            position[1] -= 1;
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("You can only either move up or move down in the staircase!");
                        }
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("Nothing to take here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
            }
            else
            {
                Console.WriteLine("This is the staircase to the 2nd floor. Proceed to go up or stay at the hallway? [move up / move down]");
                while (true)
                {
                    string ans = commands(map, position, flags, items, player);
                    if (ans.StartsWith("move"))
                    {
                        if (ans == "move up" || ans == "move forward")
                        {
                            position[1] += 1;
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else if (ans == "move down" || ans == "move backward")
                        {
                            position[1] -= 1;
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("You can only either move up or move down in the staircase!");
                        }
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("Nothing to take here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
            }
        }

        static void secondfloor(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
                Console.WriteLine("You're now in 2nd floor.");
        }

        static void patientward(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in Patient Ward.\n");
        }

        static void director(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in Director's Office.\n");
        }

        static void privateward(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            Console.Write("You are in Private Ward.\n");
        }

        static void lobby(string[,] map, int[] position, string[] flags, string[] items, string player)
        {
            if (!isItem("hide", flags)){
                flags = item(flags, index(flags), "hide");

                if (!isItem("stairFirst", flags))
                {
                    Console.WriteLine("As you entered the lobby, the faint mumble of someone having a conversation down on the second floor \n" +
                   "reached your ears. An ensemble of voices that spoke to stories hidden and yearning to be discovered.");
                }
                else
                {
                    Console.Write("Those people upstairs... ");
                }
               
                Console.Write("Could they be the reason behind the unsolved mystery cases?\n" +
                    "[1] Definitely, yes.\n" +
                    "[2] Definitely, no.\n" +
                    "[3] Too early to make decisions.\n");
                while (true)
                {
                    string decision = action();
                    if (decision == "1" || decision == "2" || decision == "3")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("“Goddamn Nyoco! I swear if someone caught us here, we’re both dead. Just like \n" +
                            "those people in the Morgue, dead. Dead Nyoco, dead.”\r\n");
                        Console.ReadKey();
                        Console.WriteLine("Nyoco's friend nervously whispered, their words strained and anxious. Panic set in as they feared \n" +
                            "being caught, each cautious syllable amplifying the tension of the moment. The air crackled with apprehension, \n" +
                            "enveloping the duo in a web of uncertainty.\n");
                        Console.ReadKey();
                        Console.WriteLine("“Calm down! Go check the security room and outside while I’ll check the Laboratory Room and the Storage Room.”\r\n");
                        Console.ReadKey();
                        Console.WriteLine("Nyoco's voice pierced the tense air, her words laden with an intensity that reverberated through the silence.\r\n");
                        Console.ReadKey();
                        Console.WriteLine("THEY ARE GOING DOWNSTAIRS, YOU MIGHT GET CAUGHT!\n" +
                            "[1] HIDE IN THE MORGUE\n" +
                            "[2] LEAVE THE BUILDING, NOW!");
                        while (true)
                        {
                            string decide = action();
                            if (decide == "1")
                            {
                                Console.Clear();

                                header(flags, position, map);
                                while (true) { 
                                    for(int i=0; i < 8; i++)
                                    {
                                        for(int j=0; j<15; j++)
                                        {
                                            Console.Write("HIDE  \t");
                                        }
                                        Console.WriteLine("\n");
                                    }

                                    Console.WriteLine($"{player.ToUpper()}, YOU NEED TO HIDE NOW! MOVE UP TO THE MORGUE!");
                                    Console.Write("> ");
                                    string hide = Console.ReadLine();
                                    if (hide == "move up" || hide == "move forward")
                                    {
                                        position[0] -= 1;
                                        prompt(position, map, flags, player, items);
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        header(flags, position, map);
                                    }
                                }
                                break;
                            }
                            else if (decide == "2")
                            {
                                Console.WriteLine("You escaped through the windows of the lobby. You chickened out and was unsuccessful in finding out the truth.");
                                break;
                            }
                            else Console.WriteLine("Invalid answer, AGAIN.");
                        }
                        break;
                    }
                    else Console.WriteLine("Invalid answer, try again.");
                }
                while (true)
                {
                    string ans = commands(map, position, flags, items, player);
                    if (ans.StartsWith("move"))
                    {
                        if (ans == "move left")
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (ans == "move up" || ans == "move right" || ans == "move down")
                        {
                            position = move(position, ans, map);
                            prompt(position, map, flags, player, items);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("Nothing to take here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("Nothing to check here.");
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
            }
            else
            {
                Console.WriteLine("It seems the lobby is clear.");
            }

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
