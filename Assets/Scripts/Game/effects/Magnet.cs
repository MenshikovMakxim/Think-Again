using UnityEngine;
using DG.Tweening;
using System;

namespace Game.Effects
{
    [RequireComponent(typeof(Collider2D))]
    public class MagnetComponent : MonoBehaviour
    {
        [Header("Налаштування магніту")] [SerializeField]
        private float duration = 0.3f;

        [SerializeField] private Ease easeType = Ease.InBack;

        [Tooltip("Чи має об'єкт зменшуватися до нуля під час польоту?")] [SerializeField]
        private bool shrink = true;

        public void MagnetizeTo(Transform target, Action onComplete = null)
        {
            if (target == null)
            {
                onComplete?.Invoke();
                return;
            }
            
            transform.DOKill();
            
            if (TryGetComponent(out Collider2D col))
            {
                col.enabled = false;
            }
            
            transform.DOMove(target.position, duration)
                .SetEase(easeType)
                .SetLink(gameObject)
                .OnComplete(() => onComplete?.Invoke());
            
            if (shrink)
            {
                transform.DOScale(Vector3.zero, duration)
                    .SetEase(easeType)
                    .SetLink(gameObject);
            }
        }
        
        public void Restore(Vector3 originalScale)
        {
            transform.DOKill();
            
            if (TryGetComponent(out Collider2D col))
            {
                col.enabled = true;
            }
            
            transform.DOScale(originalScale, duration) 
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }
    }
}