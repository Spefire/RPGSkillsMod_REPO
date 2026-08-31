using Photon.Pun;
using UnityEngine;

internal class KonamiCheatRelay : MonoBehaviour
{
    public static bool FreeClassChangeUnlocked = false;

    [PunRPC]
    public void SetFreeClassChangeRPC(bool unlocked)
    {
        FreeClassChangeUnlocked = unlocked;
    }
}
