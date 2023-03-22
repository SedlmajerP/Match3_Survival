using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementProjectile : MonoBehaviour
{
	[SerializeField] private Rigidbody2D projectileRb;
	private HealthBar healthBar;
	public int projectileDamage;
	private int origPosX;

	private void Awake()
	{
		//origPosX = (int)transform.localPosition.x;
	}
	private void Update()
	{
		//projectileRb.velocity = new Vector2(0, 10f);
		if (transform.localPosition.y >= 10)
		{
			Destroy(this.gameObject);
		}
	}

	//private void Awake()
	//{
	//	destroyPos = new Vector2 (transform.localPosition.x, 16);
	//	transform.DOLocalMove(destroyPos, 3f).OnComplete(()=>Destroy(this.gameObject));
	//}

	
	private void OnTriggerEnter2D(Collider2D enemy)
	{
		if (enemy.gameObject.CompareTag("EnemyBasic") || enemy.gameObject.CompareTag("EnemyJumping") || enemy.gameObject.CompareTag("EnemyShooting"))
		{

			EnemyControls enemyControls = enemy.gameObject.GetComponent<EnemyControls>();
			healthBar = enemy.gameObject.GetComponentInChildren<HealthBar>();

			Destroy(this.gameObject);
			enemyControls.health -= projectileDamage;

			if (enemyControls.health <= 0)
			{
				Destroy(enemy.gameObject);
			}
			int maxHealth = FindObjectOfType<EnemyBoardManager>().enemyMaxHealth;
			healthBar.UpdateHealthBar(enemyControls.health, maxHealth);
		}
	}
	//private void OnCollisionEnter2D(Collision2D enemy)
	//{
	//	EnemyControls enemyControls = enemy.gameObject.GetComponent<EnemyControls>();
	//	healthBar = enemy.gameObject.GetComponentInChildren<HealthBar>();

	//	Destroy(this.gameObject);
	//	enemyControls.health -= projectileDamage;

	//	if (enemyControls.health <= 0)
	//	{
	//		Destroy(enemy.gameObject);
	//	}
	//	int maxHealth = FindObjectOfType<EnemyBoardManager>().enemyMaxHealth;
	//	healthBar.UpdateHealthBar(enemyControls.health, maxHealth);
	//}
}
