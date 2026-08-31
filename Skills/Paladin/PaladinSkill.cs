using Photon.Pun;
using UnityEngine;

internal class PaladinSkill : Skill
{
    private const float Radius = 15f;
    private const float Duration = 10f;

    public PaladinSkill()
    {
        Name = "Divine Taunt";
        Description = "Grants invulnerability, but forces nearby enemies to focus you.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
        Properties.Add($"Radius: {Radius}m");
    }

    public override bool Execute()
    {
        PlayerAvatar.instance.playerHealth.InvincibleSet(Duration);

        TauntNearbyEnemies();

        Plugin.Log.LogInfo("Paladin skill used.");

        return true;
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

            if (!SemiFunc.IsMultiplayer())
            {
                enemy.SetChaseTarget(caster);
            }
            else
            {
                PhotonView photonView = enemy.GetComponentInParent<EnemyParent>().GetComponent<PhotonView>();
                photonView.RPC(nameof(EnemyMapReveal.SetChaseTargetRPC), RpcTarget.MasterClient, caster.photonView.ViewID);
            }
        }
    }
}
