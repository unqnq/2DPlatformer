using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int proggressAmount = 0;
    public Slider progressSlider;
    public int maxProgress;
    public bool isLevelComplete = false;
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
        maxProgress = GameObject.Find("GameController").GetComponent<ObjectSpawn>().maxObjectsToSpawn;
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
