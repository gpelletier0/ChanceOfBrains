using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class AmmoPack : MonoBehaviour
{
    [SerializeField] public int m_Ammo = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Roads"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(transform.position.x, other.transform.position.y + 0.2f, transform.position.z);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHUD.Instance.SetObjectInteractionText("Picked up Ammo");

            PlayerStats.Instance.AmmoCount += m_Ammo;
            Destroy(gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
}
