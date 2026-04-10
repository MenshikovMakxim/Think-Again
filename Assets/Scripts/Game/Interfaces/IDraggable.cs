using UnityEngine;

public interface IDraggable
{
    void OnBeginDrag(Vector2 worldPosition); 
    void OnDrag(Vector2 mousePosition); 
    void OnEndDrag(); 
}
