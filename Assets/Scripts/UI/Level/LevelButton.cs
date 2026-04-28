using System;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private int _levelNumber;
    private Action<int> _onClickAction;
    
    public void OnClick()
    {
        _onClickAction?.Invoke(_levelNumber);
    }
    
    public void Setup(int number, Action<int> onClickAction)
    {
        _levelNumber = number;
        buttonText.text = number.ToString();
        _onClickAction = onClickAction;
    }
}