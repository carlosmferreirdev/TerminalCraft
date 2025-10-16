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
            
            // Ensure player data is saved if the app is closed unexpectedly
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;

            TitleScreen.Show();
            Console.Clear();
            GameManager.Start();
        }
        
        private static void OnProcessExit(object? sender, EventArgs e)
        {
            // Save player data on unexpected exit
            GameManager.SavePlayerOnExit();
        }
        
        private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            // Save player data when Ctrl+C is pressed
            GameManager.SavePlayerOnExit();
        }
    }
}
