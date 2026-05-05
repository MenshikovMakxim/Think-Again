using UnityEngine;
using DG.Tweening;

namespace Game.Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PulseGlow : MonoBehaviour
    {
        [Header("Магія Світіння")]
        [ColorUsage(true, true)]
        [SerializeField]
        private Color hdrGlowColor = Color.yellow;

        [Tooltip("Час одного вдиху/видиху (в секундах)")] [SerializeField]
        private float cycleDuration = 1.5f;

        [Tooltip("На скільки відсотків збільшується ореол (1.2 = на 20%)")] [SerializeField]
        private float scaleMultiplier = 1.2f;

        private void Start()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = hdrGlowColor;
            
            sr.DOFade(0.4f, cycleDuration)
                .From(1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
            
            transform.DOScale(transform.localScale * scaleMultiplier, cycleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}