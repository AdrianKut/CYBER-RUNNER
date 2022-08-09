using System.Linq;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    [SerializeField]
    private GameObject deathEffect;

    private bool detonateOnce = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && detonateOnce == false)
        {
            detonateOnce = true;
            Instantiate(deathEffect, transform.position, Quaternion.identity);

            var objectsToDestroy = GameObject.FindObjectsOfType<Enemy>();
            objectsToDestroy.ToList().ForEach(obj => obj.GetComponent<Enemy>().Die());

            Destroy(this.gameObject);
        }
    }

}
