using UnityEngine;
using Game.SO;

namespace Game.Interfaces
{
    public interface IMergeSystem
    {
        RecipeSO TryGetRecipe(IMergeable item1, IMergeable item2);

        ItemSO GetItemDataByType(ItemType type);

        GameObject SpawnItem(ItemType resultType, Vector2 position);

        void TryMerge(IMergeable item1, IMergeable item2);
    }
}