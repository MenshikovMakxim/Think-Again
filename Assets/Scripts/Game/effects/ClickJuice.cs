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

    private Vector3 _originalScale;
    private bool _isInitialized = false;

    private void Start()
    {

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
        transform.DOKill();
        
        transform.localScale = _originalScale;
        
        transform.DOScale(_originalScale * squishScale, animationDuration / 2f)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo)
            .SetLink(gameObject);
    }

    private void PlayParticleEffect()
    {
        // Якщо ти вже налаштував VFXSystem, можна просто викликати ефект пилку/зірочок!
        // Наприклад, кинути івент:
        // GameEvents.RaiseEffectRequested(clickEffectType, transform.position);
    }
}