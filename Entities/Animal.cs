using System;
using System.Collections.Generic;

namespace TerminalCraft
{
    public class Animal : Mob, TerminalCraft.Systems.IEncounter
    {
        public int FoodYield { get; private set; }

        private static readonly HashSet<string> TamableAnimals = new() { "Wolf", "Parrot", "Frog" };

        public Animal(string name, int foodYield)
            : base(name, health: 10, damage: 0, isHostile: false)
        {
            FoodYield = foodYield;
        }

        public override void Interact(Player player)
        {
            // For direct interaction, fallback to encounter logic
            Trigger(player, new Random());
        }

        public void Trigger(Player player, Random rand)
        {
            if (TamableAnimals.Contains(Name))
            {
                Console.Write($"You see a {Name}. Would you like to tame it? (yes/no): ");
                string? choice = Console.ReadLine()?.Trim();
                if (string.Equals(choice, "yes", StringComparison.OrdinalIgnoreCase))
                    player.TameAnimal(Name);
                else
                    Console.WriteLine($"You decide not to tame the {Name}.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.Write($"You see a {Name}. Hunt it? (yes/no): ");
                string? choice = Console.ReadLine()?.Trim();
                if (string.Equals(choice, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    if (FoodYield > 0)
                        player.Collect("Food", FoodYield);
                    else
                        Console.WriteLine($"The {Name} doesn't drop food.");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
