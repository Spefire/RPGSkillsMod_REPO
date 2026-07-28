using UnityEngine;

internal class TreasureHunterSkill : Skill
{
    private const float Radius = 15f;

    public TreasureHunterSkill()
    {
        Name = "Treasure Sense";
        Description = "Reveals valuable items around you.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;

        Properties.Add($"Radius: {Radius}m");
    }

    public override bool Execute()
    {
        RevealNearbyValuables();

        Plugin.Log.LogInfo("Treasure Hunter skill used.");

        return true;
    }

    private void RevealNearbyValuables()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        // TODO:
        // FindObjectsOfType is fine for a one-shot cast but is fairly slow;
        // if this ends up being called often, consider caching the list of
        // valuables (e.g. from EnemyDirector-style tracking, or the room
        // volumes) instead of scanning the whole scene each time.
        foreach (ValuableObject valuable in Object.FindObjectsOfType<ValuableObject>())
        {
            float distance = Vector3.Distance(
                valuable.transform.position,
                caster.transform.position);

            if (distance > Radius)
                continue;

            // Discover(...) is what the game itself uses to reveal a
            // valuable (name/value popup). "Reminder"/"Bad"/"Custom" are
            // the other State values; "Discover" matches the normal reveal.
            valuable.Discover(ValuableDiscoverGraphic.State.Discover);
        }
    }
}
