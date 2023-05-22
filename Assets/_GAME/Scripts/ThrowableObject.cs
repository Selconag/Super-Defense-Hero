using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ThrowableObject : MonoBehaviour
{
    [Header("ThrowableObject Variables")]
    [Tooltip("Damage Caused when object hits an enemy directly")]
    public float PhysicalDamage;
    public int ObjectLevel = 1;
    public bool Landed = true;
    [Header("ThrowableObject References")]
    public Rigidbody Rigidbody;


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.constraints = RigidbodyConstraints.FreezePosition;
    }

    //Objects will reposition themselves according to their normals(Flip to original pos)
    public void RepositionOnLand(Collider point)
    {
        if (Landed) return;
        Vector3 contactPoint = point.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.position = contactPoint;
        Landed = true;
        Debug.Log("Landed");
    }
}