using HarmonyLib;

// Adds KonamiCheatRelay to every PlayerAvatar's GameObject (local AND
// remote copies on every client), same pattern as NecromancerRevivePatch.
// Photon resolves [PunRPC] methods by scanning components already present
// on the target networked GameObject at receive time, so the relay must
// exist on every client's copy of every avatar before the RPC is ever sent.
[HarmonyPatch(typeof(PlayerAvatar), "Awake")]
internal static class KonamiCheatPatch
{
    private static void Postfix(PlayerAvatar __instance)
    {
        if (__instance.GetComponent<KonamiCheatRelay>() == null)
            __instance.gameObject.AddComponent<KonamiCheatRelay>();
    }
}
