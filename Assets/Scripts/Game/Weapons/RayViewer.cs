using UnityEngine;
using System.Collections;

public class RayViewer : MonoBehaviour
{
    public M4A1 m_Weapon;

    void Update()
    {
        
        Vector3 line = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(line, Camera.main.transform.forward * m_Weapon.m_Range, Color.green);
    }
}