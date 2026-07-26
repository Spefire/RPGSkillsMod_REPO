using UnityEngine;

internal class AssassinSkill : Skill
{
    private const float Duration = 15f;

    public AssassinSkill()
    {
        Name = "Phantom";
        Description = "Become invisible and undetectable by enemies for a short time, floating like a ghost.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        PlayerAvatar.instance.EnemyVisionFreezeTimerSet(Duration);

        PlayerController.instance.AntiGravity(Duration);

        Plugin.Log.LogInfo("Assassin skill used.");

        return true;
    }
}
