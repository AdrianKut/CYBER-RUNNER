using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ClipsType
{
    highscore = 0,
    money = 1,
    mysteriousEffect = 2,
}

public class BigTextManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    // 0 - New Highscore 
    // 1 - Money money money 
    // 2 - Mysterious 

    private GameObject gameObjectText;
    private TextMeshProUGUI text;
    private void Awake()
    {
        gameObjectText = transform.GetChild(0).gameObject;
        text = gameObjectText.GetComponent<TextMeshProUGUI>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        Destroy(this.gameObject, 3.5f);
    }

    public void DisplayText(string message, Color color, ClipsType clipType)
    {
        text.text = message;
        text.color = color;

        switch (clipType)
        {
            case ClipsType.highscore:
                audioSource.PlayOneShot(clips[0]);
                break;
            case ClipsType.money:
                audioSource.PlayOneShot(clips[1]);
                break;
            case ClipsType.mysteriousEffect:
                audioSource.PlayOneShot(clips[2]);
                break;
        }
        
        StartCoroutine("Display");
    }

    private IEnumerator Display()
    {    
        LeanTween.rotateAround(gameObjectText.transform.gameObject, Vector3.forward, 360, 0.25f);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();
        yield return new WaitForSeconds(1f);
        this.gameObject.LeanScale(new Vector3(0, 0, 0), 1f).setEaseInQuart();
        LeanTweenExt.LeanAlphaText(text, 0, 1f).setEase(LeanTweenType.linear);
    }
}
