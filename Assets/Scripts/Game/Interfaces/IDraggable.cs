using UnityEngine;

namespace Game.Interfaces
{
    public interface IDraggable
    {
        bool IsDragged { get; set; }
        void OnBeginDrag(Vector2 worldPosition);
        void OnDrag(Vector2 mousePosition);
        void OnEndDrag();
    }
}