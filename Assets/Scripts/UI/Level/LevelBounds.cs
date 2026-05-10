using UnityEngine;
using DG.Tweening;
public class LevelBounds : MonoBehaviour
{
    [Header("Розмір безпечної зони")]
    [SerializeField] private Vector2 playableArea = new Vector2(16f, 8f);
    
    [Header("Зміщення (щоб опустити під Top Bar)")]
    [SerializeField] private Vector2 offset = new Vector2(0f, -1.5f);

    [Header("Налаштування Фону")]
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private float backgroundScaleMultiplier = 1.0f;
    
    [Header("Анімація появи")]
    [SerializeField] private float fadeInDuration = 1f;

    [Header("Налаштування Рамки (Outline)")]
    [SerializeField] private LineRenderer outlineRenderer;
    [SerializeField] private float outlineWidth = 0.1f;
    
    [Header("Заокруглення кутів")]
    [Tooltip("Радіус кута (не може бути більшим за половину ширини/висоти)")]
    [SerializeField] private float cornerRadius = 1f;
    [Tooltip("Скільки точок витрачати на один кут (більше = плавніше, 10-15 ідеально)")]
    [SerializeField] private int cornerSegments = 12;
    
    [SerializeField] private Color color = Color.black;

    private void Start()
    {
        StretchBackground();
        DrawRoundedOutline();
        outlineRenderer.startColor = color;
        outlineRenderer.endColor = color;
        
        SpawnAndFadeLocalOverlay();
        
    }
    
    [ContextMenu("Stretch Background & Draw Outline")] 
    public void StretchBackground()
    {
        if (backgroundSprite != null)
        {
            backgroundSprite.transform.position = new Vector3(
                transform.position.x + offset.x, 
                transform.position.y + offset.y, 
                backgroundSprite.transform.position.z
            );

            Vector2 spriteBaseSize = backgroundSprite.sprite.bounds.size;
            float scaleX = (playableArea.x * backgroundScaleMultiplier) / spriteBaseSize.x;
            float scaleY = (playableArea.y * backgroundScaleMultiplier) / spriteBaseSize.y;

            backgroundSprite.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            outlineRenderer.startColor = color;
            outlineRenderer.endColor = color;
        }
        
        DrawRoundedOutline();
    }
    
    private void DrawRoundedOutline()
    {
        if (outlineRenderer == null) return;
        
        int safeSegments = Mathf.Max(2, cornerSegments); 
        
        outlineRenderer.positionCount = safeSegments * 4;
        outlineRenderer.loop = true;
        outlineRenderer.startWidth = outlineWidth;
        outlineRenderer.endWidth = outlineWidth;
        outlineRenderer.useWorldSpace = true;
        
        float maxRadius = Mathf.Min(playableArea.x / 2f, playableArea.y / 2f);
        float safeRadius = Mathf.Clamp(cornerRadius, 0f, maxRadius);

        Vector3 center = transform.position + new Vector3(offset.x, offset.y, 0);
        float halfX = playableArea.x / 2f;
        float halfY = playableArea.y / 2f;
        
        Vector3 topRightCenter = center + new Vector3(halfX - safeRadius, halfY - safeRadius, 0);
        Vector3 topLeftCenter = center + new Vector3(-halfX + safeRadius, halfY - safeRadius, 0);
        Vector3 bottomLeftCenter = center + new Vector3(-halfX + safeRadius, -halfY + safeRadius, 0);
        Vector3 bottomRightCenter = center + new Vector3(halfX - safeRadius, -halfY + safeRadius, 0);

        int pointIndex = 0;
        
        void DrawArc(Vector3 arcCenter, float startAngle, float endAngle)
        {
            for (int i = 0; i < safeSegments; i++)
            {
                float t = i / (float)(safeSegments - 1);
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;
                
                float x = Mathf.Cos(angle) * safeRadius;
                float y = Mathf.Sin(angle) * safeRadius;

                outlineRenderer.SetPosition(pointIndex, arcCenter + new Vector3(x, y, 0));
                pointIndex++;
            }
        }
        
        DrawArc(topRightCenter, 0f, 90f);       // 1. Правий верхній
        DrawArc(topLeftCenter, 90f, 180f);      // 2. Лівий верхній
        DrawArc(bottomLeftCenter, 180f, 270f);  // 3. Лівий нижній
        DrawArc(bottomRightCenter, 270f, 360f); // 4. Правий нижній
    }
    
    private void SpawnAndFadeLocalOverlay()
    {
        GameObject overlayObj = new GameObject("LevelFadeOverlay");
        overlayObj.transform.parent = transform;
        
        overlayObj.transform.position = new Vector3(
            transform.position.x + offset.x, 
            transform.position.y + offset.y, 
            transform.position.z - 1f 
        );

        SpriteRenderer sr = overlayObj.AddComponent<SpriteRenderer>();
    
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();
        
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        sr.sortingOrder = 32000; 
        
        float width = playableArea.x + (outlineWidth * 4);
        float height = playableArea.y + (outlineWidth * 4);
        overlayObj.transform.localScale = new Vector3(width, height, 1f);
        
        sr.DOFade(0f, fadeInDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => 
            {
                Destroy(sr.sprite.texture); 
                Destroy(overlayObj); 
            });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + offset, playableArea);
    }
}