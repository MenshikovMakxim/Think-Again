using System.Collections;
using UnityEngine;

// Вимагаємо наявності CanvasGroup, щоб панель можна було ще й прозорою робити
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class UIPanelAnimator : MonoBehaviour
{
    public enum SlideDirection { FromBottom, FromTop }

    [Header("Налаштування Анімації")]
    public SlideDirection direction = SlideDirection.FromBottom;
    public float animationDuration = 0.4f;
    
    [Tooltip("Намалюй тут гірку, щоб панель 'пружинила' в кінці")]
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    
    private Vector2 _shownPosition;  // Позиція на екрані
    private Vector2 _hiddenPosition; // Позиція за екраном

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        // 1. Запам'ятовуємо її ідеальну позицію (там, де ти її поставив у Редакторі)
        _shownPosition = _rectTransform.anchoredPosition;

        // 2. Рахуємо позицію за межами екрана (Screen.height гарантовано сховає її)
        float offset = Screen.height * 1.5f; 
        
        if (direction == SlideDirection.FromBottom)
            _hiddenPosition = _shownPosition + new Vector2(0, -offset);
        else
            _hiddenPosition = _shownPosition + new Vector2(0, offset);

        // 3. При старті гри миттєво ховаємо панель
        _rectTransform.anchoredPosition = _hiddenPosition;
        _canvasGroup.alpha = 0f;      // Робимо невидимою
        _canvasGroup.blocksRaycasts = false; // Вимикаємо кліки, поки вона схована
    }

    // ВИКЛИКАЙ ЦЕ, ЩОБ ПОКАЗАТИ ВІКНО
    public void ShowWindow()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(SlideRoutine(_hiddenPosition, _shownPosition, 0f, 1f));
        _canvasGroup.blocksRaycasts = true; // Вмикаємо кліки
    }

    // ВИКЛИКАЙ ЦЕ, ЩОБ СХОВАТИ ВІКНО
    public void HideWindow()
    {
        _canvasGroup.blocksRaycasts = false; // Одразу вимикаємо кліки
        StopAllCoroutines();
        StartCoroutine(SlideRoutine(_shownPosition, _hiddenPosition, 1f, 0f));
    }

    // МАГІЯ АНІМАЦІЇ (Корутина)
    private IEnumerator SlideRoutine(Vector2 startPos, Vector2 endPos, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / animationDuration;
            
            // Застосовуємо криву для "соковитості" руху
            float curvePercent = animationCurve.Evaluate(percent);

            // Рухаємо
            _rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, curvePercent);
            
            // Змінюємо прозорість (плавно з'являється)
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, percent);

            yield return null; // Чекаємо наступного кадру
        }

        // Гарантуємо, що в кінці панель стане точно на своє місце
        _rectTransform.anchoredPosition = endPos;
        _canvasGroup.alpha = endAlpha;

        // Якщо сховали повністю — вимикаємо об'єкт для економії пам'яті
        if (endAlpha <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
