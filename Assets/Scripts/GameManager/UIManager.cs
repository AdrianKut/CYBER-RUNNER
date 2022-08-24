using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("LEVEL UI")]
    [SerializeField] private GameObject GameObjectLevelUI;
    [SerializeField] private GameObject AskToExitGameObject;
    [SerializeField] private GameObject GameObjectPowerUpUI;

    [Header("Pause")]
    [SerializeField] private GameObject PauseGameObject;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        HideUI();
    }

    public void Pause()
    {
        gameManager.audioSource.Pause();
        ShowOrHideEnemies(activate: false);
        gameManager.HideLaser();
        gameManager.PlayerActivator(false);
        gameManager.isPaused = true;

        PauseGameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        gameManager.audioSource.UnPause();
        ShowOrHideEnemies(activate: true);
        AskToExitGameObject.SetActive(false);
        gameManager.SetStartPosPlayer();
        gameManager.PlayerActivator(true);
        gameManager.isPaused = false;

        PauseGameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private List<GameObject> gameObjectsToHide = new List<GameObject>();
    private void ShowOrHideEnemies(bool activate)
    {
        if (activate == false)
        {
            gameObjectsToHide = GameObject.FindGameObjectsWithTag("Enemy").ToList();

            foreach (var enemy in gameObjectsToHide)
            {
                enemy.SetActive(activate);
            }
        }
        else if (activate == true)
        {
            foreach (var enemy in gameObjectsToHide)
            {
                enemy.SetActive(activate);
            }

            gameObjectsToHide.Clear();
        }
    }

    public void Exit()
    {
        PauseGameObject.SetActive(false);
        AskToExitGameObject.SetActive(true);
    }

    public void HideUI()
    {
        GameObjectLevelUI.SetActive(false);
        gameManager.PlayerActivator(false);
        GameObjectPowerUpUI.SetActive(false);
    }

    public void ShowUI()
    {
        GameObjectLevelUI.SetActive(true);
        gameManager.PlayerActivator(true);
        GameObjectPowerUpUI.SetActive(true);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        gameManager.isPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
