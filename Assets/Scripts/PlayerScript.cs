using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float movementSpeed;
    public float horizontalMovement;
    private bool facingRight;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the components that we will need later
        //RigidBody2D for movement
        rigidbody2D = GetComponent<Rigidbody2D>();
        //Animator to set variables accordingly
        animator = GetComponent<Animator>();
        //Starts facing right
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the horizontal movemente from the Input (Legacy)
        horizontalMovement = Input.GetAxis("Horizontal");

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
    }

    private void Turn()
    {
        //Scale-based turning, negative scaling
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        //Alternate the orientation
        facingRight = !facingRight;
    }
}
