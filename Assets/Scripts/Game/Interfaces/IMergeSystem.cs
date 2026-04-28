using UnityEngine;


public interface IMergeSystem
{
    ItemSO TryGetMergeResult(string id1, string id2);
    
    void SpawnItem(ItemSO data, Vector2 position);
}
