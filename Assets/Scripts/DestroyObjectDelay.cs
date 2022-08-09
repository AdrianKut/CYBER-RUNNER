using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectDelay : MonoBehaviour
{
    [SerializeField]
    float destroyDelay;

    private void OnEnable()
    {
        Destroy(this.gameObject, destroyDelay);
    }
}
