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
                @"║                  - BETA v1.0.0 -                   ║",
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
            ShowTypewriter("Press any key to start...", delayMs: 25, center: true);
            Console.ReadKey();
            Console.Clear();
        }

        private static void ShowTypewriter(string text, int delayMs = 30, bool center = false)
        {
            int padding = 0;
            if (center)
                padding = (Console.WindowWidth - text.Length) / 2;

            Console.Write(new string(' ', Math.Max(padding, 0)));

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
        }
    }
}


/*
string[] topHalf = new[]
            {
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

            };

            // Brown bottom half (dirt block)
            string[] bottomHalf = new[]
            {
                @"║                                                    ║",
                @"║           TERMINAL EDITION - BETA v1.0.0           ║",
                @"║                                                    ║",
                @"╚════════════════════════════════════════════════════╝"
            };
            */