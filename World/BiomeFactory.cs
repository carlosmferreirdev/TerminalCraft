// File: World/BiomeFactory.cs\n// Refactored to namespace TerminalCraft\n\nusing System;
using System.Collections.Generic;

namespace TerminalCraft
{
    static class BiomeFactory
    {
        private static readonly System.Random _rand = new System.Random();

        public static Biome GetRandomBiome()
        {
            var biomes = GetAllBiomes();
            return biomes[_rand.Next(biomes.Count)];
        }

        // New: Weather-weighted biome selection
        public static Biome GetRandomBiome(Systems.Weather.WeatherContext weather)
        {
            var biomes = GetAllBiomes();

            // Base weights
            var weights = new Dictionary<string, double>();
            foreach (var b in biomes) weights[b.Name] = 1.0; // equal baseline

            // Temperature influences
            if (weather.IsCold)
            {
                AddWeight(weights, "Snowy Tundra", 4);
                AddWeight(weights, "Taiga", 2.5);
                AddWeight(weights, "Mountains", 1.5);
                ReduceWeight(weights, "Desert", 0.2);
                ReduceWeight(weights, "Savanna", 0.3);
            }
            else if (weather.IsCool)
            {
                AddWeight(weights, "Taiga", 1.5);
                AddWeight(weights, "Mountains", 1.2);
                ReduceWeight(weights, "Desert", 0.5);
            }
            else if (weather.IsWarm)
            {
                AddWeight(weights, "Forest", 1.3);
                AddWeight(weights, "Plains", 1.2);
                AddWeight(weights, "Jungle", 1.1);
            }
            else if (weather.IsHot)
            {
                AddWeight(weights, "Desert", 3.0);
                AddWeight(weights, "Savanna", 2.0);
                ReduceWeight(weights, "Snowy Tundra", 0.2);
                ReduceWeight(weights, "Taiga", 0.4);
            }

            // Precipitation influences
            if (weather.IsWet)
            {
                AddWeight(weights, "Swamp", 2.5);
                AddWeight(weights, "Jungle", 1.7);
                ReduceWeight(weights, "Desert", 0.5); // rain lowers desert likelihood
            }

            // Build cumulative list
            double total = 0;
            var cumulative = new List<(Biome biome, double upto)>();
            foreach (var b in biomes)
            {
                total += weights[b.Name];
                cumulative.Add((b, total));
            }

            double roll = _rand.NextDouble() * total;
            foreach (var entry in cumulative)
            {
                if (roll <= entry.upto)
                    return entry.biome;
            }
            return biomes[0]; // fallback
        }

        private static void AddWeight(Dictionary<string, double> weights, string biome, double add)
        {
            if (weights.ContainsKey(biome)) weights[biome] += add;
        }
        private static void ReduceWeight(Dictionary<string, double> weights, string biome, double factor)
        {
            if (weights.ContainsKey(biome)) weights[biome] *= factor;
        }

        public static List<Biome> GetAllBiomes()
        {
            return new List<Biome>
            {
                //Each biome has a diferent chance of spawning trees and animals

                new Biome("Forest",
                    new List<string> { "Oak", "Birch" },
                    new List<Animal> { new Animal("Pig", 2), new Animal("Chicken", 1), new Animal("Cow", 3) },
                    false, 0.6), // 60% chance for trees and animals

                new Biome("Jungle",
                    new List<string> { "Jungle Tree", "Tall Mushroom" },
                    new List<Animal> { new Animal("Parrot", 1), new Animal("Chicken", 1) },
                    false, 0.5), // Moderate chance for trees and animals

                new Biome("Mountains",
                    new List<string>(),
                    new List<Animal> { new Animal("Goat", 2) },
                    false, 0.4), // Low chance for trees

                new Biome("Plains",
                    new List<string> { "Oak" },
                    new List<Animal> { new Animal("Cow", 3), new Animal("Sheep", 2) },
                    false, 0.7), // High chance for trees

                new Biome("Taiga",
                    new List<string> { "Spruce" },
                    new List<Animal> { new Animal("Wolf", 0) },
                    true), // Always spawn animals

                new Biome("Desert",
                    new List<string> { "Cactus"},
                    new List<Animal>(),
                    false, 0.0), // No trees or animals

                new Biome("Swamp",
                    new List<string> { "Oak", "Mangrove" },
                    new List<Animal> { new Animal("Frog", 0), new Animal("Chicken", 1) },
                    false, 0.5), // Moderate chance for trees and animals

                new Biome("Snowy Tundra",
                    new List<string> { "Spruce" },
                    new List<Animal> { new Animal("Polar Bear", 5), new Animal("Rabbit", 1) },
                    false, 0.3), // Low chance for trees and animals


                new Biome("Savanna",
                    new List<string> { "Acacia" },
                    new List<Animal> { new Animal("Armadillo", 0), new Animal("Cow", 3) },
                    false, 0.6), // Moderate chance for trees and animals
            };
        }
    }
}
