using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }
    public LevelManager level;

    [Header("База рецептів")]
    [SerializeField] private GameObject universalItemPrefab;
    [SerializeField] private List<RecipeSO> allRecipes = new List<RecipeSO>();
    
    private Dictionary<string, RecipeSO> _recipeDatabase;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeRecipes();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void InitializeRecipes()
    {
        _recipeDatabase = new Dictionary<string, RecipeSO>();

        foreach (RecipeSO recipe in allRecipes)
        {
            RegisterRecipe(recipe);
        }
        
        Debug.Log($"CraftingManager готовий! Завантажено рецептів: {_recipeDatabase.Count}");
    }
    
    public void RegisterRecipe(RecipeSO recipe)
    {
        if (recipe == null || recipe.InputItem1 == null || recipe.InputItem2 == null) 
        {
            Debug.LogWarning("Знайдено поламаний рецепт (не вистачає інгредієнтів)!");
            return;
        }
        
        string key = BuildRecipeKey(recipe.InputItem1.ID, recipe.InputItem2.ID);
        
        if (!_recipeDatabase.ContainsKey(key))
        {
            _recipeDatabase.Add(key, recipe);
        }
        else
        {
            Debug.LogError($"Конфлікт рецептів! Комбінація {key} вже існує.");
        }
    }
    
    public string BuildRecipeKey(string id1, string id2)
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
        if (universalItemPrefab == null)
        {
            return;
        }
        
        GameObject newObj = Instantiate(universalItemPrefab, position, Quaternion.identity, level.GetCurrentLevel());
        
        MergeableItem mergeable = newObj.GetComponent<MergeableItem>();
        if (mergeable != null)
        {
            mergeable.SetItemData(itemData);
        }
    }
}