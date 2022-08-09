using System.Collections;
using UnityEngine;

public class LoopBackground : MonoBehaviour
{

    [SerializeField]
    public float xBound = -47.32f;

    [SerializeField]
    public float speed = 5f;

    [SerializeField]
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        speed = 5f;

        StartCoroutine(IncreaseSpeedBackground());
    }

    private float increaseSpeedDelay = 10f;
    private void Update()
    {
        if (GameManager.GameManagerInstance.isGameOver == false && GameManager.GameManagerInstance.isPaused == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);

            if (transform.position.x <= xBound)
                transform.position = startPos;
        }
    }

    
    IEnumerator IncreaseSpeedBackground()
    {
        while (true)
        {        
                yield return new WaitForSeconds(increaseSpeedDelay);
                speed += 0.5f;
                GameManager.GameManagerInstance.distanceMultipier += 0.01f;     
        }
    }


}

