using HarmonyLib;
using TMPro;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillUI
{
    private static GameObject skillUI;
    private static GameObject lobbyShopRoot;
    private static GameObject levelRoot;

    private static TextMeshProUGUI lobbyTitle;
    private static TextMeshProUGUI lobbyDescription;
    private static TextMeshProUGUI lobbyProperties;

    private static TextMeshProUGUI levelTitle;
    private static TextMeshProUGUI levelCooldown;

    private static PlayerClass currentClass = PlayerClass.None;

    private static void Postfix()
    {
        if (LevelGenerator.Instance == null)
            return;

        if (!LevelGenerator.Instance.Generated)
            return;

        if (skillUI == null)
            CreateUI();

        if (Plugin.SelectedClass == PlayerClass.None)
        {
            skillUI.SetActive(false);
            return;
        }

        if (SemiFunc.RunIsLobby() || SemiFunc.RunIsShop())
        {
            skillUI.SetActive(true);
            lobbyShopRoot.SetActive(true);
            levelRoot.SetActive(false);

            if (currentClass != Plugin.SelectedClass)
            {
                currentClass = Plugin.SelectedClass;
                RefreshLobbyShop();
            }
        }
        else if (SemiFunc.RunIsLevel())
        {
            skillUI.SetActive(true);
            lobbyShopRoot.SetActive(false);
            levelRoot.SetActive(true);
            RefreshLevel();
        }
        else
        {
            skillUI.SetActive(false);
            lobbyShopRoot.SetActive(false);
            levelRoot.SetActive(false);
        }
    }

    private static void CreateUI()
    {
        // Crée le conteneur principal
        skillUI = UnityEngine.Object.Instantiate(
            EnergyUI.instance.gameObject,
            EnergyUI.instance.transform.parent);

        skillUI.name = "SkillUI";

        UnityEngine.Object.Destroy(skillUI.GetComponent<EnergyUI>());

        // Supprime les anciens enfants de l'UI Energy
        DestroyChild("EnergyMax");
        DestroyChild("Zap");
        DestroyChild("Scanlines");

        lobbyShopRoot = new GameObject("LobbyShop");
        lobbyShopRoot.transform.SetParent(skillUI.transform, false);

        levelRoot = new GameObject("Level");
        levelRoot.transform.SetParent(skillUI.transform, false);

        // Configure le texte principal
        lobbyTitle = CreateText(
            lobbyShopRoot.transform,
            "Title",
            Vector2.zero,
            32);

        lobbyDescription = CreateText(
            lobbyShopRoot.transform,
            "Description",
            new Vector2(0, -35),
            18);

        lobbyProperties = CreateText(
            lobbyShopRoot.transform,
            "Properties",
            new Vector2(0, -70),
            18);

        levelTitle = CreateText(
            levelRoot.transform,
            "Title",
            Vector2.zero,
            32);

        levelCooldown = CreateText(
            levelRoot.transform,
            "Cooldown",
            new Vector2(0, -40),
            24);

        RectTransform rect = skillUI.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(260, 160);

        RefreshLevel();
    }

    private static void RefreshLobbyShop()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass);
        lobbyTitle.text = skill.Name;
        lobbyDescription.text = skill.Description;
        lobbyProperties.text = string.Join("\n", skill.Properties.ConvertAll(p => $"• {p}"));
        lobbyProperties.color = Color.green;
    }

    private static void RefreshLevel()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass);
        levelTitle.text = skill.Name;

        if (SkillManager.IsReady)
        {
            levelCooldown.color = Color.green;
            levelCooldown.text = "READY !";
        }
        else
        {
            levelCooldown.color = Color.white;
            levelCooldown.text = $"{SkillManager.RemainingCooldown:0}s remaining...";
        }
    }

    private static TextMeshProUGUI CreateText(
        Transform parent,
        string name,
        Vector2 position,
        float size)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(skillUI.transform, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(480, 50);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();

        // On récupère les mêmes réglages que le texte principal
        tmp.font = lobbyTitle.font;
        tmp.fontSharedMaterial = lobbyTitle.fontSharedMaterial;

        tmp.fontSize = size;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.TopLeft;

        return tmp;
    }

    private static void DestroyChild(string child)
    {
        Transform t = skillUI.transform.Find(child);

        if (t != null)
            UnityEngine.Object.Destroy(t.gameObject);
    }
}