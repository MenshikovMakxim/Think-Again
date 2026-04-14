using UnityEngine;

public class WinObject : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        Debug.Log("Ціль знайдена!");
        
        LevelManager manager = FindAnyObjectByType<LevelManager>();
        
        if (manager is not null)
        {
            manager.ResultLevel();
        }
    }
}
