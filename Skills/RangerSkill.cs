using System.Collections;
using UnityEngine;

internal class RangerSkill : Skill
{
    private const float Radius = 20f;
    private const float Duration = 8f;

    private static readonly Color RevealLightColor = new Color(1f, 0.2f, 0.2f);

    public RangerSkill()
    {
        Name = "Predator Sense";
        Description = "Reveals nearby enemies, even through walls, for a short time.";
        Cooldown = Plugin.DebugAllow ? 20f : 45f;
        ActiveDuration = Duration;

        Properties.Add($"Radius: {Radius}m");
        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        RevealNearbyEnemies();

        Plugin.Log.LogInfo("Ranger skill used.");

        return true;
    }

    private void RevealNearbyEnemies()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        foreach (Enemy enemy in Object.FindObjectsOfType<Enemy>())
        {
            float distance = Vector3.Distance(
                enemy.CenterTransform.position,
                caster.transform.position);

            if (distance > Radius)
                continue;

            SpawnRevealMarker(caster, enemy);
        }
    }

    // TODO:
    // No API was found to force an enemy to be "seen" through walls - the
    // game's own detection systems all work the other way around (the
    // ENEMY's vision of the PLAYER, e.g. EnemyVision/EnemyVisionFreezeTimerSet).
    // Simplest robust approach instead: a plain point light attached to
    // the enemy, ignoring line of sight entirely, so it stays visible to
    // the caster regardless of walls/darkness.
    private void SpawnRevealMarker(PlayerAvatar caster, Enemy enemy)
    {
        GameObject markerObject = new GameObject("RPG_RevealLight");

        markerObject.transform.SetParent(enemy.CenterTransform, false);
        markerObject.transform.localPosition = Vector3.zero;

        Light marker = markerObject.AddComponent<Light>();
        marker.type = LightType.Point;
        marker.color = RevealLightColor;
        marker.range = 6f;
        marker.intensity = 3f;

        caster.StartCoroutine(DestroyAfter(markerObject, Duration));
    }

    private IEnumerator DestroyAfter(GameObject markerObject, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (markerObject != null)
            Object.Destroy(markerObject);
    }
}
