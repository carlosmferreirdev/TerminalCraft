using System;
// File: Entities/HostileMob.cs
// Updated to inherit from Mob

namespace TerminalCraft
{
    public class HostileMob : Mob
    {
        public double SpawnChance { get; set; }

        public HostileMob(string name, int damage = 1, int health = 5, double spawnChance = 1.0)
            : base(name, health, damage, isHostile: true)
        {
            SpawnChance = spawnChance;
        }

        public override void Interact(Player player)
        {
            // Placeholder for future combat system
            Console.WriteLine($"The {Name} growls menacingly...");
        }

        public override string ToString()
        {
            return $"{Name} (DMG: {Damage}, HP: {Health})";
        }
    }
}
