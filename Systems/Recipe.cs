using System;
using System.Collections.Generic;
using System.Linq;

namespace TerminalCraft.Systems
{
    /// <summary>
    /// Represents a crafting recipe with required materials and resulting output.
    /// </summary>
    public class Recipe
    {
        public string Name { get; set; }
        public Dictionary<string, int> Materials { get; set; } // Material name -> quantity
        public string Output { get; set; }
        public int OutputQuantity { get; set; }

        public Recipe(string name, Dictionary<string, int> materials, string output, int outputQuantity = 1)
        {
            Name = name;
            Materials = materials;
            Output = output;
            OutputQuantity = outputQuantity;
        }

        /// <summary>
        /// Check if the player has all required materials to craft this recipe.
        /// </summary>
        public bool CanCraft(Player player)
        {
            foreach (var material in Materials)
            {
                // Check for exact material name
                if (player.Inventory.TryGetValue(material.Key, out int playerAmount))
                {
                    if (playerAmount < material.Value)
                        return false;
                }
                // If material is "Wood", accept any wood type
                else if (material.Key == "Wood")
                {
                    int totalWood = 0;
                    foreach (var item in player.Inventory)
                    {
                        if (item.Key.Contains("Wood"))
                            totalWood += item.Value;
                    }
                    if (totalWood < material.Value)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Consume the materials from player's inventory.
        /// </summary>
        public void ConsumeMaterials(Player player)
        {
            foreach (var material in Materials)
            {
                int remaining = material.Value;
                
                // If material is "Wood", consume any wood types
                if (material.Key == "Wood")
                {
                    var woodItems = player.Inventory
                        .Where(kv => kv.Key.Contains("Wood"))
                        .OrderBy(kv => kv.Value) // Consume smallest stacks first
                        .ToList();

                    foreach (var wood in woodItems)
                    {
                        if (remaining <= 0) break;
                        
                        int toConsume = Math.Min(wood.Value, remaining);
                        player.Inventory[wood.Key] -= toConsume;
                        if (player.Inventory[wood.Key] <= 0)
                            player.Inventory.Remove(wood.Key);
                        remaining -= toConsume;
                    }
                }
                else
                {
                    // Consume exact material
                    if (player.Inventory.ContainsKey(material.Key))
                    {
                        player.Inventory[material.Key] -= material.Value;
                        if (player.Inventory[material.Key] <= 0)
                            player.Inventory.Remove(material.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Get a formatted string showing required materials.
        /// </summary>
        public string GetMaterialsString()
        {
            return string.Join(", ", Materials.Select(m => $"{m.Value}x {m.Key}"));
        }
    }
}
