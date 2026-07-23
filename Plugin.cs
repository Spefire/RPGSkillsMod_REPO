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
    public const string ModVersion = "0.0.1";
    public const string totallyNormalString = "Why would you want to cheat?... :o It's no fun. :') :'D";

    public static ConfigEntry<bool> EnableMod;
    public static ConfigEntry<KeyCode> PreviousClassKey;
    public static ConfigEntry<KeyCode> NextClassKey;
    public static ConfigEntry<KeyCode> SkillKey;

    internal static ManualLogSource Log;

    public static PlayerClass SelectedClass = PlayerClass.None;

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
            "Key used to activate the skill."
        );

        Log.LogInfo($"{ModName} loaded...");

        var harmony = new Harmony(ModGUID);
        harmony.PatchAll();

        Log.LogInfo($"...with Harmony patched !");
    }
}
