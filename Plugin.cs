using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string ModGUID = "com.spefire.rpgskillsmod";
    public const string ModName = "RPGSkillsMod";
    public const string ModVersion = "1.0.0";
    public const string totallyNormalString = "Why would you want to cheat?... :o It's no fun. :') :'D";

    public static ConfigEntry<bool> EnableMod;
    public static ConfigEntry<KeyCode> PreviousClassKey;
    public static ConfigEntry<KeyCode> NextClassKey;
    public static ConfigEntry<KeyCode> SkillKey;
    public static ConfigEntry<KeyCode> SkillKey2;

    internal static ManualLogSource Log;

    public static PlayerClass SelectedClass = PlayerClass.None;
    public static bool DebugAllow = false;

    void Awake()
    {
        Log = Logger;

        EnableMod = Config.Bind(
            "General",
            "EnableMod",
            true,
            "Enable or disable the mod."
        );

        PreviousClassKey = Config.Bind(
            "Controls",
            "PreviousClassKey",
            KeyCode.F6,
            "Key used to select the previous class."
        );

        NextClassKey = Config.Bind(
            "Controls",
            "NextClassKey",
            KeyCode.F8,
            "Key used to select the next class."
        );

        SkillKey = Config.Bind(
            "Controls",
            "SkillKey",
            KeyCode.F,
            "Key used to activate the primary skill."
        );

        SkillKey2 = Config.Bind(
            "Controls",
            "SkillKey2",
            KeyCode.G,
            "Key used to activate the secondary skill."
        );

        Log.LogInfo($"{ModName} loaded...");

        var harmony = new Harmony(ModGUID);
        harmony.PatchAll();

        Log.LogInfo($"...with Harmony patched !");
    }
}
