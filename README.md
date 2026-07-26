# RPG Skills Mod

Adds RPG classes to R.E.P.O., each with a unique active skill on a cooldown.

![Status](https://img.shields.io/badge/Status-In%20Development-orange)
![Testing](https://img.shields.io/badge/Testing-Active-yellow)

> [!WARNING]
> This plugin is currently under active development and testing.
> Features, balance, and behavior may change between versions.

## Features

- 8 playable classes, each with a unique active skill (plus a "None" option)
- Cooldown system with an in-game HUD showing readiness / remaining time
- Class selection in the shop and lobby, with an in-level HUD
- Configurable keybinds and an on/off toggle (BepInEx config)
- Save compatibility: your selected class is stored per save file

## Classes & Skills

| Class | Skill | Effect | Status |
|---|---|---|---|
| Warrior | Berserk | Temporarily increases your strength. | ✅ OK |
| Druid | Nature's Blessing | Heals nearby allies within a small radius, sharing a fixed pool of health. | 🧪 Testing |
| Necromancer | Raise Dead | Sacrifices your own health to revive yourself, or a fallen ally whose severed head you're holding. | 🧪 Testing |
| Assassin | Phantom | Become invisible and undetectable by enemies for a short time, floating like a ghost. | ❌ Broken |
| Scout | Best Runner | Grants infinite stamina for a short duration. | ✅ OK  |
| Paladin | Divine Taunt | Grants temporary invulnerability to all damage, but forces nearby enemies to focus you. | ✅ OK |
| Blacksmith | Reinforce | Makes the item you're currently holding unbreakable for as long as you keep holding it. | ✅ OK |
| Treasure Hunter | Treasure Sense | Reveals valuable items within a radius around you. | ✅ OK |

## Controls

- F  -> Use skill
- F6 -> Previous class
- F8 -> Next class

These keybinds, as well as enabling/disabling the mod, can be changed in the BepInEx configuration file.

## Screenshots

![Class Selection HUD](https://raw.githubusercontent.com/Spefire/RPGSkillsMod_REPO/refs/heads/master/assets/class-selection-preview.png)
![Skill Cooldown HUD](https://raw.githubusercontent.com/Spefire/RPGSkillsMod_REPO/refs/heads/master/assets/skill-cooldown-preview.png)

## Contact, Bugs & Ideas

If you find a bug, have a suggestion, or want to share feedback:

- Open an issue: [GitHub Issues](https://github.com/Spefire/RPGSkillsMod_REPO/issues)
- Contact me directly via [Gmail](mailto:spefire@gmail.com) or [Ko-fi](https://ko-fi.com/spefire)
