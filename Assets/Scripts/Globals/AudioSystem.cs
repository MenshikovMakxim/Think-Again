using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioSystem : MonoBehaviour
{
    [System.Serializable]
    public struct SoundEntry
    {
        public SoundType Type;
        public AudioClip Clip;
    }
    
    private const string MusicVolKey = "MusicVol";
    private const string SFXVolKey = "SFXVol";

    [Header("База звуків")]
    [SerializeField] private SoundEntry[] soundEntries;

    [Header("Джерела звуку (Audio Sources)")]
    [SerializeField] private AudioMixer mainMixer;
    
    [Tooltip("Джерело для музики (грає постійно)")]
    [SerializeField] private AudioSource musicSource;
    
    [Tooltip("Джерело для ефектів (короткі звуки)")]
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Налаштування варіації тону")]
    
    [Tooltip("Мінімальний тон (1 = стандартний)")]
    [SerializeField] private float minPitch = 0.9f;
    
    [Tooltip("Максимальний тон")]
    [SerializeField] private float maxPitch = 1.1f;

    private Dictionary<SoundType, AudioClip> _soundDatabase = new Dictionary<SoundType, AudioClip>();

    private void Start()
    {
        PlayMusic(musicSource.clip, true); 
        
        float savedMusicVol = PlayerPrefs.GetFloat(MusicVolKey, 1f);
        float savedSFXVol = PlayerPrefs.GetFloat(SFXVolKey, 1f);
        
        SetMusicVolume(savedMusicVol);
        SetSFXVolume(savedSFXVol);
    }

    private void Awake()
    {
        // Пакуємо масив у словник для швидкого пошуку
        foreach (var entry in soundEntries)
        {
            if (!_soundDatabase.ContainsKey(entry.Type) && entry.Clip != null)
            {
                _soundDatabase.Add(entry.Type, entry.Clip);
            }
        }
    }

    private void OnEnable()
    {
        EventBus.OnItemCrafted += HandleItemCrafted;
        EventBus.OnLevelFinished += HandleLevelFinished;
        EventBus.OnObjectClicked += HandleClick;
        EventBus.OnUIButtonClicked += HandleUIButtonClicked;
    }

    private void OnDisable()
    {
        EventBus.OnItemCrafted -= HandleItemCrafted;
        EventBus.OnLevelFinished -= HandleLevelFinished;
        EventBus.OnObjectClicked -= HandleClick;
        EventBus.OnUIButtonClicked -= HandleUIButtonClicked;
    }
    
    public void SetMusicVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat(MusicVolKey, sliderValue);
        
        float dbValue = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        mainMixer.SetFloat(MusicVolKey, dbValue); 
    }

    public void SetSFXVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat(SFXVolKey, sliderValue);
        
        float dbValue = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        mainMixer.SetFloat(SFXVolKey, dbValue);
    }
    
    public float GetMusicVolume()
    {
       return PlayerPrefs.GetFloat(MusicVolKey, 1f);
    }

    public float GetSFXVolume()
    {
       return PlayerPrefs.GetFloat(SFXVolKey, 1f);
    } 

    // --- ОБРОБНИКИ ІВЕНТІВ ---

    private void HandleItemCrafted(EventBus.ItemData data)
    {
        // Граємо звук успішного крафту!
        PlaySFX(SoundType.CraftSuccess);
    }

    private void HandleLevelFinished(EventBus.ItemData data)
    {
        if (data.IsWin)
        {
            PlaySFX(SoundType.WinFanfare);
        }
    }
    
    private void HandleClick()
    {
        PlaySFX(SoundType.Click);
    }
    
    private void HandleUIButtonClicked()
    {
        PlaySFX(SoundType.UIClick);
    }

    // --- ГОЛОВНІ МЕТОДИ ---

    public void PlaySFX(SoundType type, bool randomizePitch = true)
    {
        if (_soundDatabase.TryGetValue(type, out AudioClip clip))
        {
            sfxSource.pitch = randomizePitch ? Random.Range(minPitch, maxPitch) : 1f;

            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[AudioSystem] Звук {type} не знайдено!");
        }
    }
    
    public void PlayMusic(AudioClip track, bool loop = true)
    {
        if (track == null) return;
        
        if (musicSource == null || !musicSource.isActiveAndEnabled)
        {
            Debug.LogWarning("[AudioSystem] Спроба відтворити музику, але AudioSource вимкнений або відсутній!");
            return;
        }
        
        if (musicSource.isPlaying && musicSource.clip == track) return;

        musicSource.clip = track;
        musicSource.loop = loop;
        musicSource.Play(); 
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
