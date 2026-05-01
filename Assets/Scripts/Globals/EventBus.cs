using System;
using UnityEngine;

public static class EventBus
{
    public struct ItemData
    {
        public bool IsWin;
        public ItemSO Item;
        public Vector3 TargetPosition; 
        public Transform TargetTransform;
    }
    
    public static ItemData SetItemData(bool isWin, ItemSO item, Vector3 targetPosition, Transform targetTransform)
    {
        ItemData itemData = new ItemData();
        
        itemData.IsWin = isWin;
        itemData.Item = item;
        itemData.TargetPosition = targetPosition;
        itemData.TargetTransform = targetTransform;
        
        return itemData;
    }
    
    public static event Action<ItemData> OnLevelFinished;
    
    public static void RaiseLevelFinished(ItemData winObject)
    {
        OnLevelFinished?.Invoke(winObject);
    }
    
    public static event Action<GameObject, int> OnLevelStarted;

    public static void RaiseLevelStarted(GameObject level, int index)
    {
        OnLevelStarted?.Invoke(level, index);
    }
    
    public static event Action<ItemData> OnItemCrafted;

    public static void RaiseItemCrafted(ItemData item)
    {
        OnItemCrafted?.Invoke(item);
    }
    
    public static event Action OnObjectClicked;

    public static void RaiseObjectClicked()
    {
        OnObjectClicked?.Invoke();
    } 
    
    public static event Action OnUIButtonClicked;

    public static void RaiseUIButtonClicked()
    {
        OnUIButtonClicked?.Invoke();
    }
}