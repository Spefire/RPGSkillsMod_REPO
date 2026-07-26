using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillPatch
{
    private static void Postfix()
    {
        if (!Plugin.EnableMod.Value)
            return;

        if (LevelGenerator.Instance == null)
            return;

        if (!LevelGenerator.Instance.Generated)
            return;

        if (SemiFunc.RunIsLevel())
        {
            SkillManager.Update();

            if (Input.GetKeyDown(Plugin.SkillKey.Value))
            {
                SkillManager.TryUseSkill(SkillSlot.Primary);
            }

            if (Input.GetKeyDown(Plugin.SkillKey2.Value))
            {
                SkillManager.TryUseSkill(SkillSlot.Secondary);
            }
        }
        else
        {
            SkillManager.ResetCooldown();
        }
    }
}