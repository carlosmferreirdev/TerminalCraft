using TerminalCraft.Systems;
    /// <summary>
    /// Base class for all tools (pickaxes, weapons, etc.)
    /// </summary>
    public abstract class Tool
    {
        public ToolQuality Quality { get; set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }

        protected Tool(ToolQuality quality, int maxDurability)
        {
            Quality = quality;
            MaxDurability = maxDurability;
            Durability = maxDurability;
        }

        public string GetDurabilityBar(int barLength = 12)
        {
            double percent = MaxDurability == 0 ? 0 : (double)Durability / MaxDurability;
            int filled = (int)(barLength * percent);
            int empty = barLength - filled;
            return $"[" + new string('█', filled) + new string('░', empty) + $"] {Durability}/{MaxDurability}";
        }
    }

    /// <summary>
    /// Pickaxe tool (mining)
    /// </summary>
    public class Pickaxe : Tool
    {
        public Pickaxe(ToolQuality quality) : base(quality, quality switch
        {
            ToolQuality.Wooden => 30,
            ToolQuality.Stone => 60,
            ToolQuality.Iron => 120,
            ToolQuality.Diamond => 250,
            _ => 0
        }) { }

        public string GetName() => ToolHelper.GetToolName(Quality);
    }
namespace TerminalCraft.Systems
{
    /// <summary>
    /// Represents tool quality/tier for mining and crafting.
    /// </summary>
    public enum ToolQuality
    {
        None = 0,
        Wooden = 1,
        Stone = 2,
        Iron = 3,
        Diamond = 4
    }

    /// <summary>
    /// Helper utilities for tool-based mechanics.
    /// </summary>
    public static class ToolHelper
    {
        /// <summary>
        /// Get the mining session duration in seconds based on tool quality.
        /// </summary>
        public static int GetMiningDuration(ToolQuality tool)
        {
            return tool switch
            {
                ToolQuality.Wooden => 45,
                ToolQuality.Stone => 60,
                ToolQuality.Iron => 75,
                ToolQuality.Diamond => 90,
                _ => 0
            };
        }

        /// <summary>
        /// Get click reduction multiplier based on tool quality.
        /// Better tools require fewer clicks to mine blocks.
        /// </summary>
        public static double GetClickMultiplier(ToolQuality tool)
        {
            return tool switch
            {
                ToolQuality.Wooden => 1.0,     // 100% clicks (no reduction)
                ToolQuality.Stone => 0.75,     // 75% clicks (25% faster)
                ToolQuality.Iron => 0.5,       // 50% clicks (50% faster)
                ToolQuality.Diamond => 0.33,   // 33% clicks (67% faster)
                _ => 1.0
            };
        }

        /// <summary>
        /// Check if the player has a pickaxe of at least the given quality.
        /// </summary>
        public static ToolQuality GetPlayerPickaxeQuality(Player player)
        {
            // Preferred: Check actual pickaxe objects (with durability)
            if (player?.Pickaxes != null && player.Pickaxes.Count > 0)
            {
                ToolQuality best = ToolQuality.None;
                foreach (var pick in player.Pickaxes)
                {
                    if (pick.Durability > 0 && pick.Quality > best)
                        best = pick.Quality;
                }
                if (best != ToolQuality.None)
                    return best;
            }

            // Legacy fallback: Check inventory keys (older saves)
            if (player?.Inventory != null)
            {
                if (player.Inventory.ContainsKey("Diamond Pickaxe")) return ToolQuality.Diamond;
                if (player.Inventory.ContainsKey("Iron Pickaxe")) return ToolQuality.Iron;
                if (player.Inventory.ContainsKey("Stone Pickaxe")) return ToolQuality.Stone;
                if (player.Inventory.ContainsKey("Wooden Pickaxe")) return ToolQuality.Wooden;
            }
            return ToolQuality.None;
        }

        /// <summary>
        /// Get a user-friendly name for tool quality.
        /// </summary>
        public static string GetToolName(ToolQuality tool)
        {
            return tool switch
            {
                ToolQuality.Wooden => "Wooden Pickaxe",
                ToolQuality.Stone => "Stone Pickaxe",
                ToolQuality.Iron => "Iron Pickaxe",
                ToolQuality.Diamond => "Diamond Pickaxe",
                _ => "None"
            };
        }
    }
}
