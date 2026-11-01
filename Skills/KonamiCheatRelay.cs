using Photon.Pun;
using UnityEngine;

internal class KonamiCheatRelay : MonoBehaviour
{
    public static bool FreeClassChangeUnlocked = false;

    [PunRPC]
    public void SetFreeClassChangeRPC(bool unlocked, PhotonMessageInfo info)
    {
        PhotonView view = GetComponent<PhotonView>();

        if (view == null || info.Sender == null || view.Owner != info.Sender)
        {
            Plugin.Log.LogWarning("Ignored a Konami RPC that wasn't sent by the avatar's owner.");
            return;
        }

        FreeClassChangeUnlocked = unlocked;
    }
}
