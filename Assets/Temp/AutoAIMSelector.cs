using System.Collections;
using UnityEngine;

public class AutoAIMSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectAIMSelector;

    private GameObject gameObjectVisual;
    private Color gameObjectColor;
    private void OnEnable()
    {
        gameObjectAIMSelector = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnMouseDown()
    {
        StartCoroutine(ChangeAutoAIMPosition());
    }

    IEnumerator ChangeAutoAIMPosition()
    {
        Debug.LogWarning("NEW TARGET!");
        AutoAIM.Instance.Target = this.gameObject.transform;
        AutoAIM.Instance.Selected = true;

        //foreach (var item in gameObjectVisual.transform.GetComponentsInChildren<SpriteRenderer>())
        //{
        //    item.color = Color.red;
        //}

        //yield return new WaitForSeconds(0.1f);

        //foreach (var item in gameObjectVisual.transform.GetComponentsInChildren<SpriteRenderer>())
        //{
        //    item.color = gameObjectColor;
        //}


        gameObjectAIMSelector.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameObjectAIMSelector.SetActive(false);
        //var a = Instantiate(gameObjectSelectorPrefab, gameObjectVisual.transform.position, Quaternion.identity);
        //Destroy(a);

    }

    private void OnDisable()
    {
        AutoAIM.Instance.Selected = false;
        gameObjectAIMSelector.SetActive(false);
    }
}
