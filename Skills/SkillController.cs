using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillPatch
{
    private static void Postfix()
    {
        if (!Plugin.EnableMod.Value)
        {
            SkillManager.CancelAll();
            return;
        }

        ModNetwork.Announce();

        if (LevelGenerator.Instance == null || !LevelGenerator.Instance.Generated)
        {
            SkillManager.CancelAll();
            return;
        }

        if (SemiFunc.RunIsLevel())
        {
            SkillManager.Update();

            if (InputGuard.IsTyping)
                return;

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
            SkillManager.CancelAll();
            SkillManager.ResetCooldown();
            NecromancerReviveRelay.ClearDeathPositions();
        }
    }
}