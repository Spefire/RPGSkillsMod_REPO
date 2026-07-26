using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class SkillVfx
{
    private const float FadeOutBuffer = 1f;

    private static readonly Color DefaultLightColor = Color.white;

    private static readonly Dictionary<PlayerClass, Color> LightColors = new Dictionary<PlayerClass, Color>
    {
        { PlayerClass.Warrior, new Color(0.9f, 0.1f, 0.1f) },     // rouge
        { PlayerClass.Paladin, new Color(0.1f, 0.2f, 0.9f) },     // bleu
        { PlayerClass.Scout, new Color(0.9f, 0.9f, 0.1f) },       // jaune
        { PlayerClass.Druid, new Color(0.25f, 0.85f, 0.25f) },    // vert
        { PlayerClass.Mage, new Color(0.6f, 0.1f, 0.9f) },        // violet
    };

    public static void PlayCastEffect(PlayerAvatar caster, PlayerClass playerClass, float duration)
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
        effect.gameObject.name = "RPG_Particles";
        effect.transform.localPosition = new Vector3(0f, 1.1f, 0f);
        effect.transform.localRotation = Quaternion.identity;

        Color lightColor = LightColors.TryGetValue(playerClass, out Color mappedLight) ? mappedLight : DefaultLightColor;

        ParticleSystem.ShapeModule shape = effect.shape;
        shape.scale = new Vector3(0.1f, 0.1f, 0.1f);

        ParticleSystem.EmissionModule emission = effect.emission;
        emission.rateOverTime = 10f;

        foreach (ParticleSystem ps in effect.GetComponentsInChildren<ParticleSystem>(true))
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = Color.white;
            main.loop = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.2f, 1f);

            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
            if (colorOverLifetime.enabled)
                colorOverLifetime.enabled = false;

            ParticleSystem.ColorBySpeedModule colorBySpeed = ps.colorBySpeed;
            if (colorBySpeed.enabled)
                colorBySpeed.enabled = false;

            ParticleSystem.LightsModule lights = ps.lights;
            if (lights.enabled && lights.light != null)
            {
                Light lightTemplate = Object.Instantiate(lights.light, effect.transform);
                lightTemplate.gameObject.name = "RPG_LightTemplate";
                lightTemplate.color = lightColor;
                lightTemplate.enabled = false;

                lights.light = lightTemplate;
                lights.useParticleColor = false;
            }
        }

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
