using UnityEngine;
using Game.Interfaces;
using Game.SO;
using Game.Effects;
using DG.Tweening;

namespace Game.Interactive
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MergeableItem : MonoBehaviour, IMergeable
    {
        
        [Tooltip("Вибери тип, і об'єкт сам знайде свої дані в Базі")]
        [SerializeField] private ItemType initialType = ItemType.None;
        
        private ItemSO _itemData;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        private IMergeSystem _mergeSystem;
        private DraggableItem _draggableComponent;
        private AutoSizeCollider _autoSizeCollider;
        
        public Transform Transform => transform;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            _draggableComponent = GetComponent<DraggableItem>();
            _autoSizeCollider = GetComponent<AutoSizeCollider>();
        }

        private void Start()
        {
            ActiveCollider(true);
        }
        
        public void ActiveCollider(bool flag)
        {
            if (_collider2D != null) _collider2D.enabled = flag;
            if (_draggableComponent != null) _draggableComponent.SetReturn(flag);
        }

        public void Construct(IMergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            
            if (_itemData == null && initialType != ItemType.None)
            {
                ItemSO myData = _mergeSystem.GetItemDataByType(initialType);
                if (myData != null)
                {
                    SetItemData(myData);
                    UpdateVisuals();
                }
            }
        }
        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.TryGetComponent(out IMergeable otherItem))
            {
                if (_mergeSystem != null)
                {
                    _mergeSystem.TryMerge(this, otherItem);
                }
            }
        }
        
        public int GetID()
        {
            return gameObject.GetInstanceID();
        }
        public float GetDistanceToHome()
        {
            return Vector3.Distance(transform.position, GetStartPosition());
        }
        public ItemSO GetItemData()
        {
            return _itemData;
        }

        public ItemType GetItemType()
        {
            if (_itemData != null)
            {
                return _itemData.itemType;
            }
            
            return initialType;
        }

        public void SetItemData(ItemSO data)
        {
            _itemData = data;
            
            if (data != null)
            {
                initialType = data.itemType;
                UpdateVisuals();
            } 
        }
        
        public Vector3 GetStartPosition()
        {
            if (_draggableComponent != null)
            {
                return _draggableComponent.StartPosition;
            }
            
            return transform.position;
        }
        
        public void DestroyItem()
        {
            Destroy(gameObject);
        }
        
        public void MergeTo(IMergeable target, RecipeSO recipe)
        {
            bool amIMoving = GetDistanceToHome() >= target.GetDistanceToHome();

            IMergeable movingItem = amIMoving ? this : target;
            IMergeable stationaryItem = amIMoving ? target : this;

            void Craft()
            {
                ExecuteCraft(target, recipe, stationaryItem.Transform.position);
            }

            if (((MonoBehaviour)movingItem).TryGetComponent(out MagnetComponent magnet))
            {
                magnet.MagnetizeTo(stationaryItem.Transform, Craft);
                return;
            }

            Craft();
        }
        
        private void ExecuteCraft(IMergeable target, RecipeSO recipe, Vector2 spawnPos)
        {
            bool amIConsumed = recipe.ShouldConsume(this.GetItemType());
            bool isTargetConsumed = recipe.ShouldConsume(target.GetItemType());

            Vector3 resultDestination = CalculateResultDestination(target, amIConsumed, isTargetConsumed, spawnPos);

            if (recipe.resultItem != ItemType.None)
            {
                SpawnAndAnimateResult(recipe.resultItem, spawnPos, resultDestination);
            }

            ResolveItemsLifecycle(target, amIConsumed, isTargetConsumed);
        }
        
        private Vector3 CalculateResultDestination(IMergeable target, bool amIConsumed, bool isTargetConsumed, Vector2 defaultPos)
        {
            if (amIConsumed && !isTargetConsumed)
            {
                return this.GetStartPosition();
            }
    
            if (!amIConsumed && isTargetConsumed && target is MergeableItem targetItem)
            {
                return targetItem.GetStartPosition(); 
            }

            return defaultPos;
        }
        
        private void SpawnAndAnimateResult(ItemType resultType, Vector2 spawnPos, Vector3 destination)
        {
            GameObject newResult = _mergeSystem.SpawnItem(resultType, spawnPos);
            if (newResult == null) return;
        
            Collider2D col = newResult.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            
            void FinalizeSetup()
            {
                if (col != null) col.enabled = true;
                if (newResult.TryGetComponent(out DraggableItem drag)) drag.SetStartPosition(destination);
            }
        
            if (Vector3.Distance(newResult.transform.position, destination) > 0.05f)
            {
                newResult.transform.DOMove(destination, 0.25f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(FinalizeSetup);
            }
            else 
            {
                FinalizeSetup();
            }
        }
        
        private void ResolveItemsLifecycle(IMergeable target, bool amIConsumed, bool isTargetConsumed)
        {
            if (amIConsumed) this.DestroyItem();
            else this.RestoreAfterCraft(); 
    
            if (isTargetConsumed) target.DestroyItem();
            else if (target is MergeableItem otherItem) otherItem.RestoreAfterCraft();
        }
        
        public void RestoreAfterCraft()
        {
            if (_draggableComponent != null)
            {
                ActiveCollider(false);
                _draggableComponent.ForceReturn();
                ActiveCollider(true);
            }
            
            if (TryGetComponent(out MagnetComponent magnet) && _itemData != null)
            {
                magnet.Restore(_itemData.defaultScale);
            }
        }
        
        private void UpdateVisuals()
        {
            if (_itemData != null && _itemData.itemSprite != null)
            {
                if (_spriteRenderer != null) 
                {
                    _spriteRenderer.sprite = _itemData.itemSprite;
                    // ResizeCollider(_spriteRenderer);
                    _autoSizeCollider.ResizeCollider();

                }
                
                transform.localScale = _itemData.defaultScale; 
                gameObject.name = "Item_" + _itemData.itemType;
            }
        }

        // private void ResizeCollider(SpriteRenderer sprite)
        // {
        //     Vector2 spriteSize = sprite.sprite.bounds.size;
        //     
        //     if (TryGetComponent<CircleCollider2D>(out var circle))
        //     {
        //         circle.radius = Mathf.Max(spriteSize.x, spriteSize.y) / resizeCollision;
        //     }
        //     
        //     if (TryGetComponent<BoxCollider2D>(out var box))
        //     {
        //         box.size = spriteSize;
        //         box.offset = sprite.sprite.bounds.center - transform.position;
        //     }
        // }

        private void OnValidate()
        {
        #if UNITY_EDITOR
            if (initialType != ItemType.None && !Application.isPlaying)
            {
                ItemSO[] allEditorItems = Resources.LoadAll<ItemSO>("Items");
                foreach (var item in allEditorItems)
                {
                    if (item.itemType == initialType)
                    {
                        _itemData = item; 
                        UpdateVisuals();
                        return; 
                    }
                }
            }
        #endif
        }
    }
}