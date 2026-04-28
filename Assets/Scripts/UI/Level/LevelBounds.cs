using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    [Header("Розмір безпечної зони")]
    [SerializeField] private Vector2 playableArea = new Vector2(16f, 8f);
    
    [Header("Зміщення (щоб опустити під Top Bar)")]
    [SerializeField] private Vector2 offset = new Vector2(0f, -1.5f);
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + offset, playableArea);
    }
}
