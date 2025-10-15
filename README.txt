# TerminalCraft

This project was refactored from the original 'MinecraftTerminalRPG' project and is serving as a canvas for me to relearn and practice coding in C#.
I will try and implement as many functionalities as possible while also trying to stay somewhat faithful to the original game we all know and love.

Key changes:
- Namespace renamed to 'TerminalCraft'.
- Old Program.cs replaced with a minimal entry point for the program to run.
- Added GameManager.cs to handle menus, world management and the main loop.
- Added Compendium.cs to handle tamed animals tracking and display.
- Files reorganized into folders: Entities, World, Systems, UI and CreatedWorlds for clarity.

Files and locations:
- /Program.cs
- /Entities/Animal.cs
- /Entities/HostileMob.cs
- /Entities/Player.cs
- /World/Biome.cs
- /World/BiomeFactory.cs
- /Systems/Exploration.cs
- /Systems/GameManager.cs
- /Systems/Compendium.cs
- /UI/TitleScreen.cs

Build notes:
- Drop the folder into a C# project, or adjust your .csproj to include the files.
- No logic changes beyond namespacing and modularization; runtime behavior should remain the same.
