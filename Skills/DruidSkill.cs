using System.Collections.Generic;
using UnityEngine;

internal class DruidSkill : Skill
{
    private const int Health = 40;
    private const float Radius = 5f;

    public DruidSkill()
    {
        Name = "Nature's Blessing";
        Description = "Heal nearby allies within a small radius.";
        Cooldown = Plugin.DebugAllow ? 20f : 45f;

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

            if (player == caster)
                player.playerHealth.Heal(Health, true);
            else
                player.playerHealth.HealOther(Health, true);
        }
    }
}