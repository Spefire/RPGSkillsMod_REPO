internal class PaladinSkill : Skill
{
    private const float Duration = 10f;

    public PaladinSkill()
    {
        Name = "Divine Shield";
        Description = "Grants temporary invulnerability to all damage.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;

        Properties.Add($"Duration: {Duration}s");
    }

    public override void Execute()
    {
        // PlayerHealth.InvincibleSet(float) directly manages its own
        // internal invincibility timer, so there's no need to track
        // active/timer state or manually revert anything here.
        PlayerAvatar.instance.playerHealth.InvincibleSet(Duration);

        Plugin.Log.LogInfo("Paladin skill used.");
    }
}
