using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSO", menuName = "Game Data/Item SO")]
public class ItemSO : ScriptableObject
{
    [Header("Базові дані")]
    public string ID;
    public Sprite itemSprite;

    // public string GetDisplayName()
    // {
    //     if (string.IsNullOrEmpty(ID))
    //     {
    //         return this.name; 
    //     }
    //     return ID;
    // }
    
    // public void UpdateID(string newId)
    // {
    //     ID = newId;
    //     Debug.Log($"ID для {this.name} змінено на {ID}");
    // }
}