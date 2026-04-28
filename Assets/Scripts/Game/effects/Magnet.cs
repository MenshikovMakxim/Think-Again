using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Collider2D))] // Жорстко вимагаємо колайдер, щоб вимикати його в польоті
public class MagnetComponent : MonoBehaviour
{
    [Header("Налаштування магніту")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease easeType = Ease.InBack;
    
    [Tooltip("Чи має об'єкт зменшуватися до нуля під час польоту?")]
    [SerializeField] private bool shrink = true;

    /// <summary>
    /// Запускає політ до цілі.
    /// </summary>
    public void MagnetizeTo(Transform target, Action onComplete = null)
    {
        if (target == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Спроба намагнітити до порожнечі! Відміняю політ.");
            onComplete?.Invoke();
            return;
        }

        // 1. Вбиваємо всі попередні анімації на цьому об'єкті, щоб його не розірвало
        transform.DOKill();

        // 2. Вимикаємо фізику. Ми ж не хочемо, щоб він по дорозі вдарився об інше яблуко і запустив ланцюгову реакцію багів.
        if (TryGetComponent(out Collider2D col))
        {
            col.enabled = false;
        }

        // 3. Ривок до цілі
        transform.DOMove(target.position, duration)
            .SetEase(easeType)
            .SetLink(gameObject) // Захист від NullReference, якщо об'єкт раптом видалять раніше часу
            .OnComplete(() => onComplete?.Invoke());

        // 4. Ефект всмоктування
        if (shrink)
        {
            transform.DOScale(Vector3.zero, duration)
                .SetEase(easeType)
                .SetLink(gameObject);
        }
    }
}