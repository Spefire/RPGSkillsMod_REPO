using UnityEngine;

internal static class SkillManager
{
    private static float remainingCooldown = 0f;

    public static bool IsReady => remainingCooldown <= 0f;

    public static float RemainingCooldown => Mathf.Max(0f, remainingCooldown);

    public static void Update()
    {
        if (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;

            if (remainingCooldown < 0f)
                remainingCooldown = 0f;
        }

        SkillDatabase.Get(Plugin.SelectedClass).Update();
    }

    public static bool TryUseSkill()
    {
        if (!IsReady)
            return false;

        Skill skill = SkillDatabase.Get(Plugin.SelectedClass);

        Plugin.Log.LogInfo($"Using skill : {skill.Name}");
        AnnounceSkillInChat(skill);

        skill.Execute();

        remainingCooldown = skill.Cooldown;

        return true;
    }

    private static void AnnounceSkillInChat(Skill skill)
    {
        if (ChatManager.instance != null)
            ChatManager.instance.ForceSendMessage($"*{skill.Name}*");
    }

    public static void ResetCooldown()
    {
        remainingCooldown = 0f;
    }
}