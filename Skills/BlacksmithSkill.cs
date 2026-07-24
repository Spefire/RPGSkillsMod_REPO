using UnityEngine;

internal class BlacksmithSkill : Skill
{
    // PhysGrabObject.OverrideIndestructible(float) takes a duration rather
    // than being a permanent toggle. There's no "forever" value, so a very
    // large duration is used to approximate "unbreakable".
    private const float IndestructibleDuration = 999999f;

    public BlacksmithSkill()
    {
        Name = "Reinforce";
        Description = "Makes the item you are currently holding unbreakable.";
        Cooldown = 90f;
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

        // TODO:
        // There's also a ResetIndestructible() method if we ever need to
        // cancel this early (e.g. if the skill should be undoable, or if a
        // shorter/refreshable duration turns out to be more appropriate
        // than a huge fixed value).
    }
}
