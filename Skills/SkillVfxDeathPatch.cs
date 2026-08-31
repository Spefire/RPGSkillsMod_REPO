using HarmonyLib;

[HarmonyPatch(typeof(PlayerAvatar), "PlayerDeathDone")]
internal static class SkillVfxDeathPatch
{
    [HarmonyPrefix]
    private static void Prefix(PlayerAvatar __instance)
    {
        SkillVfx.ClearActiveEffects(__instance);
    }
}
