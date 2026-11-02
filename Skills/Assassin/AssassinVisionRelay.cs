using HarmonyLib;
using Photon.Pun;
using UnityEngine;

internal class AssassinVisionRelay : MonoBehaviour
{
    [PunRPC]
    public void SetVisionFreezeRPC(float duration)
    {
        PlayerAvatar target = GetComponent<PlayerAvatar>();

        if (target != null)
            target.EnemyVisionFreezeTimerSet(duration);
    }
}

[HarmonyPatch(typeof(PlayerAvatar), "Awake")]
internal static class AssassinVisionRelayPatch
{
    private static void Postfix(PlayerAvatar __instance)
    {
        if (__instance.GetComponent<AssassinVisionRelay>() == null)
            __instance.gameObject.AddComponent<AssassinVisionRelay>();
    }
}
