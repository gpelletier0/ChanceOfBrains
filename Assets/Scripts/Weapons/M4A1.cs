using UnityEngine;

public class M4A1 : MonoBehaviour
{
    private const int MAX_AMMO = 100;
    [SerializeField] public int m_CurrentAmmo = MAX_AMMO;
    [SerializeField] public int m_Dammage = 1;
    [SerializeField] public int m_Range = 50;
    [SerializeField] public float m_FireRate = 0.2f;
    [SerializeField] public float m_Timer;
    [SerializeField] public AudioSource m_FireSound;

    private void Update()
    {
        if(m_Timer < m_FireRate)
            m_Timer += Time.deltaTime;
    }

    public void Fire()
    {
        if (m_Timer >= m_FireRate)
        {
            m_Timer = 0f;
            if (m_CurrentAmmo > 0)
            {
                // Muzzle Flash
                m_FireSound.Play();

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
                m_CurrentAmmo--;
            }
        }
    }
}