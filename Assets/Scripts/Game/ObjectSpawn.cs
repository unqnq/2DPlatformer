using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawn : MonoBehaviour
{
    // Перерахування можливих типів об'єктів
    public enum ObjectType { SmallMilk, BigMilk, Enemy }

    public Tilemap tilemap; // Tilemap, по якій будуть визначатись позиції для спавну
    public GameObject[] objectPrefabs; // Префаби об'єктів (індекси відповідають enum ObjectType)
    public float bigMilkProbability = 0.2f; // Ймовірність спавну великого молока
    public float enemyProbability = 0.1f; // Ймовірність спавну ворога
    public int maxObjectsToSpawn = 10; // Максимальна кількість одночасних об'єктів на сцені
    public float spawnInterval = 2f; // Інтервал між спавнами
    public float lifeTime = 10f; // Час життя об'єктів, крім ворогів

    private List<Vector3> validSpawnPositions = new List<Vector3>(); // Позиції, де дозволено спавнити
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Список заспавнених об'єктів
    private bool isSpawning = false; // Чи відбувається зараз спавн

    void Start()
    {
        GatherValidSpawnPositions(); // Збираємо всі допустимі позиції зі сцени
        StartCoroutine(SpawnObjectsIfNeeded()); // Запускаємо процес спавну
    }

    void Update()
    {
        // Якщо не спавниться і кількість об'єктів менша за максимум — запускаємо спавн
        if (!isSpawning && ActiveObjectCount() < maxObjectsToSpawn)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }

    // Повертає кількість активних об'єктів, фільтруючи знищені
    int ActiveObjectCount()
    {
        spawnedObjects.RemoveAll(item => item == null);
        return spawnedObjects.Count;
    }

    // Корутина, що спавнить об'єкти з інтервалами
    IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;

        while (ActiveObjectCount() < maxObjectsToSpawn)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    // Перевіряє, чи вже є об'єкт поблизу заданої позиції
    bool PositionHasObject(Vector3 positionToCheck)
    {
        return spawnedObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1f);
    }

    // Випадково обирає тип об'єкта для спавну
    ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;

        if (randomChoice < enemyProbability)
        {
            return ObjectType.Enemy;
        }
        else if (randomChoice <= (enemyProbability + bigMilkProbability))
        {
            return ObjectType.BigMilk;
        }
        else
        {
            return ObjectType.SmallMilk;
        }
    }

    // Основна логіка спавну одного об'єкта
    void SpawnObject()
    {
        if (validSpawnPositions.Count == 0) return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        // Шукаємо підходящу позицію, де поруч нічого немає
        while (!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[randomIndex];
            Vector3 leftPosition = potentialPosition + Vector3.left;
            Vector3 rightPosition = potentialPosition + Vector3.right;

            // Перевіряємо, щоб зліва і справа не було інших об'єктів
            if (!PositionHasObject(leftPosition) && !PositionHasObject(rightPosition))
            {
                spawnPosition = potentialPosition;
                validPositionFound = true;
            }

            // Видаляємо перевірену позицію зі списку
            validSpawnPositions.RemoveAt(randomIndex);
        }

        if (validPositionFound)
        {
            // Вибираємо тип об'єкта та спавнимо
            ObjectType objectType = RandomObjectType();
            GameObject gameObject = Instantiate(objectPrefabs[(int)objectType], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(gameObject);

            // Якщо це не ворог — знищуємо через деякий час
            if (objectType != ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(gameObject, lifeTime)); 
            }
        }
    }

    // Корутина для автоматичного знищення об'єкта після певного часу
    IEnumerator DestroyObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
        {
            spawnedObjects.Remove(gameObject); // Прибираємо з списку заспавнених об'єктів
            validSpawnPositions.Add(gameObject.transform.position); // Додаємо позицію в список допустимих
            Destroy(gameObject); // Знищуємо об'єкт
        }
    }

    // Збір всіх позицій, де є плитки, і можна розмістити об'єкти над ними
    void GatherValidSpawnPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt boundsInt = tilemap.cellBounds; // Межі тайлмапи
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt); // Всі плитки
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];
                if (tile != null)
                {
                    // Розміщує об'єкт трохи вище над тайлом (на 2 одиниці вгору, по центру клітинки)
                    Vector3 place = start + new Vector3(x + 0.5f, y + 2f, 0);
                    validSpawnPositions.Add(place);
                }
            }
        }
    }
}
