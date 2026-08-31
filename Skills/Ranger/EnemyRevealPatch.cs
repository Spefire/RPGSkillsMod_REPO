using HarmonyLib;

[HarmonyPatch(typeof(EnemyParent), "Awake")]
internal static class EnemyRevealPatch
{
    private static void Postfix(EnemyParent __instance)
    {
        if (__instance.GetComponent<EnemyMapReveal>() == null)
            __instance.gameObject.AddComponent<EnemyMapReveal>();
    }
}
