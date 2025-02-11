using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [Header("WallJump")]
    private float wallJumpCooldown;

    private float horizontalInput;

    [SerializeField] private float wallJumpX; 
    [SerializeField] private float wallJumpY; 

    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;

    [Header ("Kyote Time")]
    [SerializeField] private float kyoteTime; // How long after leaving a platform can the player still jump
     private float kyoteCounter; // Timer for kyote time

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        anim.SetBool("Running", horizontalInput != 0);
        anim.SetBool("Ground", isGrounded());

        //Jump
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Adjustable Jump Height
        if(Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
           body.linearVelocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 1;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (isGrounded())
            {
                kyoteCounter = kyoteTime; // Reset kyote counter when on the ground
            }
            else
            {
                kyoteCounter -= Time.deltaTime; // Decrease kyote counter when not on the ground
            }
        }
    }

    private void Jump()
    {
        if (kyoteCounter <= 0 && !onWall()) return;
            SoundManager.instance.PlaySound(jumpSound);


        if (onWall())
        {
            WallJump();
        }
        else
        {
            if(isGrounded())
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            else
            {
                // Kyote Time Jump
                if (kyoteCounter> 0)
                    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            }
            kyoteCounter = 0;
        }
        //if (isGrounded())
        //{
        //    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            
        //}
        //else if (onWall() && !isGrounded())
        //{
        //    if (horizontalInput == 0)
        //    {
        //        body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
        //        transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //    }
        //    else
        //        body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * speed, jumpPower);

        //    wallJumpCooldown = 0;
        //}
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(wallJumpX * -transform.localScale.x, wallJumpY));
        wallJumpCooldown = 0;

    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}