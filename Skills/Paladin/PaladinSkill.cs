using Photon.Pun;
using UnityEngine;

internal class PaladinSkill : Skill
{
    private readonly float Radius = SkillConfig.Radius("Paladin", 15f);
    private readonly float Duration = SkillConfig.Duration("Paladin", 10f);

    public PaladinSkill()
    {
        Name = "Divine Taunt";
        Description = "Grants invulnerability, but forces nearby enemies to focus you.";
        Cooldown = SkillConfig.Cooldown("Paladin", 60f);
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
        Properties.Add($"Radius: {Radius}m");
    }

    public override bool Execute()
    {
        if (!TauntNearbyEnemies())
            return false;

        PlayerAvatar.instance.playerHealth.InvincibleSet(Duration);

        Plugin.Log.LogInfo("Paladin skill used.");

        return true;
    }

    private bool TauntNearbyEnemies()
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

                if (!ModNetwork.TryRpcToMaster(photonView, nameof(EnemyMapReveal.SetChaseTargetRPC), caster.photonView.ViewID))
                    return false;
            }
        }

        return true;
    }
}
