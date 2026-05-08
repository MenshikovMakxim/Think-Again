using UnityEngine;
using DG.Tweening;
using System;


namespace Game.Effects
{
    public class MagnetComponent : MonoBehaviour
    {
        [Header("Налаштування магніту")] 
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease easeType = Ease.InBack;

        [Tooltip("Чи має об'єкт зменшуватися до нуля під час польоту?")] 
        [SerializeField] private bool shrink = true;

        public void MagnetizeTo(Vector3 targetPosition, Action onComplete = null)
        {
            transform.DOKill();
        
            transform.DOMove(targetPosition, duration)
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

        public void MagnetizeTo(Transform target, Action onComplete = null)
        {
            if (target == null)
            {
                onComplete?.Invoke();
                return;
            }
            MagnetizeTo(target.position, onComplete);
        }
    
        public void Restore(Vector3 originalScale)
        {
            transform.DOKill();
        
            transform.DOScale(originalScale, duration) 
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }
    }
}
