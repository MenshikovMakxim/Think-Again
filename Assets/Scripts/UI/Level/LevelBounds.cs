using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    [Header("Розмір безпечної зони")]
    public Vector2 playableArea = new Vector2(16f, 8f);
    
    [Header("Зміщення (щоб опустити під Top Bar)")]
    public Vector2 offset = new Vector2(0f, -1.5f);

    // Цей магічний метод малює штуки ТІЛЬКИ в редакторі Unity (у вікні Scene)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Колір нашої рамки
        // Малюємо порожній квадрат за заданими розмірами
        Gizmos.DrawWireCube((Vector2)transform.position + offset, playableArea);
    }
}
