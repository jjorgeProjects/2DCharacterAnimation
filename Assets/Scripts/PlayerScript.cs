using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    //Visible variables to control:
    // Character's movement speed
    public float movementSpeed;
    // Character's jump force
    public float jumpForce;

    // Variable to store the input axis changes
    private Vector2 movement;
    private float horizontalMovement;

    //Bool variables to control:
    //If the character is grounded
    public bool isGrounded = false;
    //If the character is facing the right direction
    private bool facingRight;
  

    //Unity components
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private PlayerInput input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the component   s that we will need later
        //RigidBody2D for movement
        rigidbody2D = GetComponent<Rigidbody2D>();
        //Animator to set variables accordingly
        animator = GetComponent<Animator>();

        input = GetComponent<PlayerInput>();

        //Starts facing right
        facingRight = true;
        //Starts in the air, ergo not grounded
        isGrounded = false;

        //Avoiding collisions with the internal collider
        Physics2D.queriesStartInColliders = false;
    }

// Update is called once per frame
void Update()
    {
        //Get the horizontal movemente from the Input (Legacy)
        //horizontalMovement = Input.GetAxis("Horizontal");

        movement = input.actions["Move"].ReadValue<Vector2>();
        horizontalMovement = movement.x;

        //Set the float variable for the animator to change the state
        animator.SetFloat("movementSpeed", Mathf.Abs(horizontalMovement));

        //Dealing with the facing, if the character changes his direction...
        if (horizontalMovement < 0 && facingRight || horizontalMovement > 0 && !facingRight)
        {
            //...then turn
            Turn();
        }

        
    }

    private void FixedUpdate()
    {
        //Update the velocity vector, the X component, using the input value times the movement speed
        rigidbody2D.linearVelocityX = horizontalMovement * movementSpeed;

        animator.SetFloat("verticalVelocity", rigidbody2D.linearVelocityY);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

        //If the character is not grounded and the raycast detected the collision closer to 0.4f
        if (!isGrounded && rigidbody2D.linearVelocityY < 0f && hit.collider != null && hit.distance < 0.15f)
        {
            //Mark that now is grounded
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
            rigidbody2D.linearVelocityX = 0;
        }

        if (Input.GetAxis("Jump") > 0 && isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log(rigidbody2D.linearVelocityY);
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    /*
    public void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log(callbackContext.valueType);

            movement = callbackContext.ReadValue<Vector2>();
            Debug.Log(movement);
        }

    }*/

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log(rigidbody2D.linearVelocityY);
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    private void Turn()
    {
        //Scale-based turning, negative scaling
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        //Alternate the orientation
        facingRight = !facingRight;
    }
}
