using UnityEngine;
using UnityEditor;

namespace Editor
{
   public class SaveWiper
   {
       [MenuItem("Tools/💀 Знищити всі збереження (PlayerPrefs)")]
       public static void ResetPlayerPrefs()
       {
           PlayerPrefs.DeleteKey("UnlockedLevel");
           PlayerPrefs.Save();
           Debug.LogWarning("Усі збереження успішно знищено! Починаємо з нуля.");
       }
   } 
}

