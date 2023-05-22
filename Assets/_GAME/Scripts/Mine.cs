using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : ThrowableObject
{
    [Header("Mine Variables")]
    public float ExplosionInnerDamage = 130f;
    public float ExplosionOuterDamage = 100f;

    [Header("Mine References")]
    public SphereCollider InnerExplosionZone;
    public SphereCollider OuterExplosionZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground")
        {
            RepositionOnLand(other);
        }
        else if (other.transform.tag == "Enemy")
        {
            Explode(other.transform.GetComponent<Enemy>());
        }
    }

    public void Explode(Enemy target)
    {
        //Do fancy explosion stuff

        //Damage all enemies in the zone
    }

    private void OnDrawGizmosSelected()
    {
        //Collider gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, InnerExplosionZone.radius);
        //Trigger Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OuterExplosionZone.radius);
    }

}
