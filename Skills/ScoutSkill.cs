using UnityEngine;

internal class ScoutSkill : Skill
{
    private bool active;
    private float timer;

    private const float Duration = 15f;

    public ScoutSkill()
    {
        Name = "Second Wind";
        Description = "Grants infinite stamina for a short duration.";
        Cooldown = 60f;

        Properties.Add($"Duration: {Duration}s");
    }

    public override void Execute()
    {
        if (active)
            return;

        active = true;
        timer = Duration;

        Plugin.Log.LogInfo("Scout skill activated.");
    }

    public override void Update()
    {
        if (!active)
            return;

        // SemiFunc.LocalPlayerOverrideEnergyUnlimited() takes no duration
        // parameter, so it's not clear whether a single call is enough or
        // if it needs to be re-applied every frame like this.
        // Calling it every tick while active is the safe assumption for now.
        SemiFunc.LocalPlayerOverrideEnergyUnlimited();

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            active = false;

            Plugin.Log.LogInfo("Scout skill ended.");
        }
    }
}
