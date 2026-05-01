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

        // 1. Отримуємо головну камеру (вона має бути Orthographic)
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("BackgroundFitter не знайшов головну камеру!");
            return;
        }

        // 2. Скидаємо скейл до одиниці, щоб рахувати від чистого розміру картинки
        transform.localScale = Vector3.one;

        // 3. Рахуємо реальні розміри екрана у світових координатах Unity
        float screenHeight = cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // 4. Дізнаємося розмір самої картинки (спрайта)
        float spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = _spriteRenderer.sprite.bounds.size.y;

        // 5. Рахуємо, у скільки разів треба збільшити картинку по ширині та висоті
        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;

        // Беремо НАЙБІЛЬШЕ значення з двох. 
        // Це гарантує, що фон перекриє екран повністю (без чорних смуг).
        // Зайве просто вилізе за краї екрана (обріжеться), що є нормою.
        float finalScale = Mathf.Max(scaleX, scaleY);

        // 7. Застосовуємо масштаб
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }
}