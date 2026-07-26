using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class SkillVfx
{
    private const float FadeOutBuffer = 1f;
    private const string CastEffectName = "RPG_Particles";

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
        effect.gameObject.name = CastEffectName;
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

    // When a player dies, their PlayerAvatar GameObject gets fully
    // deactivated (SetActive(false)) by the game. Since PlayCastEffect's
    // cast VFX is parented to that same transform and cleaned up by a
    // coroutine hosted on it, deactivating the avatar silently kills that
    // coroutine mid-way - the particle/light effect never gets stopped or
    // destroyed. It then sits there (still "playing"/looping) inside the
    // disabled hierarchy and reappears permanently once the avatar is
    // reactivated (revive or respawn). Call this right when the player
    // dies to force-clean any leftover cast VFX immediately.
    public static void ClearActiveEffects(PlayerAvatar avatar)
    {
        if (avatar == null)
            return;

        Transform existing = avatar.transform.Find(CastEffectName);
        if (existing == null)
            return;

        ParticleSystem effect = existing.GetComponent<ParticleSystem>();
        if (effect != null)
            effect.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Object.Destroy(existing.gameObject);
    }

    // Bigger, world-anchored "explosion" style burst for AoE impact skills
    // (e.g. Monk's Shockwave), as opposed to PlayCastEffect's small burst
    // that follows the caster. Not parented to anyone since it represents
    // a one-shot blast at a fixed point, plus a real camera shake (same
    // public API the game's own ItemShockwave/explosive grenades use) for
    // extra impact.
    public static void PlayShockwaveEffect(Vector3 position, PlayerClass playerClass, float radius, float duration = 1.2f)
    {
        GameObject template = AssetManager.instance != null ? AssetManager.instance.prefabTeleportEffect : null;
        if (template == null)
            return;

        ParticleSystem source = template.GetComponentInChildren<ParticleSystem>();
        if (source == null)
            return;

        ParticleSystem effect = Object.Instantiate(source, position, Quaternion.identity);
        effect.gameObject.name = "RPG_Shockwave";

        Color lightColor = LightColors.TryGetValue(playerClass, out Color mappedLight) ? mappedLight : DefaultLightColor;

        float scale = Mathf.Max(0.2f, radius / 5f);

        ParticleSystem.ShapeModule shape = effect.shape;
        shape.scale = Vector3.one * scale;

        ParticleSystem.EmissionModule emission = effect.emission;
        emission.rateOverTime = 80f;

        foreach (ParticleSystem ps in effect.GetComponentsInChildren<ParticleSystem>(true))
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = Color.white;
            main.loop = false;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.4f, duration);
            main.startSpeedMultiplier *= scale;

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
                lightTemplate.gameObject.name = "RPG_ShockwaveLight";
                lightTemplate.color = lightColor;
                lightTemplate.range = Mathf.Max(lightTemplate.range, radius * 2f);
                lightTemplate.intensity *= 4f;
                lightTemplate.enabled = false;

                lights.light = lightTemplate;
                lights.useParticleColor = false;
            }
        }

        effect.Play(withChildren: true);

        if (GameDirector.instance != null)
        {
            GameDirector.instance.CameraShake.ShakeDistance(6f, radius * 0.4f, radius * 1.5f, position, 0.2f);
            GameDirector.instance.CameraImpact.ShakeDistance(14f, radius * 0.4f, radius * 1.5f, position, 0.2f);
        }

        Object.Destroy(effect.gameObject, duration + FadeOutBuffer);
    }
}
