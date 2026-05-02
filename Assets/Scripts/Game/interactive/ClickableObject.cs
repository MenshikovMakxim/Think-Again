using UnityEngine;
using DG.Tweening;

public class ClickableObject : MonoBehaviour, IClickable
{
    [SerializeField] private bool isWinObject = false;
    public void OnClick()
    {
        OnClickEffect();
        if (isWinObject)
        {
            EventBus.RaiseLevelFinished(EventBus.SetItemData(true, null, transform.position, transform));
        }
    }
    
    private void OnClickEffect()
    {
        EventBus.RaiseObjectClicked();
        
        transform.DOScale(1.02f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
