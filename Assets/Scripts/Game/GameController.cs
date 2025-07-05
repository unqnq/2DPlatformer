using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public int proggressAmount = 0;
    public Slider progressSlider;
    public int maxProgress;
    public bool isLevelComplete = false;
    public GameObject gameOverPanel;
    public TMP_Text itemsText, itemsText_Border;
    public GameObject optionsPanel;
    public GameObject optionsButton;
    public GameObject player;
    void Start()
    {
        // GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        // foreach (GameObject food in foods)
        // {
        //     Food foodScript = food.GetComponent<Food>();
        //     if (foodScript != null)
        //     {
        //         maxProgress += foodScript.foodValue;
        //     }
        // }
        optionsPanel = GameObject.Find("OptionsPanel");
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        optionsButton = GameObject.Find("OptionsButton");
        player = GameObject.Find("Player");
        maxProgress = GameObject.Find("ObjectSpawn").GetComponent<ObjectSpawn>().maxObjectsToSpawn;
        Food.OnFoodCollected += AddProgress;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        progressSlider.value = 0;
        progressSlider.maxValue = maxProgress;
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);
        PlayerHearts.OnPlayerDied += GameOverPanel;
    }

    void AddProgress(int foodValue)
    {
        proggressAmount += foodValue;
        progressSlider.value = proggressAmount;
        if (proggressAmount >= maxProgress)
        {
            isLevelComplete = true;
        }
    }

    void LoadNextLevel()
    {
        //  Debug.Log(SceneManager.sceneCountInBuildSettings + " / " + SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void GameOverPanel()
    {
        MusicManager.PauseBackgroundMusic();
        gameOverPanel.SetActive(true);
        itemsText.text = "You have collected " + proggressAmount + "/" + maxProgress;
        itemsText_Border.text = "You have collected " + proggressAmount + "/" + maxProgress;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        MusicManager.PlayBackgroundMusic(false);
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void OpenOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            optionsButton.SetActive(false);
            Time.timeScale = 0;
            if (player != null)
            {
                player.GetComponent<PlayerShoot>().canShoot = false;
            }
        }
    }

    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            optionsButton.SetActive(true);
            Time.timeScale = 1;
            if (player != null)
            {
                player.GetComponent<PlayerShoot>().canShoot = true;
            }
        }
    }

    void OnDestroy()
    {
        Food.OnFoodCollected -= AddProgress;
        HoldToLoadLevel.OnHoldComplete -= LoadNextLevel;
        PlayerHearts.OnPlayerDied -= GameOverPanel;
    }
}
