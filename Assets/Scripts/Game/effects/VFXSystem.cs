using System.Collections.Generic;
using UnityEngine;

public class VFXSystem : MonoBehaviour
{
    [System.Serializable]
    public struct EffectEntry
    {
        public EffectType Type;
        public GameObject Prefab; // Сюди в Інспекторі кидаємо префаб з ParticleSystem або анімацією
    }

    [Header("Налаштування ефектів")]
    [SerializeField] private EffectEntry[] effectEntries;
    
    [Header("Кошик")]
    [SerializeField] private Transform vfxContainer;

    // Словник для швидкого пошуку префаба за його типом
    private Dictionary<EffectType, GameObject> _effectDatabase = new Dictionary<EffectType, GameObject>();

    private void Awake()
    {
        // Заповнюємо базу
        foreach (var entry in effectEntries)
        {
            if (!_effectDatabase.ContainsKey(entry.Type) && entry.Prefab != null)
            {
                _effectDatabase.Add(entry.Type, entry.Prefab);
            }
        }
    }

    private void OnEnable()
    {
        // Підписуємося на НАШІ оновлені івенти (які передають дані, а не GameObject)
        EventBus.OnItemCrafted += HandleItemCrafted;
        EventBus.OnLevelFinished += HandleLevelFinished;
    }

    private void OnDisable()
    {
        EventBus.OnItemCrafted -= HandleItemCrafted;
        EventBus.OnLevelFinished -= HandleLevelFinished;
    }

    // --- Обробники івентів ---

    private void HandleItemCrafted(EventBus.ItemData data)
    {
        // Кажемо системі: "Кинь димок ось тут"
        PlayEffect(EffectType.CraftSmoke, data.TargetPosition);
    }

    private void HandleLevelFinished(EventBus.ItemData data)
    {
        if (data.IsWin)
        {
            // Для світіння передаємо parent Transform, щоб світіння прилипло до об'єкта
            PlayEffect(EffectType.WinGlow, data.TargetPosition, data.TargetTransform);
        }
    }

    // --- ГОЛОВНИЙ МЕТОД СИСТЕМИ ---

    public GameObject PlayEffect(EffectType type, Vector3 position, Transform parent = null)
    {
        if (!_effectDatabase.TryGetValue(type, out GameObject prefab))
        {
            Debug.LogWarning($"[VFXSystem] Ефект {type} не знайдено в базі!");
            return null;
        }

        // В ІДЕАЛІ ТУТ МАЄ БУТИ ПУЛ: GameObject effect = pool.Get();
        // Але для спрощення поки що залишимо Instantiate.
        Transform actualParent = parent != null ? parent : vfxContainer;
        
        GameObject effectInstance = Instantiate(prefab, position, Quaternion.identity, actualParent);

        // Якщо це Fire & Forget ефект без прив'язки, даємо йому команду самознищитись (або повернутися в пул)
        if (parent == null)
        {
            Destroy(effectInstance, 2f); // Заглушка. Краще налаштувати автознищення в самій ParticleSystem (Stop Action -> Destroy)
        }

        return effectInstance;
    }
}