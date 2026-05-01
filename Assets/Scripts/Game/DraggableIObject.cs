using System.Collections; 
using UnityEngine;

public class DraggableItem : MonoBehaviour, IDraggable
{
    [SerializeField] private bool shouldReturn = true; 
    
    private Vector2 _offset;
    private Vector3 _startPosition;
    private Collider2D _collider;
    public bool isDragged {get; set;}
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag(Vector2 worldPosition) 
    {
        isDragged = true;
        
        StopAllCoroutines(); 
        
        SwitchCollision(true);
        
        _startPosition = transform.position;
        _offset = (Vector2)transform.position - worldPosition;
        transform.localScale *= 1.1f;
    }

    public void OnDrag(Vector2 worldPosition)
    {
        transform.position = (Vector3)(worldPosition + _offset);
    }

    public void OnEndDrag()
    {
        isDragged = false;
        
        transform.localScale /= 1.1f;
        
        if (shouldReturn)
        {
            StartCoroutine(SmoothReturn());
        }
    }
    
    private IEnumerator SmoothReturn()
    {
        SwitchCollision(false);
        
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
        
        SwitchCollision(true);
    }

    private void SwitchCollision(bool flag)
    {
        if (_collider != null) _collider.enabled = flag;
    }
    
    public void OnDontReturn()
    {
        shouldReturn = false;
    }
}