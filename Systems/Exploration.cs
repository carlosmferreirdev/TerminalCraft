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
                // Optional small sound cue (day brighter, night lower)
                TextFx.PlayTransitionSound(isDay);
                TextFx.Typewriter("Press any key to continue...", delayMs: 12, center: false);
                Console.ReadKey();
                Console.Clear();
            }

            // Check for hostile mobs only at night
            if (!isDay)
            {
                var mob = hostileMobs[rand.Next(hostileMobs.Count)];
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"A hostile {mob.Name} appears in the shadows!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Do you want to fight it? (yes/no): ");

                string choice = Console.ReadLine()?.ToLowerInvariant() ?? "";
                if (choice != "yes")
                {
                    Console.WriteLine("You ran away safely, but missed your chance to explore.");
                    return; // Cancel exploration
                }
                else
                {
                    Console.WriteLine($"You bravely fought the {mob.Name} and survived!");
                }
            }

            Biome? biome = weather == null ? BiomeFactory.GetRandomBiome() : BiomeFactory.GetRandomBiome(weather);
            if (biome == null)
            {
                Console.WriteLine("No biome found.");
                return;
            }
            Console.WriteLine($"You find a {biome.Name} biome.");

            // Cave logic (50% chance)
            bool hasCave = rand.NextDouble() < 0.50;

            if (hasCave)
            {
                Console.Write("You see a dark cave entrance. Explore it? (yes/no): ");
                string caveChoice = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (caveChoice == "yes")
                {
                    //Start a stopwatch to simulate time spent in cave
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    Console.WriteLine("\nYou enter the cave... it's cold and dark.");

                    //While wip
                }
                else
                {
                    Console.WriteLine("You decide to avoid the cave and continue exploring the surface.");
                }
            }

            string? tree = biome.GetRandomTree();
            if (!string.IsNullOrEmpty(tree))
            {
                Console.Write($"You see {tree} trees. Chop them down? (yes/no): ");

                if ((Console.ReadLine() ?? "").ToLowerInvariant() == "yes")
                {
                    switch (tree)
                    {
                        case "Oak":
                            player.Collect("Oak Wood", rand.Next(1, 4)); // 1-3 wood
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

            Animal? animal = biome.GetRandomAnimal();
            if (animal != null)
            {
                if (animal.Name == "Wolf" || animal.Name == "Parrot" || animal.Name == "Frog")
                {
                    Console.Write($"You see a {animal.Name}. Attempt to tame it? (yes/no): ");
                    if ((Console.ReadLine() ?? "").ToLowerInvariant() == "yes")
                    {
                        double chance = animal.Name switch
                        {
                            "Wolf" => 0.3,
                            "Parrot" => 0.5,
                            "Frog" => 0.4,
                            _ => 0
                        };

                        if (rand.NextDouble() < chance)
                        {
                            Console.WriteLine($"You successfully tamed the {animal.Name}! It will now follow you.");
                            player.TameAnimal(animal.Name);
                        }
                        else
                        {
                            Console.WriteLine($"The {animal.Name} escaped.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"You chose not to interact with the {animal.Name}.");
                    }
                }
                else
                {
                    Console.Write($"You see a {animal.Name}. Hunt it? (yes/no): ");
                    if ((Console.ReadLine() ?? "").ToLowerInvariant() == "yes")
                    {
                        if (animal.FoodYield > 0)
                        {
                            player.Collect("Food", animal.FoodYield);
                        }
                        else
                        {
                            Console.WriteLine($"The {animal.Name} doesn't drop food.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"You chose not to hunt the {animal.Name}.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No animals in sight.");
            }
        }
    }
}
