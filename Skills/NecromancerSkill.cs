using HarmonyLib;
using UnityEngine;

internal class NecromancerSkill : Skill
{
    private const int HealthSacrifice = 25;

    public NecromancerSkill()
    {
        Name = "Raise Dead";
        Description = "Sacrifice your own health to revive yourself, or a fallen ally.";
        Cooldown = Plugin.DebugAllow ? 20f : 120f;

        Properties.Add($"Health sacrificed: {HealthSacrifice}");
    }

    public override bool Execute()
    {
        bool revived = ReviveSelfOrHeldAlly();

        Plugin.Log.LogInfo(revived
            ? "Necromancer skill used."
            : "Necromancer skill failed: not downed and not holding a dead ally's head.");

        return revived;
    }

    private bool ReviveSelfOrHeldAlly()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        PlayerAvatar target = FindReviveTarget(caster);

        if (target == null)
            return false;

        // PlayerHealth.health is internal to the game's assembly, so it
        // can't be accessed directly from our mod. Harmony's Traverse reads
        // it through reflection instead, which bypasses the access
        // modifier at runtime (this is the standard BepInEx/Harmony way to
        // reach internal/private game fields).
        //
        // TODO:
        // The exact meaning of Hurt's "savingGrace", "enemyIndex" and
        // "hurtByHeal" parameters isn't fully confirmed. Using safe-ish
        // defaults here (no enemy involved, not a saving-grace hit).
        caster.playerHealth.Hurt(HealthSacrifice, false, -1, false);

        // TODO:
        // Confirm whether "_revivedByTruck: false" is the right value
        // here (it's the only parameter of Revive, normally used by the
        // truck-return revive flow).
        target.Revive(false);

        return true;
    }

    private PlayerAvatar FindReviveTarget(PlayerAvatar caster)
    {
        // Self-revive: if the caster is the one who's down, let them raise
        // themselves - keep this behavior as-is, it works fine already.
        bool casterDisabled = Traverse.Create(caster).Field("isDisabled").GetValue<bool>();

        if (casterDisabled)
            return caster;

        // Otherwise, the caster must be holding the dead ally's severed
        // head (PlayerDeathHead) to revive them - a held object with that
        // component links back to its owning PlayerAvatar via the public
        // "playerAvatar" field.
        Transform heldTransform = caster.physGrabber.grabbedObjectTransform;

        if (heldTransform == null)
            return null;

        PlayerDeathHead deathHead = heldTransform.GetComponent<PlayerDeathHead>();

        return deathHead != null ? deathHead.playerAvatar : null;
    }
}
