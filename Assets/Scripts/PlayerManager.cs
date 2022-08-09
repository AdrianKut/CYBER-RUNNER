using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float forceJump = 900f;
    public bool isGrounded = true;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] audioClip;
    //0 - died
    //1 - jump

    public GameObject JumpEffect;
    public GameObject DiedEffect;

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameManager.GameManagerInstance.OnGameOverEvent.AddListener(GameOver);
    }

    private void GameOver()
    {
        animator.SetBool("isDead", true);
        audioSource.PlayOneShot(audioClip[0]);

        transform.position = new Vector3(transform.position.x, 2.9f, 0f);
        var diedEffect = Instantiate(DiedEffect, new Vector3(transform.position.x + 0.485f, transform.position.y - 0.30f, transform.position.z), Quaternion.identity);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(this.gameObject.GetComponent<Collider2D>());
        Destroy(diedEffect, 1f);
    }

    void Update()
    {
        Jump();
    }

    public void JumpOnButton()
    {
        if ( isGrounded && GameManager.GameManagerInstance.isGameOver == false)
        {
            audioSource.PlayOneShot(audioClip[1]);
            rb.AddForce(Vector2.up * forceJump, ForceMode2D.Force);

            var jumpEffect = Instantiate(JumpEffect, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), Quaternion.Euler(70f, 0, 0));
            Destroy(jumpEffect, 1f);
        }
    }

    public void Jump()
    {
        if (Input.touchCount > 0)
        {
            var touch_1 = Input.GetTouch(0);
           // Debug.Log(touch_1.position);

            if (touch_1.position.x <= 450 /*&& touch_1.position.y >= 200 */
                && touch_1.phase == TouchPhase.Began && isGrounded &&
                GameManager.GameManagerInstance.isGameOver == false)
            {
                audioSource.PlayOneShot(audioClip[1]);
                rb.AddForce(Vector2.up * forceJump, ForceMode2D.Force);

                var jumpEffect = Instantiate(JumpEffect, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), Quaternion.Euler(70f, 0, 0));
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




}
