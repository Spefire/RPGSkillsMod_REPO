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

        LastDeathPosition.Remove(target);
    }

    internal static void RememberDeathPosition(PlayerAvatar avatar)
    {
        List<PlayerAvatar> destroyed = new List<PlayerAvatar>();

        foreach (PlayerAvatar known in LastDeathPosition.Keys)
        {
            if (known == null)
                destroyed.Add(known);
        }

        foreach (PlayerAvatar known in destroyed)
            LastDeathPosition.Remove(known);

        LastDeathPosition[avatar] = avatar.transform.position;
    }

    internal static void ClearDeathPositions()
    {
        LastDeathPosition.Clear();
    }
}
