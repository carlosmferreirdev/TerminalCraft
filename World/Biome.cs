// File: World/Biome.cs
// Refactored to namespace TerminalCraft

using System;
using System.Collections.Generic;

namespace TerminalCraft
{
	public class Biome
	{
		public string Name { get; private set; }
		public List<string> TreeTypes { get; private set; }
		public List<Animal> Animals { get; private set; }
		public bool AlwaysSpawnsAnimals { get; private set; }
		public double AnimalSpawnChance { get; private set; }

		private static readonly System.Random rand = new System.Random();

		public Biome(string name, List<string> treeTypes, List<Animal> animals, bool alwaysSpawnsAnimals = false, double animalSpawnChance = 0.5)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			TreeTypes = treeTypes ?? new List<string>();
			Animals = animals ?? new List<Animal>();
			AlwaysSpawnsAnimals = alwaysSpawnsAnimals;
			AnimalSpawnChance = animalSpawnChance;
		}

		public string? GetRandomTree()
		{
			if (TreeTypes.Count == 0) return null;
			return TreeTypes[rand.Next(TreeTypes.Count)];
		}

		public Animal? GetRandomAnimal()
		{
			if (Animals.Count == 0) return null;

			if (AlwaysSpawnsAnimals || rand.NextDouble() < AnimalSpawnChance)
			{
				return Animals[rand.Next(Animals.Count)];
			}

			return null;
		}
	}
}
