using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject MenuUI;

    [SerializeField]
    private GameObject HelpUI;

    [SerializeField]
    private GameObject TopScoreUI;

    private void Start() => Time.timeScale = 1f;

    public void StartGame() => SceneManager.LoadScene("Game Scene");

    public void QuitGame() => Application.Quit();

    public void OpenHelpUI()
    {
        MenuUI.SetActive(false);
        HelpUI.SetActive(true);
    }

    public void OpenTopScoreUI()
    {
        MenuUI.SetActive(false);
        TopScoreUI.SetActive(true);
    }

    public void OpenMenuUI()
    {
        SceneManager.LoadScene("Menu");
    }
}
