using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PowerUpType
{
    Shield = 25,
    SlowMotion = 50,
    Laser = 75,
    SuperAmmo = 100,
    Nuke = 125
}

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject GameObjectParentPowerUpDurationIcons;
    [SerializeField] private GameObject GameObjectChildPowerUpDurationIcons;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource audioSource;

    [Header("Power Ups")]
    public PowerUp Shield;
    public PowerUp SlowMotion;
    public PowerUp Nuke;
    public PowerUp SuperAmmo;
    public PowerUp Laser;

    public bool isShieldActivated;
    public bool isSuperAmmoActivated;
    public static PowerUpManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        gameManager.OnGameOverEvent.AddListener(HidePowerUpsUI);
    }

    private void HidePowerUpsUI()
    {
        if (gameManager.isGameOver || gameManager.isPaused)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, Sprite powerUpSprite, float powerUpDuration)
    {
        powerUpIcon = Instantiate(GameObjectChildPowerUpDurationIcons, GameObjectChildPowerUpDurationIcons.transform.position, Quaternion.identity);
        powerUpIcon.transform.SetParent(GameObjectParentPowerUpDurationIcons.transform);
        powerUpIcon.GetComponent<Image>().sprite = powerUpSprite;

        textPowerUpDuration = powerUpIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textPowerUpDuration.SetText("" + powerUpDuration);
    }
}
