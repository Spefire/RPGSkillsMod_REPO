using Photon.Pun;
using UnityEngine;

// PlayerAvatar.Revive(bool) internally sends a [PunRPC] "ReviveRPC" via
// RpcTarget.All, and ReviveRPC's body starts with
// "if (!SemiFunc.MasterOnlyRPC(_info)) return;" - it only actually revives
// the player if the RPC's SENDER is the master client. Calling
// PlayerAvatar.Revive(...) directly from a non-host caster's client
// therefore silently no-ops for everyone (same "host-only" trap documented
// for Enemy.Freeze / PlayerHealth.MaterialEffectOverride).
//
// This relay lets any client ask the master client to perform the revive
// on its behalf: send RequestReviveRPC via RpcTarget.MasterClient, so the
// method body below only ever executes on the master client - which then
// calls Revive() itself, making the master the RPC sender and satisfying
// MasterOnlyRPC for everyone.
internal class NecromancerReviveRelay : MonoBehaviour
{
    [PunRPC]
    public void RequestReviveRPC(bool revivedByTruck)
    {
        PlayerAvatar target = GetComponent<PlayerAvatar>();

        if (target != null)
            target.Revive(revivedByTruck);
    }
}
