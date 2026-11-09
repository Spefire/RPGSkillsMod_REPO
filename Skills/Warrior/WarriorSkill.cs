using UnityEngine;

internal class WarriorSkill : Skill
{
    private bool active;
    private float timer;
    private string grantedSteamId;

    private readonly int StrengthLevels = SkillConfig.Int("Warrior", "StrengthLevels", 20, 1, 100, "Grab and throw strength levels granted.");
    private readonly float Duration = SkillConfig.Duration("Warrior", 15f);

    public WarriorSkill()
    {
        Name = "Berserk";
        Description = "Increases your strength.";
        Cooldown = SkillConfig.Cooldown("Warrior", 90f);
        ActiveDuration = Duration;

        Properties.Add($"Strength levels: +{StrengthLevels}");
        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        if (!ActivateBerserk())
            return false;

        Plugin.Log.LogInfo("Warrior skill activated.");

        return true;
    }

    public override void Update()
    {
        if (!active)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            DeactivateBerserk();
    }

    private bool ActivateBerserk()
    {
        if (active)
            return false;

        active = true;
        timer = Duration;
        grantedSteamId = SemiFunc.PlayerGetSteamID(PlayerAvatar.instance);

        PunManager.instance.UpgradePlayerGrabStrength(grantedSteamId, StrengthLevels);
        PunManager.instance.UpgradePlayerThrowStrength(grantedSteamId, StrengthLevels);

        return true;
    }

    public override void Cancel()
    {
        if (active)
            DeactivateBerserk();
    }

    private void DeactivateBerserk()
    {
        active = false;

        if (PunManager.instance != null && grantedSteamId != null)
        {
            PunManager.instance.UpgradePlayerGrabStrength(grantedSteamId, -StrengthLevels);
            PunManager.instance.UpgradePlayerThrowStrength(grantedSteamId, -StrengthLevels);
        }
        else
        {
            Plugin.Log.LogWarning("Warrior skill ended but the strength bonus could not be reverted.");
        }

        grantedSteamId = null;

        Plugin.Log.LogInfo("Warrior skill ended.");
    }
}