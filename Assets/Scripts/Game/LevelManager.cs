using UnityEngine;
using System;
using Game.Interfaces;
using Game.Systems;

public class LevelManager : MonoBehaviour
{
    [Header("Налаштування")]
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private GameObject levelHolder;
    [SerializeField] private GameObject craftingSystem;
    
    private int _currentLevelIndex;
    private LevelController _levelController;
    
    private void Awake()
    {
        _levelController = levelHolder.GetComponent<LevelController>();
        _levelController.SetMergeProvider(craftingSystem.GetComponent<IMergeSystem>());
    }
    
    public int CountLevels() => levelPrefabs.Length;
    
    public void LoadLevel(int index)
    {
        UIManager.Instance.ActiveHub(true);
        
        _currentLevelIndex = index;
        GameObject prefabToSpawn = levelPrefabs[_currentLevelIndex - 1];
        _levelController.SpawnLevel(prefabToSpawn, _currentLevelIndex);
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        _levelController.DestroyCurrentLevel();
        UIManager.Instance.ActiveHub(false);
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        LoadLevel(_currentLevelIndex);
    }
    
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        UIManager.Instance.ActiveHub(true);
        
        if (_currentLevelIndex < CountLevels())
        {
            LoadLevel(_currentLevelIndex+1);
        }
    }
    
    public bool IsLastLevel()
    {
        return _currentLevelIndex == CountLevels();
    }
    
    public void RaiseCompletedLevel()
    {
        EventBus.RaiseLevelCompleted(_currentLevelIndex);
    }
    
}
