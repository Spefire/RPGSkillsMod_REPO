using UnityEngine;

internal class WarriorSkill : Skill
{
    private bool active;
    private float timer;

    private const int Strength = 20;
    private const float Duration = 5f;

    public WarriorSkill()
    {
        Name = "Berserk";
        Description = "Temporarily increases your strength.";
        Cooldown = 60f;

        Properties.Add($"Strength: {Strength}");
        Properties.Add($"Duration: {Duration}s");
    }

    public override void Execute()
    {
        if (active)
            return;

        active = true;
        timer = 15f;

        // TODO:
        // Increase the player's strength.
        //
        // Example:
        // PlayerController.instance.StrengthMultiplier *= 1.5f;

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

            // TODO:
            // Restore the player's original strength.

            Plugin.Log.LogInfo("Warrior skill ended.");
        }
    }
}