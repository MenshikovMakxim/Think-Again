using System;
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

        public ItemType resultItem; 

        [Header("Обов'язки: Правила знищення")]
        [Tooltip("Чи зникає перший предмет?")]
        public bool consumeInput1 = true; 
        
        [Tooltip("Чи зникає другий предмет?")]
        public bool consumeInput2 = true;

        public void Awake()
        {
            if(consumeInput1 == false && consumeInput2 == false)
            {
                Debug.LogWarning($"Рецепт {name} неправильний, оскільки обидва предмети не зникають");
            }
        }

        public bool OnValidRecipe()
        {
            if(consumeInput1 == false && consumeInput2 == false)
            {
                Debug.LogWarning($"Рецепт {name} неправильний, оскільки обидва предмети не зникають");
                return false;
            }
            
            if(input1 == ItemType.None || input2 == ItemType.None)
            {
                Debug.LogWarning($"Рецепт {name} неправильний, оскільки один з інгредієнтів не визначений");
                return false;
            }

            if (resultItem == ItemType.None)
            {
                Debug.LogWarning($"Рецепт {name} неправильний, оскільки кінцевий результат не визначений");
                return false;
            }
            
            return true;
        }

        public bool CanCraft(ItemType itemA, ItemType itemB)
        {
            if (itemA == ItemType.None || itemB == ItemType.None) return false;
            
            bool isStraightMatch = (itemA == input1 && itemB == input2);
            bool isReverseMatch = (itemA == input2 && itemB == input1);

            return isStraightMatch || isReverseMatch;
        }

        /// <summary>
        /// Повертає, чи повинен конкретний предмет зникнути під час цього крафту
        /// </summary>
        public bool ShouldConsume(ItemType itemToCheck)
        {
            if (itemToCheck == input1) return consumeInput1;
            
            if (itemToCheck == input2) return consumeInput2;
            
            return false; 
        }
    }
}