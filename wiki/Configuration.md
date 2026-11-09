# Configuration

All settings are stored in the BepInEx config file for the mod
(`BepInEx/config/com.spefire.rpgskillsmod.cfg`), generated the first time
you launch the game with the mod installed.

## General

| Setting | Default | Description |
|---|---|---|
| `EnableMod` | `true` | Enable or disable the mod entirely. |

## Controls

| Setting | Default | Description |
|---|---|---|
| `PreviousClassKey` | `F6` | Key used to select the previous class. |
| `NextClassKey` | `F8` | Key used to select the next class. |
| `SkillKey` | `F` | Key used to activate the primary skill. |
| `SkillKey2` | `R` | Key used to activate the secondary skill. |
| `ToggleSkillUIKey` | `H` | Key used to show or hide the skill UI. |

Any of these can be changed to any `KeyCode` value by editing the config
file, or through a config manager mod (e.g. BepInEx Configuration Manager)
in-game.

## Skills

Each skill has its own section (named after the skill) in the config file.
Changes require a game restart. In multiplayer, the values used are those of
the player casting the skill.

| Section | Settings (default) |
|---|---|
| `Paladin` | `Cooldown` (60), `Duration` (10), `Radius` (15) |
| `Druid` | `Cooldown` (45), `Radius` (5), `Health` (50) |
| `Scout` | `Cooldown` (60), `Duration` (15) |
| `Monk` | `Cooldown` (75), `Radius` (10), `Duration` (5, stun) |
| `Blacksmith` | `Cooldown` (90) |
| `Assassin` | `Cooldown` (60), `Duration` (15) |
| `Necromancer` | `Cooldown` (120), `HealthSacrifice` (25) |
| `Ranger` | `Cooldown` (60), `Radius` (30), `Duration` (20) |
| `Warrior` | `Cooldown` (90), `Duration` (15), `StrengthLevels` (20) |
| `TreasureHunter` | `Cooldown` (60), `Radius` (15) |

Cooldowns and durations are in seconds, radii in meters.
