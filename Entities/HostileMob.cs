namespace TerminalCraft
{
    public class HostileMob : Mob, TerminalCraft.Systems.IEncounter
    {
        public double SpawnChance { get; set; }

        public HostileMob(string name, int damage = 1, int health = 5, double spawnChance = 1.0)
            : base(name, health, damage, isHostile: true)
        {
            SpawnChance = spawnChance;
        }


        public override void Interact(Player player)
        {
            // For direct interaction, fallback to encounter logic
            Trigger(player, new Random());
        }

        public void Trigger(Player player, Random rand)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"A hostile {Name} appears in the shadows!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Do you want to fight it? (yes/no): ");
            string choice = Console.ReadLine()?.ToLowerInvariant() ?? "";
            if (choice != "yes")
            {
                Console.WriteLine("You ran away safely, but missed your chance to explore.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"You bravely fought the {Name} and survived!");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        public override string ToString() => $"{Name} (DMG: {Damage}, HP: {Health})";
    }
}
