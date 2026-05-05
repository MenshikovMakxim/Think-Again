using UnityEngine;
using Game.Interfaces;
using Game.SO;

namespace Game.Interactive
{
    public class SwitchObject : MonoBehaviour, IClickable
    {
        [Header("Налаштування самого перемикача")] [Tooltip("Чи змінює він свій вигляд при кліку?")] [SerializeField]
        private bool selfSwitch = true;

        [Tooltip("Спрайт, коли перемикач УВІМКНЕНО")] [SerializeField]
        private Sprite onSprite;

        [Tooltip("Спрайт, коли перемикач ВИМКНЕНО")] [SerializeField]
        private Sprite offSprite;

        [Header("Дані для передачі")]
        [Tooltip("Той самий ItemSO, який ми будемо 'впихати' в інші об'єкти")]
        [SerializeField]
        private ItemSO itemDataToPass;

        [Header("Кому ми це передаємо (Піддослідні)")] [Tooltip("Об'єкти, які отримають новий ItemSO")] [SerializeField]
        private GameObject[] objectsToToggle;

        private SpriteRenderer _selfSpriteRenderer;
        private bool _isOn = false;

        private void Awake()
        {
            _selfSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void OnClick()
        {
            EventBus.RaiseObjectClicked();
            ToggleSwitch();
        }

        private void ToggleSwitch()
        {
            _isOn = !_isOn;

            if (selfSwitch && _selfSpriteRenderer != null)
            {
                _selfSpriteRenderer.sprite = _isOn ? onSprite : offSprite;
            }

            if (itemDataToPass == null)
            {
                Debug.LogError($"[SwitchObject] {gameObject.name} не призначено ItemSO!");
                return;
            }

            foreach (GameObject obj in objectsToToggle)
            {
                if (obj == null) continue;

                if (obj.TryGetComponent(out IMergeable mergeObject))
                {
                    mergeObject.SetItemData(itemDataToPass);
                }
                else
                {
                    Debug.LogWarning($"[SwitchObject] {obj.name} немає скрипта MergeItem!");
                }
            }
        }
    }
}