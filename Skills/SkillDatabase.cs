using System.Collections.Generic;

public static class SkillDatabase
{
    private static readonly Dictionary<PlayerClass, Skill> Skills = new Dictionary<PlayerClass, Skill>()
    {
        {
            PlayerClass.None,
            new Skill()
        },
        {
            PlayerClass.Assassin,
            new AssassinSkill()
        },
        {
            PlayerClass.Blacksmith,
            new BlacksmithSkill()
        },
        {
            PlayerClass.Druid,
            new DruidSkill()
        },
        {
            PlayerClass.Necromancer,
            new NecromancerSkill()
        },
        {
            PlayerClass.Paladin,
            new PaladinSkill()
        },
        {
            PlayerClass.Scout,
            new ScoutSkill()
        },
        {
            PlayerClass.TreasureHunter,
            new TreasureHunterSkill()
        },
        {
            PlayerClass.Warrior,
            new WarriorSkill()
        }
    };

    public static Skill Get(PlayerClass playerClass)
    {
        return Skills[playerClass];
    }
}