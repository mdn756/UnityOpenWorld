using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; protected set;}
    public Camera MainCamera;
    public Transform CameraPosition;

    [Header("Control Settings")]
    public float PlayerSpeed = 3.0f;
    public float MouseSensitivity = 100.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;
    float m_VerticalSpeed = 0.0f;
    bool m_IsPaused = false;
    bool m_Grounded;
    float m_VerticalAngle, m_HorizontalAngle;
    public float Speed { get; private set; }
    public bool LockControl { get; set; }
    public bool Grounded => m_Grounded;
    CharacterController m_CharacterController;
    public InputAction playerControls;

    float m_GroundedTimer;
    float m_SpeedAtJump = 0.0f;

    private void OnEnable()
    {
        playerControls.Enable();
    }
    
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        m_IsPaused = false;
        m_Grounded = true;
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wasGrounded = m_Grounded;
        bool loosedGrounding = false;

        if (!m_CharacterController.isGrounded)
        {
            if (m_Grounded)
            {
                m_GroundedTimer += Time.deltaTime;
                if (m_GroundedTimer >= 0.5f)
                {
                    loosedGrounding = true;
                    m_Grounded = false;
                }
            }
        }
        else
        {
            m_GroundedTimer = 0.0f;
            m_Grounded = false;
        }
        Speed = 0;
        Vector3 move = Vector3.zero;
        if (!LockControl)
        {
            if (m_Grounded && Input.GetButtonDown("Jump")) // Player Jumps
            {
                m_VerticalSpeed = JumpSpeed;
                m_Grounded = false;
                loosedGrounding = true;
            }
            bool running = Input.GetButton("Run");
            float actualSpeed = running ? RunningSpeed : PlayerSpeed;
            
            if (loosedGrounding)
            {
                m_SpeedAtJump = actualSpeed;
            }
            
            // WASD
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (move.sqrMagnitude > 1.0f)
            {
                move.Normalize();
            }
            // if grounded, use actual(current) speed. if in air, use speed at jump
            float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump; 
            move = move * usedSpeed * Time.deltaTime;
            move = transform.TransformDirection(move);
            m_CharacterController.Move(move);

            // Turn Player
            float turnPlayer = Input.GetAxis("Mouse X") * MouseSensitivity;
            m_HorizontalAngle = m_HorizontalAngle + turnPlayer;
            
            if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
            if (m_HorizontalAngle < 360) m_HorizontalAngle += 360.0f;
            
            Vector3 currentAngles = transform.localEulerAngles; 
            currentAngles.y = m_HorizontalAngle;
            transform.localEulerAngles = currentAngles;

            // Camera look up/down
            var turnCam = Input.GetAxis("Mouse Y");
            turnCam = turnCam * MouseSensitivity;
            m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
            currentAngles = CameraPosition.transform.localEulerAngles;
            currentAngles.x = m_VerticalAngle;
            CameraPosition.transform.localEulerAngles = currentAngles;

            Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);



        } 
    }

    public void DisplayCursor(bool display)
    {
        m_IsPaused = display;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }
}
