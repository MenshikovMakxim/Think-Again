using UnityEngine;
using UnityEngine.InputSystem;

public class UICursor : MonoBehaviour
{
    [Header("Курсори")]
    public Texture2D defaultCursor; 
    public Texture2D dragCursor;    
    public Texture2D clickCursor;   

    [Header("Налаштування")]
    public Vector2 hotSpot = Vector2.zero; 
    
    private float _clickTimer = 0f;
    private readonly float _clickVisualDuration = 0.05f;
    
    private bool _isHolding = false;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }
    
    void Update()
    {
        if (Mouse.current == null) return;

        var leftBtn = Mouse.current.leftButton;
    
        if (leftBtn.wasReleasedThisFrame)
        {
            _isHolding = false;
            _clickTimer = 0f;
            Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
            return; 
        }

        if (leftBtn.wasPressedThisFrame)
        {
            Cursor.SetCursor(clickCursor != null ? clickCursor : defaultCursor, hotSpot, CursorMode.Auto);
            _clickTimer = _clickVisualDuration;
        }
        else if (leftBtn.isPressed)
        {
            if (_isHolding)
            {
                Cursor.SetCursor(dragCursor, hotSpot, CursorMode.Auto);
                _clickTimer = 0f; 
            }
            else if (_clickTimer > 0f) 
            {
                _clickTimer -= Time.deltaTime;
            
                if (_clickTimer <= 0f)
                {
                    Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
                }
            }
        }
    }

    public void ActiveHolding(bool flag)
    {
        _isHolding = flag;
        
        if (_isHolding && Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Cursor.SetCursor(dragCursor, hotSpot, CursorMode.Auto);
        }
    }
}