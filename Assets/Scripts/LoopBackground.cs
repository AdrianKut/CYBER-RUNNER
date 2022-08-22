using System.Collections;
using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    [SerializeField] private float xBound = -47.32f;
    [SerializeField] private Vector3 startPos;

    public float Speed { get; set; } = 5f;
    private float increaseSpeedDelay = 10f;
    private void Awake()
    {
        startPos = transform.position;
        Speed = 5f;

        StartCoroutine(IncreaseSpeedBackground());
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (GameManager.Instance.isGameOver == false 
            && GameManager.Instance.isPaused == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * Speed);

            if (transform.position.x <= xBound)
                transform.position = startPos;
        }
    }

    IEnumerator IncreaseSpeedBackground()
    {
        while (true)
        {        
                yield return new WaitForSeconds(increaseSpeedDelay);
                Speed += 0.5f;
        }
    }
}

