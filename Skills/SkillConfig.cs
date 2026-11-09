using BepInEx.Configuration;

internal static class SkillConfig
{
    private static ConfigFile file;

    public static void Init(ConfigFile configFile)
    {
        file = configFile;
    }

    public static float Cooldown(string skill, float defaultValue)
    {
        float cooldown = Float(skill, "Cooldown", defaultValue, 0f, 600f, "Cooldown in seconds.");

        return Plugin.DebugAllow ? 20f : cooldown;
    }

    public static float Duration(string skill, float defaultValue)
    {
        return Float(skill, "Duration", defaultValue, 0.5f, 600f, "Duration of the effect in seconds.");
    }

    public static float Radius(string skill, float defaultValue)
    {
        return Float(skill, "Radius", defaultValue, 0.5f, 200f, "Radius of the effect in meters.");
    }

    public static float Float(string skill, string key, float defaultValue, float min, float max, string description)
    {
        if (file == null)
            return defaultValue;

        return file.Bind(
            skill,
            key,
            defaultValue,
            new ConfigDescription(description + " Requires a game restart.", new AcceptableValueRange<float>(min, max))
        ).Value;
    }

    public static int Int(string skill, string key, int defaultValue, int min, int max, string description)
    {
        if (file == null)
            return defaultValue;

        return file.Bind(
            skill,
            key,
            defaultValue,
            new ConfigDescription(description + " Requires a game restart.", new AcceptableValueRange<int>(min, max))
        ).Value;
    }
}
