using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

internal class NecromancerSkill : Skill
{
    private const int HealthSacrifice = 40;
    private const float Radius = 8f;

    public NecromancerSkill()
    {
        Name = "Raise Dead";
        Description = "Sacrifice your own health to revive a fallen ally nearby.";
        Cooldown = 90f;

        Properties.Add($"Health sacrificed: {HealthSacrifice}");
        Properties.Add($"Radius: {Radius}m");
    }

    public override void Execute()
    {
        ReviveNearbyAlly();

        Plugin.Log.LogInfo("Necromancer skill used.");
    }

    private void ReviveNearbyAlly()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        // PlayerHealth.health is internal to the game's assembly, so it
        // can't be accessed directly from our mod. Harmony's Traverse reads
        // it through reflection instead, which bypasses the access
        // modifier at runtime (this is the standard BepInEx/Harmony way to
        // reach internal/private game fields).
        int casterHealth = Traverse.Create(caster.playerHealth).Field("health").GetValue<int>();

        // Let the caster kill themselves with the sacrifice.
        /*if (casterHealth <= HealthSacrifice)
        {
            Plugin.Log.LogInfo("Not enough health to sacrifice.");
            return;
        }*/

        List<PlayerAvatar> players = SemiFunc.PlayerGetAll();

        foreach (PlayerAvatar player in players)
        {
            // PlayerAvatar.deadSet is also internal - same Traverse approach.
            bool isDead = Traverse.Create(player).Field("deadSet").GetValue<bool>();

            if (!isDead)
                continue;

            float distance = Vector3.Distance(
                player.transform.position,
                caster.transform.position);

            if (distance > Radius)
                continue;

            // TODO:
            // The exact meaning of Hurt's "savingGrace", "enemyIndex" and
            // "hurtByHeal" parameters isn't fully confirmed. Using safe-ish
            // defaults here (no enemy involved, not a saving-grace hit).
            caster.playerHealth.Hurt(HealthSacrifice, false, -1, false);

            // TODO:
            // Confirm whether "_revivedByTruck: false" is the right value
            // here (it's the only parameter of Revive, normally used by the
            // truck-return revive flow).
            player.Revive(false);

            break;
        }
    }
}
