using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour, IMergeSystem
{
    [Header("База рецептів")]
    [SerializeField] private GameObject universalItemPrefab;
    [SerializeField] private List<RecipeSO> allRecipes;
    
    private Dictionary<string, RecipeSO> _recipeDatabase =  new Dictionary<string, RecipeSO>();
    private GameObject _currentLevel;
    
    private void Awake()
    {
        InitializeRecipes();
    }

    public void OnEnable()
    {
        EventBus.OnLevelStarted += GetCurrentLevel;
    }
    
    public void OnDisable()
    {
        EventBus.OnLevelStarted -= GetCurrentLevel;
    }
    
    private void InitializeRecipes()
    {
        _recipeDatabase = new Dictionary<string, RecipeSO>();

        foreach (RecipeSO recipe in allRecipes)
        {
            RegisterRecipe(recipe);
        }
    }
    
    private void RegisterRecipe(RecipeSO recipe)
    {
        if (recipe == null || recipe.InputItem1 == null || recipe.InputItem2 == null) 
        {
            return;
        }
        
        string key = BuildRecipeKey(recipe.InputItem1.ID, recipe.InputItem2.ID);
        
        if (!_recipeDatabase.ContainsKey(key))
        {
            _recipeDatabase.Add(key, recipe);
        }
    }
    
    private string BuildRecipeKey(string id1, string id2)
    {
        if (string.Compare(id1, id2) < 0)
        {
            return $"{id1}_{id2}";
        }
        else
        {
            return $"{id2}_{id1}";
        }
    }
    
    public ItemSO TryGetMergeResult(string id1, string id2)
    {
        string key = BuildRecipeKey(id1, id2);
        
        if (_recipeDatabase.TryGetValue(key, out RecipeSO recipe))
        {
            return recipe.GetResultItem();
        }
        
        return null;
    }
    
    public void SpawnItem(ItemSO itemData, Vector2 position)
    {
        if (universalItemPrefab is null)
        {
            return;
        }
        
        GameObject newObj = Instantiate(universalItemPrefab, position, Quaternion.identity, _currentLevel.transform);
        Setup(itemData, newObj);
        EventBus.RaiseItemCrafted(EventBus.SetItemData(false, itemData, position, newObj.transform));
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