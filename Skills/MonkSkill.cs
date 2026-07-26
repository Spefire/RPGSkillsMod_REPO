using UnityEngine;

internal class MonkSkill : Skill
{
    private const float Radius = 10f;
    private const float StunDuration = 5f;
    private const float ExplosionForce = 35f;
    private const float UpwardsModifier = 5f;

    public MonkSkill()
    {
        Name = "Shockwave";
        Description = "Releases a powerful shockwave that launches back and briefly stuns all nearby enemies.";
        Cooldown = Plugin.DebugAllow ? 20f : 75f;
        ActiveDuration = StunDuration;

        Properties.Add($"Radius: {Radius}m");
        Properties.Add($"Stun duration: {StunDuration}s");
    }

    public override bool Execute()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        PushBackNearbyEnemies(caster);

        try
        {
            SkillVfx.PlayShockwaveEffect(caster.transform.position, Plugin.SelectedClass, Radius);
        }
        catch (System.Exception ex)
        {
            Plugin.Log.LogWarning($"Shockwave VFX failed: {ex}");
        }

        Plugin.Log.LogInfo("Monk skill used.");

        return true;
    }

    private void PushBackNearbyEnemies(PlayerAvatar caster)
    {
        foreach (Enemy enemy in Object.FindObjectsOfType<Enemy>())
        {
            Vector3 offset = enemy.CenterTransform.position - caster.transform.position;
            float distance = offset.magnitude;

            if (distance > Radius)
                continue;

            // EnemyStateStunned.Set(...) is what drives Enemy.CurrentState
            // to EnemyState.Stunned (confirmed public via ilspycmd, same
            // mechanism the game uses for fall-stun). While stunned, the
            // enemy's NavMeshAgent stops overriding its position every
            // frame, so the physics push below actually moves it instead
            // of being cancelled out immediately.
            EnemyStateStunned stunned = enemy.GetComponent<EnemyStateStunned>();

            if (stunned != null)
                stunned.Set(StunDuration);

            Rigidbody rb = enemy.GetComponent<Rigidbody>();

            if (rb == null)
                continue;

            // AddExplosionForce gives a natural distance falloff (strong up
            // close, weaker near the edge of the radius) instead of the
            // previous flat push, which reads as a much more powerful
            // "shockwave" impact - same built-in Unity API real explosions
            // (e.g. grenades) commonly rely on.
            rb.AddExplosionForce(ExplosionForce, caster.transform.position, Radius, UpwardsModifier, ForceMode.Impulse);
        }
    }
}
