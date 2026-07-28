using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

internal class DruidSkill : Skill
{
    private const int Health = 50;
    private const float Radius = 5f;

    public DruidSkill()
    {
        Name = "Nature's Blessing";
        Description = "Heal nearby allies within a small radius.";
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

        List<PlayerAvatar> recipients = new List<PlayerAvatar> { caster };

        foreach (PlayerAvatar player in SemiFunc.PlayerGetAll())
        {
            if (player == caster)
                continue;

            float distance = Vector3.Distance(player.transform.position, caster.transform.position);
            if (distance <= Radius)
                recipients.Add(player);
        }

        // Players already at full health don't consume a share of the
        // pool - they're skipped entirely so the health can go to whoever
        // still needs it instead of being wasted.
        List<PlayerAvatar> needsHeal = new List<PlayerAvatar>();
        Dictionary<PlayerAvatar, int> missingHealth = new Dictionary<PlayerAvatar, int>();

        foreach (PlayerAvatar player in recipients)
        {
            int missing = GetMissingHealth(player);
            if (missing > 0)
            {
                needsHeal.Add(player);
                missingHealth[player] = missing;
            }
        }

        if (needsHeal.Count == 0)
            return false;

        int pool = Health;
        Dictionary<PlayerAvatar, int> healAmounts = new Dictionary<PlayerAvatar, int>();

        // Water-filling: split the remaining pool evenly among players who
        // still need healing, capping each at their missing health so
        // nothing is wasted overhealing someone close to full - any
        // leftover carries over to whoever's still short in the next pass.
        while (pool > 0 && needsHeal.Count > 0)
        {
            int share = pool / needsHeal.Count;
            if (share <= 0)
                break;

            List<PlayerAvatar> capped = new List<PlayerAvatar>();

            foreach (PlayerAvatar player in needsHeal)
            {
                int give = Mathf.Min(share, missingHealth[player]);
                healAmounts[player] = healAmounts.TryGetValue(player, out int existing) ? existing + give : give;
                pool -= give;

                missingHealth[player] -= give;
                if (missingHealth[player] <= 0)
                    capped.Add(player);
            }

            foreach (PlayerAvatar player in capped)
                needsHeal.Remove(player);
        }

        if (healAmounts.Count == 0)
            return false;

        foreach (KeyValuePair<PlayerAvatar, int> entry in healAmounts)
        {
            if (entry.Key == caster)
                caster.playerHealth.Heal(entry.Value, true);
            else
                entry.Key.playerHealth.HealOther(entry.Value, true);
        }

        return true;
    }

    // PlayerHealth.health and .maxHealth are both internal - read via
    // Harmony's Traverse.
    private static int GetMissingHealth(PlayerAvatar player)
    {
        Traverse healthTraverse = Traverse.Create(player.playerHealth);
        int currentHealth = healthTraverse.Field("health").GetValue<int>();
        int maxHealth = healthTraverse.Field("maxHealth").GetValue<int>();
        return Mathf.Max(0, maxHealth - currentHealth);
    }
}