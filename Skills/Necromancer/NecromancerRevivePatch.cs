using HarmonyLib;

[HarmonyPatch(typeof(PlayerAvatar), "Awake")]
internal static class NecromancerRevivePatch
{
    private static void Postfix(PlayerAvatar __instance)
    {
        if (__instance.GetComponent<NecromancerReviveRelay>() == null)
            __instance.gameObject.AddComponent<NecromancerReviveRelay>();
    }
}

[HarmonyPatch(typeof(PlayerAvatar), "PlayerDeathDone")]
internal static class NecromancerDeathPositionPatch
{
    [HarmonyPrefix]
    private static void Prefix(PlayerAvatar __instance)
    {
        NecromancerReviveRelay.LastDeathPosition[__instance] = __instance.transform.position;
    }
}
