using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForcePlayerMove : MonoBehaviour
{

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float accelForce = 20;


    // player related vars
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 hVelo;
    private Quaternion rotation;
    private float jumpSpeed;
    private float jumpImpulse;
    private bool inTeleporter;
    private bool onGround;
    private bool movingGround;

    //var to save platforms velocity
    private Vector3 groundVelocity;


    //Input Axes 
    private float hInput;
    private float vInput;
    private bool jumpInput;
    private bool horizontalInput;
    private bool verticalInput;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        inTeleporter = false;

        // calculates impulse required to obtain designer specified jump height
        jumpSpeed = Mathf.Sqrt(2 * Constants.getGravity() * jumpHeight);
        jumpImpulse = (rb.mass * jumpSpeed);

        //isolates horizontal velocity of player
        hVelo = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rotation = Quaternion.identity;
   
    }

    // Update is called once per frame
    void Update()
    {

        //gets inputs
        hInput = Input.GetAxis(InputAxes.Horizontal);
        vInput = Input.GetAxis(InputAxes.Vertical);
        jumpInput = jumpInput || Input.GetButtonDown(InputAxes.Jump);
        horizontalInput = Input.GetButton(InputAxes.Horizontal);
        verticalInput = Input.GetButton(InputAxes.Vertical);


        // updates the game manager with current player position
        GameManager.Instance.UpdatePlayerPos(transform.position);
        


    }

    private void FixedUpdate()
    {

        // inputs added to new vector
        moveDirection = transform.InverseTransformDirection(new Vector3(hInput, 0, vInput));

        hVelo = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (!movingGround)
        {
            groundVelocity = Vector3.zero;
        }

        // movement
        if (horizontalInput || verticalInput || onGround)
        {
            Vector3 maxMoveDir;
            Vector3 velocity;
            Vector3 moveForce;
            if (moveDirection.magnitude > 1)
            {
                maxMoveDir = moveDirection.normalized;
            }
            else
            {
                maxMoveDir = moveDirection;
            }

            velocity = (maxMoveDir * maxSpeed) + groundVelocity;
            moveForce = (velocity - hVelo) * accelForce;
            rb.AddForce(moveForce);
        }

        // Jumping 
        if (jumpInput && !inTeleporter && onGround)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            jumpInput = false;  
        }
        else if (!onGround) // ensure no buffering of jump
        {
            jumpInput = false;
        }

        // Falling
        if (rb.velocity.y > -maxFallSpeed)
        {
            rb.AddForce(Vector3.down * Constants.getGravity(), ForceMode.Acceleration); // add additional acceleration until terminal velocity specified
        }

    }

    public void killPlayer()
    {

        GameManager.Instance.RestartAtCheckpoint(); // spawns a new avatar and sets camera to new avatar
        GameManager.Instance.PlayerDied(); // writes analytics 
        Destroy(gameObject); // destroyes old player

    }

    private void OnCollisionExit(Collision collision)
    {

        if(collision.gameObject.layer == 3) // ground
        {
            onGround = false;
            movingGround = false;
        }

    }

    private void OnCollisionStay(Collision collision)
    {

        // turn on friction and vary depending on ground type i.e moving platform or not
        if (collision.gameObject.layer == 3) // ground
        {
            if (collision.rigidbody != null)
            {
                movingGround = true;
                groundVelocity = new Vector3(collision.rigidbody.velocity.x, 0, collision.rigidbody.velocity.z);
            }
            else
            {
                movingGround = false;
            }
            onGround = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 9) // checkpoint
        {
            GameManager.Instance.SetPlayerCheckpoint(other.gameObject); // set checkpoint
        }

    }
    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.layer == 10) // telporter
        {
            inTeleporter = true;
            if (jumpInput)
            {
                jumpInput = false;
                GameManager.Instance.LevelComplete(); // move to next level
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject.layer == 10) // leave teleporter without utilising
        {
            inTeleporter = false;
        }

    }

}



