using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : ThrowableObject
{
    [Header("Mine Variables")]
    public float ExplosionInnerDamage = 130f;
    public float ExplosionOuterDamage = 100f;
    public DamageEffects Type = DamageEffects.None;
    public bool Exploded;

    [Header("Mine References")]
    //public Collider InnerExplosionZone;
    public SphereCollider OuterExplosionZone;
    [SerializeField] private TriggerDetector detector;

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.transform.tag == "Ground" && !exploded)
    //    {
    //        RepositionOnLand(other);
    //    }
    //    else if (other.transform.tag == "Enemy")
    //    {
    //        Explode(other.transform.GetComponent<Enemy>());
    //        exploded = true;
    //        Destroy(this.gameObject);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Ground" && !exploded)
    //    {
    //        RepositionOnLand(other);
    //    }
    //    else if (other.transform.tag == "Enemy")
    //    {
    //        Explode(other.transform.GetComponent<Enemy>());
    //        exploded = true;
    //        Destroy(this.transform.parent.gameObject);
    //    }
    //}

    public void Explode(Enemy target)
    {
        //Do fancy explosion stuff

        //Do a particle explosion ?
        Debug.Log("Exploded");
        //Do an explosion sound

        //Do an explosion Haptic

        //Damage all enemies in the zone
        target.TakeDamageAndEffect(ExplosionInnerDamage, Type);
        foreach (var item in detector.GetList())
        {
            item.GetComponent<Enemy>().TakeDamageAndEffect(ExplosionOuterDamage, Type);
        }
        detector.ClearCollection();
        ApplyAttackEffect(Type);
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

    public override void ApplyAttackEffect(DamageEffects effectType)
    {
        switch (effectType)
        {
            case DamageEffects.None:
                //Just apply damage and maybe knockback

                break;
            case DamageEffects.Fire:
                //Create a fire ring for a period of time, anyone enters it will be get burned

                break;
            case DamageEffects.Ice:
                //Create a ice ring, anyone in it will be freezed for a period of time

                break;
            case DamageEffects.Lightning:
                //Create a lightning zone, anyone in it will be hit by lightning and lightning will be jumped neares X enemies
                //Every jump will be decrease damage by %Y percent

                break;
            default:

                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Collider gizmos
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireMesh(transform.position, InnerExplosionZone.radius);
        //Trigger Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OuterExplosionZone.radius);
    }

}
