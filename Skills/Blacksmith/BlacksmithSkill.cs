using Photon.Pun;
using UnityEngine;

internal class BlacksmithSkill : Skill
{
    private const float IndestructibleDuration = 999999f;

    public BlacksmithSkill()
    {
        Name = "Reinforce";
        Description = "Makes the item you're currently holding unbreakable.";
        Cooldown = SkillConfig.Cooldown("Blacksmith", 90f);
    }

    public override bool Execute()
    {
        if (!ReinforceHeldItem())
        {
            Plugin.Log.LogInfo("Blacksmith skill failed: no item held, or the host can't relay it.");
            return false;
        }

        Plugin.Log.LogInfo("Blacksmith skill used.");

        return true;
    }

    private bool ReinforceHeldItem()
    {
        Transform heldTransform = PlayerAvatar.instance.physGrabber.grabbedObjectTransform;

        if (heldTransform == null)
            return false;

        PhysGrabObject heldObject = heldTransform.GetComponent<PhysGrabObject>();

        if (heldObject == null)
            return false;

        if (!SemiFunc.IsMultiplayer())
        {
            heldObject.OverrideIndestructible(IndestructibleDuration);
            return true;
        }

        return ModNetwork.TryRpcToMaster(heldObject.GetComponent<PhotonView>(), nameof(PhysGrabObjectRelay.OverrideIndestructibleRPC), IndestructibleDuration);
    }
}
