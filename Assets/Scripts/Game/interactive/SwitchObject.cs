using Game.Effects;
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
        
        [Tooltip("стати рухомим об`єктом")]
        [SerializeField] private bool makeDraggable = false;

        private SpriteRenderer _selfSpriteRenderer;
        private bool _isOn = false;

        private void Awake()
        {
            _selfSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (_isOn)
            {
                _selfSpriteRenderer.sprite = onSprite;
            }
            else
            {
                _selfSpriteRenderer.sprite = offSprite;
            }
        }
        
        public void OnClick()
        {
            EventBus.RaiseObjectClicked();
            ToggleSwitch();
            
            if (makeDraggable)
            {
                MakeDraggable();
            }
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
                Debug.LogWarning($"[SwitchObject] {gameObject.name} не призначено ItemSO!");
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

        private void MakeDraggable()
        {
            if (!TryGetComponent(out DraggableItem _)) gameObject.AddComponent<DraggableItem>();
            if (!TryGetComponent(out DraggableVisuals _)) gameObject.AddComponent<DraggableVisuals>();

            IClickable[] clickables = GetComponents<IClickable>();

            foreach (IClickable clickable in clickables)
            {
                MonoBehaviour mb = clickable as MonoBehaviour;

                if (mb == null) continue;

                if (mb == this) continue;

                mb.enabled = false;
            }
        }
    }
}