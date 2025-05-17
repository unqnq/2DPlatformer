using UnityEngine;

public class Food : MonoBehaviour, IItem
{
    public void Collect()
    {
        Destroy(gameObject);
    }
}
