using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LevelFitter : MonoBehaviour
{
    [Header("Що центруємо?")]
    [Tooltip("Перетягни сюди фоновий спрайт твоєї дошки (рівня)")]
    [SerializeField] private SpriteRenderer boardBackground;

    [Header("Відступи (Повітря навколо)")]
    [Tooltip("1.0 = впритул до країв, 1.1 = 10% відступу, 1.2 = 20% і т.д.")]
    [SerializeField] private float paddingMultiplier = 1.15f;

    private void Start()
    {
        FitBoardToScreen();
    }

    // Якщо ти генеруєш рівень динамічно, можеш викликати цей метод після генерації
    public void FitBoardToScreen()
    {
        if (boardBackground == null)
        {
            Debug.LogWarning("[LevelFitter] Не призначено фон дошки!");
            return;
        }

        Camera cam = GetComponent<Camera>();

        // Отримуємо фізичні розміри нашої дошки у 2D світі
        float boardWorldWidth = boardBackground.bounds.size.x;
        float boardWorldHeight = boardBackground.bounds.size.y;

        // Рахуємо пропорції екрана поточного пристрою (співвідношення сторін)
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // 1. Який розмір камери потрібен, щоб влізла вся ВИСОТА дошки?
        float requiredSizeForHeight = boardWorldHeight / 2f;

        // 2. Який розмір камери потрібен, щоб влізла вся ШИРИНА дошки?
        float requiredSizeForWidth = (boardWorldWidth / 2f) / screenAspect;

        // 3. Беремо максимальне з цих двох значень, щоб дошка ТОЧНО помістилася,
        // і множимо на наш відступ (наприклад, +15% простору навколо)
        cam.orthographicSize = Mathf.Max(requiredSizeForHeight, requiredSizeForWidth) * paddingMultiplier;

        // 4. БОНУС: Жорстко центруємо камеру рівно по центру дошки
        // (тепер ти можеш совати дошку в редакторі куди завгодно, камера сама її знайде)
        Vector3 boardCenter = boardBackground.bounds.center;
        cam.transform.position = new Vector3(boardCenter.x, boardCenter.y, cam.transform.position.z);
    }
}
