using System;
using UnityEngine;

[ExecuteAlways] 
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AutoSizeCollider : MonoBehaviour
{
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    public float resizeCollision = 2f; // більше - зменшення, менше - збільшення

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResizeCollider();
    }

    public void ResizeCollider()
    {
        if (_spriteRenderer == null) return;
        
        Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;
            
        if (TryGetComponent<CircleCollider2D>(out var circle))
        {
            circle.radius = Mathf.Max(spriteSize.x, spriteSize.y) / resizeCollision;
        }
            
        if (TryGetComponent<BoxCollider2D>(out var box))
        {
            box.size = spriteSize;
            box.offset = _spriteRenderer.sprite.bounds.center - transform.position;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ResizeCollider();
    }
#endif
}