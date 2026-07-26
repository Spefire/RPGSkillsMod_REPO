using System.Collections.Generic;

public static class SkillDatabase
{
    private class ClassSkills
    {
        public readonly Skill Primary;
        public readonly Skill Secondary;

        public ClassSkills(Skill primary, Skill secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }
    }

    private static readonly Dictionary<PlayerClass, ClassSkills> Skills = new Dictionary<PlayerClass, ClassSkills>()
    {
        {
            PlayerClass.None,
            new ClassSkills(new Skill(), new Skill())
        },
        {
            PlayerClass.Druid,
            new ClassSkills(new DruidSkill(), new NecromancerSkill())
        },
        {
            PlayerClass.Warrior,
            new ClassSkills(new WarriorSkill(), new RangerSkill())
        },
        {
            PlayerClass.Scout,
            new ClassSkills(new ScoutSkill(), new TreasureHunterSkill())
        },
        {
            PlayerClass.Paladin,
            new ClassSkills(new PaladinSkill(), new MonkSkill())
        },
        {
            PlayerClass.Mage,
            new ClassSkills(new AssassinSkill(), new BlacksmithSkill())
        }
    };

    public static Skill Get(PlayerClass playerClass, SkillSlot slot)
    {
        ClassSkills classSkills = Skills[playerClass];

        return slot == SkillSlot.Primary ? classSkills.Primary : classSkills.Secondary;
    }
}