using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using TMPro;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class RunManager_Update_Patch
{
    public static GameObject classUI;

    private static void Postfix()
    {
        if (LevelGenerator.Instance == null)
        {
            return;
        }

        if (!LevelGenerator.Instance.Generated)
        {
            return;
        }

        if (classUI == null)
        {
            CreateUI();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Plugin.Log.LogInfo("F5 détectée !");
            HealPlayer();
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            if (!SemiFunc.RunIsLobby() && !SemiFunc.RunIsShop())
                return;
            PreviousClass();
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            if (!SemiFunc.RunIsLobby() && !SemiFunc.RunIsShop())
                return;
            NextClass();
            UpdateUI();
        }
    }

    private static void CreateUI()
    {
        classUI = UnityEngine.Object.Instantiate(
            EnergyUI.instance.gameObject,
            EnergyUI.instance.transform.parent
        );
        UnityEngine.Object.Destroy(classUI.GetComponent<EnergyUI>());
        UpdateUI();
    }

    private static void UpdateUI()
    {
        TextMeshProUGUI text = classUI.GetComponent<TextMeshProUGUI>();
        text.text = GetClassName();
        text.color = Color.white;
    }

    public static string GetClassName()
    {
        switch (Plugin.SelectedClass)
        {
            case PlayerClass.None:
                return "Aucune classe";

            case PlayerClass.Druid:
                return "Druide";

            case PlayerClass.Warrior:
                return "Guerrier";

            case PlayerClass.Assassin:
                return "Assassin";

            default:
                return Plugin.SelectedClass.ToString();
        }
    }

    private static void PreviousClass()
    {
        int count = Enum.GetValues(typeof(PlayerClass)).Length;

        Plugin.SelectedClass =
            (PlayerClass)(((int)Plugin.SelectedClass - 1 + count) % count);

        Plugin.Log.LogInfo($"Classe : {Plugin.SelectedClass}");
    }

    private static void NextClass()
    {
        int count = Enum.GetValues(typeof(PlayerClass)).Length;

        Plugin.SelectedClass =
            (PlayerClass)(((int)Plugin.SelectedClass + 1) % count);

        Plugin.Log.LogInfo($"Classe : {Plugin.SelectedClass}");
    }

    private static void HealPlayer()
    {
        try
        {
            PlayerAvatar avatar = PlayerAvatar.instance;

            if (avatar == null || avatar.playerHealth == null)
            {
                Plugin.Log.LogWarning("Impossible de soigner : joueur local introuvable.");
                return;
            }

            avatar.playerHealth.Heal(9999);

            Plugin.Log.LogInfo("Joueur soigné !");
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"HealPlayer a échoué : {ex}");
        }
    }
}