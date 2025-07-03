using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    public int proggressAmount = 0;
    public Slider progressSlider;
    public int maxProgress;
    public bool isLevelComplete = false;
    public GameObject gameOverPanel;
    public TMP_Text itemsText, itemsText_Border;
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
            SceneManager.LoadScene("Menu");

        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void GameOverPanel()
    {
        gameOverPanel.SetActive(true);
        itemsText.text = "You have collected " + proggressAmount + "/" + maxProgress;
        itemsText_Border.text = "You have collected " + proggressAmount + "/" + maxProgress;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    void OnDestroy()
    {
        Food.OnFoodCollected -= AddProgress;
        HoldToLoadLevel.OnHoldComplete -= LoadNextLevel;
        PlayerHearts.OnPlayerDied -= GameOverPanel;
    }
}
