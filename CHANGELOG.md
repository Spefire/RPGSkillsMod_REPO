# Changelog

All notable changes to this project will be documented in this file.

The format is based on **Keep a Changelog**, and this project follows **Semantic Versioning**.

---

## [0.4.0] - Reliability pass

### Changed

* Reworked the Paladin's *Divine Shield* into **Divine Taunt**: still grants temporary invulnerability, but now also forces nearby enemies to focus the caster.
* Reworked the Warrior's *Berserk* to use the same permanent strength/throw upgrade mechanism as the in-game shop items, instead of a temporary stat override that didn't always stick.
* Added a status column (✅ OK / 🧪 Testing / ❌ Broken) to the class/skill table in the README for testing transparency.

### Fixed

* Fixed the Scout's *Best Runner* hardcoding energy to 100 instead of the player's real max, which caused an incorrect HUD readout and leftover excess stamina after the skill ended.

### Known Issues

* Assassin's *Shadow Step* only prevents new enemy detections; it doesn't un-taunt enemies that are already chasing the caster when the skill is used.

---

## [0.3.0] - Active skills release

### Added

* Implemented active skills for every class (previously just a framework with no effect).
* Added the **Paladin** class (*Divine Shield*: temporary invulnerability to all damage).
* Added the **Treasure Hunter** class (*Treasure Sense*: reveals nearby valuables).
* Added the **Blacksmith** class, replacing Alchemist (*Reinforce*: makes the held item unbreakable).
* Skill activation is now announced in the in-game chat.

### Fixed

* Fixed skills' `Execute`/`Update` logic not running for some classes (e.g. Warrior).

---

## [0.2.0] - Config and UI improvements

### Added

* Added a configuration file using BepInEx.
* Added configurable keybinds
* Added an option to enable or disable the mod.
* Added support for future configuration options.

### Changed

* Reworked the skill UI.
* The skill activation key is now displayed when the skill is ready.

### Fixed

* Fixed several UI initialization issues.
* Improved UI stability and cleanup.

---

## [0.1.0] - Initial Preview

### Added

* Added the class system.
* Added 8 selectable classes:

  * Necromancer
  * Druid
  * Warrior
  * Assassin
  * Scout
  * Guardian
  * Alchemist
  * None
* Added class selection UI in the Shop and Lobby.
* Added an in-game class HUD.
* Added class persistence through the game's save system.
* Added the framework for active class skills.
* Added cooldown infrastructure for future skills.

### Notes

* Active skills are **not implemented yet**.
* This is an early preview release intended for testing and feedback.
* Existing saves should remain compatible with future updates.
