using UnityEngine;

internal class BlacksmithSkill : Skill
{
    private const float IndestructibleDuration = 999999f;

    public BlacksmithSkill()
    {
        Name = "Reinforce";
        Description = "Makes the item you're currently holding unbreakable.";
        Cooldown = Plugin.DebugAllow ? 20f : 90f;
    }

    public override bool Execute()
    {
        if (!ReinforceHeldItem())
        {
            Plugin.Log.LogInfo("Blacksmith skill failed: no item held.");
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

        heldObject.OverrideIndestructible(IndestructibleDuration);

        return true;
    }
}
