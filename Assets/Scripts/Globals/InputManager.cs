using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Game.Interfaces;

public class InputManager : MonoBehaviour
{
    private IDraggable _currentDraggedObject;

    void Update()
    {
        Vector2 screenPosition = Vector2.zero;
        bool isPressDown = false;
        bool isPressing = false;
        bool isPressUp = false;
        
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
        
        if (isPressDown && EventSystem.current.IsPointerOverGameObject()) return;
        
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector3 tempScreenPos = new Vector3(screenPosition.x, screenPosition.y, 10f); 
        worldPosition = Camera.main.ScreenToWorldPoint(tempScreenPos);
        
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
                    IClickable[] clickables = hit.collider.GetComponents<IClickable>();

                    foreach (var clickable in clickables)
                    {
                        clickable.OnClick();
                    }
                }
            }
        }
        
        if (isPressing && _currentDraggedObject != null)
        {
            if (_currentDraggedObject as MonoBehaviour == null)
            {
                _currentDraggedObject = null;
            }
            else
            {
                _currentDraggedObject.OnDrag(worldPosition);
            }
        }
        
        if (isPressUp && _currentDraggedObject != null)
        {
            if (_currentDraggedObject as MonoBehaviour != null)
            {
                _currentDraggedObject.OnEndDrag();
            }
            
            _currentDraggedObject = null; 
        }
    }
}