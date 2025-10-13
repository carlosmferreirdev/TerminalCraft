// File: Root/Program.cs\n// Refactored to namespace TerminalCraft\n\nusing System;

using System.Collections.Generic;
using System.IO;
using System;
using TerminalCraft.Systems;

namespace TerminalCraft
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TerminalCraft - b 1.0.0";
            TitleScreen.Show();

            GameManager.Start();
        }
    }
}
