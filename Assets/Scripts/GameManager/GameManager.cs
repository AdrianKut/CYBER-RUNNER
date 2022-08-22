using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerGameObject;
    public void PlayerActivator(bool state) => playerGameObject.SetActive(state);
    private Vector3 startPosPlayer;
    public void SetStartPosPlayer() => playerGameObject.transform.position = startPosPlayer;
    public void HideLaser() => playerGameObject.transform.GetChild(1).GetComponent<LineRenderer>().enabled = false;
    
    [Space]
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private float money;
    public float GetMoney() => money;
    public void IncreaseMoney(int moneyAmount) => money += moneyAmount;
    public void BuyPowerUpTypeAndDecreaseMoney(PowerUpType powerUpType) => money -= (int)powerUpType;

    [Space]
    public bool isPaused = false;
    public bool isStarted;

    [Space]
    [SerializeField] private TextMeshProUGUI textPressAnyKeyToStart;
    [SerializeField] private UIManager uiManager;

    [Header("Game Over")]
    [SerializeField] private GameObject GameOverObject;
    public bool isGameOver = false;
    public float gameOverDelay = 1.5f;
    public UnityEvent OnGameOverEvent;

    public AudioSource audioSource;
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        startPosPlayer = playerGameObject.transform.position;
    }

    void Start()
    {
        Time.timeScale = 0f;
        isStarted = false;
    }

    private void Update()
    {
        if (Input.anyKey && isStarted == false)
        {
            isStarted = true;
            Time.timeScale = 1f;
            AudioListener.volume = 1f;
            AudioListener.pause = false;
            audioSource.Play();

            StartCoroutine("HideTextPressAnyKeyToStart");
            uiManager.ShowUI();
        }

        if (!isPaused && !isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                uiManager.Pause();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                uiManager.Resume();
        }
    }

    void FixedUpdate()
    {
        if (!isPaused && !isGameOver && isStarted)
        {
            IncreaseMoneyPerSeconds();

        }

        if (isGameOver)
        {
            gameOverDelay -= Time.deltaTime * 1f;
            if (gameOverDelay <= 0)
            {
                uiManager.HideUI();
                GameOverObject.SetActive(true);

                audioSource.Stop();
            }
        }
    }

    private IEnumerator HideTextPressAnyKeyToStart()
    {
        var gameObject = textPressAnyKeyToStart.gameObject;
        textPressAnyKeyToStart.alpha = 1f;

        LeanTweenExt.LeanAlphaText(textPressAnyKeyToStart, 0, 1.25f).setEaseOutSine();

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }



    private void IncreaseMoneyPerSeconds()
    {
        money += 0.02f;
        textMoney.SetText($"{money:F0} $");
    }

    public void GameOver()
    {
        isGameOver = true;
        OnGameOverEvent?.Invoke();

        AdsInitializer.Instance.ShowIntersitialAd();
    }
}
