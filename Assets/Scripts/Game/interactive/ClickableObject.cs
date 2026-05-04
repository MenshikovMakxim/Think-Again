using UnityEngine;
using Game.Interfaces;

namespace Game.Interactive
{
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
        }
    }
}