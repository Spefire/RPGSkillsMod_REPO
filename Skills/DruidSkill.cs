using System.Collections.Generic;
using UnityEngine;

internal class DruidSkill : Skill
{
    private const int Health = 50;
    private const float Radius = 10f;

    public DruidSkill()
    {
        Name = "Nature's Blessing";
        Description = "Heal nearby allies within a small radius.";
        Cooldown = 30f;

        Properties.Add($"Radius: {Radius}m");
        Properties.Add($"Total health: {Health}");
    }

    public override void Execute()
    {
        HealNearbyPlayers();

        Plugin.Log.LogInfo("Druid skill used.");
    }

    private void HealNearbyPlayers()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        List<PlayerAvatar> players = SemiFunc.PlayerGetAll();
        foreach (PlayerAvatar player in players)
        {
            float distance = Vector3.Distance(player.transform.position, caster.transform.position);
            if (distance > Radius)
                continue;

            // Heal() is for healing yourself, HealOther() is the networked
            // (RPC) call used to heal someone else - "effect" (bool) likely
            // toggles the heal VFX/SFX, so we leave it enabled (true).
            if (player == caster)
                player.playerHealth.Heal(Health, true);
            else
                player.playerHealth.HealOther(Health, true);
        }
    }
}