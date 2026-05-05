using System.Collections.Generic;
using UnityEngine;
using Game.SO;

namespace Game.Systems
{
    public class GameDatabase : MonoBehaviour
    {
        private Dictionary<ItemType, ItemSO> _itemDatabase;
        private Dictionary<(ItemType, ItemType), RecipeSO> _recipeDatabase;

        private void Awake()
        {
            InitializeItems();
            InitializeRecipes();
        }

        #region Ініціалізація

        private void InitializeItems()
        {
            _itemDatabase = new Dictionary<ItemType, ItemSO>();
            
            ItemSO[] loadedItems = Resources.LoadAll<ItemSO>("Items");

            if (loadedItems.Length == 0)
            {
                Debug.LogWarning("[Database] Не знайдено предметів!");
                return;
            }

            foreach (var item in loadedItems)
            {
                if (item.itemType == ItemType.None) continue;

                if (!_itemDatabase.ContainsKey(item.itemType))
                {
                    _itemDatabase.Add(item.itemType, item);
                }
                else
                {
                    Debug.LogError($"[Database] Конфлікт! Тип {item.itemType} вже зайнятий предметом {_itemDatabase[item.itemType].name}.");
                }
            }
            Debug.Log($"[Database] Завантажено {_itemDatabase.Count} предметів.");
        }

        private void InitializeRecipes()
        {
            _recipeDatabase = new Dictionary<(ItemType, ItemType), RecipeSO>();
            
            // Завантажуємо з папки Resources/Recipes
            RecipeSO[] loadedRecipes = Resources.LoadAll<RecipeSO>("Recipes");

            if (loadedRecipes.Length == 0)
            {
                Debug.LogWarning("[Database] Не знайдено рецептів!");
                return;
            }
            
            foreach (var recipe in loadedRecipes)
            {
                if (recipe == null || recipe.input1 == ItemType.None || recipe.input2 == ItemType.None || recipe.resultItem == ItemType.None) 
                    continue;

                var key = BuildRecipeKey(recipe.input1, recipe.input2);
                
                if (!_recipeDatabase.ContainsKey(key))
                {
                    _recipeDatabase.Add(key, recipe);
                }
                else
                {
                    Debug.LogError($"[Database] Конфлікт рецептів! Злиття {recipe.input1} + {recipe.input2} вже існує.");
                }
            }
            Debug.Log($"[Database] Завантажено {_recipeDatabase.Count} рецептів.");
        }

        private (ItemType, ItemType) BuildRecipeKey(ItemType type1, ItemType type2)
        {
            return type1 < type2 ? (type1, type2) : (type2, type1);
        }

        #endregion

        #region Публічні методи для інших систем (API Бази)

        /// <summary>
        /// Повертає рецепт для двох типів предметів, або null, якщо такого крафту не існує
        /// </summary>
        public RecipeSO GetRecipe(ItemType type1, ItemType type2)
        {
            var key = BuildRecipeKey(type1, type2);
            return _recipeDatabase.TryGetValue(key, out RecipeSO recipe) ? recipe : null;
        }

        /// <summary>
        /// Повертає файл даних предмета за його Енамом
        /// </summary>
        public ItemSO GetItemData(ItemType type)
        {
            if (_itemDatabase.TryGetValue(type, out ItemSO itemData))
            {
                return itemData;
            }
            
            Debug.LogError($"[Database] Предмет типу {type} не знайдено!");
            return null;
        }

        #endregion
    }
}
