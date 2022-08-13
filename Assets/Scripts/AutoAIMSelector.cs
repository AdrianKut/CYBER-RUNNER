using System.Collections;
using UnityEngine;

public class AutoAIMSelector : MonoBehaviour
{

    private GameObject gameObjectAIMSelector;

    public void HideSelector() => gameObjectAIMSelector.SetActive(false);
    private void OnEnable()
    {
        gameObjectAIMSelector = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnMouseDown()
    {
        AutoAIM.Instance.SetNewTarget(this.gameObject.transform);
        StartCoroutine(ChangeAutoAIMPosition());
    }

    private IEnumerator ChangeAutoAIMPosition()
    {   
        gameObjectAIMSelector.SetActive(true);
        LeanTween.alpha(gameObjectAIMSelector, 1f,0f);
        LeanTween.alpha(gameObjectAIMSelector, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad);
        yield return new WaitForSeconds(0.5f);
        gameObjectAIMSelector.SetActive(false);
        
    }
}
