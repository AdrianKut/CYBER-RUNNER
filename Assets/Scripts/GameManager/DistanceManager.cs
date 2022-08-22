using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    [Header("Distance Score")]
    [SerializeField] private TextMeshProUGUI textDistance;
    [SerializeField] private float distanceCounter;
    [SerializeField] private float distanceMultipier = 0.3f;
    private float increaseSpeedDelay = 10f;

    [Header("Best Distance Score")]
    [SerializeField] private TextMeshProUGUI textBestDistance;
    [SerializeField] private float currentBestScore;
    [SerializeField] private bool isNewHighScore = false;

    [Header("On Game Over UI")]
    [SerializeField] private GameObject GameObjectButtonSaveNewHighScoreTextName;
    [SerializeField] private GameObject GameObjectButtonsRetryExit;
    [SerializeField] private GameObject GameObjectInputFieldTextHighScoreName;
    [SerializeField] private TextMeshProUGUI TextMeshProUGUINickname;

    [SerializeField] private TextMeshProUGUI FinalScoreText;
    [SerializeField] private TextMeshProUGUI NewHighScoreText;

    [Space]
    [SerializeField] private GameObject GameObjectBigTextPrefab;

    private bool isNewScoreOnScoreBoard = false;
    private int highScoreIndex;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        currentBestScore = HighScoreManager.Instance.GetSelectedScoreValue(0);

        gameManager.OnGameOverEvent.AddListener(CheckYourScoreToLoeaderBoard);
        UpdateBestScoreText();

        StartCoroutine(IncreaseSpeedBackground());
    }

    private void FixedUpdate()
    {
        if (CanShowDistance())
        {
            UpdateTimerText();
        }

        DisplayAfterReachDistance((int)distanceCounter);
    }

    private bool CanShowDistance()
    {
        return gameManager.isPaused == false && gameManager.isGameOver == false && gameManager.isStarted == true;
    }

    private IEnumerator IncreaseSpeedBackground()
    {
        while (true)
        {
            yield return new WaitForSeconds(increaseSpeedDelay);
            distanceMultipier += 0.01f;
        }
    }

    private void UpdateBestScoreText()
    {
        textBestDistance.SetText($"BEST: {currentBestScore:F0} M");
    }

    private void CheckYourScoreToLoeaderBoard()
    {
        FinalScoreText.SetText("YOUR SCORE: \n " + textDistance.text);

        for (int i = 0; i < HighScoreManager.Instance.GetScoreLength(); i++)
        {
            if (distanceCounter > HighScoreManager.Instance.GetSelectedScoreValue(i) && isNewScoreOnScoreBoard == false)
            {
                var tempHighscoreResults = new float[5];
                Array.Copy(HighScoreManager.Instance.GetScoresArray(), tempHighscoreResults, 5);

                var tempHighScoreNames = new string[5];
                Array.Copy(HighScoreManager.Instance.GetTextNamesArray(), tempHighScoreNames, 5);

                for (int j = i + 1; j < tempHighScoreNames.Length; j++)
                {
                    HighScoreManager.Instance.SetScore(tempHighscoreResults[j - 1], j);
                    HighScoreManager.Instance.SetTextName(tempHighScoreNames[j - 1], j);
                }

                highScoreIndex = i;
                GameObjectButtonsRetryExit.SetActive(false);
                GameObjectInputFieldTextHighScoreName.SetActive(true);


                HighScoreManager.Instance.SetScore(distanceCounter, i);
                HighScoreManager.Instance.Save();
                NewHighScoreText.SetText("NEW HIGH SCORE!");
                isNewScoreOnScoreBoard = true;
                break;
            }
        }
    }

    public void SaveNewHighScoreTextName()
    {
        if (TextMeshProUGUINickname.text.Length == 1)
            HighScoreManager.Instance.SetTextName("ROG RUNNER", highScoreIndex);
        else
            HighScoreManager.Instance.SetTextName(TextMeshProUGUINickname.text, highScoreIndex);

        HighScoreManager.Instance.Save();

        GameObjectInputFieldTextHighScoreName.SetActive(false);
        GameObjectButtonSaveNewHighScoreTextName.SetActive(false);
        GameObjectButtonsRetryExit.SetActive(true);
    }

    private List<int> distanceToReachList = new List<int> { 500, 1000, 2500, 5000, 10000, 20000 };
    private void UpdateTimerText()
    {
        distanceCounter += distanceMultipier;
        textDistance.SetText($"{distanceCounter:F0} M");

        if ((currentBestScore < distanceCounter) && !isNewHighScore)
        {
            isNewHighScore = true;

            var gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
            gameObject.DisplayText("NEW HIGHSCORE", Color.red, ClipsType.highscore);
        }
    }

    public void DisplayAfterReachDistance(int distance)
    {
        if (distanceToReachList.Contains(distance))
        {
            switch (distance)
            {
                case 500:
                    var gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
                    gameObject.DisplayText("GOOD!", Color.yellow, ClipsType.highscore);
                    goto removeDistance;
                case 1000:
                    gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
                    gameObject.DisplayText("WOW!", Color.red, ClipsType.highscore);
                    goto removeDistance;
                case 2500:
                    gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
                    gameObject.DisplayText("AMAZING!", Color.green, ClipsType.highscore);
                    goto removeDistance;
                case 5000:
                    gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
                    gameObject.DisplayText("OUTSTANDING!", Color.blue, ClipsType.highscore);
                    goto removeDistance;
                case 10000:
                    gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
                    gameObject.DisplayText("AWESOME!", Color.magenta, ClipsType.highscore);
                    goto removeDistance;
                case 20000:
                    gameObject = Instantiate(GameObjectBigTextPrefab, GameObjectBigTextPrefab.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
                    gameObject.DisplayText("CHEATER!", Color.grey, ClipsType.highscore);
                    goto removeDistance;

                removeDistance:
                    distanceToReachList.Remove(distance);
                    break;
            }
        }
    }
}
