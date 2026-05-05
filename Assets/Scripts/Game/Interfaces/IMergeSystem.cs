using UnityEngine;
using Game.SO;

namespace Game.Interfaces
{
    public interface IMergeSystem
    {
        RecipeSO TryGetRecipe(ItemType type1, ItemType type2);

        ItemSO GetItemDataByType(ItemType type);

        GameObject SpawnItem(ItemType resultType, Vector2 position);
    }
}