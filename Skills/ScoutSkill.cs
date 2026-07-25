using UnityEngine;

internal class ScoutSkill : Skill
{
    private bool active;
    private float timer;

    private const float Duration = 15f;

    public ScoutSkill()
    {
        Name = "Best Runner";
        Description = "Grants infinite stamina for a short duration.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;

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

        PlayerController.instance.EnergyCurrent = PlayerController.instance.EnergyStart;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            active = false;

            Plugin.Log.LogInfo("Scout skill ended.");
        }
    }
}
