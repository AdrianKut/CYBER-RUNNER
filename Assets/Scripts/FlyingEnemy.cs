using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            LeanTween.move(this.gameObject, new Vector3(this.gameObject.transform.position.x, 0.5f), 0.1f);
            yield return new WaitForSeconds(0.5f);
            LeanTween.move(this.gameObject, new Vector3(this.gameObject.transform.position.x, -1f), 0.1f);
            yield return new WaitForSeconds(0.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
