using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public int levelNumber; // Номер рівня для запуску
    public TextMeshProUGUI buttonText; // Посилання на текст на кнопці

    // Метод, який ми викликаємо при натисканні
    public void OnClick()
    {
        // Звертаємося до головного менеджера і просимо завантажити рівень
        LevelManager.Instance.LoadLevel(levelNumber);
    }

    // Допоміжний метод, щоб автоматично міняти цифру на кнопці
    public void Setup(int number)
    {
        levelNumber = number;
        buttonText.text = number.ToString();
    }
}