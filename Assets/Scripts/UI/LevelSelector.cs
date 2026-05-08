using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private LevelButton buttonPrefab; 
    [SerializeField] private Transform buttonsContainer; 

    [Header("Dependencies")]
    [SerializeField] private LevelManager levelManager;
    
    private readonly List<LevelButton> _spawnedButtons = new List<LevelButton>();
    
    private void Awake()
    {
        GenerateLevelButtons();
    } 
    private void OnEnable()
    {
        RefreshButtonsState();
    }
    private void GenerateLevelButtons()
    {
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }
        
        int totalLevels = levelManager.CountLevels();
        
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 1; i <= totalLevels; i++)
        {
            LevelButton newButton = Instantiate(buttonPrefab, buttonsContainer);
            
            bool isUnlocked = i <= unlockedLevel;
            
            newButton.Setup(i, levelManager.LoadLevel, isUnlocked); 
            
            _spawnedButtons.Add(newButton);
        }
    }
    
    private void RefreshButtonsState()
    {
        if (_spawnedButtons.Count == 0) return;
        
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
                    
        for (int i = 0; i < _spawnedButtons.Count; i++)
        {
            int levelIndex = i + 1;
            bool isUnlocked = levelIndex <= unlockedLevel;
            
            _spawnedButtons[i].RefreshLockState(isUnlocked);
        }
    }
}