using UnityEngine;
public interface IMergeable
{
    ItemSO GetItemData();

    void SetItemData(ItemSO data);
    
    int GetInstanceID(); 
    
    Transform Transform { get; }
    
    void DestroyItem(); 
}
