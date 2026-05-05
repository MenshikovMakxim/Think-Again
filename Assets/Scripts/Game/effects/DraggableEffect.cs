using UnityEngine;
using Game.Interactive;

namespace Game.Effects
{
    [RequireComponent(typeof(DraggableItem))] 
    public class DraggableVisuals : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float dragScaleMultiplier = 1.1f;
        [SerializeField] private int dragSortingOrder = 999;

        private DraggableItem _draggable;
        private SpriteRenderer _spriteRenderer;
        
        private Vector3 _originalScale;
        private int _originalSortingOrder;

        private void Awake()
        {
            _draggable = GetComponent<DraggableItem>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalScale = transform.localScale; 
        }
        

        private void OnEnable()
        {
            _draggable.OnGrabbed += ApplyDragEffects;
            _draggable.OnReleased += ResetDragEffects;
        }

        private void OnDisable()
        {
            _draggable.OnGrabbed -= ApplyDragEffects;
            _draggable.OnReleased -= ResetDragEffects;
        }

        private void ApplyDragEffects()
        {
            _originalSortingOrder = _spriteRenderer.sortingOrder;
            _spriteRenderer.sortingOrder = dragSortingOrder;
            transform.localScale = _originalScale * dragScaleMultiplier;
        }

        private void ResetDragEffects()
        {
            _spriteRenderer.sortingOrder = _originalSortingOrder;
            transform.localScale = _originalScale; 
        }
    }
}