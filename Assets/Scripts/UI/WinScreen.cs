// using UnityEngine;
// using DG.Tweening;
//
// [RequireComponent(typeof(CanvasGroup))]
// public class WinScreen : MonoBehaviour
// {
//     [Header("UI Елементи з твоєї ієрархії")]
//     [SerializeField] private CanvasGroup backgroundFade; 
//     [SerializeField] private RectTransform windowPanel; 
//     [SerializeField] private RectTransform[] buttons; 
//
//     private CanvasGroup _mainCanvasGroup;
//
//     private void Awake()
//     {
//         InitializeIfNeeded();
//         ResetWindowContent(); 
//     }
//
//     // Додаємо цей метод, щоб рятувати нас в Едиторі
//     private void InitializeIfNeeded()
//     {
//         if (_mainCanvasGroup == null)
//         {
//             _mainCanvasGroup = GetComponent<CanvasGroup>();
//         }
//     }
//
//     [ContextMenu("Play Victory Animation")]
//     public void PlayAnimation()
//     {
//         InitializeIfNeeded(); // Обов'язково перевіряємо перед стартом
//         ResetWindowContent();
//         
//         _mainCanvasGroup.alpha = 1f;
//         _mainCanvasGroup.blocksRaycasts = true;
//
//         Sequence seq = DOTween.Sequence();
//
//         // ⏱ МАГІЯ ЗАТРИМКИ: Черга чекає 2 секунди перед тим, як іти далі
//         seq.AppendInterval(2f);
//
//         // Далі все як було - спочатку фон...
//         if (backgroundFade != null)
//             seq.Append(backgroundFade.DOFade(0.8f, 0.3f));
//
//         // ...потім вікно...
//         if (windowPanel != null)
//             seq.Append(windowPanel.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack));
//
//         // ...потім кнопки
//         for (int i = 0; i < buttons.Length; i++)
//         {
//             if (buttons[i] != null)
//                 seq.Append(buttons[i].DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
//         }
//     }
//
//     [ContextMenu("Reset Window")]
//     public void ResetWindowContent()
//     {
//         InitializeIfNeeded(); // І тут теж перевіряємо!
//
//         _mainCanvasGroup.blocksRaycasts = false;
//         
//         if (backgroundFade != null) backgroundFade.alpha = 0f;
//         
//         if (windowPanel != null) windowPanel.localScale = Vector3.zero;
//         
//         foreach (var btn in buttons)
//         {
//             if (btn != null) btn.localScale = Vector3.zero;
//         }
//     }
// }

using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class WinScreen : MonoBehaviour
{
    [Header("UI Елементи з твоєї ієрархії")]
    [SerializeField] private CanvasGroup backgroundFade; 
    [SerializeField] private RectTransform windowPanel; 
    [SerializeField] private RectTransform[] buttons; 

    [Header("Ефекти (Конфеті)")]
    [Tooltip("Кинь сюди Particle System з лівого боку")]
    [SerializeField] private ParticleSystem leftConfetti;
    [Tooltip("Кинь сюди Particle System з правого боку")]
    [SerializeField] private ParticleSystem rightConfetti;

    private CanvasGroup _mainCanvasGroup;

    private void Awake()
    {
        InitializeIfNeeded();
        ResetWindowContent(); 
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

        // 🎉 ЗАПУСКАЄМО КОНФЕТІ ОДРАЗУ!
        if (leftConfetti != null) leftConfetti.Play();
        if (rightConfetti != null) rightConfetti.Play();

        Sequence seq = DOTween.Sequence();

        // ⏱ Черга чекає 2 секунди (поки летить конфеті)
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
        
        if (backgroundFade != null) backgroundFade.alpha = 0.8f; // Робимо фон трохи темним, як у грі
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

        // Зупиняємо і чистимо конфеті, якщо вікно скидається достроково
        if (leftConfetti != null) { leftConfetti.Stop(); leftConfetti.Clear(); }
        if (rightConfetti != null) { rightConfetti.Stop(); rightConfetti.Clear(); }
    }
}