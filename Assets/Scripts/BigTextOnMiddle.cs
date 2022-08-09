using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BigTextOnMiddle : MonoBehaviour
{

    private TextMeshProUGUI text;
    private RectTransform transfom;
    private AudioSource audioSource;

    [Header("0 - HIGH SCORE | 1 - EXTRA MONEY")]
    [Space]
    [Header("Audio Clips")]
    public AudioClip[] clips;
    // 0 - New Highscore 
    // 1 - Money money money 
    // 2 - Mysterious 

    public Dictionary<float, string> distanceToReachDictionary = new Dictionary<float, string>();

    private GameManager gameManager;
    private PowerUpManager powerUpManager;
    void Start()
    {
        gameManager = GameManager.GameManagerInstance;
        powerUpManager = PowerUpManager.PowerUpManagerInstance;
        gameManager.OnHighscore.AddListener(DisplayTextHighScore);
        gameManager.OnDestroyMoneyPig.AddListener(DisplayTextExtraMoney);
        gameManager.OnDestroyMysteriousBox.AddListener(DisplayAndActivateMysteriousEffect);

        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        audioSource = this.gameObject.GetComponent<AudioSource>();

        InitalizeDistanceToReach();
    }

    private void InitalizeDistanceToReach()
    {
        distanceToReachDictionary.Add(500, "GOOD!");
        distanceToReachDictionary.Add(1000, "WOW!");
        distanceToReachDictionary.Add(2500, "AMAZING!");
        distanceToReachDictionary.Add(5000, "OUTSTANDING!");
        distanceToReachDictionary.Add(10000, "AWESOME!");
        distanceToReachDictionary.Add(20000, "CHEATER!");
    }

    public void DisplayAfterReachDistance(float distance)
    {
        if (distanceToReachDictionary.ContainsKey(distance))
        {
            text.text = distanceToReachDictionary[distance];

            switch (distance)
            {
                case 500:
                    text.color = Color.yellow;
                    break;
                case 1000:
                    text.color = Color.red;
                    break;
                case 2500:
                    text.color = Color.green;
                    break;
                case 5000:
                    text.color = Color.blue;
                    break;
                case 10000:
                    text.color = Color.magenta;
                    break;
                case 20000:
                    text.color = Color.grey;
                    break;
            }

            distanceToReachDictionary.Remove(distance);

            StartCoroutine("Display");
            audioSource.PlayOneShot(clips[0]);
        }

    }


    private void DisplayTextHighScore()
    {
        text.text = "NEW BEST SCORE!";
        text.color = Color.yellow;
        StartCoroutine("Display");
        audioSource.PlayOneShot(clips[0]);
    }


    private void DisplayTextExtraMoney()
    {
        int[] moneys = { 5, 5, 5, 10, 10, 15, 15, 20, 25, 50 };
        int randomMoneyAmount = Random.Range(0, moneys.Length);

        GameManager.GameManagerInstance.money += moneys[randomMoneyAmount];

        text.text = $"+{moneys[randomMoneyAmount]} $";
        text.color = Color.green;

        audioSource.PlayOneShot(clips[1]);
        StartCoroutine("Display");
    }



    [SerializeField]
    GameObject gameObjectMonster;
    private void DisplayAndActivateMysteriousEffect()
    {
        //4x nothing 4x bad effect 10x powerup
        string[] effects = {
            "SHIELD", "LASER", "NUKE", "SLOW MOTION", "SUPER AMMO",
            "SHIELD", "LASER", "NUKE", "SLOW MOTION", "SUPER AMMO",
            "?$#^@$!@S4","FLIPPED CAMERA", "LENS DISTORTION", "MONSTER",
            "NOTHING", "NOTHING", "NOTHING", "NOTHING"};

        int randomEffect = Random.Range(0, effects.Length);
        switch (effects[randomEffect])
        {

            case "SHIELD":
                powerUpManager.ActivateShieldForFree();
                break;

            case "LASER":
                powerUpManager.ActivateLaserForFree();
                break;

            case "NUKE":
                powerUpManager.ActivateNukeForFree();
                break;

            case "SLOW MOTION":
                powerUpManager.ActivateSlowMoForFree();
                break;

            case "SUPER AMMO":
                powerUpManager.ActivateSuperAmmoForFree();
                break;

            //ecstasy mode
            case "?$#^@$!@S4":
                powerUpManager.ActivateEcstasy();
                break;

            case "LENS DISTORTION":
                powerUpManager.ActivateLensDistorion();
                break;

            case "FLIPPED CAMERA":
                powerUpManager.ActivateFlippedCamera();
                break;

            case "MONSTER":
                Instantiate(gameObjectMonster, gameObjectMonster.transform.position, Quaternion.identity);
                break;
        }


        text.text = effects[randomEffect];
        text.color = Color.grey;
        StartCoroutine("Display");

        audioSource.PlayOneShot(clips[2]);
    }

    private IEnumerator Display()
    {
        transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.eulerAngles = Vector3.zero;    
        transfom.localScale = new Vector3(0, 0, 0);
        text.alpha = 1f;

        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 0.25f);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();
        yield return new WaitForSeconds(1f);
        this.gameObject.LeanScale(new Vector3(0, 0, 0), 1f).setEaseInQuart();
        LeanTweenExt.LeanAlphaText(text, 0, 1f).setEase(LeanTweenType.linear);
    }
}
