// File: UI/TitleScreen.cs\n// Refactored to namespace TerminalCraft\n\nusing System;
using System.Threading;

namespace TerminalCraft
{
    public static class TitleScreen
    {
        public static void Show()
        {
            Console.Clear();

            // Green top half (grass block)
            string[] topHalf = new[]
            {
                @"╔════════════════════════════════════════════════════╗",
                @"║░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░║",
                @"║░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░║",
                @"░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░",
            };

            // Brown bottom half (dirt block)
            string[] bottomHalf = new[]
            {
                @"║                                                    ║",
                @"║                  ░░░░░░░░░░░░░░░                   ║",
                @"║                  ░░████░░░████░░                   ║",
                @"║                  ░░████░░░████░░                   ║",
                @"║                  ░░░░░░███░░░░░░                   ║",
                @"║                  ░░░░███████░░░░                   ║",
                @"║                  ░░░░███████░░░░                   ║",
                @"║                  ░░░░██░░░██░░░░                   ║",
                @"║                                                    ║",
                @"║                 M I N E C R A F T                  ║",
                @"║                 TERMINAL EDITION                   ║",
                @"║                    - v1.0.0 -                      ║",
                @"╚════════════════════════════════════════════════════╝"
            };

            int windowWidth = Console.WindowWidth;

            // Grass (top) in Green
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (string line in topHalf)
            {
                int padding = (windowWidth - line.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(padding, 0)) + line);
            }

            // Dirt (bottom) in Dark Yellow
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (string line in bottomHalf)
            {
                int padding = (windowWidth - line.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(padding, 0)) + line);
            }

            // Final prompt in white
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            TextFx.Typewriter("Press any key to start...", delayMs: 25, center: true);
            Console.ReadKey();
            Console.Clear();
        }
    }
}


/*
                @"╔════════════════════════════════════════════════════╗",
                @"║░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░║",
                @"║░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░║",
                @"║  ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ░ ║",
                @"║                                                    ║",
                @"║                  ░░░░░░░░░░░░░░░                   ║",
                @"║                  ░░████░░░████░░                   ║",
                @"║                  ░░████░░░████░░                   ║",
                @"║                  ░░░░░░███░░░░░░                   ║",
                @"║                  ░░░░███████░░░░                   ║",
                @"║                  ░░░░███████░░░░                   ║",
                @"║                  ░░░░██░░░██░░░░                   ║",
                @"║                 M I N E C R A F T                  ║",
                @"║                 TERMINAL EDITION                   ║",
                @"║                  - BETA v1.0.0 -                   ║",
                @"╚════════════════════════════════════════════════════╝"
*/