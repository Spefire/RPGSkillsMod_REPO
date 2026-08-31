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

        FreezeEnemyVision(caster);

        PlayerController.instance.AntiGravity(Duration);

        Plugin.Log.LogInfo("Assassin skill used.");

        return true;
    }

    private void FreezeEnemyVision(PlayerAvatar caster)
    {
        if (!SemiFunc.IsMultiplayer())
        {
            caster.EnemyVisionFreezeTimerSet(Duration);
        }
        else
        {
            caster.photonView.RPC(nameof(NecromancerReviveRelay.SetVisionFreezeRPC), RpcTarget.MasterClient, Duration);
        }
    }
}
