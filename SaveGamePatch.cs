using UnityEngine;
using HarmonyLib;
using System.IO;

[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.SaveGame))]
internal static class SaveGamePatch
{
    [HarmonyPostfix]
    private static void SaveClass(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return;

        string path = Path.Combine(
            Application.persistentDataPath,
            "saves",
            fileName,
            fileName + ".es3");

        ES3Settings settings = new ES3Settings(
            path,
            ES3.EncryptionType.AES,
            Plugin.totallyNormalString);

        ES3.Save(
            "selectedClass",
            Plugin.SelectedClass.ToString(),
            settings);

        Plugin.Log.LogInfo($"Saved class: {Plugin.SelectedClass}");
    }
}