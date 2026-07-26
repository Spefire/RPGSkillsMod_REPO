internal class AssassinSkill : Skill
{
    private const float Duration = 15f;

    public AssassinSkill()
    {
        Name = "Shadow Step";
        Description = "Become invisible and undetectable by enemies for a short time.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
    }

    public override void Execute()
    {
        PlayerAvatar.instance.EnemyVisionFreezeTimerSet(Duration);

        Plugin.Log.LogInfo("Assassin skill used.");
    }
}
