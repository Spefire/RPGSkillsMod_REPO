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
        // TODO:
        // Retrieve every player in the game.
        //
        // Example:
        // foreach (PlayerAvatar player in PlayerManager.instance.Players)

        /*
        foreach (...)
        {
            float distance = Vector3.Distance(
                player.transform.position,
                PlayerAvatar.instance.transform.position);

            if (distance > Radius)
                continue;

            // TODO:
            // Heal the player.
            //
            // Example:
            // player.Heal(20);
        }
        */
    }
}