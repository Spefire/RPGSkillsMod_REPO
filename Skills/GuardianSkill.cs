using UnityEngine;

internal class GuardianSkill : Skill
{
    private const float Radius = 15f;

    public GuardianSkill()
    {
        Name = "Taunt";
        Description = "Forces nearby enemies to focus you.";
        Cooldown = 45f;

        Properties.Add($"Radius: {Radius}m");
    }

    public override void Execute()
    {
        TauntNearbyEnemies();

        Plugin.Log.LogInfo("Guardian skill used.");
    }

    private void TauntNearbyEnemies()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        foreach (Enemy enemy in Object.FindObjectsOfType<Enemy>())
        {
            float distance = Vector3.Distance(
                enemy.CenterTransform.position,
                caster.transform.position);

            if (distance > Radius)
                continue;

            // Forces this enemy to chase/target the caster.
            enemy.SetChaseTarget(caster);
        }
    }
}
