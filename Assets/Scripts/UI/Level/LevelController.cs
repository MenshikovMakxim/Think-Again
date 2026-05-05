using UnityEngine;
using Game.Interfaces;
using Game.Interactive;

public class LevelController :  MonoBehaviour
{
    private GameObject _currentLevel;
    private LevelData _currentLevelData;
    private IMergeSystem _mergeSystem;
    
    public void OnEnable()
    {
        EventBus.OnItemCrafted += CheckWinCondition;
    }
    
    public void OnDisable()
    {
        EventBus.OnItemCrafted -= CheckWinCondition;
    }
    
    public void SetMergeProvider(IMergeSystem mergeSystem)
    {
        _mergeSystem = mergeSystem;
    }

    private void SetMergeSystem()
    {
        MergeableItem[] mergeableItems = _currentLevel.GetComponentsInChildren<MergeableItem>(); 

        foreach (var item in mergeableItems)
        {
            item.Construct(_mergeSystem);
        }
    }
    
    public void SpawnLevel(GameObject levelPrefab, int index)
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        
        _currentLevel = Instantiate(levelPrefab, gameObject.transform);
        SetMergeSystem();
        
        EventBus.RaiseLevelStarted(_currentLevel, index);
    }
    
    private void CheckWinCondition(EventBus.ItemData item)
    {
        _currentLevelData = _currentLevel.GetComponent<LevelData>();
        
        if(_currentLevelData.IsWinObject(item.Item))
        {
            EventBus.RaiseLevelFinished(EventBus.SetItemData(true, item.Item, item.TargetPosition, item.TargetTransform));
        }
    }
    
    public void DestroyCurrentLevel()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
            _currentLevel = null;
        }
    }
}
