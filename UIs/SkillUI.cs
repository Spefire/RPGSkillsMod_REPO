using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UnityEngine;

[HarmonyPatch(typeof(RunManager), "Update")]
internal static class SkillUI
{
    private static GameObject skillUI;
    private static GameObject skillUI2;

    private static TextMeshProUGUI skillText;
    private static TextMeshProUGUI lobbyDescription;
    private static TextMeshProUGUI lobbyProperties;
    private static TextMeshProUGUI levelCooldown;
    private static TextMeshProUGUI levelKey;

    private static TextMeshProUGUI skillText2;
    private static TextMeshProUGUI lobbyDescription2;
    private static TextMeshProUGUI lobbyProperties2;
    private static TextMeshProUGUI levelCooldown2;
    private static TextMeshProUGUI levelKey2;

    private static void Postfix()
    {
        if (!Plugin.EnableMod.Value)
            return;

        if (LevelGenerator.Instance == null)
            return;

        if (!LevelGenerator.Instance.Generated)
            return;

        if (skillUI == null && skillUI2 == null)
            CreateUI();

        if (Plugin.SelectedClass == PlayerClass.None)
        {
            skillUI.SetActive(false);
            skillUI2.SetActive(false);
            return;
        }

        if (SemiFunc.RunIsLobby() || SemiFunc.RunIsShop())
        {
            skillUI.SetActive(true);
            lobbyDescription.gameObject.SetActive(true);
            lobbyProperties.gameObject.SetActive(true);
            levelCooldown.gameObject.SetActive(false);
            levelKey.gameObject.SetActive(false);

            skillUI2.SetActive(true);
            lobbyDescription2.gameObject.SetActive(true);
            lobbyProperties2.gameObject.SetActive(true);
            levelCooldown2.gameObject.SetActive(false);
            levelKey2.gameObject.SetActive(false);

            RefreshLobbyShop();
        }
        else if (SemiFunc.RunIsLevel())
        {
            skillUI.SetActive(true);
            lobbyDescription.gameObject.SetActive(false);
            lobbyProperties.gameObject.SetActive(false);
            levelCooldown.gameObject.SetActive(true);

            skillUI2.SetActive(true);
            lobbyDescription2.gameObject.SetActive(false);
            lobbyProperties2.gameObject.SetActive(false);
            levelCooldown2.gameObject.SetActive(true);

            RefreshLevel();
        }
        else
        {
            skillUI.SetActive(false);
            skillUI2.SetActive(false);
        }
    }

    private static void CreateUI()
    {
        // Crée les conteneurs principaux
        skillUI = UnityEngine.Object.Instantiate(
            EnergyUI.instance.gameObject,
            EnergyUI.instance.transform.parent);
        skillUI2 = UnityEngine.Object.Instantiate(
            EnergyUI.instance.gameObject,
            EnergyUI.instance.transform.parent);

        skillUI.name = "SkillUI";
        skillUI2.name = "SkillUI";

        UnityEngine.Object.Destroy(skillUI.GetComponent<EnergyUI>());
        UnityEngine.Object.Destroy(skillUI2.GetComponent<EnergyUI>());

        // Supprime TOUS les anciens enfants de l'UI Energy clonée (on
        // reconstruit tout nous-mêmes avec CreateText juste après). L'ancien
        // code ne détruisait que 3 enfants connus par leur nom ("EnergyMax",
        // "Zap", "Scanlines"), ce qui laissait passer d'éventuels autres
        // enfants du prefab EnergyUI d'origine (ex: un objet "Description"
        // avec un TMP_SpriteAnimator repéré dans l'inspecteur runtime) qui
        // entrait alors en collision de nom avec notre propre "Description"
        // créé plus bas.
        DestroyAllChildren(skillUI);
        DestroyAllChildren(skillUI2);

        // Configure le texte principal
        skillText = skillUI.GetComponent<TextMeshProUGUI>();
        skillText.fontSize = 28;
        skillText.color = Color.yellow;

        skillText2 = skillUI2.GetComponent<TextMeshProUGUI>();
        skillText2.fontSize = 28;
        skillText2.color = Color.yellow;

        RectTransform rect = skillUI.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(0, 130);
        rect.sizeDelta = new Vector2(260, 160);

        RectTransform rect2 = skillUI2.GetComponent<RectTransform>();
        rect2.anchorMin = new Vector2(0, 0);
        rect2.anchorMax = new Vector2(0, 0);
        rect2.pivot = new Vector2(0, 0);
        rect2.anchoredPosition = new Vector2(0, 20);
        rect2.sizeDelta = new Vector2(260, 160);

        // Ajoute les nouveaux éléments
        lobbyDescription = CreateText(skillUI.transform, "Description", "", new Vector2(0, -85), 18, Color.white);
        lobbyProperties = CreateText(skillUI.transform, "Properties", "", new Vector2(0, -110), 12, Color.green);

        levelCooldown = CreateText(skillUI.transform, "Cooldown", "", new Vector2(0, -85), 18, Color.white);
        levelKey = CreateText(skillUI.transform, "Key", Plugin.SkillKey.Value.ToString() + " to use", new Vector2(0, -105), 18, Color.grey);

        lobbyDescription2 = CreateText(skillUI2.transform, "Description", "", new Vector2(0, -85), 18, Color.white);
        lobbyProperties2 = CreateText(skillUI2.transform, "Properties", "", new Vector2(0, -110), 12, Color.green);

        levelCooldown2 = CreateText(skillUI2.transform, "Cooldown", "", new Vector2(0, -85), 18, Color.white);
        levelKey2 = CreateText(skillUI2.transform, "Key", Plugin.SkillKey2.Value.ToString() + " to use", new Vector2(0, -105), 18, Color.grey);

        RefreshLevel();
    }

