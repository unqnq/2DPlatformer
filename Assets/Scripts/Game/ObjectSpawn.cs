using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public enum ObjectType { SmallMilk, BigMilk, Enemy }

    public GameObject[] objectPrefabs;
    public float bigMilkProbability = 0.2f; // 10% chance for big milk 
    public float enemyProbability = 0.1f; // 10% chance for enemy
    public float spawnInterval = 2f; // Time in seconds between spawns

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool isSpawning = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
