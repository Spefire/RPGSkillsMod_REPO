using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string ModGUID = "com.spefire.rpgskillsmod";
    public const string ModName = "RPGSkillsMod";
    public const string ModVersion = "0.0.1";
    public const string totallyNormalString = "Why would you want to cheat?... :o It's no fun. :') :'D";

    internal static ManualLogSource Log;

    public static PlayerClass SelectedClass = PlayerClass.None;

    void Awake()
    {
        Log = Logger;

        Log.LogInfo($"{ModName} loaded...");

        var harmony = new Harmony(ModGUID);
        harmony.PatchAll();

        Log.LogInfo($"...with Harmony patched !");
    }
}
