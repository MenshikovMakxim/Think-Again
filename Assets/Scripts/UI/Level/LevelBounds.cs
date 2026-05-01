// using UnityEngine;
//
// public class LevelBounds : MonoBehaviour
// {
//     [Header("Розмір безпечної зони")]
//     [SerializeField] private Vector2 playableArea = new Vector2(16f, 8f);
//     
//     [Header("Зміщення (щоб опустити під Top Bar)")]
//     [SerializeField] private Vector2 offset = new Vector2(0f, -1.5f);
//
//     [Header("Налаштування Фону")]
//     [Tooltip("Кинь сюди SpriteRenderer твого фону")]
//     [SerializeField] private SpriteRenderer backgroundSprite;
//     [Tooltip("На скільки фон має виступати за межі зони (1 = рівно по рамці, 1.1 = на 10% більший)")]
//     [SerializeField] private float backgroundScaleMultiplier = 1.0f;
//
//     [Header("Налаштування Рамки (Outline)")]
//     [Tooltip("Кинь сюди LineRenderer (створи на цьому ж об'єкті)")]
//     [SerializeField] private LineRenderer outlineRenderer;
//     [Tooltip("Товщина лінії рамки")]
//     [SerializeField] private float outlineWidth = 0.1f;
//
//     private void Start()
//     {
//         StretchBackground();
//         DrawOutline();
//     }
//
//     [ContextMenu("Stretch Background Now")] 
//     public void StretchBackground()
//     {
//         if (backgroundSprite == null) return;
//
//         backgroundSprite.transform.position = new Vector3(
//             transform.position.x + offset.x, 
//             transform.position.y + offset.y, 
//             backgroundSprite.transform.position.z
//         );
//
//         Vector2 spriteBaseSize = backgroundSprite.sprite.bounds.size;
//         float scaleX = (playableArea.x * backgroundScaleMultiplier) / spriteBaseSize.x;
//         float scaleY = (playableArea.y * backgroundScaleMultiplier) / spriteBaseSize.y;
//
//         backgroundSprite.transform.localScale = new Vector3(scaleX, scaleY, 1f);
//         
//         DrawOutline();
//     }
//     
//     private void DrawOutline()
//     {
//         if (outlineRenderer == null) return;
//         
//         outlineRenderer.positionCount = 4;
//         outlineRenderer.loop = true; 
//         outlineRenderer.startWidth = outlineWidth;
//         outlineRenderer.endWidth = outlineWidth;
//         outlineRenderer.useWorldSpace = true;
//
//         // Рахуємо центр і половинки сторін
//         Vector3 center = transform.position + new Vector3(offset.x, offset.y, 0);
//         float halfX = playableArea.x / 2f;
//         float halfY = playableArea.y / 2f;
//         
//         outlineRenderer.SetPosition(0, center + new Vector3(-halfX, -halfY, 0));
//         outlineRenderer.SetPosition(1, center + new Vector3(-halfX, halfY, 0));
//         outlineRenderer.SetPosition(2, center + new Vector3(halfX, halfY, 0));
//         outlineRenderer.SetPosition(3, center + new Vector3(halfX, -halfY, 0));
//     }
//
//     private void OnDrawGizmos()
//     {
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireCube((Vector2)transform.position + offset, playableArea);
//     }
// }

