using UnityEngine;

// ВИДАЛЯЄМО public enum ItemType {...} ПОВНІСТЮ!

[System.Serializable]
public struct CraftRecipe
{
    [Tooltip("Картка предмета, на який треба кинути")]
    public ItemData targetItemData; // <-- Зміна тут
    
    public GameObject resultPrefab; 
}

public class CombinableItem : MonoBehaviour, IDraggable
{
    [Header("Мої налаштування")]
    public ItemData myItemData; // <-- Зміна тут. Тепер це посилання на файл!
    public CraftRecipe[] recipes;

    private Vector2 startPosition;
    private Vector2 offset;
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag(Vector2 worldPosition)
    {
        startPosition = transform.position;
        offset = (Vector2)transform.position - worldPosition;
        myCollider.enabled = false; 
        transform.localScale *= 1.1f; 
    }

    public void OnDrag(Vector2 worldPosition)
    {
        transform.position = worldPosition + offset;
    }

    public void OnEndDrag()
    {
        transform.localScale /= 1.1f;
        bool craftSuccessful = false;

        Collider2D hit = Physics2D.OverlapPoint(transform.position);

        if (hit != null)
        {
            CombinableItem targetItem = hit.GetComponent<CombinableItem>();
            
            if (targetItem != null)
            {
                foreach (CraftRecipe recipe in recipes)
                {
                    // Unity вміє ідеально порівнювати ScriptableObject!
                    // Вона просто перевіряє, чи це один і той самий файл на диску.
                    if (recipe.targetItemData == targetItem.myItemData) 
                    {
                        Debug.Log("Крафт успішний!");
                        Instantiate(recipe.resultPrefab, targetItem.transform.position, Quaternion.identity);
                        Destroy(targetItem.gameObject); 
                        Destroy(gameObject); 
                        craftSuccessful = true;
                        break; 
                    }
                }
            }
        }

        if (!craftSuccessful)
        {
            transform.position = startPosition; 
            myCollider.enabled = true; 
        }
    }
}
