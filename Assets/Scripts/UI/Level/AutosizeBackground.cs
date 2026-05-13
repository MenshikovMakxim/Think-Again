using UnityEngine;

public class AutosizeBackground: MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer backgroundSprite;

    private void Start()
    {
        StretchBackgroundToFullScreen();
    }
    private void StretchBackgroundToFullScreen()
    {
        if (backgroundSprite == null) return;

        Camera cam = Camera.main;
        if (cam == null) return;
        
        backgroundSprite.transform.position = new Vector3(
            cam.transform.position.x, 
            cam.transform.position.y, 
            backgroundSprite.transform.position.z
        );
        
        float screenHeight = cam.orthographicSize * 2f; 
        float screenWidth = screenHeight * cam.aspect;
        
        Vector2 spriteBaseSize = backgroundSprite.sprite.bounds.size;
        
        float scaleX = screenWidth / spriteBaseSize.x;
        float scaleY = screenHeight / spriteBaseSize.y;
        
        backgroundSprite.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    } 
}
