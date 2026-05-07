using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalCursorManager : MonoBehaviour
{
    [Header("Курсори")]
    public Texture2D defaultCursor; // Звичайна стрілочка
    public Texture2D dragCursor;    // Курсор для перетягування (наприклад, кулак)
    public Texture2D clickCursor;   // Опціонально: курсор при самому кліку

    [Header("Налаштування")]
    public float holdThreshold = 0.2f; // Скільки секунд тримати, щоб вважалося "перетягуванням"
    public Vector2 hotSpot = Vector2.zero; // Центр кліку курсора

    private float pressTime;
    private bool isHolding = false;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }

    void Update()
    {
        if (Mouse.current == null) return;

        var leftBtn = Mouse.current.leftButton;
        
        if (leftBtn.wasPressedThisFrame)
        {
            pressTime = Time.time;
            isHolding = false;
            
            Cursor.SetCursor(clickCursor != null ? clickCursor : defaultCursor, hotSpot, CursorMode.Auto);
        }

        if (leftBtn.isPressed)
        {
            if (!isHolding && (Time.time - pressTime > holdThreshold))
            {
                isHolding = true;
                Cursor.SetCursor(dragCursor, hotSpot, CursorMode.Auto);
            }
        }
        
        if (leftBtn.wasReleasedThisFrame)
        {
            Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
            isHolding = false;
        }
    }
}