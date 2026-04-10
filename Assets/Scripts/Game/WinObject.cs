using UnityEngine;

public class WinObject : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        Debug.Log("Ціль знайдена! Викликаємо екран перемоги.");
        
        LevelManager manager = FindAnyObjectByType<LevelManager>();
        
        if (manager is not null)
        {
            manager.ResultLevel();
        }
    }
}
