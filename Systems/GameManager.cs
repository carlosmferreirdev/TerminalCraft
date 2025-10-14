using System;
using System.Collections.Generic;
using System.IO;

namespace TerminalCraft.Systems
{
    public static class GameManager
    {
        #region Fields / State
        private static Player? player;
        private static readonly Random rand = new();
        private static int tickCount = 0;
        private static bool isDay = true;
        private static Weather.WeatherContext? _weather;
        private static Weather.IWeatherService _weatherService = new Weather.OpenMeteoWeatherService();
        private static readonly List<HostileMob> hostileMobs = new()
        {
            new HostileMob("Zombie", 2, 6),
            new HostileMob("Skeleton", 3, 5),
            new HostileMob("Spider", 2, 4),
            new HostileMob("Creeper", 4, 3),
            new HostileMob("Slime", 1, 2)
        };
        private static readonly string WorldsFolder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");
        #endregion

        #region Lifecycle
        public static void Start()
        {
            Directory.CreateDirectory(WorldsFolder);
            while (true)
            {
                var worldName = ShowMainMenu();
                if (string.IsNullOrWhiteSpace(worldName)) break; // Exit program

                player = Player.LoadFromFile(worldName) ?? new Player("Steve", worldName);
                GenerateAutomaticWeather();
                GameLoop(); // returns on Save & Quit
            }
        }
        #endregion

        #region Persistence
        public static void SavePlayerOnExit()
        {
            if (player == null) return;
            try { player.SaveToFile(); } catch { /* swallow on exit */ }
        }
        #endregion

        #region Weather
        private static void GenerateAutomaticWeather()
        {
            var randomWeatherService = new Weather.RandomWeatherService(_weatherService);
            try { _weather = randomWeatherService.GenerateRandomWeatherAsync().GetAwaiter().GetResult(); }
            catch { _weather = randomWeatherService.GenerateFictionalWeather(); }
        }
        #endregion

        #region HUD
        private static void DisplayGameHUD()
        {
            Console.ForegroundColor = isDay ? ConsoleColor.Yellow : ConsoleColor.DarkBlue;
            Console.Write("== " + (isDay ? "☀️ Day" : "🌙 Night"));
            if (_weather != null)
            {
                Console.ResetColor(); Console.Write(" | ");
                if (_weather.IsCold) Console.ForegroundColor = ConsoleColor.Cyan;
                else if (_weather.IsCool) Console.ForegroundColor = ConsoleColor.Blue;
                else if (_weather.IsWarm) Console.ForegroundColor = ConsoleColor.Yellow;
                else if (_weather.IsHot) Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{_weather.TemperatureC:F0}°C");
                Console.ResetColor(); Console.Write(" | ");
                Console.ForegroundColor = _weather.IsWet ? ConsoleColor.DarkBlue : ConsoleColor.Gray;
                Console.Write(_weather.IsWet ? "🌧️ Wet" : "☀️ Dry");
            }
            Console.ResetColor(); Console.WriteLine(" ==\n");
        }
        #endregion

        #region Menus
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
                switch (Console.ReadLine())
                {
                    case "1": return CreateWorld();
                    case "2": return LoadWorld();
                    case "3": DeleteWorld(); break;
                    case "4":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Thanks for playing TerminalCraft!");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        return null;
                    default: InvalidChoice(); break;
                }
            }
        }

        private static void InvalidChoice()
        {
            Console.WriteLine("Invalid choice. Press any key to try again...");
            Console.ReadKey();
        }

        private static string? CreateWorld()
        {
            Console.Write("Enter a name for your new world: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return null;

            string path = Path.Combine(WorldsFolder, $"{name}.json");
            if (File.Exists(path))
            {
                Console.WriteLine("World already exists. Use Load World instead.");
                Console.ReadKey();
                return null;
            }
            player = new Player("Steve", name);
            player.SaveToFile();
            Console.WriteLine($"World '{name}' created successfully!");
            Console.ReadKey();
            return name;
        }

        private static string? LoadWorld()
        {
            while (true)
            {
                var files = Directory.GetFiles(WorldsFolder, "*.json");
                if (files.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No saved worlds found.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    return null;
                }
                Console.WriteLine("Saved worlds:");
                foreach (var file in files) Console.WriteLine(Path.GetFileNameWithoutExtension(file));
                Console.Write("\nEnter the name of the world to load (or 'back'): ");
                var name = Console.ReadLine()?.Trim();
                if (string.Equals(name, "back", StringComparison.OrdinalIgnoreCase)) return null;
                if (string.IsNullOrWhiteSpace(name)) continue;
                string path = Path.Combine(WorldsFolder, $"{name}.json");
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
            }
        }

        private static void DeleteWorld()
        {
            var files = Directory.GetFiles(WorldsFolder, "*.json");
            if (files.Length == 0)
            {
                Console.WriteLine("No saved worlds to delete.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Saved worlds:");
            foreach (var file in files) Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            Console.Write("\nEnter the name of the world to delete: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;
            string path = Path.Combine(WorldsFolder, $"{name}.json");
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"World '{name}' deleted.");
            }
            else Console.WriteLine("World not found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
        #endregion


        #region Game Loop
        private static void GameLoop()
        {
            while (true)
            {
                Console.Clear();
                DisplayGameHUD();
                if (player == null) { Console.WriteLine("No player loaded. Returning to main menu."); return; }
                Console.Write($"Player: {player.Name} | World: ");
                Console.ForegroundColor = ConsoleColor.Green; Console.Write(player.World); Console.ResetColor();
                Console.WriteLine("\n");
                Console.WriteLine("1. Explore");
                Console.WriteLine("2. View Inventory");
                Console.WriteLine("3. Crafting (Work in Progress)");
                Console.WriteLine("4. Build (Work in Progress)");
                Console.WriteLine("5. Compendium");
                Console.WriteLine("6. Save & Quit");
                switch (Console.ReadLine())
                {
                    case "1": Exploration.Explore(player, rand, ref tickCount, ref isDay, hostileMobs, _weather); break;
                    case "2": player.ShowInventory(); break;
                    case "3": Console.WriteLine("Feature coming soon!"); break;
                    case "4": Console.WriteLine("Feature coming soon!"); break;
                    case "5": Compendium.Show(player); break;
                    case "6": SaveAndReturnToMenu(); return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void SaveAndReturnToMenu()
        {
            Console.Clear();
            Console.WriteLine("Saving Game...\n");
            player?.SaveToFile();
            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Game saved successfully!"); Console.ResetColor();
            Console.WriteLine("Returning to main menu...\nPress any key to continue...");
            Console.ReadKey();
        }
        #endregion
    }
}
