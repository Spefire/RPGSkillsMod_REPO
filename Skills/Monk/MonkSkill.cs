using Photon.Pun;
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
        Description = "Releases a powerful shockwave, stunning all nearby enemies.";
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

            if (!SemiFunc.IsMultiplayer())
            {
                EnemyStateStunned stunned = enemy.GetComponent<EnemyStateStunned>();

                if (stunned != null)
                    stunned.Set(StunDuration);

                Rigidbody rb = enemy.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, caster.transform.position, Radius, UpwardsModifier, ForceMode.Impulse);
                }
            }
            else
            {
                PhotonView photonView = enemy.GetComponentInParent<EnemyParent>().GetComponent<PhotonView>();
                photonView.RPC(nameof(EnemyMapReveal.StunAndPushRPC), RpcTarget.MasterClient, caster.transform.position, StunDuration, ExplosionForce, Radius, UpwardsModifier);
            }
        }
    }
}
