using UnityEngine;

public class WinObject : MonoBehaviour, IClickable
{
    private LevelManager _manager;
    public void OnClick()
    {
        _manager = FindAnyObjectByType<LevelManager>();
        
        if (_manager is not null)
        {
            _manager.ResultLevel();
        }
        
        if (transform.parent != null)
        {
            EffectManager.Instance.ApplyWinGlow(gameObject);
        }
    }
}
