using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float forceJump = 900f;
    [SerializeField] private bool isGrounded = true;

    [Header("Audio")]
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;
    //0 - died
    //1 - jump

    [Header("VFX Effects")]
    [SerializeField] private GameObject jumpEffect;
    [SerializeField] private GameObject diedEffect;

    [SerializeField] private Camera cam;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {    
        GameManager.GameManagerInstance.OnGameOverEvent.AddListener(Death);
    }

    private void OnDisable()
    {
        GameManager.GameManagerInstance.OnGameOverEvent.RemoveListener(Death);
    }

    void Update()
    {
        Jump();
    }

    public void Jump()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(this.transform.position);

            if (touch.position.x <= screenPos.x + 100f && touch.position.y >= 250
                && touch.phase == TouchPhase.Began && isGrounded &&
                GameManager.GameManagerInstance.isGameOver == false)
            {
                audioSource.PlayOneShot(audioClip[1]);
                rb.AddForce(Vector2.up * forceJump, ForceMode2D.Force);

                var jumpEffect = Instantiate(this.jumpEffect, new Vector3(transform.position.x,
                    transform.position.y - 1.5f, transform.position.z),
                    Quaternion.Euler(70f, 0, 0));

                Destroy(jumpEffect, 1f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GroundDetect(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GroundDetect(collision, false);
    }

    private void GroundDetect(Collision2D collision, bool isGrounded)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            this.isGrounded = isGrounded;
            animator.SetBool("isJump", false);
        }
    }

    private void Death()
    {
        animator.SetBool("isDead", true);
        audioSource.PlayOneShot(audioClip[0]);

        transform.position = new Vector3(transform.position.x, 2.9f, 0f);
        var diedEffect = Instantiate(this.diedEffect, new Vector3(transform.position.x + 0.485f, transform.position.y - 0.30f, transform.position.z), Quaternion.identity);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(this.gameObject.GetComponent<Collider2D>());
        Destroy(diedEffect, 1f);
    }
}
