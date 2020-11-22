using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHealthPack : MonoBehaviour
{
    [SerializeField] public int m_Health = 100;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Roads"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        }

        if (other.name.Equals("Player"))
        {
            PlayerHUD.Instance.SetObjectInteractionText("Picked up Super Health Pack");
            PlayerStats.Instance.HP += m_Health;
            Destroy(gameObject);
        }
    }
}
