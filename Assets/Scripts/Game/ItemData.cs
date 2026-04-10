using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    [TextArea]
    public string itemDescription; 
    public Sprite itemIcon;
}
