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
    
    // public RecipeSO TryGetRecipe(ItemType type1, ItemType type2)
    // {
    //     if (database == null) 
    //     {
    //         Debug.LogError("[CraftingManager] Базу даних не підключено в Інспекторі!");
    //         return null;
    //     }
    //     
    //     return database.GetRecipe(type1, type2);
    // }
    
    public RecipeSO TryGetRecipe(IMergeable item1, IMergeable item2)
    {
        if (database == null) 
        {
            return null;
        }
        
        return database.GetRecipe(item1.GetItemType(), item2.GetItemType());
    }

    public void TryMerge(IMergeable item1, IMergeable item2)
    {
        RecipeSO recipe = TryGetRecipe(item1, item2);
            
        if (recipe != null)
        {
            item1.ActiveCollider(false);
            item2.ActiveCollider(false);

            if (item1.GetID() > item2.GetID())
            {
                item1.MergeTo(item2, recipe);
            }
        }
    }

    public ItemSO GetItemDataByType(ItemType type)
    {
        if (database == null) return null;
        
        return database.GetItemData(type);
    }
    
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