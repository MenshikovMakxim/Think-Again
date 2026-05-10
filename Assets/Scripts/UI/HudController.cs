using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HudController :  MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelIndex;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI popupHintText;
    
    [Header("Налаштування анімації")]
    public float animDuration = 0.3f;
    
    private RectTransform _rectTransform;
    private Vector2 _originalPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPos = _rectTransform.anchoredPosition; 
    }
    
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
    
    public void Show()
    {
        _rectTransform.anchoredPosition = new Vector2(_originalPos.x, Screen.height + 200f);
        
        _rectTransform.DOKill();
        
        _rectTransform.DOAnchorPos(_originalPos, animDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
    }

    public void Hide(Action onComplete = null)
    {
        _rectTransform.DOKill();
        
        _rectTransform.DOAnchorPosY(Screen.height + 200f, animDuration * 0.8f)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() => 
            {
                _rectTransform.anchoredPosition = _originalPos; 
                onComplete?.Invoke(); 
            });
    }
}
