using UnityEngine;
using Game.Interfaces;

public class LevelManager : MonoBehaviour
{
    [Header("Налаштування")]
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private GameObject levelHolder;
    [SerializeField] private GameObject craftingSystem;
    
    private int _currentLevelIndex;
    private LevelData _currentLevelData;
    private LevelController _levelController;
    
    public void OnEnable()
    {
        EventBus.OnLevelFinished += FinishLevelScreen;
    }
    
    public void OnDisable()
    {
        EventBus.OnLevelFinished -= FinishLevelScreen;
    }

    private void Awake()
    {
        _levelController = levelHolder.GetComponent<LevelController>();
        _levelController.SetMergeProvider(craftingSystem.GetComponent<IMergeSystem>());
    }
    
    public int CountLevels() => levelPrefabs.Length;
    
    public void LoadLevel(int index)
    {
        
        UIManager.Instance.OpenScreen(UIManager.Instance.gameHudPanel);
        
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
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        LoadLevel(_currentLevelIndex);
    }
    
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        
        if (_currentLevelIndex < CountLevels())
        {
            LoadLevel(_currentLevelIndex+1);
        }
        else
        {
            ExitToMenu();
            UIManager.Instance.OpenScreen(UIManager.Instance.levelSelectPanel);
        }
    }

    private void FinishLevelScreen(EventBus.ItemData itemData)
    {
        UIManager.Instance.ShowResultPopup();
    }
}
