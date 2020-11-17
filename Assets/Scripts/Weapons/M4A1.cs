using System.Collections;
using UnityEngine;

public class M4A1 : MonoBehaviour
{
    private const int MAX_AMMO = 100;
    private const string BULLET_PREFAB = "Prefabs/Weapons/Bullet";

    [Header("Gun Stats")]
    public int m_CurrentAmmo = MAX_AMMO;
    public int m_Dammage = 1;
    //public int m_Range = 50;
    public float m_FireRate = 0.2f;
    public float m_Timer;
    public AudioSource m_FireSound;

    [Header("Bullet")]
    [SerializeField] public Transform m_FirePoint;
    [SerializeField] public float m_BulletSpeed = 40;
    [SerializeField] public float m_LifeTime = 3;

    private GameObject m_PreviousBullet;
    private void Update()
    {
        if(m_Timer < m_FireRate)
            m_Timer += Time.deltaTime;
    }

    public void Fire(Vector3 rot)
    {
        if (m_Timer >= m_FireRate)
        {
            m_Timer = 0f;

            if (m_CurrentAmmo > 0)
            {
                /*****************************************************************************************
                 * Bullet object Code
                ******************************************************************************************/
                GameObject bullet = Instantiate(Resources.Load<GameObject>(BULLET_PREFAB));
                //GameObject bullet = ObjectPooler.GetObject(BULLET_PREFAB_PATH);

                bullet.GetComponent<Bullet>().Dammage = m_Dammage;
                bullet.SetActive(true);

                // Ignore previous bullet collider
                if (m_PreviousBullet != null)
                    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), m_PreviousBullet.GetComponent<Collider>());
                m_PreviousBullet = bullet;

                // Bullet position and rotation
                bullet.transform.position = m_FirePoint.position;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0).x + rot.x, transform.eulerAngles.y, 0);
                
                // Move bullet
                bullet.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(rot.x, rot.y, 0) * Vector3.forward * m_BulletSpeed, ForceMode.Impulse);

                // Destroy bullet after lifetime
                StartCoroutine(DestroyBullet(bullet, m_LifeTime));

                // Muzzle Flash Particles
                m_FireSound.Play();

                /*****************************************************************************************
                 * Raycast Code
                ******************************************************************************************/
                //Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
                //Debug.DrawRay(ray.origin, ray.direction * m_Range, Color.red, 2f);
                //RaycastHit hitInfo;

                //if (Physics.Raycast(ray, out hitInfo, m_Range))
                //{
                //    if (hitInfo.collider.tag.Equals("Enemy"))
                //    {
                //        IDamageable enemy = hitInfo.collider.GetComponent<IDamageable>();
                //        if (enemy != null)
                //        {
                //            enemy.TakeDamage(m_Dammage);
                //        }
                //    }
                //}

                //m_CurrentAmmo--;
            }
        }
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
        //bullet.SetActive(false);
    }
}