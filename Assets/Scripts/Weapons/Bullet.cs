using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float m_Dammage;
    
    public float Dammage
    {
        get { return m_Dammage; }
        set { m_Dammage = value; }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log($"Bullet hit: {collider.name}");
        
        if(collider.GetComponent<IDamageable>() != null)
        {
            collider.GetComponent<IDamageable>().TakeDamage(m_Dammage);
        }

        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
