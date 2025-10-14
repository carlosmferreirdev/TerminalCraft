using System;
using System.Threading;

namespace TerminalCraft
{
    /// <summary>
    /// Utility text & simple sound effects for console presentation.
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

        /// <summary>
        /// Plays a simple pitched console beep to indicate transition.
        /// Day: higher pitch, Night: lower pitch. Swallows exceptions if beeps aren't supported.
        /// </summary>
        public static void PlayTransitionSound(bool isDay)
        {
            try
            {
                // Frequency bounds for typical console beep (37 - 32767)
                int freq = isDay ? 1200 : 400;
                Console.Beep(freq, 140);
                // Add second softer accent
                int accent = isDay ? 1600 : 300;
                Console.Beep(accent, 90);
            }
            catch { /* Ignore on platforms without beep access */ }
        }

        /// <summary>
        /// Simple one-shot notification beep (neutral tone).
        /// </summary>
        public static void Notify()
        {
            try { Console.Beep(900, 120); } catch { }
        }
    }
}