using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    [Header("Розмір безпечної зони")]
    [SerializeField] private Vector2 playableArea = new Vector2(16f, 8f);
    
    [Header("Зміщення (щоб опустити під Top Bar)")]
    [SerializeField] private Vector2 offset = new Vector2(0f, -1.5f);

    [Header("Налаштування Фону")]
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private float backgroundScaleMultiplier = 1.0f;

    [Header("Налаштування Рамки (Outline)")]
    [SerializeField] private LineRenderer outlineRenderer;
    [SerializeField] private float outlineWidth = 0.1f;
    
    [Header("Заокруглення кутів")]
    [Tooltip("Радіус кута (не може бути більшим за половину ширини/висоти)")]
    [SerializeField] private float cornerRadius = 1f;
    [Tooltip("Скільки точок витрачати на один кут (більше = плавніше, 10-15 ідеально)")]
    [SerializeField] private int cornerSegments = 12;

    [Header("Неонове пульсування (Світіння)")]
    [SerializeField] private bool enableGlow = true;
    
    [ColorUsage(true, true)] 
    [SerializeField] private Color glowColorA = Color.cyan;
    
    [ColorUsage(true, true)] 
    [SerializeField] private Color glowColorB = Color.blue;
    
    [SerializeField] private float glowSpeed = 2f;

    private void Start()
    {
        StretchBackground();
        DrawRoundedOutline();
    }

    private void Update()
    {
        // Неонова пульсація в реальному часі
        if (enableGlow && outlineRenderer != null)
        {
            // PingPong ганяє значення від 0 до 1 і назад, створюючи плавний цикл
            float t = Mathf.PingPong(Time.time * glowSpeed, 1f);
            Color currentColor = Color.Lerp(glowColorA, glowColorB, t);
            
            // Застосовуємо колір до лінії
            outlineRenderer.startColor = currentColor;
            outlineRenderer.endColor = currentColor;
        }
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
        }
        
        DrawRoundedOutline();
    }

    // МАГІЯ ТРИГОНОМЕТРІЇ: Малюємо заокруглений прямокутник
    private void DrawRoundedOutline()
    {
        if (outlineRenderer == null) return;

        // Захист від дурня: мінімум 2 точки на кут, інакше ділення на нуль
        int safeSegments = Mathf.Max(2, cornerSegments); 
        
        outlineRenderer.positionCount = safeSegments * 4; // 4 кути
        outlineRenderer.loop = true;
        outlineRenderer.startWidth = outlineWidth;
        outlineRenderer.endWidth = outlineWidth;
        outlineRenderer.useWorldSpace = true;

        // Запобіжник, щоб радіус не вивернув прямокутник навиворіт
        float maxRadius = Mathf.Min(playableArea.x / 2f, playableArea.y / 2f);
        float safeRadius = Mathf.Clamp(cornerRadius, 0f, maxRadius);

        Vector3 center = transform.position + new Vector3(offset.x, offset.y, 0);
        float halfX = playableArea.x / 2f;
        float halfY = playableArea.y / 2f;

        // Координати центрів для 4-х невидимих кіл у кутах прямокутника
        Vector3 topRightCenter = center + new Vector3(halfX - safeRadius, halfY - safeRadius, 0);
        Vector3 topLeftCenter = center + new Vector3(-halfX + safeRadius, halfY - safeRadius, 0);
        Vector3 bottomLeftCenter = center + new Vector3(-halfX + safeRadius, -halfY + safeRadius, 0);
        Vector3 bottomRightCenter = center + new Vector3(halfX - safeRadius, -halfY + safeRadius, 0);

        int pointIndex = 0;

        // Локальна функція для малювання однієї дуги
        void DrawArc(Vector3 arcCenter, float startAngle, float endAngle)
        {
            for (int i = 0; i < safeSegments; i++)
            {
                float t = i / (float)(safeSegments - 1);
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad; // Переводимо градуси в радіани

                // Знаходимо X та Y на колі за допомогою Синуса і Косинуса
                float x = Mathf.Cos(angle) * safeRadius;
                float y = Mathf.Sin(angle) * safeRadius;

                outlineRenderer.SetPosition(pointIndex, arcCenter + new Vector3(x, y, 0));
                pointIndex++;
            }
        }

        // Малюємо кути по черзі. Зверни увагу на градуси (проти годинникової стрілки):
        DrawArc(topRightCenter, 0f, 90f);       // 1. Правий верхній
        DrawArc(topLeftCenter, 90f, 180f);      // 2. Лівий верхній
        DrawArc(bottomLeftCenter, 180f, 270f);  // 3. Лівий нижній
        DrawArc(bottomRightCenter, 270f, 360f); // 4. Правий нижній
    }

    private void OnDrawGizmos()
    {
        // Малюємо зелений квадрат у редакторі (він залишиться гострим, бо це просто орієнтир)
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + offset, playableArea);
    }
}