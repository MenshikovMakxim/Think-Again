using UnityEngine;
using DG.Tweening;
using Game.Interfaces;

namespace Game.Effects
{
    public class ClickJuice : MonoBehaviour, IClickable
    {
        [Header("Налаштування")]
        [Tooltip("Наскільки предмет стискається (0.8 = до 80% від розміру)")]
        [SerializeField]
        private float squishScale = 0.8f;

        [Tooltip("Час анімації")] [SerializeField]
        private float animationDuration = 0.15f;

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
    }
}