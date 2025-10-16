using TerminalCraft.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    // New: Pickaxes with durability
    [JsonInclude]
    public List<Pickaxe> Pickaxes { get; private set; } = new List<Pickaxe>();

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
            // If item is a pickaxe, add to Pickaxes list instead
            if (item.EndsWith("Pickaxe"))
            {
                var quality = item switch
                {
                    "Wooden Pickaxe" => TerminalCraft.Systems.ToolQuality.Wooden,
                    "Stone Pickaxe" => TerminalCraft.Systems.ToolQuality.Stone,
                    "Iron Pickaxe" => TerminalCraft.Systems.ToolQuality.Iron,
                    "Diamond Pickaxe" => TerminalCraft.Systems.ToolQuality.Diamond,
                    _ => TerminalCraft.Systems.ToolQuality.None
                };
                for (int i = 0; i < amount; i++)
                    Pickaxes.Add(new Pickaxe(quality));
                Console.WriteLine($"You collected {amount} {item}.");
                return;
            }
            // Normal items
            if (Inventory.ContainsKey(item))
                Inventory[item] += amount;
            else
                Inventory[item] = amount;
            Console.WriteLine($"You collected {amount} {item}.");
        }

        public void ShowInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n== Inventory ==");
                bool empty = Inventory.Count == 0 && Pickaxes.Count == 0;
                if (empty)
                {
                    Console.WriteLine("Inventory is empty.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("1. View Blocks");
                Console.WriteLine("2. View Tools");
                Console.WriteLine("3. Return");
                Console.Write("\nChoose an option: ");
                string? mainChoice = Console.ReadLine()?.Trim();
                if (mainChoice == "3" || string.IsNullOrWhiteSpace(mainChoice))
                    return;
                if (mainChoice == "1")
                {
                    ShowBlocksMenu();
                }
                else if (mainChoice == "2")
                {
                    ShowToolsMenu();
                }
            }
        }

        private void ShowBlocksMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("== Blocks ==");
                // Filter out any tool entries that might live in Inventory (legacy saves)
                var blockItems = Inventory.Where(kv => !kv.Key.EndsWith("Pickaxe", StringComparison.OrdinalIgnoreCase)).ToList();
                if (blockItems.Count == 0)
                {
                    Console.WriteLine("No blocks/items in inventory.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }
                foreach (var item in blockItems)
                {
                    Console.ForegroundColor = GetBlockColor(item.Key);
                    Console.WriteLine($"- {item.Key}: {item.Value}");
                    Console.ResetColor();
                }
                Console.WriteLine("\n1. Discard item");
                Console.WriteLine("2. Return");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine()?.Trim();
                if (choice == "2" || string.IsNullOrWhiteSpace(choice))
                    return;
                if (choice == "1")
                {
                    Console.Write("Enter item name to discard: ");
                    string? name = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(name) || !blockItems.Any(kv => string.Equals(kv.Key, name, StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Item not found.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        continue;
                    }
                    // Find canonical key casing
                    string keyName = blockItems.First(kv => string.Equals(kv.Key, name, StringComparison.OrdinalIgnoreCase)).Key;
                    Console.Write($"How many {name} to discard? (max {Inventory[name]}): ");
                    string? amountStr = Console.ReadLine()?.Trim();
                    if (int.TryParse(amountStr, out int amount) && amount > 0 && amount <= Inventory[keyName])
                    {
                        Inventory[keyName] -= amount;
                        if (Inventory[keyName] <= 0)
                            Inventory.Remove(keyName);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Discarded {amount} {name}.");
                        Console.ResetColor();
                        SaveToFile();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid amount.");
                        Console.ResetColor();
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private void ShowToolsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("== Tools ==");
                // Migrate any legacy pickaxe entries stored as items into Pickaxes list
                MigratePickaxesFromInventory();
                if (Pickaxes.Count == 0)
                {
                    Console.WriteLine("No tools in inventory.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }
                
                // Group pickaxes by quality and show count
                var groupedPickaxes = Pickaxes
                    .GroupBy(p => p.Quality)
                    .OrderBy(g => g.Key)
                    .ToList();
                
                // The menu options below are dynamically numbered based on how many tool types the player has.
                int optionNum = 1;
                foreach (var group in groupedPickaxes)
                {
                    int count = group.Count();
                    var sample = group.First();
                    Console.ForegroundColor = GetToolColor(sample.Quality);
                    Console.WriteLine($"{optionNum}. {sample.GetName()} (x{count})");
                    Console.ResetColor();
                    optionNum++;
                }
                
                Console.WriteLine($"\n{optionNum}. Discard tool");
                Console.WriteLine($"{optionNum + 1}. Return");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine()?.Trim();
                
                if (choice == $"{optionNum + 1}" || string.IsNullOrWhiteSpace(choice))
                    return;
                    
                // Check if user selected a pickaxe group to view details
                if (int.TryParse(choice, out int selected) && selected >= 1 && selected < optionNum)
                {
                    var selectedGroup = groupedPickaxes[selected - 1];
                    Console.Clear();
                    Console.WriteLine($"=== {selectedGroup.First().GetName()} Details ===\n");
                    
                    // Find the equipped pickaxe (highest quality with durability > 0)
                    var equippedPickaxe = Pickaxes
                        .Where(p => p.Durability > 0)
                        .OrderByDescending(p => p.Quality)
                        .FirstOrDefault();
                    
                    foreach (var pickaxe in selectedGroup.OrderByDescending(p => p.Durability))
                    {
                        Console.ForegroundColor = GetToolColor(pickaxe.Quality);
                        Console.Write($"{pickaxe.GetName()}");
                        Console.ResetColor();
                        
                        // Display durability bar
                        double durabilityPercentage = (double)pickaxe.Durability / pickaxe.MaxDurability;
                        int barLength = 20;
                        int filledBars = (int)(durabilityPercentage * barLength);
                        
                        Console.Write(" - [");
                        Console.ForegroundColor = durabilityPercentage > 0.5 ? ConsoleColor.Green :
                                                  durabilityPercentage > 0.25 ? ConsoleColor.Yellow :
                                                  ConsoleColor.Red;
                        Console.Write(new string('█', filledBars));
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(new string('░', barLength - filledBars));
                        Console.ResetColor();
                        Console.Write($"] {pickaxe.Durability}/{pickaxe.MaxDurability}");
                        
                        // Show equipped indicator after durability
                        if (pickaxe == equippedPickaxe)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(" [Equipped]");
                            Console.ResetColor();
                        }
                        
                        Console.WriteLine();
                    }
                    
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }
                
                if (choice == $"{optionNum}")
                {
                    Console.Write("Enter tool name to discard (e.g., 'Wooden Pickaxe'): ");
                    string? toolName = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(toolName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid tool name.");
                        Console.ResetColor();
                    }
                    else
                    {
                        // Map name to quality
                        ToolQuality? q = toolName.ToLowerInvariant() switch
                        {
                            "wooden pickaxe" => ToolQuality.Wooden,
                            "stone pickaxe" => ToolQuality.Stone,
                            "iron pickaxe" => ToolQuality.Iron,
                            "diamond pickaxe" => ToolQuality.Diamond,
                            _ => null
                        };
                        if (q == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Unknown tool name.");
                            Console.ResetColor();
                        }
                        else
                        {
                            var matches = Pickaxes.Where(p => p.Quality == q.Value).ToList();
                            if (matches.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You don't have that tool.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write($"How many to discard? (max {matches.Count}): ");
                                string? amtStr = Console.ReadLine()?.Trim();
                                if (int.TryParse(amtStr, out int amt) && amt > 0 && amt <= matches.Count)
                                {
                                    for (int i = 0; i < amt; i++)
                                    {
                                        var index = Pickaxes.FindIndex(p => p.Quality == q.Value);
                                        if (index >= 0) Pickaxes.RemoveAt(index);
                                    }
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"Discarded {amt} {toolName}.");
                                    Console.ResetColor();
                                    SaveToFile();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid amount.");
                                    Console.ResetColor();
                                }
                            }
                        }
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private void MigratePickaxesFromInventory()
        {
            string[] names = { "Wooden Pickaxe", "Stone Pickaxe", "Iron Pickaxe", "Diamond Pickaxe" };
            foreach (var n in names)
            {
                if (Inventory.TryGetValue(n, out int qty) && qty > 0)
                {
                    var quality = n switch
                    {
                        "Wooden Pickaxe" => ToolQuality.Wooden,
                        "Stone Pickaxe" => ToolQuality.Stone,
                        "Iron Pickaxe" => ToolQuality.Iron,
                        "Diamond Pickaxe" => ToolQuality.Diamond,
                        _ => ToolQuality.None
                    };
                    for (int i = 0; i < qty; i++)
                        Pickaxes.Add(new Pickaxe(quality));
                    Inventory.Remove(n);
                }
            }
            // Persist migration so it doesn't reappear
            SaveToFile();
        }

        private ConsoleColor GetBlockColor(string item)
        {
            return item switch
            {
                "Stone" => ConsoleColor.DarkGray,
                "Iron Ore" => ConsoleColor.Gray,
                "Diamond" => ConsoleColor.Cyan,
                "Oak Wood" => ConsoleColor.Yellow,
                "Birch Wood" => ConsoleColor.White,
                "Jungle Wood" => ConsoleColor.DarkYellow,
                "Spruce Wood" => ConsoleColor.DarkRed,
                "Mangrove Wood" => ConsoleColor.Red,
                "Acacia Wood" => ConsoleColor.DarkMagenta,
                "Cactus" => ConsoleColor.Green,
                "Mushroom" => ConsoleColor.Magenta,
                "Food" => ConsoleColor.DarkGreen,
                _ => ConsoleColor.White
            };
        }

        private ConsoleColor GetToolColor(TerminalCraft.Systems.ToolQuality quality)
        {
            return quality switch
            {
                TerminalCraft.Systems.ToolQuality.Wooden => ConsoleColor.Yellow,
                TerminalCraft.Systems.ToolQuality.Stone => ConsoleColor.Gray,
                TerminalCraft.Systems.ToolQuality.Iron => ConsoleColor.White,
                TerminalCraft.Systems.ToolQuality.Diamond => ConsoleColor.Cyan,
                _ => ConsoleColor.White
            };
        }

        public void ManageInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n== Inventory Management ==");
                if (Inventory.Count == 0)
                {
                    Console.WriteLine("Inventory is empty.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                int index = 1;
                var items = Inventory.ToList();
                foreach (var item in items)
                {
                    Console.WriteLine($"{index}. {item.Key}: {item.Value}");
                    index++;
                }

                Console.WriteLine("\nEnter item number to discard (or 'back' to return): ");
                string? choice = Console.ReadLine()?.Trim().ToLowerInvariant();

                if (choice == "back" || string.IsNullOrWhiteSpace(choice))
                    return;

                if (int.TryParse(choice, out int itemIndex) && itemIndex > 0 && itemIndex <= items.Count)
                {
                    var selectedItem = items[itemIndex - 1];
                    Console.Write($"How many {selectedItem.Key} to discard? (max {selectedItem.Value}): ");
                    string? amountStr = Console.ReadLine()?.Trim();

                    if (int.TryParse(amountStr, out int amount) && amount > 0 && amount <= selectedItem.Value)
                    {
                        Inventory[selectedItem.Key] -= amount;
                        if (Inventory[selectedItem.Key] <= 0)
                            Inventory.Remove(selectedItem.Key);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Discarded {amount} {selectedItem.Key}.");
                        Console.ResetColor();
                        SaveToFile();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid amount.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
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

            int retries = 5;
            int delayMs = 100;
            for (int attempt = 1; attempt <= retries; attempt++)
            {
                try
                {
                    File.WriteAllText(path, json);
                    break;
                }
                catch (IOException) when (attempt < retries)
                {
                    Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR] Failed to save player data: {ex.Message}");
                    Console.ResetColor();
                    break;
                }
            }
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
