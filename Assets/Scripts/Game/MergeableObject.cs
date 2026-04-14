using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MergeableItem : MonoBehaviour
{
    public ItemSO itemData;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        MergeableItem otherItem = collision.gameObject.GetComponent<MergeableItem>();

        if (otherItem != null)
        {
            MergeWith(otherItem);
        }
    }

    public ItemSO GetItemData() => itemData;
    
    public void SetItemData(ItemSO data)
    {
        itemData = data;
        
        if (itemData != null && itemData.itemSprite != null)
        {
            _spriteRenderer.sprite = itemData.itemSprite;
            _boxCollider.size = _spriteRenderer.sprite.bounds.size;
            gameObject.name = "Item_" + itemData.ID;
        }
    }

    private void MergeWith(MergeableItem otherItem)
    {
        string myId = this.itemData.ID;
        string otherId = otherItem.GetItemData().ID;

        ItemSO resultData = CraftingManager.Instance.TryGetMergeResult(myId, otherId);

        if (resultData != null)
        {
            if (this.gameObject.GetInstanceID() > otherItem.gameObject.GetInstanceID())
            {
                Vector2 spawnPos = (transform.position + otherItem.transform.position) / 2f;
                
                CraftingManager.Instance.SpawnItem(resultData, spawnPos);
                
                Destroy(otherItem.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    
    // Метод наглядного перетворення об`єкта
    private void OnValidate()
    {
        if (itemData != null && itemData.itemSprite != null)
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_boxCollider == null) _boxCollider = GetComponent<BoxCollider2D>();

            if (_spriteRenderer != null) _spriteRenderer.sprite = itemData.itemSprite;
            if (_boxCollider != null) _boxCollider.size = itemData.itemSprite.bounds.size;
            
            gameObject.name = "Item_" + itemData.ID;
        }
    }
}
