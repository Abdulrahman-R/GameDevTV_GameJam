using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats playerStats;
    [HideInInspector]public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;


    [SerializeField] SpriteRenderer gfxRenderer;
    [SerializeField] Animator gFXanimator;
    public bool canMove;

    void Start()
    {
        canMove = true;
        playerStats = GetComponent<PlayerStats>();
        moveSpeed = playerStats.GetSpeed();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Create a vector for the movement direction
        movement = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            gfxRenderer.flipX = true;
        }else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gfxRenderer.flipX = false;
        }

        if(rb.velocity.magnitude > 0.01)
        {
            gFXanimator.SetBool("move", true);
        }
        else
        {
            gFXanimator.SetBool("move", false);
        }
        
        
    }

    void FixedUpdate()
    {
        // Move the player using the Rigidbody2D
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = movement * moveSpeed;
        }
        
    }

}
