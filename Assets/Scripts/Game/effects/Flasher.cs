using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFlasher : MonoBehaviour
{

    [ColorUsage(true, true)]
    [Tooltip("Яким кольором підсвітити предмет при появі? (Наприклад, яскраво-жовтий)")]
    [SerializeField] private Color flashColor = Color.yellow;
    
    [Tooltip("Як довго триває згасання спалаху (в секундах)")]
    [SerializeField] private float duration = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // Запам'ятовуємо рідний колір об'єкта (зазвичай це просто білий без фільтрів)
        _originalColor = _spriteRenderer.color; 
    }

    public void PlayFlash()
    {
        // 1. Вбиваємо попередню анімацію кольору, якщо вона раптом була
        _spriteRenderer.DOKill();

        // 2. Миттєво фарбуємо предмет у колір спалаху
        _spriteRenderer.color = flashColor;

        // 3. Плавно повертаємо до рідного кольору
        _spriteRenderer.DOColor(_originalColor, duration)
            .SetEase(Ease.OutQuad) // OutQuad робить старт різким, а кінець - плавним
            .SetLink(gameObject);
    }
}