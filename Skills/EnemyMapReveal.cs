using System.Collections;
using Photon.Pun;
using UnityEngine;

internal class EnemyMapReveal : MonoBehaviour
{
    private const string MarkerName = "RPG_EnemyReveal";

    private static readonly Color MapMarkerColor = new Color(1f, 0f, 0f);

    private static Sprite markerSprite;

    private Coroutine activeCoroutine;

    [PunRPC]
    public void RevealRPC(float duration)
    {
        Enemy enemy = GetComponentInChildren<Enemy>();

        if (enemy == null || enemy.CenterTransform == null)
            return;

        Transform existingMarker = enemy.CenterTransform.Find(MarkerName);
        if (existingMarker != null)
            Destroy(existingMarker.gameObject);

        GameObject marker = new GameObject(MarkerName);
        marker.transform.SetParent(enemy.CenterTransform, false);
        marker.transform.localPosition = Vector3.zero;

        if (Map.Instance != null)
        {
            MapCustom mapCustom = marker.AddComponent<MapCustom>();
            mapCustom.sprite = GetMarkerSprite();
            mapCustom.color = MapMarkerColor;
        }

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(DestroyAfter(marker, duration));
    }

    private IEnumerator DestroyAfter(GameObject marker, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (marker != null)
            Destroy(marker);

        activeCoroutine = null;
    }

    private static Sprite GetMarkerSprite()
    {
        if (markerSprite != null)
            return markerSprite;

        const int size = 40;
        const float pixelsPerUnit = size * 5f;

        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float distance = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                bool inside = distance <= radius;
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, inside ? 1f : 0f));
            }
        }

        texture.Apply();

        markerSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), pixelsPerUnit);

        return markerSprite;
    }
}
