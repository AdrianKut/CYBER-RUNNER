using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [Header("Money Bank")]
    [SerializeField] private GameObject gameObjectMoneyBank;

    [Header("Mysterious Box")]
    [SerializeField] private GameObject gameObjectMysteriousBox;

    [Header("Obstacles")]
    [SerializeField] private GameObject[] gameObjectsObstacles;

    [Header("Monsters")]
    [SerializeField] private GameObject[] gameObjectsMonsters;

    [SerializeField] private float spawnObstacleDelay = 10f;
    [SerializeField] private float spawnMonsterDelay = 2.5f;

    [SerializeField] private PowerUp slowMotionPowerUp;
    private void Awake()
    {
        StartingSpeedOfObjects();

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnObstacles());
        SpawnMoneyBank();
        SpawnMysteriousBox();

  
        slowMotionPowerUp.OnSlowMotionActivated.AddListener(DecreaseSpeedObjects);
        slowMotionPowerUp.OnSlowMotionDeactivated.AddListener(IncreaseSpeedObjects);
        GameManager.Instance.OnGameOverEvent.AddListener(StopCoroutines);
    }

    private void StopCoroutines()
    {
        StopAllCoroutines();
    }

    private void StartingSpeedOfObjects()
    {
        gameObjectsObstacles.ToList().ForEach(x => x.GetComponent<Enemy>().SetSpeed(5f));

        gameObjectsMonsters[0].GetComponent<Enemy>().SetSpeed(5f); // Enemy_1
        gameObjectsMonsters[1].GetComponent<Enemy>().SetSpeed(3f); // Enemy_2
        gameObjectsMonsters[2].GetComponent<Enemy>().SetSpeed(1f); // Enemy_5
        gameObjectsMonsters[3].GetComponent<Enemy>().SetSpeed(1f); // Enemy_7
        gameObjectsMonsters[4].GetComponent<Enemy>().SetSpeed(0.5f); // Enemy_6
        gameObjectsMonsters[5].GetComponent<Enemy>().SetSpeed(4f); // Enemy_3
        gameObjectsMonsters[6].GetComponent<Enemy>().SetSpeed(1.5f); // Enemy_4
    }


    private void DecreaseSpeedObjects()
    {
        //Current objects on scene
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
        SetSpeedOfGameObjects(objects, '/', 2);

        //Prefabs for new objects
        SetSpeedOfGameObjects(gameObjectsObstacles, '/', 2);
        SetSpeedOfGameObjects(gameObjectsMonsters, '/', 2);
    }

    private void IncreaseSpeedObjects()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
        SetSpeedOfGameObjects(objects, '*', 2);

        //Prefabs for new objects
        SetSpeedOfGameObjects(gameObjectsObstacles, '*', 2);
        SetSpeedOfGameObjects(gameObjectsMonsters, '*', 2);
    }

    private void SetSpeedOfGameObjects(GameObject[] objects, char op, float speedValue)
    {
        foreach (var item in objects)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();

            switch (op)
            {
                case '*':
                    _enemy.SetSpeed(_speed * speedValue);
                    break;

                case '\\':
                    _enemy.SetSpeed(_speed / speedValue);
                    break;

                case '+':
                    _enemy.SetSpeed(_speed + speedValue);
                    break;
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            EnemiesToSpawn(gameObjectsMonsters, 0, 2, 13.5f, 23.5f);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            EnemiesToSpawn(gameObjectsMonsters, 2, 4, 13.5f, 23.5f);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            EnemiesToSpawn(gameObjectsMonsters, 0, 2, 13.5f, 23.5f);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            EnemiesToSpawn(gameObjectsMonsters, 2, gameObjectsMonsters.Length, 13.5f, 23.5f);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            EnemiesToSpawn(gameObjectsMonsters, 0, gameObjectsMonsters.Length, 13.5f, 23.5f);

            // == 12.5s
            SetSpeedOfGameObjects(gameObjectsMonsters,'+', 0.05f);

            if (spawnMonsterDelay >= 1.5f)
                spawnMonsterDelay -= 0.025f;
        }
    }

    private void EnemiesToSpawn(GameObject[] gameObjectEnemiesToSpawn, int min, int max, float minPos, float maxPos)
    {
        int index = Random.Range(min, max);
        var _ = Instantiate(gameObjectEnemiesToSpawn[index], new Vector3(Random.Range(minPos, maxPos),
                        gameObjectEnemiesToSpawn[index].transform.position.y, 0f),
                        gameObjectEnemiesToSpawn[index].transform.rotation);
    }

    IEnumerator SpawnObstacles()
    {
        int cycleCounter = 0;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnObstacleDelay, spawnObstacleDelay * 2));

            if (!GameManager.Instance.isGameOver)
            {
                EnemiesToSpawn(gameObjectsObstacles, 0, gameObjectsObstacles.Length, 13f, 19f);
            }

            if (cycleCounter % 5 == 0 && cycleCounter != 0)
            {
                SetSpeedOfGameObjects(gameObjectsObstacles, '+', 0.05f);
            }
            cycleCounter++;
        }
    }

    private void SpawnMoneyBank()
    {
        StartCoroutine(BonusToSpawn(gameObjectMoneyBank, 30f, 60f));
    }

    private void SpawnMysteriousBox()
    {
        StartCoroutine(BonusToSpawn(gameObjectMysteriousBox, 60f, 90f));
    }

    private IEnumerator BonusToSpawn(GameObject gameObjectToSpawn, float minDelay, float maxDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            var randomPosX = Random.Range(10f, 20f);
            var randomPosY = Random.Range(8f, 9f);

            Instantiate(gameObjectToSpawn, new Vector3(randomPosX, randomPosY, 0f), Quaternion.identity);
        }
    }
}
