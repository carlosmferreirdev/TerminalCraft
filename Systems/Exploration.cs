// File: Systems/Exploration.cs
// Refactored to namespace TerminalCraft

using System;
using System.Collections.Generic;
using TerminalCraft.Systems;

namespace TerminalCraft
{
    public static class Exploration
    {
        public static void Explore(Player player, Random rand, ref int tickCount, ref bool isDay, List<HostileMob> hostileMobs, Systems.Weather.WeatherContext? weather = null)
        {
            Console.Clear();

            // Increase tick count and toggle day/night every 10 ticks
            tickCount++;
            if (tickCount >= 10)
            {
                isDay = !isDay;
                tickCount = 0;

                Console.Clear();
                Console.ForegroundColor = isDay ? ConsoleColor.Yellow : ConsoleColor.DarkBlue;
                string transitionMsg = isDay ? "The sun rises. It's now daytime." : "Night falls. The world darkens...";
                Console.WriteLine("== Time Transition ==\n");
                TextFx.Typewriter(transitionMsg + "\n", delayMs: 20, center: false);
                Console.ResetColor();
                TextFx.Typewriter("Press any key to continue...", delayMs: 12, center: false);
                Console.ReadKey();
                Console.Clear();
            }


            // Build encounter list for this exploration
            var encounters = new List<IEncounter>();
            if (!isDay && hostileMobs.Count > 0)
                encounters.Add(hostileMobs[rand.Next(hostileMobs.Count)]);

            Biome? biome = weather == null ? BiomeFactory.GetRandomBiome() : BiomeFactory.GetRandomBiome(weather);
            if (biome == null)
            {
                Console.WriteLine("No biome found.");
                return;
            }
            Console.WriteLine($"You find a {biome.Name} biome.");

            Animal? animal = biome.GetRandomAnimal();
            if (animal != null)
                encounters.Add(animal);

            // Add more IEncounter types here (e.g., CaveEncounter, TreasureEncounter)

            if (encounters.Count > 0)
            {
                var encounter = encounters[rand.Next(encounters.Count)];
                encounter.Trigger(player, rand);
                // If the encounter ends exploration (e.g., running from mob), return early
                if (encounter is HostileMob) return;
            }

            // Cave logic (50% chance)
            bool hasCave = rand.NextDouble() < 0.50;
            if (hasCave)
            {
                Console.Write("You see a dark cave entrance. Explore it? (yes/no): ");
                string caveChoice = Console.ReadLine()?.Trim().ToLower() ?? "";
                if (caveChoice == "yes")
                {
                    Console.WriteLine("\nYou enter the cave... it's cold and dark.");
                    // Trigger mining encounter
                    var miningEncounter = new MiningEncounter();
                    miningEncounter.Trigger(player, rand);
                }
                else
                {
                    Console.WriteLine("You decide to avoid the cave and continue exploring the surface.");
                }
            }

            string? tree = biome.GetRandomTree();
            if (!string.IsNullOrEmpty(tree))
            {
                // Special-case vegetation names that aren't really trees
                string vegetation = tree switch
                {
                    "Cactus" => "Cacti",
                    "Tall Mushroom" => "Tall Mushrooms",
                    _ => $"{tree} trees"
                };
                string action = (tree == "Cactus" || tree == "Tall Mushroom") ? "Harvest them" : "Chop them down";
                Console.Write($"You see {vegetation}. {action}? (yes/no): ");
                if ((Console.ReadLine() ?? "").ToLowerInvariant() == "yes")
                {
                    switch (tree)
                    {
                        case "Oak":
                            player.Collect("Oak Wood", rand.Next(1, 4));
                            if (rand.NextDouble() < 0.3)
                            {
                                Console.WriteLine("You salvaged an apple from the oak tree!");
                                player.Collect("Food", 1);
                            }
                            break;
                        case "Birch":
                            player.Collect("Birch Wood", rand.Next(1, 3));
                            break;
                        case "Jungle Tree":
                            player.Collect("Jungle Wood", rand.Next(2, 5));
                            break;
                        case "Tall Mushroom":
                            player.Collect("Mushroom", rand.Next(1, 3));
                            break;
                        case "Spruce":
                            player.Collect("Spruce Wood", rand.Next(1, 4));
                            break;
                        case "Cactus":
                            player.Collect("Cactus", rand.Next(1, 3));
                            break;
                        case "Mangrove":
                            player.Collect("Mangrove Wood", rand.Next(2, 4));
                            break;
                        case "Acacia":
                            player.Collect("Acacia Wood", rand.Next(1, 3));
                            break;
                        default:
                            player.Collect("Wood", 1);
                            break;
                    }
                }
            }

            // Pause before returning to the game loop to avoid instant clear
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
