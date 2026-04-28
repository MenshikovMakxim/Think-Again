using UnityEngine;
public interface IMergeable
{
    ItemSO GetItemData();
    
    int GetInstanceID(); 
    
    Transform Transform { get; }
    
    void DestroyItem(); 
}
