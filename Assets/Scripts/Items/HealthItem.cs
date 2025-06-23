using System;
using System.Data;
using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    public int healthAmount = 1;
    public static event Action<int> OnHealCollect;
    public void Collect()
    {
        OnHealCollect.Invoke(healthAmount);
        Destroy(gameObject);
    }
}
