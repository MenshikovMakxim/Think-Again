using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Елементи")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Посилання на систему")]
    [SerializeField] private AudioSystem audioSystem;

    private void Start()
    {
        if (audioSystem == null)
        {
            Debug.LogError("[SettingsMenu] Не знайдено AudioSystem на сцені!");
            return;
        }

        // 2. Встановлюємо слайдери в те положення, яке збережено в налаштуваннях
        musicSlider.value = audioSystem.GetMusicVolume();
        sfxSlider.value = audioSystem.GetSFXVolume();

        // 3. Підписуємо слайдери на події зміни значення
        // Коли гравець тягне слайдер - викликаються наші методи
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void OnMusicSliderChanged(float value)
    {
        audioSystem.SetMusicVolume(value);
    }

    private void OnSFXSliderChanged(float value)
    {
        audioSystem.SetSFXVolume(value);
    }
}
