using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] public int m_Health = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Roads"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        }

        if (other.name.Equals("Player"))
        {
            PlayerHUD.Instance.SetObjectInteractionText("Picked up Health Pack");

            if (PlayerStats.Instance.HP + m_Health <= PlayerStats.Instance.MaxHP)
                PlayerStats.Instance.HP += m_Health;
            else
                PlayerStats.Instance.HP = PlayerStats.Instance.MaxHP;

            Destroy(gameObject);
        }
    }
}
