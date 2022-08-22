using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private Sprite powerUpSprite;

    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private GameObject gameObjectPowerUp;

    [SerializeField] private short powerUpDuration;
    private short powerUpDurationAtStart;

    [SerializeField] private float timeToRenewPowerUp;
    private bool IsPowerUpActivated = false;

    private Button buttonPowerUp;
    private void Awake()
    {
        buttonPowerUp = this.gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        IsPowerUpActivated = false;
        powerUpDurationAtStart = powerUpDuration;
    }

    public void ActivatePowerUpForFree()
    {
        if (IsPowerUpActivated == true)
            powerUpDuration = powerUpDurationAtStart;
        else
            EnablePowerUp();
    }

    public void BuyPowerUp()
    {
        if (GameManager.Instance.GetMoney() >= (int)powerUpType)
        {
            audioSource.Play();
            GameManager.Instance.BuyPowerUpTypeAndDecreaseMoney(powerUpType);
            EnablePowerUp();
        }
        else
            StartCoroutine(ChangeColorOfButtonToRed(buttonPowerUp));
    }


    public void DeactivateButtonPowerUp()
    {
        IsPowerUpActivated = true;
        powerUpDuration = powerUpDurationAtStart;
        buttonPowerUp.interactable = false;
    }

    private void EnablePowerUp()
    {
        switch (powerUpType)
        {
            case PowerUpType.Shield:
                StartCoroutine(EnableShield());
                break;
            case PowerUpType.SlowMotion:
                StartCoroutine(EnableSlowMotion());
                break;
            case PowerUpType.Laser:
                StartCoroutine(EnableLaser());
                break;
            case PowerUpType.SuperAmmo:
                StartCoroutine(EnableSuperAmmo());
                break;
            case PowerUpType.Nuke:
                EnableNuke();
                break;
            default:
                break;
        }
    }

    protected IEnumerator DestroyPowerUp(GameObject powerUpIcon)
    {
        Destroy(powerUpIcon);
        IsPowerUpActivated = false;
        yield return new WaitForSeconds(timeToRenewPowerUp);
        buttonPowerUp.interactable = true;
    }

    protected IEnumerator ChangeColorOfButtonToRed(Button thisButton)
    {
        thisButton.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        thisButton.GetComponent<Image>().color = Color.white;
    }

    #region Shield
    [Header("Shield")]
    [SerializeField] private GameObject player;
    public IEnumerator EnableShield()
    {
        powerUpManager.isShieldActivated = true;
        DeactivateButtonPowerUp();

        var shield = Instantiate(gameObjectPowerUp, gameObjectPowerUp.transform.position, Quaternion.identity);
        shield.transform.SetParent(player.transform);
        shield.transform.localPosition = new Vector3(0f, -0.15f, 0f);

        LeanTween.scale(shield, new Vector3(0.3f, 0.3f, 0.3f), 7f).setEasePunch();
        LeanTween.rotateAround(shield, Vector3.forward, 360, 1.75f).setLoopClamp();

        powerUpManager.ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, powerUpSprite, powerUpDuration);

        for (; powerUpDuration > 0; powerUpDuration--)
        {
            textPowerUpDuration.SetText("" + powerUpDuration);
            LeanTween.alpha(shield, 0.15f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.alpha(shield, 1.0f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(1f);
        }

        LeanTween.scale(shield, new Vector3(0f, 0f, 0f), 7f).setEasePunch().setLoopPingPong();
        LeanTween.alpha(shield, 0f, 0.8f).setEase(LeanTweenType.easeOutQuad);

        powerUpManager.isShieldActivated = false;
        Destroy(shield, 1f);
        StartCoroutine(DestroyPowerUp(powerUpIcon));
    }
    #endregion

    #region Slow Motion
    [Header("Slow Motion")]
    [SerializeField] private LoopBackground loopBackground;

    public UnityEvent OnSlowMotionActivated;
    public UnityEvent OnSlowMotionDeactivated;
    public IEnumerator EnableSlowMotion()
    {
        DeactivateButtonPowerUp();
        OnSlowMotionActivated?.Invoke();

        var currentSpeedBackground = loopBackground.Speed;
        loopBackground.Speed = (currentSpeedBackground / 2);

        powerUpManager.ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, powerUpSprite, powerUpDuration);

        for (; powerUpDuration > 0; powerUpDuration--)
        {
            textPowerUpDuration.SetText("" + powerUpDuration);
            yield return new WaitForSeconds(1f);
        }

        loopBackground.Speed = currentSpeedBackground + 0.5f;
        OnSlowMotionDeactivated?.Invoke();
        StartCoroutine(DestroyPowerUp(powerUpIcon));
    }
    #endregion

    #region Nuke
    private void EnableNuke()
    {
        DeactivateButtonPowerUp();
        powerUpManager.ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, powerUpSprite, powerUpDuration);
        Instantiate(gameObjectPowerUp, gameObjectPowerUp.transform.position, Quaternion.identity);

        StartCoroutine(DestroyPowerUp(powerUpIcon));
    }
    #endregion nuke

    #region SuperAmmo
    private IEnumerator EnableSuperAmmo()
    {
        DeactivateButtonPowerUp();

        powerUpManager.isSuperAmmoActivated = true;

        var rayCastWeapon = player.GetComponent<RayCastWeapon>();
        powerUpManager.ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, powerUpSprite, powerUpDuration);

        for (float i = powerUpDuration; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        if (rayCastWeapon.GetNameOfCurrentWeapon() == "laser")
            rayCastWeapon.UseLaser();
        else if (rayCastWeapon.GetNameOfCurrentWeapon() == "rifle")
            rayCastWeapon.UseRifle();

        powerUpManager.isSuperAmmoActivated = false;
        StartCoroutine(DestroyPowerUp(powerUpIcon));
    }
    #endregion

    #region Laser
    private IEnumerator EnableLaser()
    {
        DeactivateButtonPowerUp();

        var rayCastWeapon = player.GetComponent<RayCastWeapon>();
        rayCastWeapon.UseLaser();

        powerUpManager.ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, powerUpSprite, powerUpDuration);

        if (powerUpManager.isSuperAmmoActivated)
        {
            for (; powerUpDuration > 0; powerUpDuration--)
            {
                textPowerUpDuration.SetText("" + powerUpDuration);
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            for (; powerUpDuration > 0; powerUpDuration--)
            {
                textPowerUpDuration.SetText("" + powerUpDuration);
                yield return new WaitForSeconds(1f);
            }
        }

        StartCoroutine(DestroyPowerUp(powerUpIcon));

        if (powerUpManager.isSuperAmmoActivated)
        {
            rayCastWeapon.UseRifle();
        }
        else
            rayCastWeapon.UseRifle();
    }
    #endregion
}


