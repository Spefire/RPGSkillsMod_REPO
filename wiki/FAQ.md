# FAQ / Troubleshooting

### How do I select a class?

Select your class in the shop or lobby using the class selection HUD. Use
`F6` / `F8` (default) to cycle between classes.

### How do I use a skill?

Press the primary skill key (`F` by default) for your class's first skill,
or the secondary skill key (`G` by default) for the second. The HUD shows
each skill's name, description, properties and remaining cooldown.

### My skill doesn't seem to do anything

Some skills require a condition to succeed (e.g. Reinforce requires you to
be holding an item, Raise Dead requires being downed or holding a dead
ally's severed head). Check the in-game chat/log for a message explaining
why the skill failed.

### The skill HUD is too big/intrusive, can I hide it?

Yes, press `H` (default, configurable via `ToggleSkillUIKey`) to show or
hide the skill panels (name, description, properties, cooldown) at any
time. Hiding the HUD is purely visual and doesn't affect cooldowns or
skill usage.

### Can I disable the mod without uninstalling it?

Yes, set `EnableMod` to `false` in the config file (see
[Configuration](https://github.com/Spefire/RPGSkillsMod_REPO/blob/master/wiki/Configuration.md)).

### Where is my class saved?

- **Host:** the class is stored per save file, next to the game's own save.
- **Client:** the host's save is not yours, so your class is stored in a local
  file on your machine (`rpgSkillsMod_class.es3` in the game's persistent data
  folder) and is restored every time you launch the game.

### Is this mod multiplayer-safe?

Skills are designed to work in multiplayer sessions, but some of them rely on
the host, so **the host must have the mod installed**. If the host doesn't, the
affected skills are refused (without consuming their cooldown) and a warning is
logged in the BepInEx console.

### I found a bug, where do I report it?

- Open an issue: [GitHub Issues](https://github.com/Spefire/RPGSkillsMod_REPO/issues)
- Contact directly via [Gmail](mailto:spefire@gmail.com) or [Ko-fi](https://ko-fi.com/spefire)

See the [Changelog](https://github.com/Spefire/RPGSkillsMod_REPO/blob/master/CHANGELOG.md)
for the full history of changes, fixes and known issues per version.
