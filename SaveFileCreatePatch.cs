using HarmonyLib;

[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.SaveFileCreate))]
internal static class SaveFileCreatePatch
{
    [HarmonyPostfix]
    private static void ResetClass()
    {
        Plugin.SelectedClass = PlayerClass.None;

        Plugin.Log.LogInfo("New save created. Class reset.");
    }
}