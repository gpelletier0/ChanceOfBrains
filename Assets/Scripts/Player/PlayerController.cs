using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] [Range(0, PlayerStats.DEFAULT_STAT)] public float m_HP;
    [SerializeField] [Range(0, PlayerStats.DEFAULT_STAT)] public float m_ST;
    [SerializeField] [Range(0, PlayerStats.DEFAULT_STAT)] public float m_MinJumpST;

    [Header("Walk/Run")]
    [SerializeField] public float walkSpeed = 2.5f;
    [SerializeField] public float runSpeed = 6f;

    [Header("Jump")]
    [SerializeField] public float jumpSpeed = 8.0f;
    [SerializeField] public float gravity = 20.0f;
    [SerializeField] public float jumpRate = 1.2f;
    [SerializeField] public float jumpTimer;
    [SerializeField] public float jumpStam = 10f;

    [Header("Stamina")]
    [SerializeField] public float staminaDepletion = 5.0f;
    [SerializeField] public float staminaRecovery = 15.0f;

    [Header("Camera")]
    [SerializeField] public float lookSpeed = 2.0f;
    [SerializeField] public float lookXLimit = 60.0f;
    [SerializeField] public Transform playerCameraParent;

    [Header("Weapon")]
    [SerializeField] public M4A1 Wepon;

    private Vector3 m_moveDir = Vector3.zero;
    private Vector2 m_rotation = Vector2.zero;

    private float m_Vertical;
    private float m_Horizontal;
    private float m_Jump;
    private bool m_isRunning;
    private bool m_canMove = true;

    private PlayerHUD m_PlayerHUD;
    private Animator m_Animator;
    private CharacterController m_CharController;

    public bool CanMove
    {
        get => m_canMove;
        set => m_canMove = value;
    }

    private void Awake()
    {
        m_CharController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_PlayerHUD = GameObject.Find("PlayerHUD").GetComponent<PlayerHUD>();
    }

    private void Start()
    {
        PlayerStats.Instance.HP = m_HP;
        PlayerStats.Instance.ST = m_ST;
        PlayerStats.Instance.minJumpST = m_MinJumpST;
        PlayerStats.Instance.AmmoCount = Wepon.m_CurrentAmmo;

        m_rotation.y = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(PlayerStats.Instance.HP <=0)
        {
            Die();
        }

        if (jumpTimer <= jumpRate)
            jumpTimer += Time.deltaTime;

        if (m_canMove)
        {
            if (Input.GetButton("Fire1"))
            {
                GiveDamage();
            }

            if (m_CharController.isGrounded)
            {
                // Landed
                m_Animator.SetBool("isJumping", false);

                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                m_isRunning = Input.GetKey(KeyCode.LeftShift);

                // Current direction/speed
                m_Vertical = (!m_isRunning ? walkSpeed : GetRunSpeed()) * Input.GetAxis("Vertical");
                m_Horizontal = (!m_isRunning ? walkSpeed : GetRunSpeed()) * Input.GetAxis("Horizontal");
                m_Jump = jumpSpeed * Input.GetAxis("Jump");

                // Move direction vector
                m_moveDir = (forward * m_Vertical) + (right * m_Horizontal);

                if (Input.GetButton("Jump") && PlayerStats.Instance.ST >= PlayerStats.Instance.minJumpST && jumpTimer >= jumpRate)
                {
                    // Jumped
                    m_Animator.SetBool("isJumping", true);
                    m_moveDir.y = jumpSpeed;
                    jumpTimer = 0f;
                    PlayerStats.Instance.ST -= jumpStam;
                }

                // Set animation
                m_Animator.SetFloat("Vertical", m_Vertical / runSpeed);
                m_Animator.SetFloat("Horizontal", m_Horizontal / runSpeed);
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
                PlayerStats.Instance.ST = Mathf.Max(PlayerStats.Instance.ST - staminaDepletion * Time.deltaTime, 0.0f);
            else
                PlayerStats.Instance.ST = Mathf.Min(PlayerStats.Instance.ST + staminaRecovery * Time.deltaTime, PlayerStats.Instance.maxST);
        }
    }

    private void FixedUpdate()
    {
        m_PlayerHUD.UpdateStats();
    }

    private float GetRunSpeed()
    {
        return Mathf.Lerp(walkSpeed, runSpeed, PlayerStats.Instance.ST / PlayerStats.Instance.maxST);
    }

    public void GiveDamage()
    {
        Wepon.Fire(m_rotation);
        PlayerStats.Instance.AmmoCount = Wepon.m_CurrentAmmo;
    }

    public void TakeDamage(float dmg)
    {
        if (PlayerStats.Instance.HP > 0)
        {
            Debug.Log("Player hit for: " + dmg);
            PlayerStats.Instance.HP -= dmg;
        }
    }

    public void Die()
    {
        m_canMove = false;
        m_Animator.SetBool("isDead", true);
    }
}