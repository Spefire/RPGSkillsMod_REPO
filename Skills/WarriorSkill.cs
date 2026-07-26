using UnityEngine;

internal class WarriorSkill : Skill
{
    private bool active;
    private float timer;

    private const int StrengthLevels = 10;
    private const float Duration = 15f;

    public WarriorSkill()
    {
        Name = "Berserk";
        Description = "Temporarily increases your strength.";
        Cooldown = Plugin.DebugAllow ? 20f : 90f;
        ActiveDuration = Duration;

        Properties.Add($"Strength levels: +{StrengthLevels}");
        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        if (active)
            return false;

        active = true;
        timer = Duration;

        string steamID = SemiFunc.PlayerGetSteamID(PlayerAvatar.instance);

        PunManager.instance.UpgradePlayerGrabStrength(steamID, StrengthLevels);
        PunManager.instance.UpgradePlayerThrowStrength(steamID, StrengthLevels);

        Plugin.Log.LogInfo("Warrior skill activated.");

        return true;
    }

    public override void Update()
    {
        if (!active)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            active = false;

            string steamID = SemiFunc.PlayerGetSteamID(PlayerAvatar.instance);

            PunManager.instance.UpgradePlayerGrabStrength(steamID, -StrengthLevels);
            PunManager.instance.UpgradePlayerThrowStrength(steamID, -StrengthLevels);

            Plugin.Log.LogInfo("Warrior skill ended.");
        }
    }
}