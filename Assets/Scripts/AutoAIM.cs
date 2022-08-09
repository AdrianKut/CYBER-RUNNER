using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAIM : MonoBehaviour
{
    public float Range;
    public bool Detected = false;
    public Transform Target;

    public GameObject Gun;
    public float Force;

    void Update()
    {      
        var gameObjectEnemy = GameObject.FindGameObjectWithTag("EnemyAIM");
        if (gameObjectEnemy != null)
        {
            if (Detected == false)
            {
                Detected = true;
            }
        }
        else
        {
            if (Detected == true)
            {
                Detected = false;
            }
        }

        if (Detected)
        {
            Vector3 difference = gameObjectEnemy.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationZ = Mathf.Clamp(rotationZ, -20f, 30f);
            Gun.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }
}