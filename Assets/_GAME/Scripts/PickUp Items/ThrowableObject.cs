using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ThrowableObject : MonoBehaviour
{
    public Rigidbody Rigidbody;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
    }
}