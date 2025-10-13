// File: World/BiomeFactory.cs\n// Refactored to namespace TerminalCraft\n\nusing System;
using System.Collections.Generic;

namespace TerminalCraft
{
    static class BiomeFactory
    {
        public static Biome GetRandomBiome()
        {
            var biomes = GetAllBiomes();
            return biomes[new System.Random().Next(biomes.Count)];
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
