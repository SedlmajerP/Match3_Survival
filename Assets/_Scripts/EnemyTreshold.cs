using UnityEngine;

public class EnemyTreshold : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private EnemyBoardManager eBoardManager;
	[SerializeField] private GameObject loosePanel;
	[SerializeField] private MainUI mainUI;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("EnemyBasic") || other.gameObject.CompareTag("EnemyJumping") || other.gameObject.CompareTag("EnemyShooting"))
		{
			GameManager.Instance.playerHealth -= eBoardManager.enemyDamage;
			if (GameManager.Instance.playerHealth <= 0)
			{
				GameManager.Instance.playerHealth = 0;
				mainUI.LooseGame();
			}

			healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth, "Player");
			Destroy(other.gameObject);
		}

		if (other.gameObject.CompareTag("EnemyProjectile"))
		{
			GameManager.Instance.playerHealth -= eBoardManager.enemyProjectileDamage;
			if (GameManager.Instance.playerHealth <= 0)
			{
				GameManager.Instance.playerHealth = 0;
				mainUI.LooseGame();
			}
			healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth, "Player");
			Destroy(other.gameObject);
		}
	}

	
}

	
