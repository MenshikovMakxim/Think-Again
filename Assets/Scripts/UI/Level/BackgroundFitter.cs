using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        FitToScreen();
    }

    private void FitToScreen()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer.sprite == null) return;
        
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }
        
        transform.localScale = Vector3.one;
        
        float screenHeight = cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;
        
        float spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = _spriteRenderer.sprite.bounds.size.y;
        
        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;
        
        float finalScale = Mathf.Max(scaleX, scaleY);
        
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }
}