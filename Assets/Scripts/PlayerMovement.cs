using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isReversed;


    private Rigidbody2D rigidbody;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private int currentDoubleJumpCollDown;

    private float horizontalPress;
    private float verticalPress;
    private float ropeCollDown = 0.5f;
    private float currentCollDown;

    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private LayerMask ropeLayer;
  public float gravity = 9.8f; // Standard gravity



    void FixedUpdate()
    {
        // Example condition to check if the object is upside down
        if (transform.up.y < 0)
        {
            // Apply custom gravity upwards
            rigidbody.AddForce(Vector2.up * gravity);
        }
        else
        {
            // Apply custom gravity downwards
            rigidbody.AddForce(Vector2.down * gravity);
        }
    }

    
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentDoubleJumpCollDown = 0;
    }
    private void Start() {
        rigidbody.gravityScale = -1;
        
    }
    private void Update() {
        FixedUpdate();
        if (isGrounded()){
            currentDoubleJumpCollDown = 0;
        }
        if(isReversed){
            rigidbody.gravityScale = -1;
        }
        currentCollDown += Time.deltaTime;
        horizontalPress = Input.GetAxis("Horizontal");
        if (!ropeContact()){
        if(horizontalPress > 0) {
            transform.localScale = new Vector2(1.5f, transform.localScale.y);
        } else if(horizontalPress < 0) {
            transform.localScale = new Vector2(-1.5f, transform.localScale.y);
        }
        rigidbody.velocity = new Vector2(horizontalPress * speed, rigidbody.velocity.y);
        if (isGrounded()){
        animator.SetBool("runing", horizontalPress != 0);
        } else {
            animator.SetBool("runing", false);
        }
        }
       
        if (Input.GetKey(KeyCode.Space) && ropeContact()){
                jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded() || currentDoubleJumpCollDown <= 2)){
                jump();
        }

        if(ropeContact()){
            currentCollDown += Time.deltaTime;
            Debug.Log("Rope contact");
            rigidbody.gravityScale = 0;
            animator.SetBool("runing", false);
            animator.SetBool("climbIdle", true);
            verticalPress = Input.GetAxis("Vertical");
            if (verticalPress > 0){
            animator.SetBool("climbIdle", false);
            animator.SetBool("climb", true);
                rigidbody.velocity = new Vector2( rigidbody.velocity.x, verticalPress * speed);
            } else if (verticalPress < 0){
                animator.SetBool("climbIdle", false);
                animator.SetBool("climb", true);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalPress * speed);
            } else {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                animator.SetBool("climb", false);
                
            } 
            if(horizontalPress > 0 && currentCollDown > ropeCollDown && !Input.GetKey(KeyCode.Space)) {
            transform.localScale = new Vector2(-1.5f,transform.localScale.y);
        } else if(horizontalPress < 0 && currentCollDown > ropeCollDown && !Input.GetKey(KeyCode.Space))  {
            transform.localScale = new Vector2(1.5f, transform.localScale.y);
        }
            
        } 
        else {
            currentCollDown = 0;
            rigidbody.gravityScale = 1;
            animator.SetBool("climb", false);
            animator.SetBool("climbIdle", false);

        }
        animator.SetBool("grounded", isGrounded());
        

    }
    private void jump(){
        currentDoubleJumpCollDown += 1; 
        Debug.Log(" current double jump" + currentDoubleJumpCollDown + isGrounded());
        if(ropeContact()){
            if (horizontalPress != 0){
        animator.SetTrigger("jump");
            // transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            rigidbody.velocity = new Vector2(-1 * transform.localScale.x * 3, rigidbody.velocity.y + 5);}

            } 
            if(Input.GetKeyDown(KeyCode.Space)){
            if (currentDoubleJumpCollDown == 2){
            Debug.Log("Double Jump");
                animator.SetTrigger("doubleJump");
            }
            if(currentDoubleJumpCollDown == 1){
                animator.SetTrigger("jump");
                }
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);

            }

}
        
        

        private bool isGrounded(){
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0, Vector2.down, 0.1f, groundLayer);
            return raycastHit.collider != null;
    }
       private bool ropeContact(){
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0, new Vector2(transform.localScale.x, 0), 0.1f, ropeLayer);

            return raycastHit.collider != null;
    }
    public bool canAttack(){
        return isGrounded() && !ropeContact() && horizontalPress == 0;
    }
}

