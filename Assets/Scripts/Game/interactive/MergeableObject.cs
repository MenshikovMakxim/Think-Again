using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MergeableItem : MonoBehaviour, IMergeable
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private bool destroyAfterMerge = true;
    
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private IMergeSystem _mergeSystem;
    private DraggableItem _draggableComponent;

    // Реалізація інтерфейсу
    public Transform Transform => transform;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _draggableComponent = GetComponent<DraggableItem>();
    }
    
    public void Construct(IMergeSystem mergeSystem)
    {
        _mergeSystem = mergeSystem;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IMergeable otherItem))
        {
            MergeWith(otherItem);
        }
    }
    
    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Аналогічно
        if (otherCollider.gameObject.TryGetComponent(out IMergeable otherItem))
        {
            MergeWith(otherItem);
        }
    }

    public ItemSO GetItemData() => itemData;
    
    public void SetItemData(ItemSO data)
    {
        itemData = data;
        UpdateVisuals();
    }

    public void DestroyItem()
    {
        if (destroyAfterMerge) Destroy(gameObject);
    }
    
    private void MergeWith(IMergeable otherItem)
    {
        _draggableComponent.OnDontReturn();
        
        if (_mergeSystem == null) return;

        string myId = GetItemData().ID;
        string otherId = otherItem.GetItemData().ID;

        ItemSO resultData = _mergeSystem.TryGetMergeResult(myId, otherId);

        if (resultData != null)
        {

            if (gameObject.GetInstanceID() > ((MonoBehaviour)otherItem).gameObject.GetInstanceID())
            {

                bool amIMoving = _draggableComponent.isDragged; 

                MonoBehaviour movingObj = amIMoving ? this : (MonoBehaviour)otherItem;
                MonoBehaviour stationaryObj = amIMoving ? (MonoBehaviour)otherItem : this;
                
                if (movingObj.TryGetComponent(out MagnetComponent magnet))
                {
                    magnet.MagnetizeTo(stationaryObj.transform, () => 
                    {
                        Vector2 spawnPos = stationaryObj.transform.position; 
                        
                        _mergeSystem.SpawnItem(resultData, spawnPos);
                    
                        otherItem.DestroyItem();
                        this.DestroyItem();
                    });
                }
                else
                {
                    Vector2 spawnPos = (transform.position + ((MonoBehaviour)otherItem).transform.position) / 2f;
                    _mergeSystem.SpawnItem(resultData, spawnPos);
                    otherItem.DestroyItem();
                    this.DestroyItem();
                }
            }
        }
    }
    
    private void OnValidate()
    {
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
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
