using UnityEngine;

public class DraggableItem : MonoBehaviour, IDraggable
{
    private Vector2 offset;

    // Зверни увагу: ми отримуємо worldPosition у дужках!
    public void OnBeginDrag(Vector2 worldPosition) 
    {
        // Ніяких Input.mousePosition! Працюємо тільки з тим, що передали.
        offset = (Vector2)transform.position - worldPosition;
        
        transform.localScale *= 1.1f;
    }

    public void OnDrag(Vector2 worldPosition)
    {
        transform.position = worldPosition + offset;
    }

    public void OnEndDrag()
    {
        transform.localScale /= 1.1f;
    }
}