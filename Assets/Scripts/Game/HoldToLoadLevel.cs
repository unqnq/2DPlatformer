using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldToLoadLevel : MonoBehaviour
{
    public GameObject hintUI;
    public float holdTime = 2f; // Час, протягом якого потрібно утримувати кнопку
    public Image holdCircle; // UI елемент для відображення кола утримання

    [SerializeField] GameObject hintCircle;
    private float holdTimer = 0f; // Таймер для відстеження часу утримання
    private bool isHolding = false; // Прапорець для перевірки, чи утримується кнопка

    public static event Action OnHoldComplete;

    void Start()
    {
        hintUI = GameObject.Find("HintUI");
        if (hintUI != null)
        {
            hintUI.SetActive(false);
        }
    }
    void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().isLevelComplete)
        {
            if (hintUI != null)
            {
                hintUI.SetActive(true);
            }
            if (isHolding)
            {
                // Якщо кнопка утримується і рівень завершено, показуємо коло утримання
                if (hintUI != null)
                {
                    hintUI.SetActive(false);
                }
                if (holdCircle != null)
                {
                    holdCircle.gameObject.SetActive(true);
                }
                if (hintCircle != null)
                {
                    hintCircle.SetActive(true);
                }
                holdTimer += Time.deltaTime; // Збільшуємо таймер на час, що пройшов з останнього кадру

                // Оновлюємо заповнення кола утримання
                if (holdCircle != null)
                {
                    holdCircle.transform.localScale = new Vector2(holdTimer / holdTime, holdTimer / holdTime);
                }

                // Якщо час утримання перевищив поріг, завантажуємо рівень
                if (holdTimer >= holdTime)
                {
                    OnHoldComplete?.Invoke();
                    ResetHold();
                }
            }
        }

    }
    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true; // Починаємо утримання
        }
        else if (context.canceled)
        {
            ResetHold();
        }
    }
    void ResetHold()
    {
        isHolding = false; // Припиняємо утримання
        holdTimer = 0f; // Скидаємо таймер
        if (holdCircle != null)
        {
            holdCircle.transform.localScale = new Vector2(0, 0); // Скидаємо заповнення кола
        }
        hintCircle.SetActive(false);
    }
}
