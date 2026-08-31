using Photon.Pun;
using UnityEngine;

// Networked toggle for the Konami-code cheat. Once unlocked, every client
// relaxes ClassUI's lobby/shop-only restriction so each player can freely
// change their OWN class mid-level (see the FreeClassChangeUnlocked check
// added to UIs/ClassUI.cs). Broadcast via RpcTarget.All - this is a fully
// custom RPC (not a wrapper around a game method gated by MasterOnlyRPC),
// so any player can toggle it on/off for everyone, not just the host.
internal class KonamiCheatRelay : MonoBehaviour
{
    public static bool FreeClassChangeUnlocked = false;

    [PunRPC]
    public void SetFreeClassChangeRPC(bool unlocked)
    {
        FreeClassChangeUnlocked = unlocked;
    }
}
