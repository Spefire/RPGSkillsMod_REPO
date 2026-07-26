using HarmonyLib;

// Fixes the cast VFX (SkillVfx.PlayCastEffect) staying permanently lit if
// the caster dies while it's still active. The effect is parented to the
// PlayerAvatar's own transform and only cleans itself up via a coroutine
// hosted on that same avatar; the game fully deactivates the avatar
// GameObject on death (PlayerAvatar.PlayerDeathDone), which silently kills
// that coroutine before it can stop/destroy the effect. Force-clean it
// right before the avatar gets deactivated so it can't survive death or
// linger after a later revive.
[HarmonyPatch(typeof(PlayerAvatar), "PlayerDeathDone")]
internal static class SkillVfxDeathPatch
{
    [HarmonyPrefix]
    private static void Prefix(PlayerAvatar __instance)
    {
        SkillVfx.ClearActiveEffects(__instance);
    }
}
