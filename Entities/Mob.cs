// Base abstract entity type

namespace TerminalCraft
{

    public abstract class Mob
    {
        #region Properties
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsHostile { get; protected set; }
        #endregion

        #region Ctor
        protected Mob(string name, int health, int damage, bool isHostile)
        {
            Name = name;
            Health = health;
            Damage = damage;
            IsHostile = isHostile;
        }
        #endregion

        #region Interaction
        public abstract void Interact(Player player);
        #endregion
    }
}
