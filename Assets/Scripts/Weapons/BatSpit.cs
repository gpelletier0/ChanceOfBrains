using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpit : MonoBehaviour
{
    public float m_Speed = 4f;
    public Transform m_Target;
    public float m_Dammage;

    private Rigidbody m_Rb;
    private Vector3 m_MoveDir;


    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_MoveDir = (m_Target.position - transform.position).normalized * m_Speed;
        m_Rb.velocity = m_MoveDir;

        Destroy(gameObject, m_Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Contains("VampireBat") && !other.name.Contains("BatSpit"))
        { 
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(m_Dammage);
            }

            Destroy(gameObject);
        }
    }
}
