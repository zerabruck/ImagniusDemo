using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isReversed;
    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] protected float colliderDistance;
    [SerializeField] protected float range;
    [SerializeField] private GameObject cursor;
    [SerializeField] public Joystick movementJoystick;







    private Rigidbody2D rigidbody;
    private Animator animator;
    private PlayerHealth playerHealth;
    private BoxCollider2D boxCollider;
    private int currentDoubleJumpCollDown;

    private float horizontalPress;
    private float verticalPress;
    private float ropeCollDown = 0.5f;
    private float currentCollDown;
    private string isRockMoving = "idle";
    private GameObject selectedObject = null;


    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ropeLayer;
    [SerializeField] private LayerMask rockLayer;



    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerHealth = GetComponent<PlayerHealth>();
        currentDoubleJumpCollDown = 0;
    }
public void MoveRock() {
    if (selectedObject != null && (isRockMoving == "idle" || isRockMoving == "stop") && playerHealth.currentMana > 0) {
        isRockMoving = "moving";
    } else{
        
        isRockMoving = "stop";
    
    }
}

    private void Update()
    {
        
        RockInsight();
        if (selectedObject != null){
        SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
        Debug.Log(selectedObject);
        spriteRenderer.color = Color.green;

        }
        Debug.Log(isRockMoving);
        if (playerHealth.currentMana == 0 ){
            MoveRock();
        }
        if(isRockMoving == "stop"){
            playerHealth.ManaHeal(Time.deltaTime * 10);
            animator.SetBool("levitation", false);
            if(selectedObject != null){
                SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.white;
            }
            selectedObject = null;
        }
        if(isRockMoving == "moving"){
            playerHealth.ManaDamage(Time.deltaTime * 10);
            Rigidbody2D selectedRigidbody = selectedObject.GetComponent<Rigidbody2D>();
            Vector2 joystickPosition = movementJoystick.Direction;
            animator.SetBool("levitation", true);
            selectedRigidbody.velocity = new Vector2(joystickPosition.x * 5, joystickPosition.y * 5);

        }
        // if(selectedObject){
        //     MoveRock();
        // }
        if (isGrounded() || rockContact())
        {
            currentDoubleJumpCollDown = 0;
        }
        if (isReversed)
        {
            rigidbody.gravityScale = -1;
        }
        currentCollDown += Time.deltaTime;
        horizontalPress = movementJoystick.Direction.x;
        if (!ropeContact() && isRockMoving != "moving")
        {
            if (horizontalPress > 0)
            {
                transform.localScale = new Vector2(5f, transform.localScale.y);
            }
            else if (horizontalPress < 0)
            {
                transform.localScale = new Vector2(-5f, transform.localScale.y);
            }
            rigidbody.velocity = new Vector2(horizontalPress * speed, rigidbody.velocity.y);
            if (isGrounded() || rockContact())
            {
                animator.SetBool("runing", horizontalPress != 0);
            }
            else
            {
                animator.SetBool("runing", false);
            }
        }

        if (Input.GetKey(KeyCode.Space) && ropeContact())
        {
            jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded() || currentDoubleJumpCollDown <= 2))
        {
            jump();
        }

        if (ropeContact())
        {
            currentCollDown += Time.deltaTime;
            rigidbody.gravityScale = 0;
            animator.SetBool("runing", false);
            animator.SetBool("climbIdle", true);
            verticalPress = movementJoystick.Direction.y;
            if (verticalPress > 0)
            {
                animator.SetBool("climbIdle", false);  
                animator.SetBool("climb", true);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalPress * 5);
            }
            else if (verticalPress < 0 )
            {
                animator.SetBool("climbIdle", false);
                animator.SetBool("climb", true);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalPress * 5);
            }
            else
            {
                // rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                animator.SetBool("climb", false);

            }
            if (horizontalPress > 0 && currentCollDown > ropeCollDown && !Input.GetKey(KeyCode.Space))
            {
                transform.localScale = new Vector2(-5f, transform.localScale.y);
            }
            else if (horizontalPress < 0 && currentCollDown > ropeCollDown && !Input.GetKey(KeyCode.Space))
            {
                transform.localScale = new Vector2(5f, transform.localScale.y);
            }

        }
        else
        {
            currentCollDown = 0;
            rigidbody.gravityScale = 1;
            animator.SetBool("climb", false);
            animator.SetBool("climbIdle", false);

        }
        animator.SetBool("grounded", isGrounded());


    }
    public void jump()
    {
        currentDoubleJumpCollDown += 1;
        if (ropeContact())
        {
            if (horizontalPress != 0)
            {
                animator.SetTrigger("jump");
                // transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            Debug.Log(horizontalPress);
            Debug.Log("velocity x Press: " + rigidbody.velocity.x);
            Debug.Log("velocity y Press: " + rigidbody.velocity.y);

                rigidbody.velocity = new Vector2( -Mathf.Sign(transform.localScale.x) * 500, rigidbody.velocity.y + 5);
            }

        }
        if (Input.GetKeyDown(KeyCode.Space) && !ropeContact())
        {
            if (currentDoubleJumpCollDown == 2)
            {
                animator.SetTrigger("doubleJump");
            }
            if (currentDoubleJumpCollDown == 1)
            {
                animator.SetTrigger("jump");
            }
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);

        }

    }
    public void ScreenJump(){
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);

    }



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool rockContact()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, rockLayer);
        return raycastHit.collider != null;
    }
    private bool ropeContact()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, ropeLayer);

        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return (isGrounded() || rockContact())  && !ropeContact() && horizontalPress == 0;
    }

    public bool RockInsight()
    {
        // Check if player is insight
        RaycastHit2D raycastHit = Physics2D.BoxCast(attackCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector2(attackCollider.bounds.size.x * range, attackCollider.bounds.size.y), 0, Vector2.left, 0, rockLayer);
        if (raycastHit.collider != null && selectedObject == null)
        {
            // Object detected, prepare to drag
            selectedObject = raycastHit.collider.gameObject;
        }
        return raycastHit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCollider.bounds.center + colliderDistance * range * transform.localScale.x * transform.right, new Vector2(attackCollider.bounds.size.x * range, attackCollider.bounds.size.y));

    }
}

