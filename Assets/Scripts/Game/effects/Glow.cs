using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class PulseGlow : MonoBehaviour
{
    [Header("Магія Світіння")]
    // ОЦЕЙ АТРИБУТ ДОДАСТЬ ПОВЗУНОК ІНТЕНСИВНОСТІ!
    [ColorUsage(true, true)] 
    [SerializeField] private Color hdrGlowColor = Color.yellow;

    [Tooltip("Час одного вдиху/видиху (в секундах)")]
    [SerializeField] private float cycleDuration = 1.5f;

    [Tooltip("На скільки відсотків збільшується ореол (1.2 = на 20%)")]
    [SerializeField] private float scaleMultiplier = 1.2f;

    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // 1. Примусово вливаємо наш HDR-колір у спрайт
        sr.color = hdrGlowColor;

        // 2. Нескінченне "дихання" прозорістю (Альфа-каналом)
        sr.DOFade(0.4f, cycleDuration)
            .From(1f) 
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetLink(gameObject);

        // 3. Легке пульсування розміром
        transform.DOScale(transform.localScale * scaleMultiplier, cycleDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);
    }
}
