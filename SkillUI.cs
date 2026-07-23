using HarmonyLib;
using TMPro;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillUI
{
    public static GameObject skillUI;
    private static TextMeshProUGUI title;
    private static TextMeshProUGUI description;
    private static TextMeshProUGUI properties;

    private static PlayerClass currentClass = PlayerClass.None;

    private static void Postfix()
    {
        if (LevelGenerator.Instance == null)
            return;

        if (!LevelGenerator.Instance.Generated)
            return;

        if (skillUI == null)
            CreateUI();

        if (SemiFunc.RunIsLobby() || SemiFunc.RunIsShop())
        {
            skillUI.SetActive(true);

            if (currentClass != Plugin.SelectedClass)
            {
                currentClass = Plugin.SelectedClass;
                RefreshLobbyShop();
            }
        }
        else if (SemiFunc.RunIsLevel())
        {
            skillUI.SetActive(true);
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
        title = skillUI.GetComponent<TextMeshProUGUI>();
        title.fontSize = 32;
        title.alignment = TextAlignmentOptions.TopLeft;

        description = CreateText(
            "Description",
            new Vector2(0, -35),
            18);

        properties = CreateText(
            "Properties",
            new Vector2(0, -60),
            18);

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
        title.text = skill.Name;
        description.text = skill.Description;
        properties.text = string.Join("\n", skill.Properties.ConvertAll(p => $"• {p}"));
    }

    private static void RefreshLevel()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass);
        title.text = skill.Name;
        description.text = "";
        if (SkillManager.IsReady)
        {
            properties.color = Color.green;
            properties.text = "READY";
        }
        else
        {
            properties.color = Color.white;
            properties.text = $"{SkillManager.RemainingCooldown:0}s";
        }
    }

    private static TextMeshProUGUI CreateText(
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
        tmp.font = title.font;
        tmp.fontSharedMaterial = title.fontSharedMaterial;

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