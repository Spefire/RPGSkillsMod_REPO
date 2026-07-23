using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string ModGUID = "com.spefire.reponinitotosmod";
    public const string ModName = "NiniTotosMod";
    public const string ModVersion = "0.0.1";

    internal static ManualLogSource Log;

    public static PlayerClass SelectedClass = PlayerClass.None;

    void Awake()
    {
        Log = Logger;

        Log.LogInfo($"{ModName} chargé...");

        var harmony = new Harmony(ModGUID);
        harmony.PatchAll();

        Log.LogInfo($"...avec Harmony patché !");
    }
}
