using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace TerminalCraft.Systems
{
    /// <summary>
    /// Mining encounter that allows the player to mine for resources if they have the proper tools.
    /// </summary>
    public class MiningEncounter : IEncounter
    {
        private static readonly Random rand = new Random();

        // Base clicks required per ore type
        private const int StoneClicks = 10;
        private const int IronClicks = 25;
        private const int DiamondClicks = 50;

        // Mining session tracking
        private class MinedBlock
        {
            public string Type { get; set; } = "";
            public ConsoleColor Color { get; set; }
        }

        // Track current pickaxe quality for ore filtering
        private ToolQuality _currentPickaxeQuality;

        public void Trigger(Player player, Random rand)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n=== Cave Mining ===");
            Console.ResetColor();
            Console.WriteLine("You find a rich vein of stone in the cave walls.");

            // Select a usable pickaxe (highest quality with durability > 0)
            var pickaxe = player.Pickaxes
                .Where(p => p.Durability > 0)
                .OrderByDescending(p => p.Quality)
                .FirstOrDefault();

            if (pickaxe == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou're unable to mine, as the rock solid stone is too much for your bare hands.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("If only I had some kind of a tool...");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nYou equip your {pickaxe.GetName()}.");
            Console.ResetColor();
            Console.Write("Start mining? (yes/no): ");

            string? choice = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (choice != "yes")
            {
                Console.WriteLine("You decide to leave the ore for now.");
                return;
            }

            StartMiningSession(player, pickaxe);
        }

        private void StartMiningSession(Player player, Pickaxe pickaxe)
        {
            int duration = ToolHelper.GetMiningDuration(pickaxe.Quality);
            double clickMultiplier = ToolHelper.GetClickMultiplier(pickaxe.Quality);
            _currentPickaxeQuality = pickaxe.Quality; // Store for ore filtering
            
            Console.Clear();
            
            var stopwatch = Stopwatch.StartNew();
            var minedBlocks = new List<MinedBlock>();
            
            string currentOre = SelectRandomOre();
            int requiredClicks = GetRequiredClicks(currentOre, clickMultiplier);
            int currentClicks = 0;

            while (stopwatch.Elapsed.TotalSeconds < duration)
            {
                // Display mining UI
                DisplayMiningUI(stopwatch, duration, minedBlocks, currentOre, currentClicks, requiredClicks);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nYou stop mining early.");
                        Console.ResetColor();
                        break;
                    }
                    else if (key.Key == ConsoleKey.Spacebar)
                    {
                        currentClicks++;
                        // Check if block is fully mined
                        if (currentClicks >= requiredClicks)
                        {
                            // Add to inventory and mined blocks list (no save per block)
                            player.Collect(currentOre, 1);
                            minedBlocks.Add(new MinedBlock 
                            { 
                                Type = currentOre, 
                                Color = GetOreColor(currentOre) 
                            });
                            // Decrease tool durability per mined block
                            if (pickaxe.Durability > 0)
                                pickaxe.Durability--;
                            if (pickaxe.Durability <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"\nYour {pickaxe.GetName()} broke!");
                                Console.ResetColor();
                                // Remove broken pickaxe from inventory
                                var idx = player.Pickaxes.IndexOf(pickaxe);
                                if (idx >= 0) player.Pickaxes.RemoveAt(idx);
                                break; // end mining session on break
                            }
                            // Start new block
                            currentOre = SelectRandomOre();
                            requiredClicks = GetRequiredClicks(currentOre, clickMultiplier);
                            currentClicks = 0;
                        }
                    }
                }

                Thread.Sleep(50); // Smooth refresh rate
            }

            // Save player data once at the end of mining session
            player.SaveToFile();

            stopwatch.Stop();
            DisplayMiningComplete(minedBlocks, stopwatch.Elapsed.TotalSeconds);
        }

        private void DisplayMiningUI(Stopwatch stopwatch, int duration, List<MinedBlock> minedBlocks, string currentOre, int currentClicks, int requiredClicks)
        {
            // Only clear the mining UI area, not the whole console, to reduce flashing
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, 0);
            
            // Timer at top
            int remaining = duration - (int)stopwatch.Elapsed.TotalSeconds;
            Console.ForegroundColor = remaining < 10 ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.WriteLine($"╔════════════════════════════════════════╗");
            Console.Write($"║ ⏱  Time Remaining: ");
            Console.ForegroundColor = remaining < 10 ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.Write($"{remaining:D2}s");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                 ║");
            Console.WriteLine($"╚════════════════════════════════════════╝");
            Console.ResetColor();
            
            // Mined blocks display
            Console.WriteLine("\n=== Blocks Mined ===");
            if (minedBlocks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("None yet...");
                Console.ResetColor();
            }
            else
            {
                var stoneCount = minedBlocks.FindAll(b => b.Type == "Stone").Count;
                var ironCount = minedBlocks.FindAll(b => b.Type == "Iron Ore").Count;
                var diamondCount = minedBlocks.FindAll(b => b.Type == "Diamond").Count;

                if (stoneCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Stone: {stoneCount}");
                }
                if (ironCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Iron Ore: {ironCount}");
                }
                if (diamondCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Diamond: {diamondCount}");
                }
                Console.ResetColor();
            }

            // Current block progress
            Console.WriteLine($"\n=== Current Block ===");
            Console.ForegroundColor = GetOreColor(currentOre);
            Console.WriteLine($"Mining: {currentOre}");
            Console.ResetColor();
            
            // Progress bar
            double progress = (double)currentClicks / requiredClicks;
            int barWidth = 30;
            int filled = (int)(barWidth * progress);
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('█', filled));
            Console.ResetColor();
            Console.Write(new string('░', barWidth - filled));
            Console.WriteLine($"] {currentClicks}/{requiredClicks}");
            
            Console.WriteLine("\n[SPACE] Mine  [ESC] Exit");
            
            // Clear remaining lines to prevent flicker
            for (int i = 0; i < 5; i++)
                Console.WriteLine(new string(' ', Console.WindowWidth - 1));
        }

        private void DisplayMiningComplete(List<MinedBlock> minedBlocks, double elapsedSeconds)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n╔════════════════════════════════════════╗");
            Console.WriteLine($"║        Mining Session Complete!        ║");
            Console.WriteLine($"╚════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine($"\nTime elapsed: {elapsedSeconds:F1}s");
            Console.WriteLine($"Total blocks mined: {minedBlocks.Count}");
            
            if (minedBlocks.Count > 0)
            {
                Console.WriteLine("\n=== Haul Summary ===");
                var stoneCount = minedBlocks.FindAll(b => b.Type == "Stone").Count;
                var ironCount = minedBlocks.FindAll(b => b.Type == "Iron Ore").Count;
                var diamondCount = minedBlocks.FindAll(b => b.Type == "Diamond").Count;

                if (stoneCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"• Stone: {stoneCount}");
                }
                if (ironCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"• Iron Ore: {ironCount}");
                }
                if (diamondCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"• Diamond: {diamondCount}");
                }
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private string SelectRandomOre()
        {
            // Filter ores based on pickaxe quality
            // Wooden: Only Stone
            // Stone: Stone + Iron
            // Iron+: Stone + Iron + Diamond
            
            List<(string ore, double weight)> availableOres = new List<(string, double)>();
            
            // Stone is always available
            availableOres.Add(("Stone", 80.0));
            
            // Iron requires at least Stone pickaxe
            if (_currentPickaxeQuality >= ToolQuality.Stone)
            {
                availableOres.Add(("Iron Ore", 15.0));
            }
            
            // Diamond requires at least Iron pickaxe
            if (_currentPickaxeQuality >= ToolQuality.Iron)
            {
                availableOres.Add(("Diamond", 5.0));
            }
            
            // Calculate total weight and select random ore
            double totalWeight = availableOres.Sum(o => o.weight);
            double roll = rand.NextDouble() * totalWeight;
            double cumulative = 0;
            
            foreach (var ore in availableOres)
            {
                cumulative += ore.weight;
                if (roll <= cumulative)
                    return ore.ore;
            }
            
            return "Stone"; // Fallback
        }

        private int GetRequiredClicks(string ore, double multiplier)
        {
            int baseClicks = ore switch
            {
                "Stone" => StoneClicks,
                "Iron Ore" => IronClicks,
                "Diamond" => DiamondClicks,
                _ => StoneClicks
            };
            return (int)Math.Ceiling(baseClicks * multiplier);
        }

        private ConsoleColor GetOreColor(string ore)
        {
            return ore switch
            {
                "Stone" => ConsoleColor.DarkGray,
                "Iron Ore" => ConsoleColor.Gray,
                "Diamond" => ConsoleColor.Cyan,
                _ => ConsoleColor.White
            };
        }
    }
}
