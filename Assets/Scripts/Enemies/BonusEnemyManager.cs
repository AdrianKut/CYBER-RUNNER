using System.Collections;
using TMPro;
using UnityEngine;

public class BonusEnemyManager : MonoBehaviour
{
    [SerializeField] PowerUpManager powerUpManager;

    [Header("Bad Effects")]
    [SerializeField] private GameObject gameObjectMonster;
    [SerializeField] private GameObject GameObjectBigTextPrefab;

    [SerializeField] private GameObject gameObjectLensDistortion;
    [SerializeField] private GameObject gameObjectEcstasy;

    [SerializeField] private Sprite badEffectSprite;

    public void ActivateLensDistorion() => ActivateBadEffect("LENS DISTORTION");
    public void ActivateEcstasy() => ActivateBadEffect("?$#^@$!@S4");

    [SerializeField]
    private float timeBadEffects = 10f;

    private BonusEnemyManager() { }
    public static BonusEnemyManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void ActivateBadEffect(string nameOfBadEffect)
    {
        switch (nameOfBadEffect)
        {
            //ecstasy mode
            case "?$#^@$!@S4":
                StartCoroutine(EnableBadEffect(gameObjectEcstasy));
                break;

            case "LENS DISTORTION":
                StartCoroutine(EnableBadEffect(gameObjectLensDistortion));
                break;
        }
    }

    private IEnumerator EnableBadEffect(GameObject gameObjectBadEffect)
    {
        powerUpManager.ShowPowerUpIconDuration(out GameObject powerUpIcon, out TextMeshProUGUI textPowerUpDuration, badEffectSprite, 5);

        gameObjectBadEffect.SetActive(true);
        for (float i = timeBadEffects; i > 0; i--)
        {
            textPowerUpDuration.SetText("" + i);
            yield return new WaitForSeconds(1f);
        }

        gameObjectBadEffect.SetActive(false);
        Destroy(powerUpIcon);
    }

    public void DisplayAndActivateMysteriousEffect()
    {
        //3x nothing 3x bad effect 10x powerup
        //string[] effects = {
        //    "SHIELD", "LASER", "NUKE", "SLOW MOTION", "SUPER AMMO",
        //    "SHIELD", "LASER", "NUKE", "SLOW MOTION", "SUPER AMMO",
        //    "?$#^@$!@S4", "LENS DISTORTION", "MONSTER",
        //    "NOTHING", "NOTHING", "NOTHING"};
        string[] effects = {
            "SLOW MOTION"};

        int randomEffect = Random.Range(0, effects.Length);
        switch (effects[randomEffect])
        {

            case "SHIELD":
                powerUpManager.Shield.ActivatePowerUpForFree();
                break;

            case "LASER":
                powerUpManager.Laser.ActivatePowerUpForFree();
                break;

            case "NUKE":
                powerUpManager.Nuke.ActivatePowerUpForFree();
                break;

            case "SLOW MOTION":
                powerUpManager.SlowMotion.ActivatePowerUpForFree();
                break;

            case "SUPER AMMO":
                powerUpManager.SuperAmmo.ActivatePowerUpForFree();
                break;

            //ecstasy mode
            case "?$#^@$!@S4":
                ActivateEcstasy();
                break;

            case "LENS DISTORTION":
                ActivateLensDistorion();
                break;

            case "MONSTER":
                Instantiate(gameObjectMonster, gameObjectMonster.transform.position, Quaternion.identity);
                break;
        }

        var gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
        gameObject.DisplayText(effects[randomEffect], Color.grey, ClipsType.mysteriousEffect);
    }
}
