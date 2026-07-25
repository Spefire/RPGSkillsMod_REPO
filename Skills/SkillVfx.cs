using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class SkillVfx
{
    private const float DefaultDuration = 3.5f;
    private const float FadeOutBuffer = 1f;

    private static readonly Color DefaultColor = Color.white;

    private static readonly Dictionary<PlayerClass, Color> ClassColors = new Dictionary<PlayerClass, Color>
    {
        { PlayerClass.Warrior, new Color(0.85f, 0.15f, 0.10f) },        // rouge
        { PlayerClass.Paladin, new Color(1.00f, 0.84f, 0.30f) },        // doré
        { PlayerClass.Assassin, new Color(0.55f, 0.10f, 0.75f) },       // violet
        { PlayerClass.Druid, new Color(0.25f, 0.80f, 0.25f) },          // vert
        { PlayerClass.Necromancer, new Color(0.10f, 0.40f, 0.20f) },    // vert sombre
        { PlayerClass.Scout, new Color(0.20f, 0.85f, 0.85f) },          // cyan
        { PlayerClass.TreasureHunter, new Color(1.00f, 0.90f, 0.20f) }, // jaune
        { PlayerClass.Blacksmith, new Color(1.00f, 0.50f, 0.10f) },     // orange
    };

    public static void PlayCastEffect(PlayerAvatar caster, PlayerClass playerClass, float duration = DefaultDuration)
    {
        if (caster == null || playerClass == PlayerClass.None)
            return;

        GameObject template = AssetManager.instance != null ? AssetManager.instance.prefabTeleportEffect : null;
        if (template == null)
            return;

        ParticleSystem source = template.GetComponentInChildren<ParticleSystem>();
        if (source == null)
            return;

        ParticleSystem effect = Object.Instantiate(source, caster.transform);
        effect.transform.localPosition = new Vector3(0f, 1.1f, 0f);
        effect.transform.localRotation = Quaternion.identity;

        Color color = ClassColors.TryGetValue(playerClass, out Color mapped) ? mapped : DefaultColor;

        ParticleSystem.MainModule main = effect.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color);
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1f);

        effect.Play(withChildren: true);

        caster.StartCoroutine(StopAndDestroy(effect, duration));
    }

    private static IEnumerator StopAndDestroy(ParticleSystem effect, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (effect == null)
            yield break;

        effect.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);

        yield return new WaitForSeconds(FadeOutBuffer);

        if (effect != null)
            Object.Destroy(effect.gameObject);
    }
}
