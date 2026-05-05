using TMPro;
using UnityEngine;

public class HudController :  MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelIndex;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI popupHintText;
    
    public void OnEnable()
    {
        EventBus.OnLevelStarted += Setup;
    }
    
    public void OnDisable()
    {
        EventBus.OnLevelStarted -= Setup;
    }
    
    private void Setup(GameObject level, int index)
    {
        LevelData levelData = level.GetComponent<LevelData>();
        
        levelIndex.text = "Level " + index;
        objectiveText.text = levelData.GetDescription();
        popupHintText.text = levelData.GetHintText();
        
        
    }
    
}
