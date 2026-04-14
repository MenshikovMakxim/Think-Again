using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Завдання для гравця")]

    [TextArea(2, 4)] 
    public string objectiveDescription = "Task";

    [Header("Підказка (для кнопки '?')")]
    [TextArea(3, 5)] 
    public string hintText = "Hint";
}
