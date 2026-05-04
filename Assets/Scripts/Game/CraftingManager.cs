using UnityEngine;
using Game.Interfaces;
using Game.Interactive;
using Game.SO;
using Game.Systems;

public class CraftingManager : MonoBehaviour, IMergeSystem
{
    [Header("Префаб для створення предметів")]
    [SerializeField] private GameObject universalItemPrefab;
    
    [Header("Посилання на Базу")]
    [SerializeField] private GameDatabase database;
    
    private GameObject _currentLevel;
    
    public void OnEnable()
    {
        EventBus.OnLevelStarted += GetCurrentLevel;
    }
    
    public void OnDisable()
    {
        EventBus.OnLevelStarted -= GetCurrentLevel;
    }
    
    // ==========================================
    // API ДЛЯ ПРЕДМЕТІВ (Просто делегуємо в Базу)
    // ==========================================

    public RecipeSO TryGetRecipe(ItemType type1, ItemType type2)
    {
        if (database == null) 
        {
            Debug.LogError("[CraftingManager] Базу даних не підключено в Інспекторі!");
            return null;
        }
        
        return database.GetRecipe(type1, type2);
    }

    public ItemSO GetItemDataByType(ItemType type)
    {
        if (database == null) return null;
        
        return database.GetItemData(type);
    }
    
    // ==========================================
    // ЛОГІКА СПАВНУ ТА КРАФТУ
    // ==========================================
    
    public GameObject SpawnItem(ItemType resultType, Vector2 position)
    {
        ItemSO itemData = GetItemDataByType(resultType);
        
        if (universalItemPrefab == null || itemData == null)
        {
            Debug.LogError($"[CraftingManager] Немає префабу або в базі відсутній ItemSO для типу {resultType}!");
            return null;
        }
        
        GameObject newObj = Instantiate(universalItemPrefab, position, Quaternion.identity, _currentLevel.transform);
        Setup(itemData, newObj);
        
        EventBus.RaiseItemCrafted(EventBus.SetItemData(false, itemData, position, newObj.transform));
        
        return newObj;
    }

    private void Setup(ItemSO data, GameObject item)
    {
        if (item.TryGetComponent(out MergeableItem mergeableItem))
        {
            mergeableItem.Construct(this); 
            mergeableItem.SetItemData(data);
        }
    }

    private void GetCurrentLevel(GameObject currentLevel, int index)
    {
        _currentLevel = currentLevel;
    }
}