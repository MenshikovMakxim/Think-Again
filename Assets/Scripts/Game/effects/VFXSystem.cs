using System.Collections.Generic;
using UnityEngine;
using Game.Effects;
using UnityEngine.Serialization;

public class VFXSystem : MonoBehaviour
{
    [System.Serializable]
    public struct EffectEntry
    {
        public EffectType type;
        public GameObject prefab;
    }

    [Header("Налаштування ефектів")]
    [SerializeField] private EffectEntry[] effectEntries;
    
    [Header("Кошик")]
    [SerializeField] private Transform vfxContainer;

    private readonly Dictionary<EffectType, GameObject> _effectDatabase = new Dictionary<EffectType, GameObject>();

    private void Awake()
    {
        foreach (var entry in effectEntries)
        {
            if (!_effectDatabase.ContainsKey(entry.type) && entry.prefab != null)
            {
                _effectDatabase.Add(entry.type, entry.prefab);
            }
        }
    }

    private void OnEnable()
    {
        // Підписуємося на івенти які передають дані
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
        PlayEffect(EffectType.CraftSmoke, data.TargetPosition);
        
        if (data.TargetTransform != null && data.TargetTransform.TryGetComponent(out ItemFlasher flasher))
        {
            flasher.PlayFlash();
        }
    }

    private void HandleLevelFinished(EventBus.ItemData data)
    {
        if (data.IsWin)
        {
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
        
        Transform actualParent = parent != null ? parent : vfxContainer;
        
        GameObject effectInstance = Instantiate(prefab, position, Quaternion.identity, actualParent);

        // Якщо це Fire & Forget ефект без прив'язки, даємо йому команду самознищитись (або повернутися в пул)
        // if (parent == null)
        // {
        //     Destroy(effectInstance, 2f); // Заглушка. Краще налаштувати автознищення в самій ParticleSystem (Stop Action -> Destroy)
        // }

        return effectInstance;
    }
}