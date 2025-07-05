using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
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
        if (musicSlider == null)
        {
            musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
        }
        if (musicSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            audioSource.volume = savedVolume;
            musicSlider.value = savedVolume;
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
        }
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            Instance.audioSource.clip = audioClip;
        }
        if (Instance.audioSource.clip != null)
        {
            if (resetSong)
            {
                Instance.audioSource.Stop();
            }
            Instance.audioSource.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        Instance.audioSource.Pause();
    }
}
