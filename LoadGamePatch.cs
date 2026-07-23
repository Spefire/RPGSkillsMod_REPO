using UnityEngine;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.LoadGame))]
internal static class LoadGamePatch
{
    [HarmonyPostfix]
    private static void LoadClass(string fileName, List<string> backupList)
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "saves",
            fileName,
            fileName + ".es3");

        ES3Settings settings = new ES3Settings(
            path,
            ES3.EncryptionType.AES,
            Plugin.totallyNormalString);

        if (!ES3.KeyExists("selectedClass", settings))
        {
            Plugin.SelectedClass = PlayerClass.None;

            Plugin.Log.LogInfo("No saved class found.");

            return;
        }

        string className = ES3.Load<string>(
            "selectedClass",
            settings);

        if (Enum.TryParse(className, out PlayerClass playerClass))
        {
            Plugin.SelectedClass = playerClass;

            Plugin.Log.LogInfo($"Loaded class: {playerClass}");
        }
        else
        {
            Plugin.SelectedClass = PlayerClass.None;

            Plugin.Log.LogWarning($"Invalid saved class: {className}");
        }
    }
}