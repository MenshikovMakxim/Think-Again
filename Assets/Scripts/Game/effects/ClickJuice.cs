using UnityEngine;
using DG.Tweening;
// using Game.Interfaces; // Розкоментуй, якщо твій IClickable лежить у неймспейсі

public class ClickJuice : MonoBehaviour, IClickable
{
    [Header("Налаштування 'Жмяку'")]
    [Tooltip("Наскільки предмет стискається (0.8 = до 80% від розміру)")]
    [SerializeField] private float squishScale = 0.8f;
    [Tooltip("Час стискання (туди-сюди)")]
    [SerializeField] private float animationDuration = 0.15f;
    
    // [Header("Візуал (Опціонально)")]
    // [Tooltip("Тип ефекту в нашій VFXSystem")]
    // [SerializeField] private EffectType clickEffectType = EffectType.ItemCreated; // Або створи новий тип ParticlePuff

    private Vector3 _originalScale;
    private bool _isInitialized = false;

    private void Start()
    {
        // Запам'ятовуємо розмір тільки один раз. 
        // Робимо це в Start, щоб інші скрипти встигли налаштувати масштаб предмета, якщо треба.
        _originalScale = transform.localScale;
        _isInitialized = true;
    }

    public void OnClick()
    {
        if (!_isInitialized) return;

        PlaySquishAnimation();
        PlayParticleEffect();
    }

    private void PlaySquishAnimation()
    {
        // 1. Жорстко вбиваємо попередню анімацію, якщо гравець клікає як скажений
        transform.DOKill();

        // 2. Відновлюємо оригінальний розмір перед новим кліком (захист від ефекту "чорної діри")
        transform.localScale = _originalScale;

        // 3. Робимо "Жмяк"
        // Ми зменшуємо об'єкт до squishScale, а потім повертаємо назад (Yoyo)
        transform.DOScale(_originalScale * squishScale, animationDuration / 2f)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo)
            .SetLink(gameObject); // Захист від NullReference, якщо предмет видалять під час кліку
    }

    private void PlayParticleEffect()
    {
        // Якщо ти вже налаштував VFXSystem, можна просто викликати ефект пилку/зірочок!
        // Наприклад, кинути івент:
        // GameEvents.RaiseEffectRequested(clickEffectType, transform.position);
    }
}