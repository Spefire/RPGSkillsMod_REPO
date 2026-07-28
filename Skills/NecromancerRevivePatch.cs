using HarmonyLib;

// Adds NecromancerReviveRelay to every PlayerAvatar's GameObject (local AND
// remote copies on every client) as soon as it's created. Photon resolves
// [PunRPC] methods by scanning components already present on the target
// networked GameObject at receive time, so the relay must exist on every
// client's copy of every avatar before NecromancerSkill ever sends the RPC.
[HarmonyPatch(typeof(PlayerAvatar), "Awake")]
internal static class NecromancerRevivePatch
{
    private static void Postfix(PlayerAvatar __instance)
    {
        if (__instance.GetComponent<NecromancerReviveRelay>() == null)
            __instance.gameObject.AddComponent<NecromancerReviveRelay>();
    }
}
