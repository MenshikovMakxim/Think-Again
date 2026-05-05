using UnityEngine;
using DG.Tweening;

namespace Game.Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemFlasher : MonoBehaviour
    {
        [ColorUsage(true, true)]
        [Tooltip("Яким кольором підсвітити предмет при появі")]
        [SerializeField]
        private Color flashColor = Color.yellow;

        [Tooltip("Як довго триває згасання спалаху (в секундах)")] [SerializeField]
        private float duration = 0.5f;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        public void PlayFlash()
        {
            _spriteRenderer.DOKill();
            
            _spriteRenderer.color = flashColor;
            
            _spriteRenderer.DOColor(_originalColor, duration)
                .SetEase(Ease.OutQuad) 
                .SetLink(gameObject);
        }
    }
}