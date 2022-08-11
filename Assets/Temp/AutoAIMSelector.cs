using System.Collections;
using UnityEngine;

public class AutoAIMSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectAIMSelector;

    public void HideSelector() => gameObjectAIMSelector.SetActive(false);
    private void OnEnable()
    {
        gameObjectAIMSelector = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnMouseDown()
    {
        StartCoroutine(ChangeAutoAIMPosition());
    }

    private IEnumerator ChangeAutoAIMPosition()
    {
        AutoAIM.Instance.Selected = false;
        AutoAIM.Instance.SetNewTarget(this.gameObject.transform);

        gameObjectAIMSelector.SetActive(true);
        LeanTween.rotateAroundLocal(gameObjectAIMSelector, Vector3.forward, 360, 1.5f).setRepeat(-1);
        while (AutoAIM.Instance.Selected == true)
        {
            LeanTween.alpha(gameObjectAIMSelector, 0.15f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.alpha(gameObjectAIMSelector, 1.0f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.easeInQuad);
            yield return new WaitForSeconds(1f);
        }
        gameObjectAIMSelector.SetActive(false);
    }
    private void OnDestroy()
    {
        AutoAIM.Instance.Selected = false;
    }
}
