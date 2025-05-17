using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int proggressAmount = 0;
    public Slider progressSlider;
    public int maxProgress = 10;
    void Start()
    {
        Food.OnFoodCollected += AddProgress;
        progressSlider.value = 0;
        progressSlider.maxValue = maxProgress;
    }

    void AddProgress(int foodValue)
    {
        proggressAmount += foodValue;
        progressSlider.value = proggressAmount;
        if (proggressAmount >= maxProgress)
        {
            Debug.Log("You have collected enough food!");
        }
    }
}
