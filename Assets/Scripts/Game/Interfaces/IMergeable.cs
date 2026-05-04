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
    }
}