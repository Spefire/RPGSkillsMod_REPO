using System.Collections.Generic;
using UnityEngine;

internal class DruidSkill : Skill
{
    private const int Health = 40;
    private const float Radius = 5f;

    public DruidSkill()
    {
        Name = "Nature's Blessing";
        Description = "Heal nearby allies within a small radius, sharing a fixed pool of health.";
        Cooldown = Plugin.DebugAllow ? 20f : 45f;

        Properties.Add($"Radius: {Radius}m");
        Properties.Add($"Total health: {Health}");
    }

    public override bool Execute()
    {
        bool healed = HealNearbyPlayers();

        Plugin.Log.LogInfo(healed ? "Druid skill used." : "Druid skill failed.");

        return healed;
    }

    private bool HealNearbyPlayers()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        List<PlayerAvatar> nearbyAllies = new List<PlayerAvatar>();

        foreach (PlayerAvatar player in SemiFunc.PlayerGetAll())
        {
            if (player == caster)
                continue;

            float distance = Vector3.Distance(player.transform.position, caster.transform.position);
            if (distance <= Radius)
                nearbyAllies.Add(player);
        }

        // Health is a fixed pool shared equally between the caster and
        // every nearby ally. If nobody else is around, the caster keeps
        // the whole pool for themselves.
        int healPerPlayer = Health / (nearbyAllies.Count + 1);

        if (healPerPlayer <= 0)
            return false;

        caster.playerHealth.Heal(healPerPlayer, true);

        foreach (PlayerAvatar player in nearbyAllies)
            player.playerHealth.HealOther(healPerPlayer, true);

        return true;
    }
}