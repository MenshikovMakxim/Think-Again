using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeSO", menuName = "Game Data/Recipe SO")]
public class RecipeSO : ScriptableObject
{
    [Header("Інгредієнти")]
    public ItemSO InputItem1;
    public ItemSO InputItem2;

    [Header("Результат")]
    public ItemSO ResultItem;
    
    public bool CanCraft(ItemSO item1, ItemSO item2)
    {
        if (item1 == null || item2 == null) return false;
        
        bool isStraightMatch = (item1 == InputItem1 && item2 == InputItem2);
        bool isReverseMatch  = (item1 == InputItem2 && item2 == InputItem1);

        return isStraightMatch || isReverseMatch;
    }
    
    public ItemSO GetResultItem()
    {
        return ResultItem;
    }
    
    public void SetRecipe(ItemSO input1, ItemSO input2, ItemSO result)
    {
        InputItem1 = input1;
        InputItem2 = input2;
        ResultItem = result;
        
        Debug.Log($"Рецепт оновлено: {InputItem1?.name} + {InputItem2?.name} = {ResultItem?.name}");
    }
}
