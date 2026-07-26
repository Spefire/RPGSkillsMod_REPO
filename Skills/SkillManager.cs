using System;
using UnityEngine;

internal static class SkillManager
{
    private static float remainingCooldownPrimary = 0f;
    private static float remainingCooldownSecondary = 0f;

    public static bool IsReady(SkillSlot slot)
    {
        return RemainingCooldown(slot) <= 0f;
    }

    public static float RemainingCooldown(SkillSlot slot)
    {
        return Mathf.Max(0f, slot == SkillSlot.Primary ? remainingCooldownPrimary : remainingCooldownSecondary);
    }

    public static void Update()
    {
        if (remainingCooldownPrimary > 0f)
        {
            remainingCooldownPrimary -= Time.deltaTime;

            if (remainingCooldownPrimary < 0f)
                remainingCooldownPrimary = 0f;
        }

        if (remainingCooldownSecondary > 0f)
        {
            remainingCooldownSecondary -= Time.deltaTime;

            if (remainingCooldownSecondary < 0f)
                remainingCooldownSecondary = 0f;
        }

        SkillDatabase.Get(Plugin.SelectedClass, SkillSlot.Primary).Update();
        SkillDatabase.Get(Plugin.SelectedClass, SkillSlot.Secondary).Update();
    }

    public static bool TryUseSkill(SkillSlot slot)
    {
        if (!IsReady(slot))
            return false;

        Skill skill = SkillDatabase.Get(Plugin.SelectedClass, slot);

        if (!skill.Execute())
            return false;

        Plugin.Log.LogInfo($"Using skill : {skill.Name}");
        AnnounceSkillInChat(skill);

        if (slot == SkillSlot.Primary)
            remainingCooldownPrimary = skill.Cooldown;
        else
            remainingCooldownSecondary = skill.Cooldown;

        try
        {
            SkillVfx.PlayCastEffect(PlayerAvatar.instance, Plugin.SelectedClass, skill.ActiveDuration);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"SkillVfx failed: {ex}");
        }

        return true;
    }

    private static void AnnounceSkillInChat(Skill skill)
    {
        if (ChatManager.instance != null)
            ChatManager.instance.ForceSendMessage($"{skill.Name.ToUpper()}");
    }

    public static void ResetCooldown()
    {
        remainingCooldownPrimary = 0f;
        remainingCooldownSecondary = 0f;
    }
}