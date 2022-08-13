using UnityEngine;

public enum EnemyType
{
    Monster,
    Obstacle,
    Bonus
}

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float moveSpeed = 10f;

    public void SetSpeed(float speed) => moveSpeed = speed;
    public float GetSeed() => moveSpeed;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private int moneyAmount;

    private AudioSource audioSource;

    [SerializeField]
    private EnemyType enemyType;

    public EnemyType GetEnemyType() => enemyType;

    [System.Obsolete]
    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        if (this.enemyType == EnemyType.Bonus)
        {
            LeanTween.moveY(this.gameObject, 6f, 0.8f).setEaseInOutQuad().setLoopPingPong();
        }
    }

    private void Update()
    {
        MoveToLeftBound();
    }

    private void MoveToLeftBound()
    {
        Vector3 newPos = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, 4, 0);
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

        if (newPos.x <= -20f)
            Destroy(this.gameObject);
    }

    public void TakeDamage(int damage = 0)
    {
        health -= damage;
        if (health <= 0)
            Die("bullet");
    }

    public void Detonate()
    {
        Die("nuke");
    }

    private void Die(string typeOfDeath)
    {
        switch (typeOfDeath)
        {
            case "shield":
            case "bullet":
            case "nuke":
                GameManager.GameManagerInstance.IncreaseMoney(moneyAmount);
                Destroy(this.gameObject.GetComponent<AutoAIMSelector>()); 
                goto destroy;

            case "player":
                GameManager.GameManagerInstance.GameOver();
            destroy:
                audioSource.Play();

                Destroy(transform.GetComponent<SpriteRenderer>());
                Destroy(gameObject.GetComponent<Collider2D>());

                for (int i = 0; i < transform.childCount; i++)
                    Destroy(transform.GetChild(i).gameObject);

                Instantiate(deathEffect, transform.position, Quaternion.identity);
                AutoAIM.Instance.ReleaseAutoAIM(this.gameObject);
                Destroy(gameObject, 2f);
                break;

            case "obstacle":
                GameManager.GameManagerInstance.GameOver();
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.gameObject.name.Contains("MoneyBank") == false && this.gameObject.name.Contains("MysteriousBox") == false)
        {
            if (other.gameObject.CompareTag("Player") && PowerUpManager.ShieldActivated() && enemyType == EnemyType.Monster)
                Die("shield");

            else if (other.gameObject.CompareTag("Player") && PowerUpManager.ShieldActivated() && enemyType == EnemyType.Obstacle)
                Die("shield");

            else if (other.gameObject.CompareTag("Player") && enemyType == EnemyType.Monster)
                Die("player");

            else if (other.gameObject.CompareTag("Player") && enemyType == EnemyType.Obstacle)
                Die("obstacle");
        }
    }

    private void OnMouseDown()
    {
        DestroyOnClick();
    }

    [SerializeField]
    GameObject GameObjectBigTextPrefab;
    public void DestroyOnClick()
    {
        TakeDamage();

        if (this.gameObject.name.Contains("MysteriousBox"))
        {
            GameManager.GameManagerInstance.OnDestroyMysteriousBox?.Invoke();
        }
        else if (this.gameObject.name.Contains("MoneyBank"))
        {
            int[] moneys = { 5, 5, 5, 10, 10, 15, 15, 20, 25, 50 };
            int randomMoneyAmount = Random.Range(0, moneys.Length);

            GameManager.GameManagerInstance.money += moneys[randomMoneyAmount];

            var gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
            gameObject.DisplayText($"+{moneys[randomMoneyAmount]} $", Color.green, ClipsType.money);
        }
    }
}