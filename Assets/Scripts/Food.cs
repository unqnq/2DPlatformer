using System;
using UnityEngine;

public class Food : MonoBehaviour, IItem
{
    public static event Action<int> OnFoodCollected;
    public int foodValue = 1;
    public void Collect()
    {
        OnFoodCollected?.Invoke(foodValue);
        //Знак питання ?. означає, що подія буде викликана лише якщо на неї хтось підписаний (щоб уникнути помилки, якщо підписників немає).
        Destroy(gameObject);
    }
}
