using HarmonyLib;

[HarmonyPatch(typeof(PhysGrabObject), "Awake")]
internal static class PhysGrabObjectRelayPatch
{
    private static void Postfix(PhysGrabObject __instance)
    {
        if (__instance.GetComponent<PhysGrabObjectRelay>() == null)
            __instance.gameObject.AddComponent<PhysGrabObjectRelay>();
    }
}