    private static void RefreshLobbyShop()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass, SkillSlot.Primary);
        skillText.text = skill.Name;
        lobbyDescription.text = skill.Description;

        List<string> properties = new List<string>(skill.Properties)
        {
            $"Cooldown: {skill.Cooldown:0}s"
        };

        lobbyProperties.text = string.Join("\n", properties.ConvertAll(p => $"• {p}"));

        Skill skill2 = SkillDatabase.Get(Plugin.SelectedClass, SkillSlot.Secondary);
        skillText2.text = skill2.Name;
        lobbyDescription2.text = skill2.Description;

        List<string> properties2 = new List<string>(skill2.Properties)
        {
            $"Cooldown: {skill2.Cooldown:0}s"
        };

        lobbyProperties2.text = string.Join("\n", properties2.ConvertAll(p => $"• {p}"));
    }

    private static void RefreshLevel()
    {
        Skill skill = SkillDatabase.Get(Plugin.SelectedClass, SkillSlot.Primary);
        skillText.text = skill.Name;

        if (SkillManager.IsReady(SkillSlot.Primary))
        {
            levelKey.gameObject.SetActive(true);
            levelCooldown.color = Color.green;
            levelCooldown.text = "READY";
        }
        else
        {
            levelKey.gameObject.SetActive(false);
            levelCooldown.color = Color.white;
            levelCooldown.text = $"{SkillManager.RemainingCooldown(SkillSlot.Primary):0}s remaining...";
        }

        Skill skill2 = SkillDatabase.Get(Plugin.SelectedClass, SkillSlot.Secondary);
        skillText2.text = skill2.Name;

        if (SkillManager.IsReady(SkillSlot.Secondary))
        {
            levelKey2.gameObject.SetActive(true);
            levelCooldown2.color = Color.green;
            levelCooldown2.text = "READY";
        }
        else
        {
            levelKey2.gameObject.SetActive(false);
            levelCooldown2.color = Color.white;
            levelCooldown2.text = $"{SkillManager.RemainingCooldown(SkillSlot.Secondary):0}s remaining...";
        }
    }

    private static TextMeshProUGUI CreateText(
        Transform uiParent,
        string name,
        string text,
        Vector2 position,
        float size,
        Color color)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(uiParent, false);

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
        tmp.fontSizeMin = size;
        tmp.color = color;
        tmp.text = text;
        tmp.alignment = TextAlignmentOptions.TopLeft;

        return tmp;
    }

    private static void DestroyAllChildren(GameObject parent)
    {
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
            UnityEngine.Object.Destroy(parent.transform.GetChild(i).gameObject);
    }
}