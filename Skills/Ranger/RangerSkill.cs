using Photon.Pun;
using UnityEngine;

internal class RangerSkill : Skill
{
    private readonly float Radius = SkillConfig.Radius("Ranger", 30f);
    private readonly float Duration = SkillConfig.Duration("Ranger", 20f);

    public RangerSkill()
    {
        Name = "Predator Sense";
        Description = "Marks nearby enemies on the map, revealing them to every player.";
        Cooldown = SkillConfig.Cooldown("Ranger", 60f);
        ActiveDuration = Duration;

        Properties.Add($"Radius: {Radius}m");
        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        int revealed = RevealNearbyEnemies();

        Plugin.Log.LogInfo($"Ranger skill used, revealed {revealed} enemies.");

        return true;
    }

    private int RevealNearbyEnemies()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        int revealed = 0;

        foreach (Enemy enemy in Object.FindObjectsOfType<Enemy>())
        {
            if (enemy == null || enemy.CenterTransform == null)
                continue;

            EnemyParent enemyParent = enemy.GetComponentInParent<EnemyParent>();

            if (enemyParent == null)
                continue;

            float distance = Vector3.Distance(
                enemy.CenterTransform.position,
                caster.transform.position);

            if (distance > Radius)
                continue;

            RevealEnemy(enemyParent);
            revealed++;
        }

        return revealed;
    }

    private void RevealEnemy(EnemyParent enemyParent)
    {
        if (!SemiFunc.IsMultiplayer())
        {
            enemyParent.GetComponent<EnemyMapReveal>().RevealRPC(Duration);
        }
        else
        {
            ModNetwork.RpcToModdedPlayers(enemyParent.GetComponent<PhotonView>(), nameof(EnemyMapReveal.RevealRPC), Duration);
        }
    }
}
