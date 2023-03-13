using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   [SerializeField] private Rigidbody2D projectileRb;

	private void Update()
	{
		projectileRb.velocity = new Vector2(0, 10f);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(other.gameObject);
		Destroy(this.gameObject);
	}
}
