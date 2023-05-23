using UnityEngine;
using UnityEngine.EventSystems;

public enum DamageEffects { None, Fire, Ice, Lightning}

[RequireComponent(typeof(Rigidbody))]
public abstract class ThrowableObject : MonoBehaviour
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

    public abstract void ApplyAttackEffect(DamageEffects effectType);
}