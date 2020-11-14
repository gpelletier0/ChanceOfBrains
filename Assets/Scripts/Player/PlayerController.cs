using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Walk/Run")]
    [SerializeField] public float walkSpeed = 2.5f;
    [SerializeField] public float rungSpeed = 6f;

    [Header("Jump")]
    [SerializeField] public float jumpSpeed = 8.0f;
    [SerializeField] public float gravity = 20.0f;

    [Header("Stamina")]
    [SerializeField] public float staminaDepletion = 5.0f;
    [SerializeField] public float staminaRecovery = 15.0f;

    [Header("Camera")]
    [SerializeField] public float lookSpeed = 2.0f;
    [SerializeField] public float lookXLimit = 60.0f;
    [SerializeField] public Transform playerCameraParent;

    private Vector3 m_moveDir = Vector3.zero;
    private Vector2 m_rotation = Vector2.zero;

    private float m_velocityX;
    private float m_velocityZ;
    private float m_velocityY;
    private bool m_isRunning;
    private bool m_canMove;

    private PlayerHUD m_PlayerHUD;
    private PlayerStats m_PlayerStats = PlayerStats.Instance;
    //private PlayerInput m_PlayerInput;

    private Animator m_Animator;
    private CharacterController m_CharController;

    public bool CanMove
    {
        get { return m_canMove; }
        set { m_canMove = value; }
    }

    private void Awake()
    {
        m_canMove = true;
        m_CharController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_PlayerHUD = GameObject.Find("PlayerHUD").GetComponent<PlayerHUD>();
    }

    private void Start()
    {
        m_rotation.y = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (m_canMove)
        {
            if (m_CharController.isGrounded)
            {
                // Landed
                m_Animator.SetBool("isJumping", false);

                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                m_isRunning = Input.GetKey(KeyCode.LeftShift);

                m_velocityX = (!m_isRunning ? walkSpeed : getStaminaRunSpeed()) * Input.GetAxis("Vertical");
                m_velocityZ = (!m_isRunning ? walkSpeed : getStaminaRunSpeed()) * Input.GetAxis("Horizontal");
                m_velocityY = jumpSpeed * Input.GetAxis("Jump");

                m_moveDir = (forward * m_velocityX) + (right * m_velocityZ);

                if (Input.GetButton("Jump") && m_PlayerStats.ST >= m_PlayerStats.minJumpST)
                {
                    // Jumped
                    m_Animator.SetBool("isJumping", true);
                    m_moveDir.y = jumpSpeed;
                }

                // Set animation
                m_Animator.SetFloat("VelocityX", m_velocityX / rungSpeed);
                m_Animator.SetFloat("VelocityZ", m_velocityZ / rungSpeed);
            }

            // Character controler move
            m_moveDir.y -= gravity * Time.deltaTime;
            m_CharController.Move(m_moveDir * Time.deltaTime);

            // Camera move
            m_rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            m_rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            m_rotation.x = Mathf.Clamp(m_rotation.x, -lookXLimit, lookXLimit);

            playerCameraParent.localRotation = Quaternion.Euler(m_rotation.x, 0.0f, 0.0f);
            transform.eulerAngles = new Vector2(0f, m_rotation.y);

            // Stamina Degen/Regen
            if (m_isRunning && (m_moveDir.x != 0 || m_moveDir.z != 0))
                m_PlayerStats.ST = Mathf.Max(m_PlayerStats.ST - staminaDepletion * Time.deltaTime, 0.0f);
            else
                m_PlayerStats.ST = Mathf.Min(m_PlayerStats.ST + staminaRecovery * Time.deltaTime, m_PlayerStats.maxST);
        }
    }

    private void FixedUpdate()
    {
        m_PlayerHUD.UpdateStats();
    }

    private float getStaminaRunSpeed()
    {
        return Mathf.Lerp(walkSpeed, rungSpeed, m_PlayerStats.ST / m_PlayerStats.maxST);
    }

    public void TakeDamage(float dmg)
    {
        if(m_PlayerStats.HP > 0)
        {
            Debug.Log("Player hit for: " + dmg);
            m_PlayerStats.HP -= dmg;
        }
    }
}