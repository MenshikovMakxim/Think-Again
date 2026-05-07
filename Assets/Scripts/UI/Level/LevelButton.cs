using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;
    private int _levelNumber;
    private Action<int> _onClickAction;
    
    public void OnClick()
    {
        _onClickAction?.Invoke(_levelNumber);
    }
    
    public void Setup(int number, Action<int> onClickAction, bool isUnlocked)
    {
        _levelNumber = number;
        buttonText.text = number.ToString();
        _onClickAction = onClickAction;
        
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        
        if (button != null)
        {
            button.interactable = isUnlocked;
        }
        else
        {
            Debug.LogError($"На префабі LevelButton {number} немає компонента Button!", this);
        }
    }
    
    public void RefreshLockState(bool isUnlocked)
    {
        if (button != null)
        {
            button.interactable = isUnlocked;
        }
    }
    
    // public void Setup(int number, Action<int> onClickAction)
    // {
    //     _levelNumber = number;
    //     buttonText.text = number.ToString();
    //     _onClickAction = onClickAction;
    // }
}