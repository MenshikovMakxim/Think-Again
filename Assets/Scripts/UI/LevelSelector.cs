using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [Header("UI References")]
    public LevelButton buttonPrefab; 
    public Transform buttonsContainer; 

    [Header("Dependencies")]
    public LevelManager levelManager;
    
    public void GenerateLevelButtons()
    {
   
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }
        
        int totalLevels = levelManager.levelPrefabs.Length;

        for (int i = 1; i <= totalLevels; i++)
        {
            LevelButton newButton = Instantiate(buttonPrefab, buttonsContainer);
            newButton.Setup(i, levelManager.LoadLevel); 
        }
    }
    
    private void Start()
    {
        GenerateLevelButtons();
    }
}