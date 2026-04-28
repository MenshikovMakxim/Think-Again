using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Завдання для гравця")]

    [TextArea(2, 4)] 
    [SerializeField] private string objectiveDescription = "Task";

    [Header("Підказка (для кнопки '?')")]
    [TextArea(3, 5)] 
    [SerializeField] private string hintText = "Hint";
    
    [Header("Win Object")]
    [SerializeField] private ItemSO winObject;
    
    public bool IsWinObject(ItemSO item)
    {
        return item != null && winObject != null && item.ID == winObject.ID;
    }
    
    public string GetHintText() => hintText;
    public string GetDescription() => objectiveDescription;
}
