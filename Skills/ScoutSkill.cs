using UnityEngine;

internal class ScoutSkill : Skill
{
    private bool active;
    private float timer;

    private const float Duration = 15f;

    public ScoutSkill()
    {
        Name = "Best Runner";
        Description = "Grants infinite stamina.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        if (active)
            return false;

        active = true;
        timer = Duration;

        Plugin.Log.LogInfo("Scout skill activated.");

        return true;
    }

    public override void Update()
    {
        if (!active)
            return;

        PlayerController.instance.EnergyCurrent = PlayerController.instance.EnergyStart;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            active = false;

            Plugin.Log.LogInfo("Scout skill ended.");
        }
    }
}
