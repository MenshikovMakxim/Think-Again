using UnityEngine;

public class EffectController : MonoBehaviour
{
    [Header("Ефекти рівня")]
    [Tooltip("Префаб з ParticleSystem або твоїм SimpleGlow2D")]
    [SerializeField] private GameObject winGlowPrefab; 
    [SerializeField] private GameObject loseSmokePrefab; 

    [Header("Ефекти крафту")]
    [SerializeField] private GameObject craftSparklePrefab;

    // public void OnEnable()
    // {
    //     EventBus.OnLevelFinished += HandleLevelFinished;
    //     // Підписуємося на інші ефекти тут...
    // }
    //
    // public void OnDisable()
    // {
    //     EventBus.OnLevelFinished -= HandleLevelFinished;
    // }
    
    // private void HandleLevelFinished()
    // {
    //     if (data.IsWin)
    //     {
    //         PlayEffect(winGlowPrefab, data.TargetPosition, data.TargetTransform);
    //     }
    //     else
    //     {
    //         PlayEffect(loseSmokePrefab, data.TargetPosition, null);
    //     }
    // }

    // Універсальний метод для програвання будь-якого ефекту
    private void PlayEffect(GameObject effectPrefab, Vector3 position, Transform parent = null)
    {
        if (effectPrefab == null) return;

        // Спавнимо ефект. Якщо parent не null, він стане дочірнім об'єктом і буде рухатися за ним
        GameObject spawnedEffect = Instantiate(effectPrefab, position, Quaternion.identity, parent);

        // Якщо це просто партикли, можна зробити автознищення. 
        // Якщо це світіння, треба передбачити логіку його згасання в самому префабі.
        Destroy(spawnedEffect, 3f); // Вбиваємо через 3 секунди, щоб не засмічувати пам'ять
    }
}