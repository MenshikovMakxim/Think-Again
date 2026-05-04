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
        
        public Transform Transform => transform;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            _draggableComponent = GetComponent<DraggableItem>();
        }

        private void Start()
        {
            _draggableComponent.MustReturn();
        }
        
        public void ActiveCollider(bool flag)
        {
            if (_collider2D != null) _collider2D.enabled = flag;
            _draggableComponent.OnDontReturn();
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
                MergeWith(otherItem);
            }
        } 

        private bool IsMoving()
        {
            if (_draggableComponent != null)
            {
                return _draggableComponent.IsDragged;
            }

            return false;
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
        
        private RecipeSO TryCreateItem(IMergeable otherItem)
        {
            if (_mergeSystem == null) return null;
            
            ItemType myType = GetItemType();
            ItemType otherType = otherItem.GetItemType();
            
            return _mergeSystem.TryGetRecipe(myType, otherType);
        }

        private void MergeWith(IMergeable otherItem)
        {
            
            RecipeSO recipe = TryCreateItem(otherItem);
            
            if (recipe != null)
            {
                otherItem.ActiveCollider(false);
                ActiveCollider(false);
                
                if (gameObject.GetInstanceID() > ((MonoBehaviour)otherItem).gameObject.GetInstanceID())
                {
                    OnEnableMagnet(otherItem, recipe);
                }
            }
        }
        
        private void OnEnableMagnet(IMergeable target, RecipeSO recipe)
        {
            float myDistToHome = Vector3.Distance(transform.position, GetStartPosition());
            float targetDistToHome = 0f;
            
            if (target is MergeableItem targetItem)
            {
                targetDistToHome = Vector3.Distance(targetItem.transform.position, targetItem.GetStartPosition());
            }
            
            bool amIMoving = myDistToHome >= targetDistToHome;

            MonoBehaviour movingObj = amIMoving ? this : (MonoBehaviour)target;
            MonoBehaviour stationaryObj = amIMoving ? (MonoBehaviour)target : this;


            if (movingObj.TryGetComponent(out MagnetComponent magnet))
            {
                magnet.MagnetizeTo(stationaryObj.transform, () =>
                {
                    Vector2 spawnPos = stationaryObj.transform.position;
                    ExecuteCraft(target, recipe, spawnPos);
                });
            }
            else
            {
                Vector2 spawnPos = (transform.position + ((MonoBehaviour)target).transform.position) / 2f;
                ExecuteCraft(target, recipe, spawnPos);
            }
        }
        private void ExecuteCraft(IMergeable target, RecipeSO recipe, Vector2 spawnPos)
        {
            bool amIConsumed = recipe.ShouldConsume(this.GetItemType());
            bool isTargetConsumed = recipe.ShouldConsume(target.GetItemType());

            Vector3 resultDestination = spawnPos; 

            if (amIConsumed && !isTargetConsumed)
            {
                resultDestination = this.GetStartPosition(); 
            }
            else if (!amIConsumed && isTargetConsumed)
            {
                if (target is MergeableItem targetItem)
                {
                    resultDestination = targetItem.GetStartPosition(); 
                }
            }
            
            if (recipe.resultItem != ItemType.None)
            {
                GameObject newResult = _mergeSystem.SpawnItem(recipe.resultItem, spawnPos);
                
                if (newResult != null)
                {
                    Collider2D col = newResult.GetComponent<Collider2D>();
                    if (col != null) col.enabled = false;
                    
                    if (Vector3.Distance(newResult.transform.position, resultDestination) > 0.05f)
                    {
                        newResult.transform.DOMove(resultDestination, 0.25f)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() => 
                            {
                                if (col != null) col.enabled = true;
                                if (newResult.TryGetComponent(out DraggableItem drag)) drag.SetStartPosition(resultDestination);
                            });
                    }
                    else 
                    {
                        if (col != null) col.enabled = true;
                        if (newResult.TryGetComponent(out DraggableItem drag)) drag.SetStartPosition(resultDestination);
                    }
                }
            }
            
            if (amIConsumed) this.DestroyItem();
            else this.RestoreAfterCraft(); 
            
            if (isTargetConsumed) target.DestroyItem();
            else if (target is MergeableItem otherItem) otherItem.RestoreAfterCraft();
        }
        
        private void RestoreAfterCraft()
        {
            if (_draggableComponent != null)
            {
                _draggableComponent.ForceReturn();
                ActiveCollider(false);
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
                }
                
                transform.localScale = _itemData.defaultScale; 
                gameObject.name = "Item_" + _itemData.itemType;
            }
        }
        
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