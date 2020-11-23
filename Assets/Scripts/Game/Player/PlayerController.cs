using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class PlayerController : Singleton<PlayerController>, IDamageable
{
    [Header("Stats")]
    [SerializeField] [Range(0, PlayerStats.DEFAULT_STAT)] public float m_HP;
    [SerializeField] [Range(0, PlayerStats.DEFAULT_STAT)] public float m_ST;
    [SerializeField] [Range(0, PlayerStats.DEFAULT_STAT)] public float m_MinJumpST;

    [Header("Walk/Run")]
    [SerializeField] public float m_WalkSpeed = 2.5f;
    [SerializeField] public float m_RunSpeed = 6f;

    [Header("Jump")]
    [SerializeField] public float m_JumpSpeed = 8.0f;
    [SerializeField] public float m_Gravity = 20.0f;
    [SerializeField] public float m_JumpRate = 1.2f;
    [SerializeField] public float m_JumpStam = 10f;

    [Header("Stamina")]
    [SerializeField] public float m_StaminaDepletion = 5.0f;
    [SerializeField] public float m_StaminaRecovery = 15.0f;

    [Header("Camera")]
    [SerializeField] public float m_LookSpeed = 2.0f;
    [SerializeField] public float m_LookXLimit = 60.0f;
    [SerializeField] public Transform m_PlayerCameraParent;

    [Header("Weapon")]
    [SerializeField] public M4A1 m_Wepon;

    private Vector3 m_MoveDir = Vector3.zero;
    private Vector2 m_Rotation = Vector2.zero;
    
    private float m_Vertical;
    private float m_Horizontal;
    private bool m_isRunning;
    private float m_JumpTimer;

    private PlayerHUD m_PlayerHUD;
    private Animator m_Animator;
    private CharacterController m_CharController;


    /// <summary>
    /// Freezes the player
    /// </summary>
    public bool CanMove {get; set;}


    /// <summary>
    /// Initialize Class to default
    /// </summary>
    public void Initialize()
    {
        CanMove = true;
        
        PlayerStats.Instance.HP = m_HP;
        PlayerStats.Instance.ST = m_ST;
        PlayerStats.Instance.MinJumpST = m_MinJumpST;
        PlayerStats.Instance.AmmoCount = m_Wepon.m_StartAmmo;
    }

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    protected override void Awake()
    {
        //m_Wepon = GetComponent<M4A1>();
        m_CharController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_PlayerHUD = GameObject.Find("PlayerHUD").GetComponent<PlayerHUD>();
    }

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    private void Start()
    {
        CanMove = true;
        PlayerStats.Instance.HP = m_HP;
        PlayerStats.Instance.ST = m_ST;
        PlayerStats.Instance.MinJumpST = m_MinJumpST;
        PlayerStats.Instance.AmmoCount = m_Wepon.m_StartAmmo;

        m_Rotation.y = transform.eulerAngles.y;
    }

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    private void Update()
    {
        if (CanMove)
        {
            if (PlayerStats.Instance.HP <= 0)
                Die();

            if (m_JumpTimer > 0f)
                m_JumpTimer -= Time.deltaTime;

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
                m_Vertical = (!m_isRunning ? m_WalkSpeed : GetRunSpeed()) * Input.GetAxis("Vertical");
                m_Horizontal = (!m_isRunning ? m_WalkSpeed : GetRunSpeed()) * Input.GetAxis("Horizontal");
                //m_Jump = m_JumpSpeed * Input.GetAxis("Jump");

                // Move direction vector
                m_MoveDir = (forward * m_Vertical) + (right * m_Horizontal);

                // Check for Jump Button press, player stamina and jumpTimer
                if (Input.GetButton("Jump") &&
                    PlayerStats.Instance.ST >= PlayerStats.Instance.MinJumpST &&
                    m_JumpTimer <= 0)
                {
                    // Jumped
                    m_Animator.SetBool("isJumping", true);
                    m_MoveDir.y = m_JumpSpeed;
                    
                    m_JumpTimer = m_JumpRate;
                    PlayerStats.Instance.ST -= m_JumpStam;
                }

                // Set animation
                m_Animator.SetFloat("Vertical", m_Vertical / m_RunSpeed);
                m_Animator.SetFloat("Horizontal", m_Horizontal / m_RunSpeed);
            }

            // Character controler move
            m_MoveDir.y -= m_Gravity * Time.deltaTime;
            m_CharController.Move(m_MoveDir * Time.deltaTime);

            // Camera move
            m_Rotation.y += Input.GetAxis("Mouse X") * m_LookSpeed;
            m_Rotation.x += -Input.GetAxis("Mouse Y") * m_LookSpeed;
            m_Rotation.x = Mathf.Clamp(m_Rotation.x, -m_LookXLimit, m_LookXLimit);

            m_PlayerCameraParent.localRotation = Quaternion.Euler(m_Rotation.x, 0.0f, 0.0f);
            transform.eulerAngles = new Vector2(0f, m_Rotation.y);

            // Stamina Degen/Regen
            if (m_isRunning && (m_MoveDir.x != 0 || m_MoveDir.z != 0))
                PlayerStats.Instance.ST = Mathf.Max(PlayerStats.Instance.ST - m_StaminaDepletion * Time.deltaTime, 0.0f);
            else
                PlayerStats.Instance.ST = Mathf.Min(PlayerStats.Instance.ST + m_StaminaRecovery * Time.deltaTime, PlayerStats.Instance.MaxST);
        }
    }

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    private void FixedUpdate()
    {
        m_PlayerHUD.UpdateStats();
    }

    /// <summary>
    /// Get the current run speed
    /// </summary>
    /// <returns></returns>
    private float GetRunSpeed()
    {
        return Mathf.Lerp(m_WalkSpeed, m_RunSpeed, PlayerStats.Instance.ST / PlayerStats.Instance.MaxST);
    }

    /// <summary>
    /// IDamagable GiveDamage implementation
    /// </summary>
    public void GiveDamage()
    {
        m_Wepon.Fire();
    }

    /// <summary>
    /// IDamagable TakeDamage implementation
    /// </summary>
    public void TakeDamage(float dmg)
    {
        if (PlayerStats.Instance.HP > 0)
        {
            Debug.Log("Player hit for: " + dmg);
            PlayerStats.Instance.HP -= dmg;
        }
    }

    /// <summary>
    /// IDamagable Die implementation
    /// </summary>
    public void Die()
    {
        m_Animator.SetBool("isDead", true);
    }
}