using UnityEngine;

public interface IDraggable
{
    bool isDragged { get; set; }
    void OnBeginDrag(Vector2 worldPosition);
    void OnDrag(Vector2 mousePosition);
    void OnEndDrag();
}
