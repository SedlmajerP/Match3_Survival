using UnityEngine;

public class EnemyTreshold : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private EnemyBoardManager eBoardManager;
	[SerializeField] private PlayerBoardManager playerBoardManager;
	[SerializeField] private GameObject loosePanel;
	[SerializeField] private MainUI mainUI;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("EnemyBasic") || other.gameObject.CompareTag("EnemyJumping") || other.gameObject.CompareTag("EnemyShooting"))
		{
			AfterHitLogic(other.gameObject,eBoardManager.enemyDamage);
		}

		if (other.gameObject.CompareTag("EnemyProjectile"))
		{
			AfterHitLogic(other.gameObject,eBoardManager.enemyProjectileDamage);
		}
	}

	private void AfterHitLogic(GameObject enemy, int damageType)
	{
		playerBoardManager.playerHealth -= damageType;
		if (playerBoardManager.playerHealth <= 0)
		{
			playerBoardManager.playerHealth = 0;
			mainUI.LooseGame();
		}
		healthBar.UpdateHealthBar(playerBoardManager.playerHealth, playerBoardManager.playerMaxHealth, "Player");
		Destroy(enemy);
	}
	
}

	
