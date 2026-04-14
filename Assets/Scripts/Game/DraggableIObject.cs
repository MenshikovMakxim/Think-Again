using UnityEngine;

public class DraggableItem : MonoBehaviour, IDraggable
{
    private Vector2 _offset;
    
    public void OnBeginDrag(Vector2 worldPosition) 
    {
        _offset = (Vector2)transform.position - worldPosition;
        
        transform.localScale *= 1.1f;
    }

    public void OnDrag(Vector2 worldPosition)
    {
        transform.position = worldPosition + _offset;
    }

    public void OnEndDrag()
    {
        transform.localScale /= 1.1f;
    }
}