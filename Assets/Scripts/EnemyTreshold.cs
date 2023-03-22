using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTreshold : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private EnemyBoardManager eBoardManager;

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.tag);
		if (other.gameObject.CompareTag("EnemyBasic") || other.gameObject.CompareTag("EnemyJumping") || other.gameObject.CompareTag("EnemyShooting"))
		{
			GameManager.Instance.playerHealth -= eBoardManager.enemyDamage;

			healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
			Destroy(other.gameObject);
		}

		if (other.gameObject.CompareTag("EnemyProjectile"))
		{
			GameManager.Instance.playerHealth -= eBoardManager.enemyProjectileDamage;
			healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
			Destroy(other.gameObject);
		}
	}

}
