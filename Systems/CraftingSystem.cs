using System;
using System.Linq;

namespace TerminalCraft.Systems
{
    /// <summary>
    /// Handles the crafting menu UI and interactions.
    /// </summary>
    public static class CraftingSystem
    {
        public static void ShowCraftingMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("== Crafting ==\n");

                var recipes = CraftingRecipes.GetAllRecipes();
                
                if (recipes.Count == 0)
                {
                    Console.WriteLine("No recipes available yet.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                // Display all recipes with availability indicator
                for (int i = 0; i < recipes.Count; i++)
                {
                    var recipe = recipes[i];
                    bool canCraft = recipe.CanCraft(player);

                    Console.ForegroundColor = canCraft ? ConsoleColor.Green : ConsoleColor.DarkGray;
                    Console.Write($"{i + 1}. {recipe.Name}");
                    Console.ResetColor();
                    Console.Write($" ({recipe.OutputQuantity}x {recipe.Output})");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($" - Requires: {recipe.GetMaterialsString()}");
                    Console.ResetColor();
                }

                Console.WriteLine("\nEnter recipe number to craft (or 'back' to return): ");
                string? choice = Console.ReadLine()?.Trim().ToLowerInvariant();

                if (choice == "back" || string.IsNullOrWhiteSpace(choice))
                    return;

                if (int.TryParse(choice, out int index) && index > 0 && index <= recipes.Count)
                {
                    var selectedRecipe = recipes[index - 1];
                    CraftItem(player, selectedRecipe);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void CraftItem(Player player, Recipe recipe)
        {
            if (!recipe.CanCraft(player))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nYou don't have enough materials to craft {recipe.Name}.");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // Confirm crafting
            Console.Write($"\nCraft {recipe.OutputQuantity}x {recipe.Output}? (yes/no): ");
            string? confirm = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (confirm != "yes")
            {
                Console.WriteLine("Crafting cancelled.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // Consume materials
            recipe.ConsumeMaterials(player);

            // Give output
            player.Collect(recipe.Output, recipe.OutputQuantity);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ Successfully crafted {recipe.OutputQuantity}x {recipe.Output}!");
            Console.ResetColor();

            // Save player state
            player.SaveToFile();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
