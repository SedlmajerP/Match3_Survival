using UnityEngine;

public class ElementProjectile : MonoBehaviour
{
	public int projectileDamage;

	private void Update()
	{
		
		if (transform.localPosition.y >= 10)
		{
			Destroy(this.gameObject);
		}
	}

	

	
	private void OnTriggerEnter2D(Collider2D enemy)
	{
		if (enemy.gameObject.CompareTag("EnemyBasic") || enemy.gameObject.CompareTag("EnemyJumping") || enemy.gameObject.CompareTag("EnemyShooting"))
		{

			EnemyControls enemyControls = enemy.gameObject.GetComponent<EnemyControls>();
			HealthBar healthBar = enemy.gameObject.GetComponentInChildren<HealthBar>();

			Destroy(this.gameObject);
			enemyControls.health -= projectileDamage;

			if (enemyControls.health <= 0)
			{
				Destroy(enemy.gameObject);
			}
			int maxHealth = FindObjectOfType<EnemyBoardManager>().enemyMaxHealth;
			healthBar.UpdateHealthBar(enemyControls.health,maxHealth, "Enemy");
		}
	}
	
}
