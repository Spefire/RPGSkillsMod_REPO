using HarmonyLib;
using TMPro;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillUI
{
    private static GameObject skillUI;

    private static TextMeshProUGUI skillText;
    private static TextMeshProUGUI lobbyDescription;
    private static TextMeshProUGUI lobbyProperties;
    private static TextMeshProUGUI levelCooldown;
    private static TextMeshProUGUI levelKey;

    private static void Postfix()
    {
        if (!Plugin.EnableMod.Value)
            return;

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
            lobbyDescription.gameObject.SetActive(true);
            lobbyProperties.gameObject.SetActive(true);
            levelCooldown.gameObject.SetActive(false);
            levelKey.gameObject.SetActive(false);
            RefreshLobbyShop();
        }
        else if (SemiFunc.RunIsLevel())
        {
            skillUI.SetActive(true);
            lobbyDescription.gameObject.SetActive(false);
            lobbyProperties.gameObject.SetActive(false);
            levelCooldown.gameObject.SetActive(true);
            RefreshLevel();
        }
        else
        {
            skillUI.SetActive(false);
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

        // Configure le texte principal
        skillText = skillUI.GetComponent<TextMeshProUGUI>();
        skillText.fontSize = 32;
        skillText.color = Color.yellow;

        RectTransform rect = skillUI.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(0, 50);
        rect.sizeDelta = new Vector2(260, 160);

        // Ajoute les nouveaux éléments
        lobbyDescription = CreateText("Description", "", new Vector2(0, -85), 18, Color.white);
        lobbyProperties = CreateText("Properties", "", new Vector2(0, -120), 18, Color.green);

        levelCooldown = CreateText("Cooldown", "", new Vector2(0, -85), 18, Color.white);
        levelKey = CreateText("Key", Plugin.SkillKey.Value.ToString() + " to use", new Vector2(0, -105), 18, Color.grey);

        RefreshLevel();
    }

    private static void RefreshLobbyShop()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass);
        skillText.text = skill.Name;
        lobbyDescription.text = skill.Description;
        lobbyProperties.text = string.Join("\n", skill.Properties.ConvertAll(p => $"• {p}"));
    }

    private static void RefreshLevel()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass);
        skillText.text = skill.Name;

        if (SkillManager.IsReady)
        {
            levelKey.gameObject.SetActive(true);
            levelCooldown.color = Color.green;
            levelCooldown.text = "READY";
        }
        else
        {
            levelKey.gameObject.SetActive(false);
            levelCooldown.color = Color.white;
            levelCooldown.text = $"{SkillManager.RemainingCooldown:0}s remaining...";
        }
    }

    private static TextMeshProUGUI CreateText(
        string name,
        string text,
        Vector2 position,
        float size,
        Color color)
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
        tmp.font = skillText.font;
        tmp.fontSharedMaterial = skillText.fontSharedMaterial;

        tmp.fontSize = size;
        tmp.color = color;
        tmp.text = text;
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