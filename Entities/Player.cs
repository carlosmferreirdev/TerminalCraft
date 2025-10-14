using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TerminalCraft
{

    public class Player
    {
        #region Properties
        public string Name { get; set; }
        public string World { get; set; }

        [JsonInclude]
        public Dictionary<string, int> Inventory { get; private set; }

        [JsonInclude]
        public HashSet<string> TamedAnimals { get; private set; } = new HashSet<string>();
        #endregion

        #region Constructors
        public Player(string name, string world)
        {
            Name = name;
            World = world;
            Inventory = new Dictionary<string, int>();
        }
        #endregion

        #region Inventory Methods
        public void Collect(string item, int amount)
        {
            if (Inventory.ContainsKey(item))
                Inventory[item] += amount;
            else
                Inventory[item] = amount;

            Console.WriteLine($"You collected {amount} {item}.");
            SaveToFile(); // Save after collecting
        }

        public void ShowInventory()
        {
            Console.WriteLine("\n== Inventory ==");
            if (Inventory.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
            }
            else
            {
                foreach (var item in Inventory)
                    Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
        #endregion

        #region Animal/Compendium Methods
        public void TameAnimal(string animalName)
        {
            if (!string.IsNullOrWhiteSpace(animalName))
            {
                TamedAnimals.Add(animalName);
                Console.WriteLine($"{animalName} has been added to your Compendium!");
            }
        }
        #endregion

        #region Serialization
        public void SaveToFile()
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, $"{World}.json");
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[DEBUG] Player data saved to {path} at {DateTime.Now:HH:mm:ss.fff}");
            Console.ResetColor();
        }

        public static Player? LoadFromFile(string worldName)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");
            string path = Path.Combine(folder, $"{worldName}.json");
            if (!File.Exists(path))
                return null;
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Player>(json);
        }
        #endregion
    }
}
