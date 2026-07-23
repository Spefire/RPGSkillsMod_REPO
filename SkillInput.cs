using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillInput
{
    private static void Postfix()
    {
        if (LevelGenerator.Instance == null)
            return;

        if (!LevelGenerator.Instance.Generated)
            return;

        if (SemiFunc.RunIsLevel())
        {
            SkillManager.Update();
            if (Input.GetKeyDown(KeyCode.F))
            {
                SkillManager.TryUseSkill();
            }
        }
        else
        {
            SkillManager.ResetCooldown();
        }
    }
}