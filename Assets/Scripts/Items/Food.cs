using System;
using UnityEngine;

public class Food : MonoBehaviour, IItem
{
    public static event Action<int> OnFoodCollected;
    public int foodValue = 1;
    
    public float amplitude = 1f; // амплітуда
    public float frequency = 1f; // частота
    public void Collect()
    {
        OnFoodCollected?.Invoke(foodValue);
        //Знак питання ?. означає, що подія буде викликана лише якщо на неї хтось підписаний (щоб уникнути помилки, якщо підписників немає).
        Destroy(gameObject);
    }


    void FixedUpdate()
    {
        float y = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = new Vector3(transform.position.x, transform.position.y+y, transform.position.z);
    }
}
