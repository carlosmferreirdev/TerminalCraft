using System;
using System.Threading;

namespace TerminalCraft
{
    /// <summary>
    /// Utility text for console presentation.
    /// </summary>
    public static class TextFx
    {
        /// <summary>
        /// Writes text with a typewriter effect.
        /// </summary>
        public static void Typewriter(string text, int delayMs = 25, bool center = false)
        {
            if (string.IsNullOrEmpty(text)) return;
            int padding = 0;
            if (center)
                padding = (Console.WindowWidth - text.Replace("\n", "").Length) / 2;
            if (padding > 0)
                Console.Write(new string(' ', padding));

            foreach (char c in text)
            {
                Console.Write(c);
                if (!char.IsWhiteSpace(c))
                    Thread.Sleep(delayMs);
            }
        }
    }
}
