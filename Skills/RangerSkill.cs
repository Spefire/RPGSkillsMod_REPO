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
        PlayerAvatar caster = PlayerAvatar.instance;

        if (caster == null)
            return false;

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

        Plugin.Log.LogInfo($"Ranger skill used, revealed {revealed} enemies.");

        return true;
    }

    private void RevealEnemy(EnemyParent enemyParent)
    {
        EnemyMapReveal reveal = enemyParent.GetComponent<EnemyMapReveal>();

        if (reveal == null)
            reveal = enemyParent.gameObject.AddComponent<EnemyMapReveal>();

        if (SemiFunc.IsMultiplayer())
        {
            PhotonView photonView = enemyParent.GetComponent<PhotonView>();

            if (photonView != null)
            {
                photonView.RPC(nameof(EnemyMapReveal.RevealRPC), RpcTarget.All, Duration);
                return;
            }
        }

        reveal.RevealRPC(Duration);
    }
}
