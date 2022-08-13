using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEL : MonoBehaviour
{
    [SerializeField]
    GameObject GameObjectBigText;

    public void SHOW()
    {

        var gameObject = Instantiate(GameObjectBigText, GameObjectBigText.transform.position, Quaternion.identity).gameObject.GetComponent<BigTextManager>();
        gameObject.DisplayText("NEW HIGHSCORE", Color.red, ClipsType.highscore);

    }

}
