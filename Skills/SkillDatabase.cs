using System.Collections.Generic;

public static class SkillDatabase
{
    private static readonly Dictionary<PlayerClass, Skill> Skills = new Dictionary<PlayerClass, Skill>()
    {
        {
            PlayerClass.None,
            new Skill(
                "No Skill",
                "",
                new List<string>(),
                0)
        },
        {
            PlayerClass.Necromancer,
            new Skill(
                "Resurrection",
                "Revive a fallen ally by sacrificing part of your own health.",
                new List<string>
                {
                    "Health Cost: 50 HP",
                    "Target: Dead Ally",
                    "Cooldown: 60 s"
                },
                60)
        },
        {
            PlayerClass.Druid,
            new Skill(
                "Nature's Blessing",
                "Heal nearby allies within a small radius.",
                new List<string>
                {
                    "Heal Radius: 8 m",
                    "Affects nearby allies",
                    "Cooldown: 25 s"
                },
                25)
        },
        {
            PlayerClass.Assassin,
            new Skill(
                "Shadow Veil",
                "Become invisible and undetectable for a short duration.",
                new List<string>
                {
                    "Duration: 8 s",
                    "Breaks on attack",
                    "Cooldown: 45 s"
                },
                45)
        },
        {
            PlayerClass.Scout,
            new Skill(
                "Adrenaline Rush",
                "Gain unlimited stamina for a short duration.",
                new List<string>
                {
                    "Infinite Stamina",
                    "Duration: 12 s",
                    "Cooldown: 30 s"
                },
                30)
        },
        {
            PlayerClass.Warrior,
            new Skill(
                "Battle Rage",
                "Greatly increase your strength for a short duration.",
                new List<string>
                {
                    "Increased Strength",
                    "Duration: 15 s",
                    "Cooldown: 45 s"
                },
                45)
        },
        {
            PlayerClass.Alchemist,
            new Skill(
                "Transmutation",
                "Duplicate the item currently held in your hands.",
                new List<string>
                {
                    "Duplicates held item",
                    "Single use per activation",
                    "Cooldown: 60 s"
                },
                60)
        },
        {
            PlayerClass.Guardian,
            new Skill(
                "Taunt",
                "Force nearby enemies to focus their attention on you.",
                new List<string>
                {
                    "Taunt Radius: 15 m",
                    "Duration: 8 s",
                    "Cooldown: 35 s"
                },
                35)
        }
    };

    public static Skill Get(PlayerClass playerClass)
    {
        return Skills[playerClass];
    }
}