using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PulseOfDeceit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            playGame();
        }

        static string[] flags = { "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder" };
        static string[] items = { "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder", "holder" };
        static void playGame()
        {
            int[] position = { 6, 3 };
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
            while (true)
            {
                prompt(position, map, player);
                string ans = commands(map, position, player);
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
                    Console.ReadKey(true);
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
        static void item(int index, string replace)
        {
            items[index] = replace;
        }

        static void flag(int index, string replace)
        {
            flags[index] = replace;
        }


        //Item Breaker
        static void remove(string remove)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == remove)
                {
                    items[i] = "holder";
                    break;
                }
            }
        }

        //A function for checking the input for various commands.
        static string commands(string[,] map, int[] position, string player)
        {
            while (true)
            {
                //availableDirection(position, map);
                Console.WriteLine();
                string input = action();
                if (input.StartsWith("move") ||
                    input.StartsWith("check") ||
                    input.StartsWith("take") ||
                    input.StartsWith("go") ||
                    input.StartsWith("inspect") ||
                    input == "CS 103")
                    return input;
                else if (input == "where")
                {
                    Console.WriteLine($"You are currently at {map[position[0], position[1]]}");
                }
                else if (input == "clear")
                {
                    Console.Clear();
                    header(flags, position, map);
                    prompt(position, map, player);

                }
                else if (input.StartsWith("use"))
                {
                    if (input == "use map")
                    {
                        if (isItem("displayMap", flags) && isItem("Map", items)) Console.WriteLine("Your map is already displayed!");
                        else if (!isItem("displayMap", flags) && !isItem("Map", items)) Console.WriteLine("You don't have a map to display.");
                        else
                        {
                            Console.WriteLine("Map is now displayed.");
                            flag(index(flags), "displayMap");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                    }


                }
                else if (input == "items" || input == "item")
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
> inspect - inspect the room you are in.
");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");

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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"\\o/\t");
                        Console.ResetColor();
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
        static void prompt(int[] position, string[,] map, string player)
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
                        righttree(map, position, player);
                        break;
                    }
                case "Tree Left":
                    {
                        lefttree(map, position, player);
                        break;
                    }

                case "Outside":
                    {
                        outside(map, position, player);
                        break;
                    }

                case "Asylum":
                    {
                        asylum(map, position, player);
                        break;
                    }

                case "Ground Basement":
                    {
                        basement1(map, position, player);
                        break;
                    }

                case "Upper Basement":
                    {
                        basement2(map, position, player);
                        break;
                    }

                case "Lower Basement":
                    {
                        basement3(map, position, player);
                        break;
                    }

                case "Storage Room":
                    {
                        storageroom(map, position, player);
                        break;
                    }
                case "Laboratory":
                    {
                        laboratory(map, position, player);
                        break;
                    }
                case "Hallway1":
                    {
                        hallway1(map, position, player);
                        break;
                    }

                case "Hallway2":
                    {
                        hallway2(map, position, player);
                        break;
                    }

                case "Staircase":
                    {
                        staircase(map, position, player);
                        break;
                    }

                case "Second Floor":
                    {

                        secondfloor(map, position, player);
                        break;
                    }

                case "Patient Ward":
                    {
                        patientward(map, position, player);
                        break;
                    }

                case "Director's Office":
                    {
                        director(map, position, player);
                        break;
                    }

                case "Private Ward":
                    {
                        privateward(map, position, player);
                        break;
                    }

                case "Lobby":
                    {
                        lobby(map, position, player);
                        break;
                    }

                case "Security Room":
                    {
                        security(map, position, player);
                        break;
                    }

                case "Morgue":
                    {
                        morgue(map, position, player);
                        break;
                    }


            }

        }

        //Checker if an item is in the array.
        static bool isItem(string match, string[] array)
        {
            foreach (string x in array)
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

            if ((command == "move forward" || command == "move up" || command == "go up" || command == "go forward") && map[position[0] - 1, position[1]] != "0")
            {
                currPosition[0] -= 1;
            }
            else if ((command == "move back" || command == "move down" || command == "move backward" || command == "go back" || command == "go down" || command == "go backward") && map[position[0] + 1, position[1]] != "0")
            {
                currPosition[0] += 1;
            }
            else if ((command == "move left" || command == "go left") && map[position[0], position[1] - 1] != "0")
            {
                currPosition[1] -= 1;
            }
            else if ((command == "move right" || command == "go right") && map[position[0], position[1] + 1] != "0")
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
            Console.WriteLine("\nAmidst the Cold War paranoia, the U.S. government initiated Project Cerebicus in 1953 to counteract the spread \n" +
                "of communism. This covert project delved into mental health, hypnosis, and brain function. Test subjects, often drawn \n" +
                "from marginalized groups, were subjected to disturbing experiments involving drugs and invasive procedures. \n" +
                "The ethical boundaries blurred, as the project aimed to understand and manipulate the human mind. \r\n");
            Console.ReadKey(true);
            Console.WriteLine("Once thriving with Soviet-era scientific fervor, the research facility now sits abandoned, its halls silent \n" +
                "since the Union's fall. With no rivals, it stands as a forgotten relic of bygone innovation.\n");
            Console.ReadKey(true);
            Console.WriteLine("This is where the horror starts, terrible things happening there, series of disappearance and murder are rumored \n" +
                "to be lingering in that place. People are getting hurt yet the government is sitting on their throne making money.\n");
            Console.ReadKey(true);
            Console.WriteLine("It needs to be exposed.\n");
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
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
            Console.ReadKey(true);
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
> inspect - inspect the room you are in.
");

            Console.Write("Press any key continue...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static void header(string[] array, int[] position, string[,] map)
        {
            if (isItem("displayMap", array))
            {
                mapping(map, position);
            }
            else
            {
                pod();
            }
        }

        static void storageroom(string[,] map, int[] position, string player)
        {
            if (!isItem("storage", flags))
            {
                Console.WriteLine("\"Oh, this room is open. What can I find here?\" Anastacia said curiously.\n");
                flag(index(flags), "storage");

                Console.ReadKey(true);
                Console.WriteLine("HTE STRAOGE OROM, TRPAPED NI YACDE, FETAURDE BRKONE SWWINDO, A YTMUS LLMSE, DAN FROGTTEN SERLIC. \n" +
                   "A BOOLD-TENWRTI MSSEAEG, \"RUN IF YOU WANT TO LIVE.\" DAORDNE ETH LLAW. HTE ERIEE CILENSE THINED TA NUSTTLEING \n" +
                   "CESRETS, WTHI LOCASSIONA RSUTELS DAN TANTDIS ECHOSE, RINGFFERO A CHILNGIL WRNAING OT THESO HOW REDAD OT ETNER.");

                Console.ReadKey(true);
                item(index(items), "Flashlight");
                Console.WriteLine("\n\"Despite the unflattering confines of this room, at least they have a [Flashlight] as my only hope, \n" +
                    "lighting up my surroundings sufficiently to let me see comple– who is that!?\"");
                Console.WriteLine("\nYou got a [Flashlight].\n");

                Console.ReadKey(true);
                Console.WriteLine("The wind intensified crawling through the hallway, and amplifying the unsettling atmosphere. \n" +
                    "Doors were abruptly and violently shut by an unknown power, revealing after the faint echo of receding footsteps \n" +
                    "resounding the hallway, inciting a halt in Anastacia’s spoken expression.\r\n");

                Console.ReadKey(true);
                Console.WriteLine("You can't leave the room, what would you like to do?\n" +
                    "[1] Kick the door open or\n" +
                    "[2] Use the crowbar to break the doorknob.");
                while (true)
                {
                    string ans = action();
                    if (ans == "1")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("\"Frick! That hurt and still it won't budge?\"\n");
                        Console.WriteLine("Anastacia gruntly shouted as she hurt herself trying to break the door. \n" +
                            "Now resorting to her next option.\n");
                        Console.ReadKey(true);
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
                Console.ReadKey(true);
                Console.WriteLine("With a hint of optimism, Anastacia uttered. Her tone was covered with tension as she \n" +
                    "skillfully handled the crowbar in an attempt to remove the doorknob—a tool that she used to break in, \n" +
                    "which caused it to smash to smithereens. Fortunately, the doorknob falls to bits.\r\n");
                Console.WriteLine("Your [Crowbar] broked.\n");
                remove("Crowbar");

                Console.ReadKey(true);
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
                else
                {
                    Console.WriteLine("HTE STRAOGE OROM, TRPAPED NI YACDE, FETAURDE BRKONE SWWINDO, A YTMUS LLMSE, DAN FROGTTEN SERLIC. \n" +
                   "A BOOLD-TENWRTI MSSEAEG, \"RUN IF YOU WANT TO LIVE.\" DAORDNE ETH LLAW. HTE ERIEE CILENSE THINED TA NUSTTLEING \n" +
                   "CESRETS, WTHI LOCASSIONA RSUTELS DAN TANTDIS ECHOSE, RINGFFERO A CHILNGIL WRNAING OT THESO HOW REDAD OT ETNER.");
                }



            }

            while (true)
            {
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if ((moveValidator(ans, "down")) || moveValidator(ans, "up") || moveValidator(ans, "right"))
                    {
                        Console.WriteLine("You can't go here. This is just a wall.");
                    }
                    else if (moveValidator(ans, "left"))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, player);
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
                else if (ans.StartsWith("inspect"))
                {
                    Console.WriteLine("Nothing to inspect here.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }
        }

        static void asylum(string[,] map, int[] position, string player)
        {
            if (!isItem("asylum", flags))
            {
                Console.WriteLine("You used your crowbar to break the chains and is now inside the asylum.\n");
                flag(index(flags), "asylum");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"READ CAREFULLY, {player.ToUpper()}.");
                Console.ResetColor();
                Console.ReadKey(true);
                Console.WriteLine("\n\"This place is truly disordered, I can't even see a thing.\"");
                Console.ReadKey(true);
                Console.WriteLine("\nAnastacia said, breaking the terrifying silence as her voice trailed\n" +
                    "reflecting the unease that lingered over the room.\n");
            }

            if (!isItem("Flashlight", items))
            {
                Console.WriteLine("LKOSO LIEK TI SI YVER DRAK IN HREE. UYO MTIHG WNTA TO FNDI A FLSHALGHIT TISRF.");

                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if ((moveValidator(ans, "up")))
                        {
                            Console.WriteLine("HTE HLALWYA SI ERYV DKAR. TI SI TESB OT NDIF MSHINGTEO HATT ANC ANUMLITE HESTE LLAWS.");
                        }
                        else if (moveValidator(ans, "down"))
                        {
                            Console.WriteLine("That's outside, there's nothing to do there. You should start inspecting the inside.");
                        }
                        else if (moveValidator(ans, "left"))
                        {
                            Console.WriteLine("OYU CNA TON TERNE HET LBAROTRYAO OROM TYE UNLTI UOY DNFI YROUESLF A KYACDRE.");
                        }
                        else if (moveValidator(ans, "right"))
                        {
                            position = move(position, ans, map);
                            Console.Clear();
                            prompt(position, map, player);
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
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
            else
            {
                Console.WriteLine("You are back at the main entrance of the asylum.");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {

                        if (moveValidator(ans, "down"))
                        {
                            Console.WriteLine("That's outside, there's nothing to do there. You should start inspecting the inside.");
                        }
                        else if (moveValidator(ans, "left"))
                        {
                            if (!isItem("Keycard", items)) Console.WriteLine("You can not enter the laboratory room yet until you find yourself a keycard.");
                            else
                            {
                                position = move(position, ans, map);
                                Console.Clear();
                                prompt(position, map, player);
                                break;
                            }
                        }
                        else if (moveValidator(ans, "right") || moveValidator(ans, "up"))
                        {
                            position = move(position, ans, map);
                            Console.Clear();
                            prompt(position, map, player);
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
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }



        }

        static void laboratory(string[,] map, int[] position, string player)
        {
            Console.WriteLine("You find yourself in the Laboratory Room, the air thick with the unsettling ambiance of its hospital past. \n" +
                "Eerie remnants of laboratory equipments surround you.\n" +
                "\nTry inspecting the area.");
            while (true)
            {
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if (moveValidator(ans, "right"))
                    {
                        Console.WriteLine("That's not a good idea, you're very close in finishing the case.");
                    }
                    else if (moveValidator(ans, "up") || moveValidator(ans, "down") || moveValidator(ans, "left"))
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("inspect"))
                {
                    Console.WriteLine("As Anastacia surveyed the room, she found [Laboratory Equipment] around the room.\n" +
                        "However, her attention was drawn to a [Secret Door] cleverly concealed within the walls. ");
                }
                else if (ans.StartsWith("check"))
                {
                    if (ans == "check laboratory equipment" || ans == "check equipment" || ans == "check laboratory")
                    {
                        Console.WriteLine("You spot an old surgical table with intact leather straps, faded jars holding mysterious \n" +
                            "substances, and a metal tray bearing neatly arranged, rust-covered surgical tools");
                    }
                    else if (ans == "check secret door" || ans == "check door")
                    {
                        Console.WriteLine("Intrigued, she opened it, revealing a descending staircase leading to the basement.\n");
                        int count = 0;
                        while (true)
                        {
                            Console.WriteLine("[1] Continue exploring the Laboratory Room or\n" +
                                "[2] Descend in the basement");
                            string decide = action();
                            if (decide == "1" & count == 0)
                            {
                                Console.Clear();
                                header(flags, position, map);
                                Console.WriteLine("You spot an old surgical table with intact leather straps, faded jars holding mysterious \n" +
                            "substances, and a metal tray bearing neatly arranged, rust-covered surgical tools.\n");
                                count++;
                            }
                            else if (decide == "1" & count > 0)
                            {
                                Console.Clear();
                                header(flags, position, map);
                                Console.WriteLine("You should probably go down. There's nothing here anymore.\n");
                            }
                            else if (decide == "2")
                            {
                                position[1] -= 1;
                                prompt(position, map, player);
                                break;
                            }
                            else Console.WriteLine("Invalid action, try again.");
                        }

                    }
                    else Console.WriteLine("That can't be checked.");
                }
                else if (ans.StartsWith("take"))
                {
                    Console.WriteLine("That can't be taken.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }
        }

        static void outside(string[,] map, int[] position, string player)
        {
            if (!isItem("outside", flags))
            {
                Console.WriteLine("You entered the gate, a huge building welcomes you.\n" +
                    "As you are standing outside the asylum, feeling the chilling wind whisper through the overgrown vines \n" +
                    "and dilapidated windows, carrying a sense of foreboding, you feel the weight of the impending truth \n" +
                    "pressing down on your shoulders.\n");

                Console.WriteLine("Would you like to inspect your surroundings? [Yes] or [No] ?\n");
                while (true)
                {
                    string answer = action();
                    if (answer.ToLower() == "yes" || answer.ToLower() == "y")
                    {
                        Console.Clear();
                        pod();
                        Console.WriteLine("You are outside the asylum with a door blocking your path; as it is surrounded by chains, rejecting the curiosity. " +
                            "\nYou will need a crowbar to break the chains to conceal the mysterious secrets. " +
                            "\nAs you inspect your surroundings, you notice that you are surrounded by trees. " +
                            "\nThe tree on your left stands like a sentinel, its gnarled branches reaching out for secrets buried in the soil. " +
                            "\nTo your right, another tree glows mysteriously, its leaves whispering a way in a haunting melody.\n");
                        Console.ReadKey(true);
                        Console.WriteLine("\"Why am I entangled in this labyrinth of mysteries?  It was a naive move to go " +
                            "\nthrough these gloomy depths in the first place.\"\r\n");
                        Console.ReadKey(true);
                        Console.WriteLine("Anastacia said and let out a deep sigh, wondering why she was pursuing the unknown so eagerly.\n");
                        Console.ReadKey(true);
                        Console.WriteLine($"Could these trees be something? Take a push to discover, the floor is yours, {player}, to unravel behind" +
                            $"\nthe creaking leaves and shady branches.");
                        break;
                    }
                    else if (answer.ToLower() == "no" || answer.ToLower() == "n")
                    {
                        Console.Clear();
                        pod();
                        Console.WriteLine("You are outside the asylum with a door blocking your path; as it is surrounded by \n" +
                            "chains, rejecting the curiosity. You will need a crowbar to break the chains to conceal the mysterious secrets.");
                        break;
                    }
                    else Console.WriteLine("Invalid answer, try again.");
                }
                flag(index(flags), "outside");
            }
            else
            {
                Console.WriteLine("You are outside the asylum with a door blocking your path; as it is surrounded by \n" +
                            "chains, rejecting the curiosity. You will need a crowbar to break the chains to conceal the mysterious secrets.");
            }

            while (true)
            {
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if ((moveValidator(ans, "up")))
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
                            prompt(position, map, player);
                            break;
                        }
                    }

                    else if (moveValidator(ans, "down"))
                    {
                        Console.WriteLine("That's the gate you went into, you cannot go there.");
                    }
                    else if ((moveValidator(ans, "up")) && isItem("Map", items) && isItem("Crowbar", items))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, player);
                        break;
                    }
                    else if (moveValidator(ans, "left") || moveValidator(ans, "right"))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, player);
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
                    Console.WriteLine("That can't be taken.");
                }
                else if (ans.StartsWith("inspect"))
                {
                    Console.WriteLine("Nothing to inspect here.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }
        }

        static void righttree(string[,] map, int[] position, string player)
        {
            if (!isItem("Crowbar", items) && !isItem("Map", items))
            {
                Console.WriteLine("As you approach the dark, oak tree, you notice a rusted metal object lying flat \n" +
                "at the base of the trunk. You approach and pick up the metal object. A [Crowbar]?\n");
                Console.ReadKey(true);
                Console.WriteLine("You got a [Crowbar]!\n");
                Console.ReadKey(true);
                Console.WriteLine("Right, the key will be a crowbar—to release the chains that are blocking the main doors' entry.\n");
                Console.ReadKey(true);
                Console.WriteLine("\"Well, I guess, this could be a weapon, just in case.\" Anastacia said.\n");
                Console.ReadKey(true);
                Console.WriteLine("Amidst the huge void, how could you know your location without the indispensable help of a map?");
                item(index(items), "Crowbar");
            }
            else if (!isItem("Crowbar", items) && isItem("Map", items))
            {
                Console.WriteLine("As you approach the dark, oak tree, you notice a rusted metal object lying flat \n" +
                "at the base of the trunk. You approach and pick up the metal object. A [Crowbar]?\n");
                Console.ReadKey(true);
                Console.WriteLine("You got a [Crowbar]!\n");
                Console.ReadKey(true);
                Console.WriteLine("Right, the key will be a crowbar—to release the chains that are blocking the main doors' entry.");
                item(index(items), "Crowbar");
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
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if (moveValidator(ans, "up"))
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else if (moveValidator(ans, "down") || moveValidator(ans, "right"))
                    {
                        Console.WriteLine("There's nothing to do out there, it is best not to go there.");
                    }
                    else if (moveValidator(ans, "left"))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, player);
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
                    Console.WriteLine("That can't be taken.");
                }
                else if (ans.StartsWith("inspect"))
                {
                    Console.WriteLine("Nothing to inspect here.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }
        }

        static void lefttree(string[,] map, int[] position, string player)
        {
            if (!isItem("Map", items) && !isItem("Crowbar", items))
            {
                Console.WriteLine("An old, eerie tree with dead leaves, welcomes you with a cozy atmosphere, " +
               "\nthe leaves dancing in the breeze, as you find a piece of paper tucked in between the trunk and a branch. \n");
                Console.ReadKey(true);
                Console.WriteLine("Great start, you found a [Map].");
                item(index(items), "Map");

                Console.ReadKey(true);
                while (true)
                {
                    Console.Write("\nWould you like to display your map? [Yes] or [No]: ");
                    string displayMap = Console.ReadLine().ToLower();
                    if (displayMap == "yes" || displayMap == "y")
                    {
                        flag(index(flags), "displayMap");
                        Console.WriteLine("\n\"A map, huh?\" Anastacia said.");
                        Console.ReadKey(true);
                        Console.Clear();
                        prompt(position, map, player);
                        break;
                    }
                    else if (displayMap == "no" || displayMap == "n")
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
                Console.ReadKey(true);
                Console.WriteLine("Great, you found the [Map].");
                item(index(items), "Map");

                Console.ReadKey(true);
                while (true)
                {
                    Console.Write("\nThis map can help you keep track of where you are.\nWould you like to display your map? [Yes] or [No]: ");
                    string displayMap = Console.ReadLine().ToLower();
                    if (displayMap == "yes")
                    {
                        flag(index(flags), "displayMap");
                        Console.WriteLine("\n\"A map, huh?\" Anastacia said.");
                        Console.ReadKey(true);
                        Console.Clear();
                        prompt(position, map, player);
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
            else if (isItem("Map", items)) Console.WriteLine("An old, eerie tree with dead leves, welcomes you with a cozy atmosphere, " +
                "\nand the leaves dancing in the breeze.");

            while (true)
            {
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if (moveValidator(ans, "up") || moveValidator(ans, "left"))
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else if (moveValidator(ans, "down"))
                    {
                        Console.WriteLine("There's nothing to do out there, it is best not to go there.");
                    }
                    else if (moveValidator(ans, "right"))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, player);
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
                    Console.WriteLine("That can't be taken.");
                }
                else if (ans.StartsWith("inspect"))
                {
                    Console.WriteLine("Nothing to inspect here.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }



        }

        static void basement1(string[,] map, int[] position, string player)
        {
            Console.WriteLine("Upon reaching the Ground basement, she discovered a storage room for medical equipment. \n" +
                "The shelves were stocked with ominous tools—an unsettling array of used syringes with blood, \n" +
                "oxygen tanks, and other macabre remnants.\n");
            Console.ReadKey(true);
            Console.WriteLine("You choose to descend into the Ground basement. Upon entering, you discover that it is a room for medical equipment.\n");
            Console.ReadKey(true);

            int count = 0;
            while (true)
            {
                Console.WriteLine("How will you proceed?\n" +
                "[1] Search the storage room\n" +
                "[2] Move to the Upper Basement\r\n");
                string decide = action();
                if (decide == "1" && count == 0)
                {
                    Console.Clear();
                    header(flags, position, map);
                    Console.WriteLine("Shelves are stocked with ominous tools—used syringes with blood, oxygen tanks, and other macabre remnants.\n");
                    count++;
                }
                else if (decide == "1" && count > 0)
                {
                    Console.Clear();
                    header(flags, position, map);
                    Console.WriteLine("There seems to be nothing here, you should probably move down.\n");
                }
                else if (decide == "2")
                {
                    position[0] += 1;
                    prompt(position, map, player);
                    break;
                }
                else Console.WriteLine("Invalid action, try again.");
            }
        }

        static void basement2(string[,] map, int[] position, string player)
        {
            Console.WriteLine("Moving to the Upper basement, Anastacia found walls adorned with newspaper clippings, revealing a \n" +
                "gruesome narrative. Among the chilling discoveries, a nurse's uniform lay scattered, embroidered with \n" +
                "the initials A.D.C. The pieces of the puzzle fell into place—a shocking realization that she, Anastacia D. Cruz, \n" +
                "was not merely a caregiver but the mastermind behind the series of mysterious murders in the asylum.\r\n");
            Console.ReadKey(true);
            Console.WriteLine("Undeterred, you choose to explore the Upper basement. The walls are adorned \n" +
                "with newspaper clippings, revealing a gruesome narrative. \r\n");
            Console.ReadKey(true);
            while (true)
            {
                Console.WriteLine("What will you do next?\n" +
                "[1] Check newspaper clippings" +
                "\n[2] Continue exploring the Upper Basement" +
                "\n[3] Move to the Lower Basement");
                string ans = action();
                if (ans == "1")
                {
                    Console.Clear();
                    header(flags, position, map);
                    Console.WriteLine("You saw detailing unsolved murders eerily.\n");
                }
                else if (ans == "2")
                {
                    Console.Clear();
                    header(flags, position, map);
                    Console.WriteLine("You decide to inspect the nurse's uniform. The initials match your name—Anastacia D. Cruz. \n" +
                        "A cold shiver runs down your spine as the shocking realization sets in.\n");
                }
                else if (ans == "3")
                {
                    position[0] += 1;
                    prompt(position, map, player);
                    break;
                }
                else Console.WriteLine("Invalid action, try again.");
            }

        }

        static void basement3(string[,] map, int[] position, string player)
        {
            Console.WriteLine("Descending further to the Lower basement, Anastacia entered a room with open lights, immaculately clean, and \n" +
                "meticulously designed as a personal space. A bed sat against one wall, giving the room an uncanny homeliness.");
            Console.ReadKey(true);
            Console.WriteLine("\nYet, her shock reached a crescendo as she noticed a tarpaulin on the wall — an Employee of the Day \n" +
                "display featuring her own photo and name.\r\n");
            Console.ReadKey(true);
            Console.WriteLine("You choose to explore the Lower basement. The room feels oddly personal—a clean, well-designed space with a \n" +
                "bed. A tarpaulin on the wall reveals an 'Employee of the Day' display featuring your photo and name.\r\n");
            while (true)
            {
                Console.WriteLine("How do you react?\n" +
                    "[1] Piece together the puzzle\n" +
                    "[2] Leave the Lower basement");
                string ans = action();
                if (ans == "1")
                {
                    Console.Clear();
                    header(flags, position, map);
                    Console.WriteLine("Overwhelmed by the shocking revelations, Anastacia stumbles out of the Laboratory Room in a panic. \n" +
                        "The weight of the truth, that she is the perpetrator behind all the asylum killings, hangs heavy in the air.\n");
                    Console.ReadKey(true);
                    Console.WriteLine("The once-familiar corridors now feel like a labyrinth of guilt and horror. Her heart races, and \n" +
                        "every creaking floorboard amplifies the echoes of her disbelief.\r\n");
                    Console.ReadKey(true);
                    Console.WriteLine("“How is this all possible? Why can’t I remember not even a single memory?”\r\n");
                    Console.ReadKey(true);
                    Console.WriteLine("Anastacia said as she stumbled through the dimly lit corridor, her mind reeling with the weight of the \n" +
                        "unthinkable truth. Each step felt like an eternity, the shadows dancing menacingly around her.\r\n");
                    break;
                }
                else if (ans == "2")
                {
                    Console.WriteLine("You cannot leave the lower basement, if you leave the lower basement, the story will end.");
                    while (true)
                    {
                        Console.Write("\nWould you like to leave the basement? [Yes] or [No]: ");
                        string leave = Console.ReadLine().ToLower();
                        if (leave == "yes" || leave == "y")
                        {
                            Console.WriteLine("You were afraid of finding out the whole truth. You backed off without accepting what you just saw. The story ends here.\n");
                            Console.ReadKey(true);
                            Console.Write("Thank you for playing Pulse of Deceit! Would you like to play again? [Yes] or [No]: ");
                            string answer = Console.ReadLine().ToLower();
                            if (answer == "yes" || answer == "y")
                            {
                                Restart();
                            }
                            else if (answer == "no" || answer == "n")
                            {
                                Console.WriteLine("Thank you for playing Pulse of Deceit!");
                                Console.ReadKey(true);
                                Environment.Exit(0);
                                break;
                            }
                        }
                        else if (leave == "no" || leave == "n")
                        {
                            Console.Clear();
                            header(flags, position, map);
                            break;
                        }
                        else Console.WriteLine("Invalid action, try again.");
                    }
                }
                else Console.WriteLine("Invalid action, try again.");
            }

            Console.Clear();
            header(flags, position, map);
            Console.WriteLine("As the pieces fell into a disconcerting pattern, Anastacia found herself at the center of the asylum's dark secrets.\n" +
                "All the evidence pointed to her; the eerie occurrences, the mysterious murders, and even the room she stood in — \n" +
                "all connected to her.\n");
            Console.ReadKey(true);
            Console.WriteLine("Shocked and bewildered, she grappled with the realization that she was the one living in this room, unable to comprehend" +
                "\nthe sinister role she played in the unfolding horror within the asylum. Caught in a web of her own making, \n" +
                "Anastacia faced a chilling dilemma with no clear path forward.\r\n");
            Console.ReadKey(true);
            Console.WriteLine("As you piece together the puzzle, a shocking realization sets in.\n");
            Console.ReadKey(true);
            Console.WriteLine("You are the mastermind behind the mysterious murders, and everything in the asylum connects to you.");
            Console.ReadKey(true);
            Console.WriteLine("\nThe room you stand in feels like your own," +
                "\nleaving you in a state of shock."); ;
            Console.ReadKey(true);
            Console.ForegroundColor = ConsoleColor.Red;
            TypeWithAnimation("\nCHAPTERS CLOSED.");
            Console.ReadKey(true);
            TypeWithAnimation("\nWE HOPE THIS FINALLY HELPS ALL OF US MOVE FORWARD. WE WILL NOT BE ENTERTAINING QUESTIONS REGARDING THIS ANYMORE.\n");
            Console.ReadKey(true);
            TypeWithAnimation("THANK YOU FOR UNDERSTANDING.\n");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("That's the end of Pulse of Deceit! Would you like to play again? [Yes] or [No]: ");
                Console.ResetColor();
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "y")
                {
                    Restart();
                }
                else if (answer == "no" || answer == "n")
                {
                    Console.WriteLine("\nThank you for playing Pulse of Deceit!");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                    break;
                }
            }
        }

        static void hallway1(string[,] map, int[] position, string player)
        {
            if (!isItem("hide", flags))
            {
                Console.WriteLine("You walked through the long hallway. It seems like you need to continue moving forward.");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "right") || moveValidator(ans, "left"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "up") || moveValidator(ans, "down"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                        Console.WriteLine("That can't be taken.");
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
            else
            {
                Console.WriteLine("You are now in the hallway. You can either move forward or move backward.");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "right") || moveValidator(ans, "left"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "up") || moveValidator(ans, "down"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                        Console.WriteLine("That can't be taken.");
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
        }
        static void hallway2(string[,] map, int[] position, string player)
        {
            if (!isItem("hide", flags))
            {
                Console.WriteLine("You finished the end of the walkway. There is a lobby in front of you, and a staircase on your right");
                Console.WriteLine("\nWhere would you like to go?");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "down"))
                        {
                            Console.WriteLine("That is where you were, it's probably a waste of time to go back there for now.");
                        }
                        else if (moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                        Console.WriteLine("That can't be taken.");
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
            else if (!isItem("Keycard", items))
            {
                Console.WriteLine("You are at the end of the hallway. There is a lobby in front of you, and a staircase on your right");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "down"))
                        {
                            Console.WriteLine("That is where you were, it's probably a waste of time to go back there for now.");
                        }
                        else if (moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
            else
            {
                Console.WriteLine("You are at the end of the hallway. There is a lobby in front of you, and a staircase on your right");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "up") || moveValidator(ans, "right") || moveValidator(ans, "down"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }

            }

        }

        static void staircase(string[,] map, int[] position, string player)
        {
            if (!isItem("hide", flags))
            {
                flag(index(flags), "stairFirst");
                Console.WriteLine("As you walked to the staircase, the faint mumble of someone having a conversation \n" +
                    "on the second floor reached your ears. An ensemble of voices that spoke to stories hidden and yearning to be discovered.");
                Console.ReadKey(true);
                Console.WriteLine("\nIt's probably best not to go up for now.");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "up"))
                        {
                            Console.WriteLine("It's probably best not to go upstairs...");
                        }
                        else if (moveValidator(ans, "down"))
                        {
                            position[1] -= 1;
                            prompt(position, map, player);
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
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "up"))
                        {
                            position[1] += 1;
                            prompt(position, map, player);
                            break;
                        }
                        else if (moveValidator(ans, "down"))
                        {
                            position[1] -= 1;
                            prompt(position, map, player);
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
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
        }

        static void secondfloor(string[,] map, int[] position, string player)
        {
            Console.WriteLine("As Anastacia ascended the staircase, she could feel the weight of the asylum's history \n" +
            "pressing on her shoulders. The creaking steps echoed in the silence, and the flickering lights \n" +
            "above cast eerie shadows on the walls. The hallway on the second floor seemed to stretch endlessly, \n" +
            "each door holding a potential secret.");

            while (true)
            {
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if (moveValidator(ans, "up"))
                    {
                        position[0] -= 1;
                        prompt(position, map, player);
                        break;
                    }
                    else if (moveValidator(ans, "down"))
                    {
                        position[0] += 1;
                        prompt(position, map, player);
                        break;
                    }
                    else if (moveValidator(ans, "right") && isItem("Office Key", items))
                    {
                        position[1] += 1;
                        prompt(position, map, player);
                        break;
                    }
                    else if (moveValidator(ans, "right") && !isItem("Office Key", items))
                    {
                        Console.WriteLine("The keys you currently have don't match with the door here. Find the key that fits.");
                    }
                    else if (moveValidator(ans, "left"))
                    {
                        position[1] -= 1;
                        prompt(position, map, player);
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
                else if (ans.StartsWith("inspect"))
                {
                    Console.WriteLine("Nothing to inspect here.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }


        }

        static void patientward(string[,] map, int[] position, string player)
        {
            if (!isItem("Office Key", items))
            {
                Console.WriteLine("The Patient Ward door was slightly ajar, and Anastacia cautiously pushed it open. The room \n" +
                    "was dimly lit, with beds lined up along the walls. As she explored, she noticed a [Office Key] hanging \n" +
                    "on a hook near the nurse's station.");

                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left") || moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "down"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        if (!isItem("Office Key", items)) Console.WriteLine("As she explored, she noticed a [Office Key] hanging on a hook near the nurse's station.");
                        else Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        if (ans == "take key" || ans == "take office key")
                        {
                            Console.WriteLine("You took the [Office Key]");
                            Console.WriteLine("Anastacia thought, \"This might be the key to the Director's Room. I should continue searching for clues.\"");
                            item(index(items), "Office Key");
                        }
                        else Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }

            }
            else
            {
                Console.WriteLine("The Patient Ward door was slightly ajar, and Anastacia cautiously pushed it open. The room \n" +
                    "was dimly lit, with beds lined up along the walls.");
            }
        }

        static void director(string[,] map, int[] position, string player)
        {
            if (!isItem("Security Key", items))
            {
                Console.WriteLine("The Director's Room door stood imposingly at the end of the hallway. As Anastacia approached, she found it locked.\n" +
                    "She glanced at the key she found in the Private Ward and decided to use it.\n");
                Console.ReadKey(true);
                Console.WriteLine("The key turned smoothly in the lock, and the door creaked open. Anastacia entered the room, \n" +
                    "the air heavy with anticipation. The room was adorned with old furniture, dusty books, \n" +
                    "and a [Framed Photo] on the wall. There is also a [Security Key] on the table.");

                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "down") || moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "left"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        if (!isItem("Security Key", items)) Console.WriteLine("As she explored, she noticed a [Security Key] on the table.");
                        else Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        if (ans == "check framed photo" || ans == "check photo")
                        {
                            Console.WriteLine("You went and observed photo. You noticed that there is something behind where it was placed.");
                            Console.ReadKey(true);
                            Console.Write("\nLook behind the framed photo?\n" +
                                "[1] Yes\n" +
                                "[2] No\n" +
                                "> ");
                            while (true)
                            {
                                string look = Console.ReadLine().ToLower();
                                if (look == "1")
                                {
                                    Console.Clear();
                                    header(flags, position, map);
                                    Console.WriteLine("Behind the framed photo, Anastacia found a hidden compartment. Inside, there was a \n" +
                                        "[Diary] that seemed to belong to a nurse who worked in the asylum during the Cold War era. ");
                                    Console.ReadKey(true);
                                    Console.WriteLine("\nAnastacia thought, \"This could contain valuable information about the experiments and the mysterious \n" +
                                        "deaths. I need to read it carefully.\"");
                                    Console.ReadKey(true);
                                    Console.WriteLine("\nAs Anastacia delved into the diary, she uncovered a tale of unethical experiments, mysterious deaths, \n" +
                                        "and the mental toll it took on the nurses who once worked there. The pages spoke of a nurse who resisted the project's \n" +
                                        "dark path, leaving behind a trail of clues for those who dared to uncover the truth.\n");
                                    Console.ReadKey(true);
                                    while (true)
                                    {
                                        Console.Write("Would you like to continue reading the Diary's entries? [Yes] or [No]: ");
                                        string diary = Console.ReadLine().ToLower();
                                        if (diary == "yes" || diary == "y")
                                        {
                                            Console.WriteLine("\nAs Anastacia read the entries, a chilling realization dawned upon her – the shadows \n" +
                                                "she sought in the asylum were not only connected to the experiments but also to \n" +
                                                "her past. The diary hinted at a nurse named Maria Dela Cruz, who, driven by \n" +
                                                "compassion, sought to expose the horrors within the asylum.\n");
                                            Console.ReadKey(true);
                                            Console.WriteLine("Anastacia whispered to herself, \"Maria Dela Cruz – was she the one who left " +
                                                "\nthese clues for me? I need to follow her path and bring these dark secrets to light.\"");
                                            break;
                                        }
                                        else if (diary == "no" || diary == "n")
                                        {
                                            break;
                                        }
                                        else Console.WriteLine("Invalid action, try again.");
                                    }
                                    Console.WriteLine("\nAnastacia's journey into the asylum became more perilous, but her determination \n" +
                                        "to uncover the truth burned brighter. Little did she know that the pulse of deceit within those \n" +
                                        "walls was intertwined with her destiny, and the answers she sought would reveal \n" +
                                        "a chilling reality she never anticipated.");
                                    break;
                                }
                                else if (look == "2")
                                {
                                    break;
                                }
                                else Console.WriteLine("Invalid action, try again.");
                            }

                        }
                        else Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        if (ans == "take key" || ans == "take security key")
                        {
                            Console.WriteLine("You got a [Security Key]. This will be useful for the room near the lobby.");
                            item(index(items), "Security Key");
                        }
                        else Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
            else
            {
                Console.WriteLine("Anastacia entered the Director's Office, the air heavy with anticipation. \n" +
                    "The room was adorned with old furniture, dusty books, and a [Framed Photo] on the wall");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "down") || moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "left"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        if (ans == "check framed photo" || ans == "check photo")
                        {
                            Console.WriteLine("You went and observed photo. You noticed that there is something behind where it was placed.");
                            Console.ReadKey(true);
                            Console.Write("\nLook behind the framed photo?\n" +
                                "[1] Yes\n" +
                                "[2] No\n" +
                                "> ");
                            while (true)
                            {
                                string look = Console.ReadLine().ToLower();
                                switch (look)
                                {
                                    case "1":
                                        {
                                            Console.Clear();
                                            header(flags, position, map);
                                            Console.WriteLine("Behind the framed photo, Anastacia found a hidden compartment. Inside, there was a \n" +
                                                "[Diary] that seemed to belong to a nurse who worked in the asylum during the Cold War era. ");
                                            Console.ReadKey(true);
                                            Console.WriteLine("\nAnastacia thought, \"This could contain valuable information about the experiments and the mysterious \n" +
                                                "deaths. I need to read it carefully.\"");
                                            Console.ReadKey(true);
                                            Console.WriteLine("\nAs Anastacia delved into the diary, she uncovered a tale of unethical experiments, mysterious deaths, \n" +
                                                "and the mental toll it took on the nurses who once worked there. The pages spoke of a nurse who resisted the project's \n" +
                                                "dark path, leaving behind a trail of clues for those who dared to uncover the truth.\n");
                                            Console.ReadKey(true);
                                            while (true)
                                            {
                                                Console.Write("Would you like to continue reading the Diary's entries? [Yes] or [No]: ");
                                                string diary = Console.ReadLine().ToLower();
                                                if (diary == "yes" || diary == "y")
                                                {
                                                    Console.WriteLine("\nAs Anastacia read the entries, a chilling realization dawned upon her – the shadows \n" +
                                                        "she sought in the asylum were not only connected to the experiments but also to \n" +
                                                        "her past. The diary hinted at a nurse named Maria Dela Cruz, who, driven by \n" +
                                                        "compassion, sought to expose the horrors within the asylum.\n");
                                                    Console.ReadKey(true);
                                                    Console.WriteLine("Anastacia whispered to herself, \"Maria Dela Cruz – was she the one who left " +
                                                        "\nthese clues for me? I need to follow her path and bring these dark secrets to light.\"\n");
                                                    break;
                                                }
                                                else if (diary == "no" || diary == "n")
                                                {
                                                    break;
                                                }
                                                else Console.WriteLine("Invalid action, try again.");
                                            }
                                            Console.WriteLine("\nAnastacia's journey into the asylum became more perilous, but her determination \n" +
                                                "to uncover the truth burned brighter. Little did she know that the pulse of deceit within those \n" +
                                                "walls was intertwined with her destiny, and the answers she sought would reveal \n" +
                                                "a chilling reality she never anticipated.");
                                        }
                                        break;
                                    case "2":
                                        break;
                                    default: Console.WriteLine("Invalid action, try again."); break;
                                }
                                break;
                            }


                        }
                        else Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }


        }

        static void privateward(string[,] map, int[] position, string player)
        {
            if (!isItem("Note", items))
            {
                Console.WriteLine("The Patient Ward door opened with a rusty squeak. The room was vast, with rows of empty \n" +
                    "beds and faded curtains. As Anastacia walked deeper into the room, she noticed a [Note] on the floor.");
            }
            else
            {
                Console.WriteLine("The Patient Ward door opened with a rusty squeak. The room was vast, with rows of empty \n" +
                    "beds and faded curtains.");
            }
            while (true)
            {
                string ans = commands(map, position, player);
                if (ans.StartsWith("move") || ans.StartsWith("go"))
                {
                    if (moveValidator(ans, "left") || moveValidator(ans, "down") || moveValidator(ans, "right"))
                    {
                        Console.WriteLine("You can't go here. These are walls of the asylum.");
                    }
                    else if (moveValidator(ans, "up"))
                    {
                        position = move(position, ans, map);
                        prompt(position, map, player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else if (ans.StartsWith("inspect"))
                {
                    if (!isItem("Note", items)) Console.WriteLine("There's a [Note] you can check and read.");
                    else Console.WriteLine("Nothing to inspect here.");
                }
                else if (ans.StartsWith("check"))
                {
                    if (ans == "check note")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("The note reads, \"In the Director's room, the truth lies. Look behind the framed photo for the next clue.\"");
                        Console.ReadKey(true);
                        Console.WriteLine("\nAnastacia pondered the mysterious message, realizing that the Director's Room held more \n" +
                            "secrets than she initially thought.");
                    }
                    else Console.WriteLine("That can't be checked.");
                }
                else if (ans.StartsWith("take"))
                {
                    if (ans == "take note")
                    {
                        Console.WriteLine("You took the [Note].");
                        item(index(items), "Note");
                    }
                    else Console.WriteLine("That can't be taken.");
                }
                else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
            }
        }

        static void lobby(string[,] map, int[] position, string player)
        {
            if (!isItem("hide", flags))
            {
                flag(index(flags), "hide");

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
                        Console.ReadKey(true);
                        Console.WriteLine("Nyoco's friend nervously whispered, their words strained and anxious. Panic set in as they feared \n" +
                            "being caught, each cautious syllable amplifying the tension of the moment. The air crackled with apprehension, \n" +
                            "enveloping the duo in a web of uncertainty.\n");
                        Console.ReadKey(true);
                        Console.WriteLine("“Calm down! Go check the security room and outside while I’ll check the Laboratory Room and the Storage Room.”\r\n");
                        Console.ReadKey(true);
                        Console.WriteLine("Nyoco's voice pierced the tense air, her words laden with an intensity that reverberated through the silence.\r\n");
                        Console.ReadKey(true);
                        while (true)
                        {
                            Console.WriteLine("THEY ARE GOING DOWNSTAIRS, YOU MIGHT GET CAUGHT!\n" +
                            "[1] HIDE IN THE MORGUE\n" +
                            "[2] LEAVE THE BUILDING, NOW!");
                            string decide = action();
                            if (decide == "1")
                            {
                                Console.Clear();

                                header(flags, position, map);
                                while (true)
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        for (int j = 0; j < 15; j++)
                                        {
                                            Console.Write("HIDE  \t");
                                        }
                                        Console.WriteLine("\n");
                                    }

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"{player.ToUpper()}, YOU NEED TO HIDE NOW! MOVE UP TO THE MORGUE!");
                                    Console.ResetColor();
                                    Console.Write("> ");
                                    string hide = Console.ReadLine();
                                    if (moveValidator(hide, "up"))
                                    {
                                        position[0] -= 1;
                                        prompt(position, map, player);
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
                                while (true)
                                {
                                    while (true)
                                    {
                                        Console.Write("Are you sure? This will end the game. [Yes] or [No]: ");
                                        string confirm = Console.ReadLine().ToLower();
                                        if (confirm == "y" || confirm == "yes")
                                        {
                                            Console.WriteLine("You escaped through the windows of the lobby. You chickened out and was unsuccessful in finding out the truth.");
                                            Console.Write("Thank you for playing Pulse of Deceit! Would you like to play again? [Yes] or [No]: ");
                                            string answer = Console.ReadLine().ToLower();
                                            if (answer == "yes" || answer == "y")
                                            {
                                                Restart();
                                            }
                                            else if (answer == "no" || answer == "n")
                                            {
                                                Console.WriteLine("Thank you for playing Pulse of Deceit!");
                                                Console.ReadKey(true);
                                                Environment.Exit(0);
                                                break;
                                            }
                                        }
                                        else if (confirm == "n" || confirm == "no")
                                        {
                                            break;
                                        }
                                        else Console.WriteLine("Invalid action, try again.");
                                    }
                                    break;
                                }
                            }
                            else Console.WriteLine("Invalid answer, AGAIN.");
                        }
                        break;
                    }
                    else Console.WriteLine("Invalid answer, try again.");
                }
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "up") || moveValidator(ans, "right") || moveValidator(ans, "down"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
            else
            {
                Console.WriteLine("It seems the lobby is clear.");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left") && !isItem("Security Key", items))
                        {
                            Console.WriteLine("You can't enter the Security Room. It seems like you need another key.");
                        }
                        else if (moveValidator(ans, "left") && isItem("Security Key", items))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else if (moveValidator(ans, "right")) Console.WriteLine("You can't go here, these are the walls of the asylum.");
                        else if (moveValidator(ans, "up") || moveValidator(ans, "down"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
        }

        static void security(string[,] map, int[] position, string player)
        {
            if (!isItem("Keycard", items))
            {
                Console.WriteLine("Nestled in the heart of an abandoned hospital, a forgotten security room harbors secrets in its decaying embrace. \n" +
                    "Dust-covered consoles and decaying monitors hint at a past vigil, while a tattered logbook on the desk \n" +
                    "reveals cryptic entries. As urban explorers' chance upon this forsaken chamber, they unearth not only the remnants \n" +
                    "of surveillance equipment but also a series of mysterious artifacts— strange medical records, faded photographs, \n" +
                    "and an enigmatic key that seems to unlock more questions than answers. The security room becomes a silent witness, \n" +
                    "inviting those daring enough to unravel the untold tales hidden within its dilapidated walls.\n");
                Console.ReadKey(true);
                Console.WriteLine("\"What secrets do you hide in the shadows, old room?\" Anastacia whispered to herself.");
                Console.ReadKey(true);
                Console.WriteLine("\nHer fingers trace over dusty surfaces, stumbling upon a worn logbook that creaks open, revealing cryptic entries.\r\n");
                Console.ReadKey(true);
                Console.WriteLine("Would you like to examine the [Logbook]? [Yes] or [No].");
                while (true)
                {
                    string act = action();
                    if (act == "yes" || act == "y")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("Anastacia: (examining the logbook) Medical records, mysterious events... this place holds more \nthan just whispers of the past.\n");
                        Console.ReadKey(true);
                        Console.WriteLine("The feeble beam of her flashlight flickers over discarded documents, offering glimpses into a history long forgotten.\n");
                        Console.ReadKey(true);
                        Console.WriteLine("Amidst the relics, Anastacia's hand brushes against a forgotten laboratory keycard, \nits worn edges hinting at locked mysteries.");
                        break;
                    }
                    else if (act == "no" || act == "n")
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("As Anastacia navigates the room's secrets in near darkness, the weight of untold stories envelops her, urging her \n" +
                            "to unveil the mysteries concealed within the forsaken security room. The logbook, with its cryptic entries, \n" +
                            "remains a puzzle yet to be solved, its secrets lingering in the shadows, waiting to be discovered another day.\n");

                        Console.WriteLine("Anastacia continued exploring 'till she felt a laboratory keycard, its worn edges hinting at locked mysteries.");
                        break;
                    }
                    else Console.WriteLine("Invalid action, try again.");
                }
                Console.ReadKey(true);
                Console.WriteLine("You got a [Keycard]. A pass to the unknown, perhaps?");
                item(index(items), "Keycard");

                Console.ReadLine();
                Console.WriteLine("As she ventures further, the keycard becomes a symbol of both access and intrigue. It hints at locked doors \n" +
                    "and hidden chambers, inviting Anastacia to press on, driven by the allure of the unknown and the promise of revelations \n" +
                    "hidden within the haunting confines of the abandoned hospital's security room.");

                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "down") || moveValidator(ans, "left") || moveValidator(ans, "up"))
                        {
                            Console.WriteLine("You can't go here, these are the walls of the asylum.");
                        }
                        else if (moveValidator(ans, "right"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }

            }
            else
            {
                Console.Write("You went inside the security room.\n");
                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "down") || moveValidator(ans, "left") || moveValidator(ans, "up"))
                        {
                            Console.WriteLine("You can't go here, these are the walls of the asylum.");
                        }
                        else if (moveValidator(ans, "right"))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
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
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }

        }

        static void morgue(string[,] map, int[] position, string player)
        {
            if (!isItem("inspectMorgue", flags))
            {
                Console.WriteLine("The morgue room reeked of decay, its flickering lights casting ominous shadows on blood-stained walls. " +
                "\nLifeless bodies lay in chilling positions—some sprawled on cold metal tables, others hanging ominously, and a few " +
                "\nconfined to the isolation room. The air was thick with the silent screams of those whose final moments were frozen in " +
                "\nagony, making the morgue a nightmarish tableau of horror.\r\n");
                Console.ReadKey(true);
                Console.WriteLine("“Jesus Christ.”\n");
                Console.ReadKey(true);
                Console.WriteLine("Anastacia whispered, her voice catching a breath as she surveyed the gruesome scene before her.\r\n");
                Console.ReadKey(true);
                Console.WriteLine("“They put all the bodies here?”\n");
                Console.ReadKey(true);
                Console.WriteLine("She questioned, the words leaving her lips with a tremor of disbelief.\n");
                Console.ReadKey(true);
                Console.WriteLine("You might want to [inspect] the Morgue Room.");

                while (true)
                {
                    if (isItem("Ward Key", items) && !isItem("inspectMorgue", flags))
                    {
                        Console.Clear();
                        header(flags, position, map);
                        Console.WriteLine("\"Let's go back tomorrow.\"\n");
                        Console.ReadKey(true);
                        Console.WriteLine("Nyoco said in a hurry.");
                        Console.ReadKey(true);
                        Console.WriteLine("\nLooks like now is the best time to explore with the key on your hands.");
                        flag(index(flags), "inspectMorgue");
                    }
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left") || moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "down") && !isItem("morgue", flags))
                        {
                            Console.WriteLine("YOU CAN NOT LEAVE YET, THEY ARE STILL OUTSIDE!");
                        }
                        else if (moveValidator(ans, "down") && isItem("morgue", flags))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("While inspecting the morgue, you saw some items. A [Photo], and a [Ward Key].");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        if (ans == "check key" || ans == "check ward key")
                        {
                            Console.Clear();
                            header(flags, position, map);
                            Console.WriteLine("This might be a key to one of the locked rooms. You should [take] this.");
                        }
                        else if (ans == "check photo")
                        {
                            Console.Clear();
                            header(flags, position, map);
                            Console.WriteLine("Wh- why is there a ripped photo of every nurse from this asylum here inside the morgue excluding me?”\n");
                            Console.ReadKey(true);
                            Console.WriteLine("Anastacia uttered her words with a hint of confusion, her expression reflecting the puzzlement that danced in her eyes.");
                            Console.ReadKey(true);
                            Console.WriteLine("You might need to [take] this.");
                        }
                        else Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        if (ans == "take key" || ans == "take ward key")
                        {
                            Console.WriteLine("You got a [Ward Key].");
                            item(index(items), "Ward Key");
                            flag(index(flags), "morgue");
                        }
                        else if (ans == "take photo")
                        {
                            Console.WriteLine("You got a [Photo]");
                            item(index(items), "Photo");
                        }
                        else Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }


            }
            else
            {
                Console.WriteLine("The morgue room reeked of decay, its flickering lights casting ominous shadows \n" +
                    "on blood-stained walls. Lifeless bodies lay in chilling positions—some sprawled on cold metal \n" +
                    "tables, others hanging ominously, and a few confined to the isolation room. The air was thick with \n" +
                    "the silent screams of those whose final moments were frozen in agony, making the morgue a nightmarish tableau of horror.");

                while (true)
                {
                    string ans = commands(map, position, player);
                    if (ans.StartsWith("move") || ans.StartsWith("go"))
                    {
                        if (moveValidator(ans, "left") || moveValidator(ans, "up") || moveValidator(ans, "right"))
                        {
                            Console.WriteLine("You can't go here. These are walls of the asylum.");
                        }
                        else if (moveValidator(ans, "down") && isItem("morgue", flags))
                        {
                            position = move(position, ans, map);
                            prompt(position, map, player);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, try again.");
                        }
                    }
                    else if (ans.StartsWith("inspect"))
                    {
                        Console.WriteLine("Nothing to inspect here.");
                    }
                    else if (ans.StartsWith("check"))
                    {
                        Console.WriteLine("That can't be checked.");
                    }
                    else if (ans.StartsWith("take"))
                    {
                        Console.WriteLine("That can't be taken.");
                    }
                    else Console.WriteLine("Invalid action. Try typing \"help\" for the list of commands.");
                }
            }
        }

        static bool moveValidator(string move, string direction)
        {
            switch (direction)
            {
                case "up":
                    {
                        if (move == "move up" || move == "move forward" || move == "go up" || move == "go forward") return true;
                    }
                    break;
                case "down":
                    {
                        if (move == "move down" || move == "move back" || move == "move backward" || move == "go down" || move == "go back" || move == "go downward") return true;
                    }
                    break;
                case "left":
                    {
                        if (move == "move left" || move == "go left") return true;
                    }
                    break;
                case "right":
                    {
                        if (move == "move right" || move == "go right") return true;
                    }
                    break;
            }
            return false;

        }

        static void Restart()
        {
            Console.Clear();
            string exePath = Assembly.GetEntryAssembly().Location;

            AppDomainSetup domainSetup = new AppDomainSetup();
            AppDomain newDomain = AppDomain.CreateDomain("NewDomain", null, domainSetup);
            newDomain.ExecuteAssembly(exePath);

            AppDomain.Unload(newDomain);
        }

        static void TypeWithAnimation(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
            Console.WriteLine();
            Thread.Sleep(500);
        }
    }
}
