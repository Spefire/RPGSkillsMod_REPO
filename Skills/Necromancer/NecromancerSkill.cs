using HarmonyLib;
using Photon.Pun;
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

        caster.playerHealth.Hurt(HealthSacrifice, false, -1, false);

        ReviveTarget(target);

        return true;
    }

    private void ReviveTarget(PlayerAvatar target)
    {
        if (!SemiFunc.IsMultiplayer())
        {
            NecromancerReviveRelay.ReviveWithCorrectPosition(target, false);
        }
        else
        {
            target.photonView.RPC(nameof(NecromancerReviveRelay.RequestReviveRPC), RpcTarget.MasterClient, false);
        }
    }

    private PlayerAvatar FindReviveTarget(PlayerAvatar caster)
    {
        bool casterDisabled = Traverse.Create(caster).Field("isDisabled").GetValue<bool>();

        if (casterDisabled)
            return caster;

        Transform heldTransform = caster.physGrabber.grabbedObjectTransform;

        if (heldTransform == null)
            return null;

        PlayerDeathHead deathHead = heldTransform.GetComponent<PlayerDeathHead>();

        return deathHead != null ? deathHead.playerAvatar : null;
    }
}
