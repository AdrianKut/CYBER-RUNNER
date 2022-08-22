using UnityEngine;

public class AutoAIM : MonoBehaviour
{
    [SerializeField] private GameObject Gun;

    private Transform Target;
    private bool Selected = false;

    public static AutoAIM Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
        if (Target != null)
        {
            Vector3 difference = Target.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationZ = Mathf.Clamp(rotationZ, -20f, 30f);
            Gun.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
        else
        {
            Selected = false;
            Gun.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void SetNewTarget(Transform targetTransform)
    {
        Selected = true;
        Target = targetTransform;
    }

    public void ReleaseAutoAIM(GameObject gameObject)
    {
        if (Target != null && Target.gameObject.name == gameObject.name)
        {
            Selected = false;
            Gun.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}