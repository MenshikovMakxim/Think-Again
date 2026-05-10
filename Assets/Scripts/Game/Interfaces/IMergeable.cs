using UnityEngine;
using Game.SO;

namespace Game.Interfaces
{
    public interface IMergeable
    {
        ItemSO GetItemData();

        void SetItemData(ItemSO data);

        ItemType GetItemType();

        Transform Transform { get; }
    
        void ActiveCollider(bool flag);
        
        void DestroyItem();
        
        int GetID();
        
        void MergeTo(IMergeable target,  RecipeSO recipe);
        
        Vector3 GetStartPosition();

        float GetDistanceToHome();

        void RestoreAfterCraft();
    }
}