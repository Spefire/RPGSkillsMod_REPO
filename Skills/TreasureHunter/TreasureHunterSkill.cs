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

        foreach (ValuableObject valuable in Object.FindObjectsOfType<ValuableObject>())
        {
            float distance = Vector3.Distance(
                valuable.transform.position,
                caster.transform.position);

            if (distance > Radius)
                continue;

            valuable.Discover(ValuableDiscoverGraphic.State.Discover);
        }
    }
}
