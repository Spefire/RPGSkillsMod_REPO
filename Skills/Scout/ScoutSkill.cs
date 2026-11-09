using UnityEngine;

internal class ScoutSkill : Skill
{
    private bool active;
    private float timer;

    private readonly float Duration = SkillConfig.Duration("Scout", 15f);

    public ScoutSkill()
    {
        Name = "Best Runner";
        Description = "Grants infinite stamina.";
        Cooldown = SkillConfig.Cooldown("Scout", 60f);
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        if (!ActivateInfiniteStamina())
            return false;

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
            DeactivateInfiniteStamina();
    }

    public override void Cancel()
    {
        if (active)
            DeactivateInfiniteStamina();
    }

    private bool ActivateInfiniteStamina()
    {
        if (active)
            return false;

        active = true;
        timer = Duration;

        return true;
    }

    private void DeactivateInfiniteStamina()
    {
        active = false;

        Plugin.Log.LogInfo("Scout skill ended.");
    }
}
