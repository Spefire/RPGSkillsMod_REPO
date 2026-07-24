# Changelog

All notable changes to this project will be documented in this file.

The format is based on **Keep a Changelog**, and this project follows **Semantic Versioning**.

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
