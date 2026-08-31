using System.Collections.Generic;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

internal class NecromancerReviveRelay : MonoBehaviour
{
    internal static readonly Dictionary<PlayerAvatar, Vector3> LastDeathPosition = new Dictionary<PlayerAvatar, Vector3>();

    [PunRPC]
    public void RequestReviveRPC(bool revivedByTruck)
    {
        PlayerAvatar target = GetComponent<PlayerAvatar>();

        if (target != null)
            ReviveWithCorrectPosition(target, revivedByTruck);
    }

    [PunRPC]
    public void SetVisionFreezeRPC(float duration)
    {
        PlayerAvatar target = GetComponent<PlayerAvatar>();

        if (target != null)
            target.EnemyVisionFreezeTimerSet(duration);
    }

    internal static void ReviveWithCorrectPosition(PlayerAvatar target, bool revivedByTruck)
    {
        if (LastDeathPosition.TryGetValue(target, out Vector3 deathPosition))
        {
            PlayerDeathHead deathHead = Traverse.Create(target).Field("playerDeathHead").GetValue<PlayerDeathHead>();
            PhysGrabObject physGrabObject = deathHead != null
                ? Traverse.Create(deathHead).Field("physGrabObject").GetValue<PhysGrabObject>()
                : null;

            if (physGrabObject != null)
                physGrabObject.Teleport(deathPosition, physGrabObject.transform.rotation);
        }

        target.Revive(revivedByTruck);
    }
}
