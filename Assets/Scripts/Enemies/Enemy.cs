using UnityEngine;

public enum EnemyType
{
    Monster,
    Obstacle,
    BonusEffect,
    BonusMoney
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float moveSpeed = 10f;
    public void SetSpeed(float speed) => moveSpeed = speed;
    public float GetSeed() => moveSpeed;

    [SerializeField] private GameObject deathEffect;
    [SerializeField] private int moneyAmount;

    [SerializeField] private EnemyType enemyType;
    public EnemyType GetEnemyType() => enemyType;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MoveToLeftBound();
    }

    protected void MoveToLeftBound()
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
                GameManager.Instance.IncreaseMoney(moneyAmount);
                Destroy(this.gameObject.GetComponent<AutoAIMSelector>());
                goto destroy;

            case "player":
                GameManager.Instance.GameOver();
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
                GameManager.Instance.GameOver();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && PowerUpManager.Instance.isShieldActivated && enemyType == EnemyType.Monster)
            Die("shield");

        else if (other.gameObject.CompareTag("Player") && PowerUpManager.Instance.isShieldActivated && enemyType == EnemyType.Obstacle)
            Die("shield");

        else if (other.gameObject.CompareTag("Player") && enemyType == EnemyType.Monster)
            Die("player");

        else if (other.gameObject.CompareTag("Player") && enemyType == EnemyType.Obstacle)
            Die("obstacle");
    }
}