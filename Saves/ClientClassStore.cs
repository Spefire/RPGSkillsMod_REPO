using System;
using System.IO;
using UnityEngine;

internal static class ClientClassStore
{
    private const string FileName = "rpgSkillsMod_class.es3";
    private const string Key = "selectedClass";

    private static ES3Settings CreateSettings()
    {
        return new ES3Settings(
            Path.Combine(Application.persistentDataPath, FileName),
            ES3.EncryptionType.AES,
            Plugin.totallyNormalString);
    }

    public static PlayerClass Load()
    {
        try
        {
            ES3Settings settings = CreateSettings();

            if (!ES3.KeyExists(Key, settings))
                return PlayerClass.None;

            string className = ES3.Load<string>(Key, settings);

            return Enum.TryParse(className, out PlayerClass playerClass) ? playerClass : PlayerClass.None;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"Failed to read the local class file: {ex.Message}");
            return PlayerClass.None;
        }
    }

    public static void Save(PlayerClass playerClass)
    {
        try
        {
            ES3.Save(Key, playerClass.ToString(), CreateSettings());
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"Failed to write the local class file: {ex.Message}");
        }
    }
}
