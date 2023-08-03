using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float PlayerSpeed = 7.0f;
    public float JumpForce = 300f;
    private Animator playerAnim;
    //bool m_IsPaused = false;
    //float aerialSpeed = 1f;
    bool isGrounded = false;
    
    //float m_VerticalAngle, m_HorizontalAngle;
    //public float Speed { get; private set; }
    //public bool LockControl { get; set; }
    //public bool Grounded => m_Grounded;
    //CharacterController m_CharacterController;
    private PlayerInputActions playerControls;
    Vector2 heading = Vector2.zero;
    //private InputAction move;
    //private InputAction fire;
    //private InputAction jump;
    //float m_GroundedTimer;
    //private Vector3 velAtJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerControls = new PlayerInputActions();
        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Fire.performed += Fire;
    }
    /* private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log("new thing");
    } */
    private void OnEnable()
    {
        playerControls.Player.Enable();
        //move = playerControls.Player.Move; // InputManager > Action Map > Action
        //move.Enable();
        
        //jump = playerControls.Player.Jump;
        //jump.Enable();
        //fire = playerControls.Player.Fire;
        //fire.Enable();
        //fire.performed += Fire;
        
    }
    private void OnDisable()
    {
        playerControls.Player.Disable();
        //move.Disable();
        //fire.Disable();
        //jump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        heading = playerControls.Player.Move.ReadValue<Vector2>();
        rb.velocity = new Vector3(heading.x * PlayerSpeed, rb.velocity.y, heading.y * PlayerSpeed);
        rb.velocity = transform.TransformDirection(rb.velocity); 
        playerAnim.SetFloat("MoveSpeed", rb.velocity.magnitude * Mathf.Sign(heading.y));

        if (Input.GetKeyDown(KeyCode.T) && isGrounded)
        {
            playerAnim.SetBool("Wave", true);
        }
        if (Input.GetKeyDown(KeyCode.E) && isGrounded)
        {
            playerAnim.SetTrigger("Chop");
        }
        if (Input.GetKeyDown(KeyCode.R) && isGrounded)
        {
            playerAnim.SetTrigger("Test");
        }
        /* if (isGrounded)
        {
            rb.velocity = new Vector3(heading.x * PlayerSpeed, rb.velocity.y, heading.y * PlayerSpeed);
            rb.velocity = transform.TransformDirection(rb.velocity);
        }
        else
        {
            TODO: Aerial strafing. only to turn or decelerate. probably add vector.
            rb.velocity = new Vector3(heading.x * PlayerSpeed, rb.velocity.y, heading.y * PlayerSpeed);
            rb.velocity = transform.TransformDirection(rb.velocity);
        } */
    }
    
    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("We fired.");
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce , ForceMode.Impulse);
            isGrounded = false;
            playerAnim.SetBool("Grounded", isGrounded);
            playerAnim.SetBool("Wave", false);
            //velAtJump = rb.velocity;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            isGrounded = true;
            playerAnim.SetBool("Grounded", isGrounded);
            //TODO: Make landing sound
            //TODO: float animation needs to trigger when the player is NOT in contact with a surface
        }
        else if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            playerAnim.SetBool("Pickup", true);
            //TODO: Make item get sound
        }
    }
}
