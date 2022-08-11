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
    [SerializeField]
    GameObject GameObjectParentPowerUpDurationIcons;

    [SerializeField]
    GameObject GameObjectChildPowerUpDurationIcons;

    [SerializeField]
    Sprite shieldSprite;

    [SerializeField]
    Sprite laserSprite;

    [SerializeField]
    Sprite superAmmoSprite;

    [SerializeField]
    Sprite highSpeedSprite;

    [SerializeField]
    Sprite nukeSprite;

    [SerializeField]
    Sprite badEffectSprite;

    [SerializeField] private GameObject player;

    private LoopBackground loopBackground;
    private GameManager gameManager;
    private AudioSource audioSource;
    

    public static PowerUpManager PowerUpManagerInstance;

    private void Awake()
    {
        if (PowerUpManagerInstance == null)
            PowerUpManagerInstance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        loopBackground = GameObject.FindGameObjectWithTag("Background").GetComponent<LoopBackground>();

        gameManager = GameManager.GameManagerInstance;
        gameManager.OnGameOverEvent.AddListener(HidePowerUpsUI);

        PowerUpInitializaion();
        SetDurationPowerUpsAtStart();
    }

    private static void PowerUpInitializaion()
    {
        isShieldActivated = false;
        isSlowMotionActivated = false;
        isSuperAmmoActivated = false;
        isLaserActivated = false;
    }
    
    private void SetDurationPowerUpsAtStart()
    {
        shieldDurationAtStart = shieldDuration;
        slowMotionDurationAtStart = slowMotionDuration;
        superAmmoDurationAtStart = superAmmoDuration;
        laserDurationAtStart = laserDuration;
    }

    private void Update()
    {
        ActivationPowerUpsByKeys();
    }

    private void ActivationPowerUpsByKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && buttonShield.interactable)
            BuyShield();

        if (Input.GetKeyDown(KeyCode.Alpha2) && buttonSlowMotion.interactable)
            BuySlowMotion();

        if (Input.GetKeyDown(KeyCode.Alpha3) && buttonLaser.interactable)
            BuyLaser();

        if (Input.GetKeyDown(KeyCode.Alpha4) && buttonSuperAmmo.interactable)
            BuySuperAmmo();

        if (Input.GetKeyDown(KeyCode.Alpha5) && buttonNuke.interactable)
            BuyNuke();
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

    private IEnumerator ChangeColorOfButtonToRed(Button thisButton)
    {
        thisButton.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        thisButton.GetComponent<Image>().color = Color.white;
    }

    private void ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, Sprite powerUpSprite, float powerUpDuration)
    {
        powerUpIcon = Instantiate(GameObjectChildPowerUpDurationIcons, GameObjectChildPowerUpDurationIcons.transform.position, Quaternion.identity);
        powerUpIcon.transform.SetParent(GameObjectParentPowerUpDurationIcons.transform);
        powerUpIcon.GetComponent<Image>().sprite = powerUpSprite;

        textPowerUpDuration = powerUpIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textPowerUpDuration.SetText("" + powerUpDuration);
    }

    #region Shield
    [Header("Shield")]
    public GameObject GameObjectShield;

    [SerializeField]
    private Button buttonShield;

    [SerializeField]
    private short shieldDuration;
    private short shieldDurationAtStart;

    [SerializeField]
    private float timeToRenewShield;

    private static bool isShieldActivated = false;
    public static bool ShieldActivated() => isShieldActivated == true ? true : false;

    public void ActivateShieldForFree()
    {
        if (isShieldActivated == true)
            shieldDuration = shieldDurationAtStart;
        else
            StartCoroutine(EnableShield());
    }

    public void BuyShield()
    {
        if (gameManager.money >= (int)PowerUpType.Shield)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.Shield);
            StartCoroutine(EnableShield());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonShield));
    }

    private IEnumerator EnableShield()
    {
        isShieldActivated = true;

        shieldDuration = shieldDurationAtStart;

        var shield = Instantiate(GameObjectShield, GameObjectShield.transform.position, Quaternion.identity);
        shield.transform.SetParent(player.transform);
        shield.transform.localPosition = new Vector3(0f, -0.15f, 0f);

        LeanTween.scale(shield, new Vector3(0.3f, 0.3f, 0.3f), 7f).setEasePunch();
        buttonShield.interactable = false;

        LeanTween.rotateAround(shield, Vector3.forward, 360, 1.75f).setLoopClamp();

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, shieldSprite, shieldDuration);

        for (; shieldDuration > 0; shieldDuration--)
        {
            textPowerUpDuration.SetText("" + shieldDuration);
            LeanTween.alpha(shield, 0.15f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.alpha(shield, 1.0f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(1f);
        }

        LeanTween.scale(shield, new Vector3(0f, 0f, 0f), 7f).setEasePunch().setLoopPingPong();
        LeanTween.alpha(shield, 0f, 0.8f).setEase(LeanTweenType.easeOutQuad);

        Destroy(shield, 1f);
        Destroy(powerUpIcon);

        isShieldActivated = false;
        yield return new WaitForSeconds(timeToRenewShield);
        buttonShield.interactable = true;
    }


    #endregion

    #region SuperAmmo
    [Header("SuperAmmo")]

    [SerializeField]
    private Button buttonSuperAmmo;

    [SerializeField]
    private short superAmmoDuration;
    private short superAmmoDurationAtStart;

    [SerializeField]
    private float timeToRenewSuperAmmo;

    private static bool isSuperAmmoActivated = false;
    public static bool SuperAmmoActivated() => isSuperAmmoActivated == true ? true : false;

    public void ActivateSuperAmmoForFree()
    {
        if (isSuperAmmoActivated == true)
            superAmmoDuration = superAmmoDurationAtStart;
        else
            StartCoroutine(EnableSuperAmmo());
    }

    public void BuySuperAmmo()
    {
        if (gameManager.money >= (int)PowerUpType.SuperAmmo)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.SuperAmmo);
            StartCoroutine(EnableSuperAmmo());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonSuperAmmo));
    }

    private IEnumerator EnableSuperAmmo()
    {
        isSuperAmmoActivated = true;

        superAmmoDuration = superAmmoDurationAtStart;

        var rayCastWeapon = player.GetComponent<RayCastWeapon>();

        buttonSuperAmmo.GetComponent<Button>().interactable = false;

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, superAmmoSprite, superAmmoDuration);

        for (; superAmmoDuration > 0; superAmmoDuration--)
        {
            textPowerUpDuration.SetText("" + superAmmoDuration);
            yield return new WaitForSeconds(1f);
        }

        if (rayCastWeapon.GetNameOfCurrentWeapon() == "laser")
            rayCastWeapon.UseLaser();
        else if (rayCastWeapon.GetNameOfCurrentWeapon() == "rifle")
            rayCastWeapon.UseRifle();

        Destroy(powerUpIcon);

        isSuperAmmoActivated = false;
        yield return new WaitForSeconds(timeToRenewSuperAmmo);
        buttonSuperAmmo.interactable = true;

    }
    #endregion

    #region SlowMo

    [Header("Slow Motion")]
    public UnityEvent OnSlowMotionActivated;
    public UnityEvent OnSlowMotionDeactivated;

    [SerializeField]
    private Button buttonSlowMotion;

    [SerializeField]
    private float timeToRenewHighSpeed;

    [SerializeField]
    public short slowMotionDuration;
    public short slowMotionDurationAtStart;

    private static bool isSlowMotionActivated = false;
    public static bool SlowMotionActivated() => isSlowMotionActivated == true ? true : false;

    public void ActivateSlowMoForFree()
    {
        if (isSlowMotionActivated == true)
            slowMotionDuration = slowMotionDurationAtStart;
        else
            StartCoroutine(EnableSlowMotion());
    }
    public void BuySlowMotion()
    {
        if (gameManager.money >= (int)PowerUpType.SlowMotion)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.SlowMotion);
            StartCoroutine(EnableSlowMotion());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonSlowMotion));
    }

    private IEnumerator EnableSlowMotion()
    {
        isSlowMotionActivated = true;

        slowMotionDuration = slowMotionDurationAtStart;

        OnSlowMotionActivated?.Invoke();
        var currentSpeedBackground = loopBackground.speed;

        buttonSlowMotion.interactable = false;
        loopBackground.speed = (currentSpeedBackground / 2);

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, highSpeedSprite, slowMotionDuration);

        for (; slowMotionDuration > 0; slowMotionDuration--)
        {
            textPowerUpDuration.SetText("" + slowMotionDuration);
            yield return new WaitForSeconds(1f);
        }

        loopBackground.speed = currentSpeedBackground + 0.5f;

        isSlowMotionActivated = false;
        OnSlowMotionDeactivated?.Invoke();

        Destroy(powerUpIcon);
        yield return new WaitForSeconds(timeToRenewHighSpeed);
        buttonSlowMotion.interactable = true;
    }
    #endregion

    #region Laser
    [Header("Laser")]

    [SerializeField]
    private Button buttonLaser;

    [SerializeField]
    private short laserDuration;
    private short laserDurationAtStart;

    [SerializeField]
    private float timeToRenewLaser;

    private static bool isLaserActivated = false;
    public static bool LaserActivated() => isLaserActivated == true ? true : false;

    public void ActivateLaserForFree()
    {
        if (isLaserActivated == true)
        {
            laserDuration = laserDurationAtStart;
        }
        else
            StartCoroutine(EnableLaser());
    }
    public void BuyLaser()
    {
        if (gameManager.money >= (int)PowerUpType.Laser)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.Laser);

            var rayCastWeapon = player.GetComponent<RayCastWeapon>();
            rayCastWeapon.UseLaser();

            StartCoroutine(EnableLaser());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonLaser));
    }

    private IEnumerator EnableLaser()
    {
        var rayCastWeapon = player.GetComponent<RayCastWeapon>();
        rayCastWeapon.UseLaser();

        isLaserActivated = true;
        buttonLaser.interactable = false;

        laserDuration = laserDurationAtStart;

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, laserSprite, laserDuration);


        if (isSuperAmmoActivated)
        {
            for (; laserDuration > 0; laserDuration--)
            {
                textPowerUpDuration.SetText("" + laserDuration);
                yield return new WaitForSeconds(1f);
            }

        }
        else
        {
            for (; laserDuration > 0; laserDuration--)
            {
                textPowerUpDuration.SetText("" + laserDuration);
                yield return new WaitForSeconds(1f);
            }
        }

        Destroy(powerUpIcon);

        if (isSuperAmmoActivated)
        {
            rayCastWeapon.UseRifle();
        }
        else
            rayCastWeapon.UseRifle();

        yield return new WaitForSeconds(timeToRenewLaser);
        isLaserActivated = false;
        buttonLaser.interactable = true;
    }
    #endregion

    #region Nuke
    [Header("Nuke")]

    [SerializeField]
    private Button buttonNuke;

    [SerializeField]
    private float nukeDuration;

    [SerializeField]
    private float timeToRenewNuke;

    [SerializeField]
    GameObject gameObjectNuke;


    public void ActivateNukeForFree() => StartCoroutine(EnableNuke());
    public void BuyNuke()
    {
        if (gameManager.money >= (int)PowerUpType.Nuke)
        {
            audioSource.Play();
            gameManager.BuyPowerUpTypeAndDecreaseMoney(PowerUpType.Nuke);
            StartCoroutine(EnableNuke());
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonNuke));
    }

    private IEnumerator EnableNuke()
    {
        buttonNuke.interactable = false;

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, nukeSprite, 5);


        for (float i = nukeDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        Instantiate(gameObjectNuke, gameObjectNuke.transform.position, Quaternion.identity);


        Destroy(powerUpIcon);

        yield return new WaitForSeconds(timeToRenewNuke);
        buttonNuke.interactable = true;
    }
    #endregion

    #region Bad Effects
    [Header("Bad Effects")]
    [SerializeField]
    GameObject gameObjectFlippedCamera;

    [SerializeField]
    GameObject gameObjectLensDistortion;

    [SerializeField]
    GameObject gameObjectEcstasy;

    public void ActivateFlippedCamera() => ActivateBadEffect("FLIPPED CAMERA");
    public void ActivateLensDistorion() => ActivateBadEffect("LENS DISTORTION");
    public void ActivateEcstasy() => ActivateBadEffect("?$#^@$!@S4");

    [SerializeField]
    private float timeBadEffects = 10f;

    private void ActivateBadEffect(string nameOfBadEffect)
    {
        switch (nameOfBadEffect)
        {
            //ecstasy mode
            case "?$#^@$!@S4":
                StartCoroutine(EnableEcstasy());
                break;

            case "LENS DISTORTION":
                StartCoroutine(EnableLensDistortion());
                break;

            case "FLIPPED CAMERA":
                StartCoroutine(EnableFlippedCamera());
                break;
        }

    }

    private IEnumerator EnableEcstasy()
    {
        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, badEffectSprite, 5);

        gameObjectEcstasy.SetActive(true);
        for (float i = timeBadEffects; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        gameObjectEcstasy.SetActive(false);
        Destroy(powerUpIcon);
    }


    private IEnumerator EnableFlippedCamera()
    {

        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, badEffectSprite, 5);

        gameObjectFlippedCamera.SetActive(true);
        for (float i = timeBadEffects; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        gameObjectFlippedCamera.SetActive(false);
        Destroy(powerUpIcon);
    }


    private IEnumerator EnableLensDistortion()
    {
        GameObject powerUpIcon;
        TextMeshProUGUI textPowerUpDuration;
        ShowPowerUpIconDuration(out powerUpIcon, out textPowerUpDuration, badEffectSprite, 5);

        gameObjectLensDistortion.SetActive(true);
        for (float i = timeBadEffects; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        gameObjectLensDistortion.SetActive(false);
        Destroy(powerUpIcon);
    }

    #endregion
}
