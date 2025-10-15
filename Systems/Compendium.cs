// File: Systems/Compendium.cs
// Refactored to namespace TerminalCraft

using System;
using System.Collections.Generic;

namespace TerminalCraft
{
    public static class Compendium
    {
        private static readonly List<string> AllTamableAnimals = new()
        {
            "Wolf",
            "Parrot",
            "Frog",
            "Ocelot"
        };

    public static void Show(Player player)
    {
            Console.Clear();

            bool isComplete = player.TamedAnimals.Count >= AllTamableAnimals.Count;
            Console.ForegroundColor = isComplete ? ConsoleColor.Yellow : ConsoleColor.White;

            string header = isComplete ? "Compendium (COMPLETED!)" : "Compendium";
            Console.WriteLine(header);
            Console.WriteLine(new string('=', header.Length));
            Console.ResetColor();

            if (player.TamedAnimals.Count == 0)
                Console.WriteLine("You haven't tamed any animals yet.");
            else
                foreach (var animal in player.TamedAnimals)
                    Console.WriteLine("- " + animal);

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
