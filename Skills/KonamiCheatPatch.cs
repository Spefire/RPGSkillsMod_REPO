using HarmonyLib;

[HarmonyPatch(typeof(PlayerAvatar), "Awake")]
internal static class KonamiCheatPatch
{
    private static void Postfix(PlayerAvatar __instance)
    {
        if (__instance.GetComponent<KonamiCheatRelay>() == null)
            __instance.gameObject.AddComponent<KonamiCheatRelay>();
    }
}
