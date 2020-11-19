using System.Collections;
using UnityEngine;

public class M4A1 : MonoBehaviour
{
    private const string BULLET_PREFAB = "Prefabs/Weapons/Bullet";

    [Header("Gun Stats")]
    public int m_StartAmmo = 100;
    public int m_Dammage = 1;
    public float m_FireRate = 0.25f;
    public float m_Range = 50f;
    public Transform m_FirePoint;
    public AudioSource m_FireSound;
    
    private float m_Timer;
    private LineRenderer m_BulletLine;

    private void Start()
    {
        m_BulletLine = GetComponent<LineRenderer>();
        m_BulletLine.enabled = false;
    }

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

            if (PlayerStats.Instance.AmmoCount > 0)
            {
                StartCoroutine(ShotEffect());

                Vector3 ray = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hit;
                m_BulletLine.SetPosition(0, m_FirePoint.position);
                
                if (Physics.Raycast(ray, Camera.main.transform.forward, out hit, m_Range, ~LayerMask.GetMask("Player")))
                {
                    m_BulletLine.SetPosition(1, hit.point);
                    if (hit.collider.GetComponent<IDamageable>() != null)
                        hit.collider.GetComponent<IDamageable>().TakeDamage(m_Dammage);
                }
                else
                {
                    m_BulletLine.SetPosition(1, ray + (Camera.main.transform.forward * m_Range));
                }
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        // Reduce player ammo
        PlayerStats.Instance.AmmoCount--;

        // Play the shooting sound
        m_FireSound.Play();
        
        // Muzzle Flash Particles

        // Show bullet line
        m_BulletLine.enabled = true;

        yield return new WaitForSeconds(0.07f);

        // Hide bullet line
        m_BulletLine.enabled = false;
    }
}