using BepInEx.Logging;
using HarmonyLib;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class ClassUI
{
    public static GameObject classUI;
    private static TextMeshProUGUI classText;
    private static TextMeshProUGUI leftArrow;
    private static TextMeshProUGUI rightArrow;
    private static TextMeshProUGUI leftKey;
    private static TextMeshProUGUI rightKey;

    private static void Postfix()
    {
        if (LevelGenerator.Instance == null)
            return;

        if (!LevelGenerator.Instance.Generated)
            return;

        if (classUI == null)
            CreateUI();


        if (SemiFunc.RunIsLobby() || SemiFunc.RunIsShop())
        {
            classUI.SetActive(true);
            leftArrow.gameObject.SetActive(true);
            rightArrow.gameObject.SetActive(true);
            leftKey.gameObject.SetActive(true);
            rightKey.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F6))
            {
                PreviousClass();
                Refresh();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                NextClass();
                Refresh();
            }
        }
        else if (SemiFunc.RunIsLevel())
        {
            classUI.SetActive(true);
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            leftKey.gameObject.SetActive(false);
            rightKey.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.F5))
            {
                Plugin.Log.LogInfo("F5 touched !");
                HealPlayer();
            }
        }
        else
        {
            classUI.SetActive(false);
        }
    }

    private static void CreateUI()
    {
        // Crée le conteneur principal
        classUI = UnityEngine.Object.Instantiate(
            EnergyUI.instance.gameObject,
            EnergyUI.instance.transform.parent);

        classUI.name = "ClassUI";

        UnityEngine.Object.Destroy(classUI.GetComponent<EnergyUI>());

        // Supprime les anciens enfants de l'UI Energy
        DestroyIfExists(classUI.transform, "EnergyMax");
        DestroyIfExists(classUI.transform, "Zap");
        DestroyIfExists(classUI.transform, "Scanlines");

        // Configure le texte principal
        classText = classUI.GetComponent<TextMeshProUGUI>();
        classText.fontSize = 32;
        classText.color = Color.white;
        classText.alignment = TextAlignmentOptions.MidlineLeft;

        RectTransform rect = classUI.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(260, 30);
        rect.anchoredPosition = new Vector2(0, -62);

        // Ajoute les nouveaux éléments
        leftArrow = CreateChildText("LeftArrow", "◀", new Vector2(-120, -25), 24, Color.yellow);
        rightArrow = CreateChildText("RightArrow", "▶", new Vector2(-90, -25), 24, Color.yellow);

        leftKey = CreateChildText("LeftKey", "F6", new Vector2(-120, -50), 18, Color.white);
        rightKey = CreateChildText("RightKey", "F8", new Vector2(-90, -50), 18, Color.white);

        Refresh();
    }

    private static TextMeshProUGUI CreateChildText(
        string name,
        string text,
        Vector2 position,
        float size,
        Color color)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(classUI.transform, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = position;

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();

        // On récupère les mêmes réglages que le texte principal
        tmp.font = classText.font;
        tmp.fontSharedMaterial = classText.fontSharedMaterial;
        tmp.alignment = TextAlignmentOptions.Center;

        tmp.fontSize = size;
        tmp.color = color;
        tmp.text = text;

        return tmp;
    }

    private static void Refresh()
    {
        if (classText == null)
            return;

        classText.text = Plugin.SelectedClass.ToString();
    }

    private static void PreviousClass()
    {
        int count = Enum.GetValues(typeof(PlayerClass)).Length;

        Plugin.SelectedClass =
            (PlayerClass)(((int)Plugin.SelectedClass - 1 + count) % count);
    }

    private static void NextClass()
    {
        int count = Enum.GetValues(typeof(PlayerClass)).Length;

        Plugin.SelectedClass =
            (PlayerClass)(((int)Plugin.SelectedClass + 1) % count);
    }

    private static void HealPlayer()
    {
        try
        {
            PlayerAvatar avatar = PlayerAvatar.instance;

            if (avatar == null || avatar.playerHealth == null)
            {
                Plugin.Log.LogWarning("Heal impossible : player not found.");
                return;
            }

            avatar.playerHealth.Heal(9999);

            Plugin.Log.LogInfo("Player healed !");
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"Heal failed : {ex}");
        }
    }

    private static void DestroyIfExists(Transform parent, string name)
    {
        Transform t = parent.Find(name);

        if (t != null)
            UnityEngine.Object.Destroy(t.gameObject);
    }
}