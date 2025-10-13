//Entities/Mob.cs

using System;

namespace TerminalCraft
{

    // Base class for all living entities (animals, hostile mobs, etc...)
    public abstract class Mob
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsHostile { get; protected set; }

        protected Mob(string name, int health, int damage, bool isHostile)
        {
            Name = name;
            Health = health;
            Damage = damage;
            IsHostile = isHostile;
        }
        //Common entry point for interaction. Subclasses override this with their specific behavior.
        public abstract void Interact(Player player);
    }
}
