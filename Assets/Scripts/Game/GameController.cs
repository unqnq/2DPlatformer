using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int proggressAmount = 0;
    public Slider progressSlider;
    public int maxProgress = 10;
    public bool isLevelComplete = false;
    void Start()
    {
        Food.OnFoodCollected += AddProgress;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        progressSlider.value = 0;
        progressSlider.maxValue = maxProgress;
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
        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex+1)
        {
            SceneManager.LoadScene("Menu");

        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
