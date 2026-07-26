using UnityEngine;

internal class MonkSkill : Skill
{
    private const float Radius = 10f;
    private const float StunDuration = 3f;
    private const float PushForce = 12f;
    private const float UpwardForce = 4f;

    public MonkSkill()
    {
        Name = "Shockwave";
        Description = "Releases a shockwave that pushes back and briefly stuns all nearby enemies.";
        Cooldown = Plugin.DebugAllow ? 20f : 75f;
        ActiveDuration = StunDuration;

        Properties.Add($"Radius: {Radius}m");
        Properties.Add($"Stun duration: {StunDuration}s");
    }

    public override bool Execute()
    {
        PushBackNearbyEnemies();

        Plugin.Log.LogInfo("Monk skill used.");

        return true;
    }

    private void PushBackNearbyEnemies()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

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

            // TODO:
            // Rigidbody is a plain Unity component that lives on the same
            // GameObject as the (internal) EnemyRigidbody wrapper -
            // grabbing it directly avoids needing that internal type's
            // private "rb" field. PushForce/UpwardForce weren't verified
            // against real enemy masses; tune if enemies barely move or
            // get launched too far.
            Rigidbody rb = enemy.GetComponent<Rigidbody>();

            if (rb == null)
                continue;

            Vector3 direction = distance > 0.01f ? offset.normalized : Vector3.forward;

            rb.AddForce(direction * PushForce + Vector3.up * UpwardForce, ForceMode.Impulse);
        }
    }
}
