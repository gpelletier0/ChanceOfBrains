using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform m_ReferenceTransform;
    public float m_CollisionOffset = 0.2f; //To prevent Camera from clipping through Objects

    Vector3 m_DefaultPos;
    Vector3 m_DirectionNormalized;
    Transform m_ParentTransform;
    float m_DefaultDistance;

    // Start is called before the first frame update
    void Start()
    {
        m_DefaultPos = transform.localPosition;
        m_DirectionNormalized = m_DefaultPos.normalized;
        m_ParentTransform = transform.parent;
        m_DefaultDistance = Vector3.Distance(m_DefaultPos, Vector3.zero);
    }

    // FixedUpdate for physics calculations
    void FixedUpdate()
    {
        Vector3 currentPos = m_DefaultPos;
        RaycastHit hit;
        Vector3 dirTmp = m_ParentTransform.TransformPoint(m_DefaultPos) - m_ReferenceTransform.position;
        if (Physics.SphereCast(m_ReferenceTransform.position, m_CollisionOffset, dirTmp, out hit, m_DefaultDistance))
        {
            currentPos = (m_DirectionNormalized * (hit.distance - m_CollisionOffset));
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 15f);
    }
}