using UnityEngine;

public class BonusEnemy : Enemy
{
    [SerializeField] private GameObject GameObjectBigTextPrefab;
    void Start()
    {
        Bouncing();
    }

    private void Bouncing()
    {
        LeanTween.moveY(this.gameObject, 6f, 0.8f).setEaseInOutQuad().setLoopPingPong();
    }

    private void OnMouseDown()
    {
        DestroyOnClick();
    }

    public void DestroyOnClick()
    {
        TakeDamage();

        if (GetEnemyType() == EnemyType.BonusEffect)
        {
            BonusEnemyManager.Instance.DisplayAndActivateMysteriousEffect();
        }
        else if (GetEnemyType() == EnemyType.BonusMoney)
        {
            int[] moneys = { 5, 5, 5, 10, 10, 15, 15, 20, 25, 50 };
            int randomMoneyAmount = Random.Range(0, moneys.Length);

            GameManager.Instance.IncreaseMoney(moneys[randomMoneyAmount]);

            var gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
            gameObject.DisplayText($"+{moneys[randomMoneyAmount]} $", Color.green, ClipsType.money);
        }
    }
}
