using UnityEngine;

public class M4A1 : MonoBehaviour
{
    public const float m_MaxAmmo = 100;
    [SerializeField] [Range(0, m_MaxAmmo)] public float m_Ammo = m_MaxAmmo;
    [SerializeField] public float m_Dammage = 1;
    [SerializeField] public float m_Range = 50;
    [SerializeField] public float m_FireRate = 0.5f;
    [SerializeField] public float m_Timer;
    [SerializeField] public Transform m_firePoint;

    private void Awake()
    {
        m_Ammo = m_MaxAmmo;
        m_firePoint = gameObject.GetComponent<Transform>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
    }

    public void Fire()
    {
        if (m_Timer >= m_FireRate)
        {
            m_Timer = 0f;
            if (m_Ammo > 0)
            {
                // Muzzle Flash
                // Play Gunshot sound

                Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
                Debug.DrawRay(ray.origin, ray.direction * m_Range, Color.red, 2f);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, m_Range))
                {
                    if (hitInfo.collider.tag.Equals("Enemy"))
                    {
                        IDamageable enemy = hitInfo.collider.GetComponent<IDamageable>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(m_Dammage);
                        }
                    }
                }
                m_Ammo--;
            }
        }
    }
}