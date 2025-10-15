# TerminalCraft - Mob System Refactor Addendum

This update introduces a new shared base class for all living entities in the game.

## What's New
- Added **Mob.cs** under `/Entities` as a common parent class for both `Animal` and `HostileMob`.
- Updated both classes to inherit from `Mob`, enabling shared properties like:
  - `Name`
  - `Health`
  - `Damage`
  - `IsHostile` (future proof for being able to turn animals agressive. Ex: Polar bears)
- The abstract method `Interact(Player player)` now ensures consistent interaction entry points.

## Why
This sets the stage for a unified entity system where future features (like combat or neutral mobs) can be implemented easily without rewriting existing logic.

## Behavior
All current interactions (taming, hunting, etc.) remain exactly the same.
There are no functional changes — only the architecture was improved, mostly for future proofing.

## Future Possibilities
- Implement shared combat logic (`Attack`, `TakeDamage`)
- Add neutral mobs (e.g., villagers)
- Simplify exploration encounters via polymorphism
