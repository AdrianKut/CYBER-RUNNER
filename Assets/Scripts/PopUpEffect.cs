using System.Collections;
using UnityEngine;

public class PopUpEffect : MonoBehaviour
{
    public float effectSpeed = 3f;
    private RectTransform transfom;

    private void Start() => StartCoroutine(PopUp());
    private void OnEnable() => StartCoroutine(PopUp());

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator PopUp()
    {
        transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.localScale = new Vector3(0, 0, 0);
        this.gameObject.LeanScale(new Vector3(1, 1, 1), 1f).setEaseOutQuart();

        yield return new WaitForSeconds(0.5f);
        canRescale = true;
    }

    private bool canRescale = false;
    public void ReScale()
    {
        if (canRescale)
            StartCoroutine(ReScaleOnce());
    }

    private IEnumerator ReScaleOnce()
    {
        canRescale = false;

        LeanTween.scale(this.gameObject, new Vector3(1.25f, 1.25f, 1.25f), 0.50f).setEasePunch();
        LeanTween.scale(this.gameObject, new Vector3(1, 1, 1), 0).setDelay(0.5f);

        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        canRescale = true;
    }


}