using UnityEngine;

internal class BlacksmithSkill : Skill
{
    private const float IndestructibleDuration = 999999f;

    public BlacksmithSkill()
    {
        Name = "Reinforce";
        Description = "Makes the item you are currently holding unbreakable.";
        Cooldown = Plugin.DebugAllow ? 20f : 90f;
    }

    public override void Execute()
    {
        ReinforceHeldItem();

        Plugin.Log.LogInfo("Blacksmith skill used.");
    }

    private void ReinforceHeldItem()
    {
        Transform heldTransform = PlayerAvatar.instance.physGrabber.grabbedObjectTransform;

        if (heldTransform == null)
            return;

        PhysGrabObject heldObject = heldTransform.GetComponent<PhysGrabObject>();

        if (heldObject == null)
            return;

        heldObject.OverrideIndestructible(IndestructibleDuration);
    }
}
