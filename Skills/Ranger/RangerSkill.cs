using Photon.Pun;
using UnityEngine;

internal class RangerSkill : Skill
{
    private const float Radius = 30f;
    private const float Duration = 20f;

    public RangerSkill()
    {
        Name = "Predator Sense";
        Description = "Marks nearby enemies on the map, revealing them to every player.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
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
            enemyParent.GetComponent<PhotonView>().RPC(nameof(EnemyMapReveal.RevealRPC), RpcTarget.All, Duration);
        }
    }
}
