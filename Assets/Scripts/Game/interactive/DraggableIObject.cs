using System;
using System.Collections;
using UnityEngine;
using Game.Interfaces;

namespace Game.Interactive
{
    public class DraggableItem : MonoBehaviour, IDraggable
    {
        public event Action OnGrabbed;
        public event Action OnReleased;

        [SerializeField] private bool shouldReturn = true;

        private Vector2 _offset;
        private Vector3 _startPosition;
        
        public bool IsDragged { get; set; } 
        
        private void Start()
        {
            _startPosition = transform.position;
        }
        
        public void SetStartPosition(Vector3 pos) { _startPosition = pos; }
        public Vector3 StartPosition => _startPosition;

        public void OnBeginDrag(Vector2 worldPosition)
        {
            IsDragged = true;
            StopAllCoroutines();
            
            _offset = (Vector2)transform.position - worldPosition;
            
            OnGrabbed?.Invoke();
        }

        public void OnDrag(Vector2 worldPosition)
        {
            transform.position = (Vector3)(worldPosition + _offset);
        }

        public void OnEndDrag()
        {
            IsDragged = false;
            OnReleased?.Invoke();

            if (shouldReturn)
            {
                _startPosition = transform.position;
                StartCoroutine(SmoothReturn());
            }
        }

        private IEnumerator SmoothReturn()
        {
            float elapsed = 0;
            float duration = 0.2f;
            Vector3 currentPos = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(currentPos, _startPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = _startPosition;
        }

        public void OnDontReturn()
        {
            shouldReturn = false;
        }
        
        public void ForceReturn()
        {
            shouldReturn = true; 
            StopAllCoroutines(); 
            
            if (Vector3.Distance(transform.position, _startPosition) > 0.01f)
            {
                StartCoroutine(SmoothReturn());
            }
        }
        
        public void MustReturn()
        {
            shouldReturn = true;
        }
    }
}