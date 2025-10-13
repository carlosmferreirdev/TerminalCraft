using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TerminalCraft.Systems
{
    public static class GameManager
    {
        private static Player? player;
        private static readonly Random rand = new();
        private static int tickCount = 0;
        private static bool isDay = true;

        private static readonly List<HostileMob> hostileMobs = new()
        {
            new HostileMob("Zombie", 2, 6),
            new HostileMob("Skeleton", 3, 5),
            new HostileMob("Spider", 2, 4),
            new HostileMob("Creeper", 4, 3),
            new HostileMob("Slime", 1, 2)
        };

        public static void Start()
        {
            string? worldName = ShowMainMenu();
            if (string.IsNullOrWhiteSpace(worldName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid world name. Returning to main menu...");
                Console.ResetColor();
                while (string.IsNullOrWhiteSpace(worldName))
                {
                    worldName = ShowMainMenu();
                }
            }

            string path = worldName + ".json";
            player = Player.LoadFromFile(path) ?? new Player("Steve", worldName ?? "World");

            GameLoop();
        }

        private static string? ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nChoose an action:\n");
                Console.WriteLine("1. New World");
                Console.WriteLine("2. Load World");
                Console.WriteLine("3. Delete World");
                Console.WriteLine("4. Exit\n");

                string? choice = Console.ReadLine();
                Console.Clear();

                //use switch expression
                switch (choice)
                {
                    case "1":
                        return CreateWorld();
                    case "2":
                        return LoadWorld();
                    case "3":
                        DeleteWorld();
                        break;
                    case "4":
                        return null;
                    default:
                        InvalidChoice();
                        break;
                }
            }
        }

        private static void InvalidChoice()
        {
            Console.WriteLine("Invalid choice. Press any key to try again...");
            Console.ReadKey();
            Console.Clear();
            return; // Return to main menu
        }
        private static string? CreateWorld()
        {
            Console.Write("Enter a name for your new world: ");
            string? name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return null;

            // Ensure the folder exists
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");
            Directory.CreateDirectory(folder);

            // Correct file path
            string path = Path.Combine(folder, $"{name}.json");

            if (File.Exists(path))
            {
                Console.WriteLine("World already exists. Use Load World instead.");
                Console.ReadKey();
                return null;
            }

            player = new Player("Steve", name);
            player.SaveToFile(); // Save immediately to create the file
            Console.WriteLine($"World '{name}' created successfully!");
            Console.ReadKey();
            return name;
        }
        private static string? LoadWorld()
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");
            string[] files = Directory.GetFiles(folder, "*.json");

            if (files.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No saved worlds found.");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
                return ShowMainMenu();
            }

            Console.WriteLine("Saved worlds:");
            foreach (string file in files)
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));

            Console.Write("\nEnter the name of the world to load (or 'back'): ");
            string? name = Console.ReadLine()?.Trim();
            if (name?.ToLowerInvariant() == "back")
                return ShowMainMenu();

            string path = Path.Combine(folder, $"{name}.json");
            if (File.Exists(path))
            {
                Console.WriteLine($"World '{name}' loaded successfully.");
                Console.ReadKey();
                return name;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("World not found, try again. (Press any key to continue)");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();

            return LoadWorld();
        }

    private static string? DeleteWorld()
    {
        string folder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");
        string[] files = Directory.GetFiles(folder, "*.json");

        if (files.Length == 0)
        {
            Console.WriteLine("No saved worlds to delete.");
            Console.ReadKey();
            return ShowMainMenu();
        }

        Console.WriteLine("Saved worlds:");
        foreach (string file in files)
            Console.WriteLine(Path.GetFileNameWithoutExtension(file));

        Console.Write("\nEnter the name of the world to delete: ");
        string? name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return ShowMainMenu();

        string path = Path.Combine(folder, $"{name}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
            Console.WriteLine($"World '{name}' deleted.");
        }
        else
        {
            Console.WriteLine("World not found.");
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
        return "";
    }


        private static void GameLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = isDay ? ConsoleColor.Yellow : ConsoleColor.DarkBlue;
                Console.WriteLine("== " + (isDay ? "☀️ Day" : "🌙 Night") + " ==");
                Console.ResetColor();

                if (player == null)
                {
                    Console.WriteLine("No player loaded. Returning to main menu.");
                    return;
                }

                Console.WriteLine($"Player: {player.Name} | World: {player.World}\n");
                Console.WriteLine("1. Explore");
                Console.WriteLine("2. View Inventory");
                Console.WriteLine("3. Crafting (Work in Progress)");
                Console.WriteLine("4. Build (Work in Progress)");
                Console.WriteLine("5. Compendium");
                Console.WriteLine("6. Save & Quit");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Exploration.Explore(player, rand, ref tickCount, ref isDay, hostileMobs);
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        Console.WriteLine("Feature coming soon!");
                        break;
                    case "4":
                        Console.WriteLine("Feature coming soon!");
                        break;
                    case "5":
                        Compendium.Show(player);
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("Saving Game...\n");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Thanks for playing!");
                        Console.ResetColor();

                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        player.SaveToFile();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
