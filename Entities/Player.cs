// File: Entities/Player.cs\n// Refactored to namespace TerminalCraft\n\nusing System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace TerminalCraft
{
    public class Player
    {
        public string Name { get; set; }
        public string World { get; set; }
        public Dictionary<string, int> Inventory { get; private set; }

        public Player(string name, string world)
        {
            Name = name;
            World = world;
            Inventory = new Dictionary<string, int>();
        }

        public void Collect(string item, int amount)
        {
            if (Inventory.ContainsKey(item))
                Inventory[item] += amount;
            else
                Inventory[item] = amount;

            Console.WriteLine($"You collected {amount} {item}.");
            SaveToFile(); // Save after collecting
        }

        // Store unique tamed animals
        [JsonInclude]
        public HashSet<string> TamedAnimals { get; private set; } = new HashSet<string>();

        public void TameAnimal(string animalName)
        {
            if (!string.IsNullOrWhiteSpace(animalName))
            {
                TamedAnimals.Add(animalName);
                Console.WriteLine($"{animalName} has been added to your Compendium!");
            }
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

        public void SaveToFile()
        {
            // 1️⃣ Define the folder path
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "CreatedWorlds");

            // 2️⃣ Make sure the folder exists
            Directory.CreateDirectory(folder);

            // 3️⃣ Combine folder + filename
            string path = Path.Combine(folder, $"{World}.json");

            // 4️⃣ Serialize player data
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

            // 5️⃣ Write to file
            File.WriteAllText(path, json);
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

        private class SaveData
        {
            public string? playerName { get; set; }
            public string? worldName { get; set; }
            public Dictionary<string, int>? inventory { get; set; }
        }
    }
}
