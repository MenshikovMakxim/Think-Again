using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "Recipe_", menuName = "Game Data/Recipe SO")]
    public class RecipeSO : ScriptableObject
    {
        [Header("Інгредієнти (Що змішуємо)")] 
        public ItemType input1;
        public ItemType input2;

        [Header("Результат (Що отримуємо)")] 
        // Результат залишаємо як ItemSO, бо системі мержу потрібен сам префаб/дані для спавну нового об'єкта
        public ItemType resultItem; 

        [Header("Обов'язки: Правила знищення")]
        [Tooltip("Чи зникає перший предмет?")]
        public bool consumeInput1 = true; 
        
        [Tooltip("Чи зникає другий предмет?")]
        public bool consumeInput2 = true;
        
        public bool CanCraft(ItemType itemA, ItemType itemB)
        {
            if (itemA == ItemType.None || itemB == ItemType.None) return false;

            // Перевіряємо прямий збіг (A=1, B=2)
            bool isStraightMatch = (itemA == input1 && itemB == input2);
            // Перевіряємо зворотний збіг (A=2, B=1)
            bool isReverseMatch = (itemA == input2 && itemB == input1);

            return isStraightMatch || isReverseMatch;
        }

        /// <summary>
        /// Повертає, чи повинен конкретний предмет зникнути під час цього крафту
        /// </summary>
        public bool ShouldConsume(ItemType itemToCheck)
        {
            // Якщо предмет збігається з input1, повертаємо правило для input1
            if (itemToCheck == input1) return consumeInput1;
            
            // Якщо з input2 - повертаємо правило для input2
            if (itemToCheck == input2) return consumeInput2;

            // Якщо предмет взагалі не з цього рецепту (підстраховка)
            return false; 
        }
    }
}