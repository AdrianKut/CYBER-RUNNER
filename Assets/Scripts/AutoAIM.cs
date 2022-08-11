using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoAIM : MonoBehaviour
{
    public float Range;
    public bool Detected = false;
    public Transform Target;

    public GameObject Gun;
    public float Force;

    public bool Selected = false;

    public static AutoAIM Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Selected)
        {
            ShootOnSelectedTarget();
        }
    }

    private void ShootOnSelectedTarget()
    {      
        Vector3 difference = Target.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        rotationZ = Mathf.Clamp(rotationZ, -20f, 30f);
        Gun.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    public void SetNewTarget(Transform targetTransform)
    {
        Debug.Log(targetTransform.transform.name);
        if (Target == targetTransform)
        {
            targetTransform.gameObject.GetComponent<AutoAIMSelector>().HideSelector();
        }
        
        Target = targetTransform;
        Selected = true;
    }

   
}