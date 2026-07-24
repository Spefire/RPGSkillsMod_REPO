using UnityEngine;

internal class WarriorSkill : Skill
{
    private bool active;
    private float timer;

    private float originalGrabStrength;
    private float originalThrowStrength;

    private const float StrengthMultiplier = 1.5f;
    private const float Duration = 5f;

    public WarriorSkill()
    {
        Name = "Berserk";
        Description = "Temporarily increases your strength.";
        Cooldown = 60f;

        Properties.Add($"Strength multiplier: x{StrengthMultiplier}");
        Properties.Add($"Duration: {Duration}s");
    }

    public override void Execute()
    {
        if (active)
            return;

        active = true;
        timer = Duration;

        // grabStrength / throwStrength are the public fields that control
        // how strong the player's PhysGrabber is when grabbing/throwing items.
        PhysGrabber physGrabber = PlayerAvatar.instance.physGrabber;
        originalGrabStrength = physGrabber.grabStrength;
        originalThrowStrength = physGrabber.throwStrength;

        physGrabber.grabStrength = originalGrabStrength * StrengthMultiplier;
        physGrabber.throwStrength = originalThrowStrength * StrengthMultiplier;

        Plugin.Log.LogInfo("Warrior skill activated.");
    }

    public override void Update()
    {
        if (!active)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            active = false;

            PhysGrabber physGrabber = PlayerAvatar.instance.physGrabber;
            physGrabber.grabStrength = originalGrabStrength;
            physGrabber.throwStrength = originalThrowStrength;

            Plugin.Log.LogInfo("Warrior skill ended.");
        }
    }
}