using Photon.Pun;
using UnityEngine;

internal class AssassinSkill : Skill
{
    private const float Duration = 15f;

    public AssassinSkill()
    {
        Name = "Phantom";
        Description = "Become undetectable by enemies, floating like a ghost.";
        Cooldown = Plugin.DebugAllow ? 20f : 60f;
        ActiveDuration = Duration;

        Properties.Add($"Duration: {Duration}s");
    }

    public override bool Execute()
    {
        PlayerAvatar caster = PlayerAvatar.instance;

        if (!FreezeEnemyVision(caster))
            return false;

        PlayerController.instance.AntiGravity(Duration);

        Plugin.Log.LogInfo("Assassin skill used.");

        return true;
    }

    private bool FreezeEnemyVision(PlayerAvatar caster)
    {
        if (!SemiFunc.IsMultiplayer())
        {
            caster.EnemyVisionFreezeTimerSet(Duration);
            return true;
        }

        return ModNetwork.TryRpcToMaster(caster.photonView, nameof(NecromancerReviveRelay.SetVisionFreezeRPC), Duration);
    }
}
