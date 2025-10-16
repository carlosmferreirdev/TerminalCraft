# 🎮 Minecraft Terminal Edition v1.0.0

A text-based Minecraft RPG adventure that brings the blocky world to your terminal! Explore biomes, mine resources, craft tools, tame animals, and survive the night in this immersive command-line experience.

![Minecraft Terminal Edition](https://img.shields.io/badge/version-1.0.0-green) ![.NET](https://img.shields.io/badge/.NET-8.0-purple) ![License](https://img.shields.io/badge/license-MIT-blue)

## ✨ Features

### 🌍 Dynamic World Exploration
- **9 Unique Biomes**: Forest, Jungle, Mountains, Plains, Taiga, Desert, Swamp, Snowy Tundra, and Savanna
- **Weather System**: Real-world weather integration using OpenMeteo API (with fictional fallback)
- **Day/Night Cycle**: Experience dynamic time changes that affect mob spawning
- **Biome-Specific Resources**: Each biome offers unique trees, animals, and experiences

### ⚒️ Mining & Crafting System
- **Cave Mining**: Discover rich ore veins and mine valuable resources
- **Tiered Mining**: Progress through tool tiers - Wooden → Stone → Iron → Diamond
- **Ore Restrictions**: Higher-tier ores require better pickaxes
- **Durability System**: Tools wear down with use and break when depleted
- **Crafting Recipes**: Create better tools to enhance your mining capabilities
- **Real-time Mining UI**: Watch your progress with visual feedback bars

### 🐾 Wildlife & Encounters
- **12+ Animal Species**: From peaceful cows to rare polar bears and armadillos
- **Animal Taming**: Befriend animals with the right food items
- **Hostile Mobs**: Survive encounters with zombies, skeletons, spiders, creepers, and slimes
- **Combat System**: Fight or flee when night falls and dangers emerge

### 🎒 Advanced Inventory Management
- **Organized View**: Separate menus for blocks and tools
- **Tool Grouping**: Tools displayed by type with counts and durability ranges
- **Equipped Indicator**: See which pickaxe you're currently using
- **Detailed Durability**: Visual bars showing the exact condition of each tool
- **Resource Tracking**: Keep tabs on all collected blocks and materials

### 📚 Compendium System
- **Tamed Animals Log**: Track all the creatures you've befriended
- **Achievement Progress**: View your collection and discoveries

### 💾 Save System
- **Multiple Worlds**: Create and manage different save files
- **Auto-save**: Your progress is automatically saved
- **World Persistence**: Load any world and continue your adventure

## 🚀 Getting Started

### Prerequisites
- **.NET 8.0 Runtime** or higher
- Windows, macOS, or Linux terminal
- Minimum terminal size: 80x24

### Installation

#### Option 1: Download Pre-built Release
1. Go to the [Releases](https://github.com/carlosmferreirdev/MinecraftTerminalRPG/releases) page
2. Download the latest release for your platform
3. Extract the files
4. Run the executable:
   - **Windows**: `MCT_test.exe`
   - **macOS/Linux**: `./MCT_test`

#### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/carlosmferreirdev/MinecraftTerminalRPG.git

# Navigate to the project directory
cd MinecraftTerminalRPG

# Build the project
dotnet build

# Run the game
dotnet run
```

## 🎯 How to Play

### Main Menu
1. **New World** - Create a fresh adventure
2. **Load World** - Continue a saved game
3. **Delete World** - Remove a saved world
4. **Exit** - Close the game

### In-Game Actions
1. **Explore** - Venture into randomly generated biomes
2. **View Inventory** - Check your blocks and tools
3. **Crafting** - Create new tools and items
4. **Build** - *(Coming Soon)*
5. **Compendium** - View tamed animals and achievements
6. **Save & Quit** - Save progress and return to main menu

### Gameplay Tips
- 🌙 **Avoid Night Exploration**: Hostile mobs spawn after dark
- ⛏️ **Upgrade Your Tools**: Better pickaxes mine faster and access rare ores
- 🌲 **Collect Wood First**: Essential for crafting your first tools
- 🐄 **Tame Animals**: Befriend creatures by feeding them their favorite foods
- 💎 **Mine in Caves**: 50% chance to find cave entrances while exploring
- 📦 **Manage Durability**: Keep multiple pickaxes to avoid being stuck without tools

### Crafting Recipes
- **Sticks**: 2 Wood → 4 Sticks
- **Wooden Pickaxe**: 3 Wood + 2 Sticks → 1 Wooden Pickaxe (100 durability)
- **Stone Pickaxe**: 3 Stone + 2 Sticks → 1 Stone Pickaxe (150 durability)
- **Iron Pickaxe**: 3 Iron + 2 Sticks → 1 Iron Pickaxe (250 durability)
- **Diamond Pickaxe**: 3 Diamond + 2 Sticks → 1 Diamond Pickaxe (1000 durability)

### Tool Tiers & Mining Access
- **Wooden Pickaxe**: Can mine Stone only
- **Stone Pickaxe**: Can mine Stone and Iron
- **Iron+ Pickaxe**: Can mine Stone, Iron, and Diamond

## 🛠️ Technical Details

### Built With
- **C# .NET 8.0** - Core game engine
- **System.Text.Json** - Save/load functionality
- **OpenMeteo API** - Real-world weather integration
- **Console Graphics** - ASCII art and colored text

### Project Structure
```
MinecraftTerminalRPG/
├── Entities/          # Player, Animal, Mob classes
├── Systems/           # Game logic (Mining, Crafting, Exploration)
├── World/             # Biome generation and factories
├── UI/                # Title screen and text effects
├── CreatedWorlds/     # Save files
└── README.md          # This file
```

## 🐛 Known Issues
- Weather API may timeout on slow connections (falls back to fictional weather)
- Terminal resizing during gameplay may cause display issues

## 🔮 Roadmap
- [ ] Building system implementation
- [ ] More crafting recipes (weapons, armor, tools)
- [ ] Boss encounters
- [ ] Multiplayer support
- [ ] Custom biome seeds
- [ ] Achievements system expansion

## 🤝 Contributing
Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](https://github.com/carlosmferreirdev/MinecraftTerminalRPG/issues).

## 📝 License
This project is for educational and entertainment purposes. Minecraft is a trademark of Mojang Studios. This fan project is not affiliated with or endorsed by Mojang Studios or Microsoft.

## 👤 Author
**Carlos Ferreira**
- GitHub: [@carlosmferreirdev](https://github.com/carlosmferreirdev)

## ⭐ Show Your Support
Give a ⭐️ if you enjoyed this project!

---
*Made with ❤️ and lots of mining*
