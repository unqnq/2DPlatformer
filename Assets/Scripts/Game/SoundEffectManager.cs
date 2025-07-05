using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectsLibrary soundEffectsLibrary;
    [SerializeField] private Slider sfxSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectsLibrary = GetComponent<SoundEffectsLibrary>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndAssignSlider();
    }

    void FindAndAssignSlider()
    {
        if (sfxSlider == null)
        {
            sfxSlider = GameObject.Find("SfxSlider")?.GetComponent<Slider>();
        }
        if (sfxSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("SfxVolume", 0.2f);
            audioSource.volume = savedVolume;
            sfxSlider.value = savedVolume;
            sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
            //  .onValueChanged очікує функцію з параметром float
            // використовуємо delegate { ... }, щоб проігнорувати значення й викликати метод без параметрів
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectsLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
