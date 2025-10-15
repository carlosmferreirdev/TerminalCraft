using System.Collections.Generic;

namespace TerminalCraft.Systems
{
    /// <summary>
    /// Static repository of all available crafting recipes.
    /// </summary>
    public static class CraftingRecipes
    {
        private static List<Recipe>? _allRecipes;

        public static List<Recipe> GetAllRecipes()
        {
            if (_allRecipes == null)
            {
                _allRecipes = new List<Recipe>
                {
                    // Basic Tools
                    new Recipe("Wooden Pickaxe", 
                        new Dictionary<string, int> { { "Wood", 3 } },
                        "Wooden Pickaxe", 1),

                    new Recipe("Stone Pickaxe",
                        new Dictionary<string, int> { { "Stone", 3 }, { "Wood", 2 } },
                        "Stone Pickaxe", 1),

                    new Recipe("Iron Pickaxe",
                        new Dictionary<string, int> { { "Iron Ore", 3 }, { "Wood", 2 } },
                        "Iron Pickaxe", 1),

                    new Recipe("Diamond Pickaxe",
                        new Dictionary<string, int> { { "Diamond", 3 }, { "Wood", 2 } },
                        "Diamond Pickaxe", 1),

                    // Utility items
                    new Recipe("Sticks",
                        new Dictionary<string, int> { { "Wood", 2 } },
                        "Stick", 4),

                    // Future: Add weapon recipes, armor, food processing, etc.
                };
            }
            return _allRecipes;
        }
    }
}
