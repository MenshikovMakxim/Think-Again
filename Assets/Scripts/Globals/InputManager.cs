using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private IDraggable _currentDraggedObject;

    void Update()
    {
        // 1. Отримуємо дані з нової системи
        Vector2 screenPosition = Vector2.zero;
        bool isPressDown = false;
        bool isPressing = false;
        bool isPressUp = false;

        // Перевірка для ПК (Миша) та Мобілок (Touch)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            var touch = Touchscreen.current.primaryTouch;
            screenPosition = touch.position.ReadValue();
            isPressDown = touch.press.wasPressedThisFrame;
            isPressing = touch.press.isPressed;
            isPressUp = touch.press.wasReleasedThisFrame;
        }
        else if (Mouse.current != null)
        {
            screenPosition = Mouse.current.position.ReadValue();
            isPressDown = Mouse.current.leftButton.wasPressedThisFrame;
            isPressing = Mouse.current.leftButton.isPressed;
            isPressUp = Mouse.current.leftButton.wasReleasedThisFrame;
        }

        // 2. Перевірка UI (тепер через PointerEventData, щоб було надійніше)
        if (isPressDown && EventSystem.current.IsPointerOverGameObject()) return;

        // 3. Конвертація координат
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        
        Vector3 tempScreenPos = new Vector3(screenPosition.x, screenPosition.y, 10f); 

        // Тепер конвертуємо
        worldPosition = Camera.main.ScreenToWorldPoint(tempScreenPos);
        
        // --- ЛОГІКА ЗАЛИШАЄТЬСЯ ТАКА САМА ---
        if (isPressDown)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider is not null)
            {
                IDraggable draggable = hit.collider.GetComponent<IDraggable>();
                if (draggable != null)
                {
                    _currentDraggedObject = draggable;
                    _currentDraggedObject.OnBeginDrag(worldPosition);
                }
                else
                {
                    IClickable clickable = hit.collider.GetComponent<IClickable>();
                    clickable?.OnClick();
                }
            }
        }

        if (isPressing && _currentDraggedObject != null)
        {
            _currentDraggedObject.OnDrag(worldPosition);
        }

        if (isPressUp && _currentDraggedObject != null)
        {
            _currentDraggedObject.OnEndDrag();
            _currentDraggedObject = null;
        }
    }
}