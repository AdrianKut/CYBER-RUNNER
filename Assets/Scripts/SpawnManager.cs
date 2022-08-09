using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Money Bank")]
    [SerializeField]
    GameObject gameObjectMoneyBank;

    [Header("Mysterious Box")]
    [SerializeField]
    GameObject gameObjectMysteriousBox;

    [Header("Obstacles")]
    [SerializeField]
    GameObject[] gameObjectsObstaclesToSpawn;

    public float spawnObstacleDelay = 2.5f;

    [Header("Monsters")]
    [SerializeField]
    GameObject[] gameObjectsMonstersToSpawn;

    public float spawnMonsterDelay = 2.5f;

    private PowerUpManager powerUpManager;
    private void Awake()
    {
        StartingSpeedOfObjects();

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnObstacles());
        StartCoroutine(SpawnMoneyBank());
        StartCoroutine(SpawnMysteriousBox());

        powerUpManager = PowerUpManager.PowerUpManagerInstance;
        powerUpManager.OnSlowMotionActivated.AddListener(DecreaseSpeedObjects);
        powerUpManager.OnSlowMotionDeactivated.AddListener(IncreaseSpeedObjects);
    }

    private void StartingSpeedOfObjects()
    {
        gameObjectsObstaclesToSpawn.ToList().ForEach(x => x.GetComponent<Enemy>().SetSpeed(5f));

        gameObjectsMonstersToSpawn[0].GetComponent<Enemy>().SetSpeed(5f); // Enemy_1
        gameObjectsMonstersToSpawn[1].GetComponent<Enemy>().SetSpeed(3f); // Enemy_2
        gameObjectsMonstersToSpawn[2].GetComponent<Enemy>().SetSpeed(1f); // Enemy_5
        gameObjectsMonstersToSpawn[3].GetComponent<Enemy>().SetSpeed(1f); // Enemy_7
        gameObjectsMonstersToSpawn[4].GetComponent<Enemy>().SetSpeed(0.5f); // Enemy_6
        gameObjectsMonstersToSpawn[5].GetComponent<Enemy>().SetSpeed(4f); // Enemy_3
        gameObjectsMonstersToSpawn[6].GetComponent<Enemy>().SetSpeed(1.5f); // Enemy_4

    }


    private void DecreaseSpeedObjects()
    {
        //Objects on scene
        var objects = GameObject.FindObjectsOfType<Enemy>();
        foreach (var item in objects)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }

        //Prefab Obstacles
        foreach (var item in gameObjectsObstaclesToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }

        //Prefab Monsters
        foreach (var item in gameObjectsMonstersToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }

    }

    private void IncreaseSpeedObjects()
    {
        var objects = GameObject.FindObjectsOfType<Enemy>();
        foreach (var item in objects)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed * 2);
        }

        foreach (var item in gameObjectsObstaclesToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed * 2);
        }


        foreach (var item in gameObjectsMonstersToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed * 2);
        }
    }


    IEnumerator SpawnEnemies()
    {
        int index;
        while (true)
        {
            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            index = Random.Range(0, 2);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            index = Random.Range(2, 4);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            index = Random.Range(0, 2);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            index = Random.Range(2, gameObjectsMonstersToSpawn.Length);
            ObjectToSpawn(index);
            
            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s
            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            ObjectToSpawn(index);


            // == 12.5s
            foreach (var item in gameObjectsMonstersToSpawn)
            {
                var _enemy = item.GetComponent<Enemy>();
                var _speed = _enemy.GetSeed();
                _enemy.SetSpeed(_speed += 0.05f);
            }

            if (spawnMonsterDelay >= 1.5f)
                spawnMonsterDelay -= 0.025f;
        }
    }

    private void ObjectToSpawn(int index)
    {
        var _ = Instantiate(gameObjectsMonstersToSpawn[index], new Vector3(Random.Range(10.5f, 20.5f),
                        gameObjectsMonstersToSpawn[index].transform.position.y, 0f),
                        gameObjectsMonstersToSpawn[index].transform.rotation);
    }

    IEnumerator SpawnObstacles()
    {
        int cycleCounter = 0;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnObstacleDelay, 5f));

            int index = Random.Range(0, gameObjectsObstaclesToSpawn.Length);
            if (!GameManager.GameManagerInstance.isGameOver)
            {
                var gameObject = Instantiate(gameObjectsObstaclesToSpawn[index], new Vector3(Random.Range(10f, 16f),
                    gameObjectsObstaclesToSpawn[index].transform.position.y, 0f),
                    gameObjectsObstaclesToSpawn[index].transform.rotation);
            }

            if (cycleCounter % 5 == 0 && cycleCounter != 0)
            {
                foreach (var item in gameObjectsObstaclesToSpawn)
                {
                    var _enemy = item.GetComponent<Enemy>();
                    var _speed = _enemy.GetSeed();
                    _enemy.SetSpeed(_speed += 0.5f);
                }
            }
            cycleCounter++;
        }
    }

    IEnumerator SpawnMoneyBank()
    {
        float spawnDelay;
        while (true)
        {
            spawnDelay = Random.Range(30f, 60f);
            yield return new WaitForSeconds(spawnDelay);
            var randomPosX = Random.Range(10f, 20f);
            var randomPosY = Random.Range(8f, 9f);

            Instantiate(gameObjectMoneyBank, new Vector3(randomPosX,randomPosY,0f), Quaternion.identity);

        }
    }

    IEnumerator SpawnMysteriousBox()
    {
        float spawnDelay;
        while (true)
        {
            spawnDelay = Random.Range(60f, 90f);
            yield return new WaitForSeconds(spawnDelay);
            var randomPosX = Random.Range(10f, 20f);
            var randomPosY = Random.Range(8f, 9f);

            Instantiate(gameObjectMysteriousBox, new Vector3(randomPosX, randomPosY, 0f), Quaternion.identity);

        }
    }
}
