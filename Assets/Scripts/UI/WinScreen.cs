using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class WinScreen : MonoBehaviour
{
    [Header("UI Елементи з ієрархії")]
    [SerializeField] private CanvasGroup backgroundFade; 
    [SerializeField] private RectTransform windowPanel; 
    [SerializeField] private RectTransform[] buttons; 

    [Header("Ефекти (Конфеті)")]
    [Tooltip("Particle System з лівого боку")]
    [SerializeField] private ParticleSystem leftConfetti;
    [Tooltip("Particle System з правого боку")]
    [SerializeField] private ParticleSystem rightConfetti;
    
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Button nextLevelButton;

    private CanvasGroup _mainCanvasGroup;

    private void Awake()
    {
        InitializeIfNeeded();
        ResetWindowContent(); 
    }
    
    private void OnEnable()
    {
        SetupPopup();
        PlayAnimation();
        levelManager.RaiseCompletedLevel();
    }

    private void InitializeIfNeeded()
    {
        if (_mainCanvasGroup == null)
        {
            _mainCanvasGroup = GetComponent<CanvasGroup>();
        }
    }

    [ContextMenu("Play Victory Animation")]
    public void PlayAnimation()
    {
        InitializeIfNeeded(); 
        ResetWindowContent();
        
        _mainCanvasGroup.alpha = 1f;
        _mainCanvasGroup.blocksRaycasts = true;
        
        if (leftConfetti != null) leftConfetti.Play();
        if (rightConfetti != null) rightConfetti.Play();

        Sequence seq = DOTween.Sequence();
        
        seq.AppendInterval(2f);

        if (backgroundFade != null)
            seq.Append(backgroundFade.DOFade(0.8f, 0.3f));

        if (windowPanel != null)
            seq.Append(windowPanel.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack));

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
                seq.Append(buttons[i].DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
        }
    }
    
    [ContextMenu("Show Window (For Editing)")]
    public void ShowWindowForEditing()
    {
        InitializeIfNeeded(); 

        _mainCanvasGroup.alpha = 1f;
        _mainCanvasGroup.blocksRaycasts = true;
        
        if (backgroundFade != null) backgroundFade.alpha = 0.8f;
        if (windowPanel != null) windowPanel.localScale = Vector3.one;
        
        foreach (var btn in buttons)
        {
            if (btn != null) btn.localScale = Vector3.one;
        }
    }

    [ContextMenu("Reset Window")]
    public void ResetWindowContent()
    {
        InitializeIfNeeded(); 

        _mainCanvasGroup.blocksRaycasts = false;
        
        if (backgroundFade != null) backgroundFade.alpha = 0f;
        if (windowPanel != null) windowPanel.localScale = Vector3.zero;
        
        foreach (var btn in buttons)
        {
            if (btn != null) btn.localScale = Vector3.zero;
        }
        
        if (leftConfetti != null) { leftConfetti.Stop(); leftConfetti.Clear(); }
        if (rightConfetti != null) { rightConfetti.Stop(); rightConfetti.Clear(); }
    }
    
    private void SetupPopup()
    {
        nextLevelButton.gameObject.SetActive(!levelManager.IsLastLevel());
    }
}