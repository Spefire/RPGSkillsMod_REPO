using UnityEngine;

internal class PaladinSkill : Skill
{
    private const float Radius = 15f;
    private const float Duration = 10f;

    public PaladinSkill()
    {
        Name = "Divine Taunt";
        Description = "Grants temporary invulnerability to all damage, but forces nearby enemies to focus you.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
        Properties.Add($"Radius: {Radius}m");
    }

    public override void Execute()
    {
        // PlayerHealth.InvincibleSet(float) directly manages its own
        // internal invincibility timer, so there's no need to track
        // active/timer state or manually revert anything here.
        PlayerAvatar.instance.playerHealth.InvincibleSet(Duration);

        TauntNearbyEnemies();

        Plugin.Log.LogInfo("Paladin skill used.");
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
