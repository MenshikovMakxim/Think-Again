using System;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    public TextMeshProUGUI buttonText;
    private Action<int> _onClickAction;
    
    public void OnClick()
    {
        _onClickAction?.Invoke(levelNumber);
    }
    
    public void Setup(int number, Action<int> onClickAction)
    {
        levelNumber = number;
        buttonText.text = number.ToString();
        _onClickAction = onClickAction;
    }
}