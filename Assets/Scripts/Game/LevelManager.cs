using System;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Налаштування")]
    public GameObject[] levelPrefabs;
    public Transform levelHolder;
    public TextMeshProUGUI hudLevelText;
    [Header("UI Елементи")]
    public GameObject nextLevelButton;
    [Header("UI Тексти")]
    public TextMeshProUGUI objectiveText;
    private string _currentHint;
    [Header("UI Панелі")]
    public TextMeshProUGUI popupHintText;
    
    private GameObject _currentLevelInstance;
    private int _currentLevelIndex;
    private LevelData _currentLevelData;
    private SimpleGlow2D _winGlowEffect;

    private void Awake()
    {
        _winGlowEffect = GetComponent<SimpleGlow2D>();
    }

    public Transform GetCurrentLevel()
    {
        return _currentLevelInstance != null ? _currentLevelInstance.transform : null;
    }
    
    public LevelData GetCurrentLevelData()
    {
        return _currentLevelData != null ? _currentLevelData : null;
    }
    public void LoadLevel(int levelIndex)
    {

        _currentLevelIndex = levelIndex;
        if (_currentLevelInstance != null)
        {
            Destroy(_currentLevelInstance);
        }
        
        GameObject prefabToSpawn = levelPrefabs[levelIndex - 1];
        _currentLevelInstance = Instantiate(prefabToSpawn, levelHolder);
        _currentLevelData = _currentLevelInstance.GetComponent<LevelData>();
        
    
        if (_currentLevelData != null)
        {
            if (objectiveText != null)
                objectiveText.text = _currentLevelData.objectiveDescription;
            
            _currentHint = _currentLevelData.hintText;
        }
        
        hudLevelText.text = "Level " + levelIndex;
        
        UIManager.Instance.OpenScreen(UIManager.Instance.gameHudPanel);
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
        
        if (_currentLevelInstance != null)
        {
            Destroy(_currentLevelInstance);
        }
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        
        LoadLevel(_currentLevelIndex); 
        
        UIManager.Instance.OpenRootScreen(UIManager.Instance.gameHudPanel);
    }

    public SimpleGlow2D GetWinEffect()
    {
        return _winGlowEffect ? _winGlowEffect : null;
    }
    
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        
        if (_currentLevelIndex < levelPrefabs.Length)
        {
            LoadLevel(_currentLevelIndex + 1);
        }
        else
        {
            ExitToMenu();
            UIManager.Instance.OpenScreen(UIManager.Instance.levelSelectPanel);
        }
    }
    
    public void ResultLevel()
    {
        // Time.timeScale = 0f;
        
        if (nextLevelButton is not null)
        {
            nextLevelButton.SetActive(_currentLevelIndex < levelPrefabs.Length);
        }
        
        UIManager.Instance.SlowShowPopup(UIManager.Instance.resultPanel);
    }
    
    public void ShowHint()
    {
        if (popupHintText != null)
        {
            popupHintText.text = _currentHint;
        }
        
        UIManager.Instance.ShowPopup(UIManager.Instance.hintPanel);
    }
    
}
