using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public Mine Parent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground" && !Parent.Exploded)
        {
            Parent.RepositionOnLand(other);
        }
        else if (other.transform.tag == "Enemy")
        {
            Parent.Explode(other.transform.GetComponent<Enemy>());
            Parent.Exploded = true;
            Destroy(this.transform.parent.gameObject);
        }
    }
}
