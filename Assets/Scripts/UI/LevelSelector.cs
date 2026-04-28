using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private LevelButton buttonPrefab; 
    [SerializeField] private Transform buttonsContainer; 

    [Header("Dependencies")]
    [SerializeField] private LevelManager levelManager;
    
    private void GenerateLevelButtons()
    {
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }
        
        int totalLevels = levelManager.CountLevels();

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