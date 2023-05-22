using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	[Range(0f, 4f)]
	[Tooltip("Increases lifetime of a bullet.")]
	[SerializeField] private float bulletTimeExtend = 2f;

	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.GetComponent<Bullet>())
		{
			//Reset bullets removal timer to 0 
			col.gameObject.GetComponent<Bullet>().DecreaseBounce = 1;
		}
	}
}
