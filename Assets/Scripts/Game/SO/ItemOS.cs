using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "Item_", menuName = "Game Data/Item SO")]
    public class ItemSO : ScriptableObject
    {
        [Header("Ідентифікація")]
        public ItemType itemType;
        public Sprite itemSprite;
        public Vector3 defaultScale = new  Vector3(0.7f, 0.7f, 0.7f);
    }
}