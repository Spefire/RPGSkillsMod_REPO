internal class AssassinSkill : Skill
{
    private const float Duration = 8f;

    public AssassinSkill()
    {
        Name = "Shadow Step";
        Description = "Become invisible and undetectable by enemies for a short time.";
        Cooldown = 60f;

        Properties.Add($"Duration: {Duration}s");
    }

    public override void Execute()
    {
        // The game already exposes a per-player "freeze enemy vision" timer,
        // so there's no need to track our own active/timer state here: the
        // engine handles the countdown and re-enables detection on its own.
        PlayerAvatar.instance.EnemyVisionFreezeTimerSet(Duration);

        Plugin.Log.LogInfo("Assassin skill activated.");
    }
}
