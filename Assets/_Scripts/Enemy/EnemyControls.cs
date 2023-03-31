using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyControls : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	[SerializeField] SpriteRenderer spriteRenderer;
	private EnemyBoardManager eBoardManager;

	public int column;
	public int row;
	public int health;

	public bool enemyLastRowAttack = false;
	public bool isFrozen = false;
	private Color frozenColor = new Color(0f, 0f, 255f, 255f);

	private void Awake()
	{
		eBoardManager = FindObjectOfType<EnemyBoardManager>();
		health = eBoardManager.enemyMaxHealth;
	}


	public void Move()
	{
		StartCoroutine(MoveAll());
	}

	private IEnumerator MoveAll()
	{
		if (isFrozen == false)
		{
			MoveBasicEnemy();
			moveShootingEnemy();
			MoveJumpingEnemy();
		}
		yield return new WaitForSeconds(0.8f);
		isFrozen = false;
			spriteRenderer.color = Color.white;
		
	}

	private void MoveBasicEnemy()
	{
		if (CompareTag("EnemyBasic"))
		{
			if (enemyLastRowAttack == false)
			{
				Vector2 basicEnemyNextPos = new Vector2(column, row - 1);
				if (eBoardManager.allEnemyArray[column, row - 1] == null)
				{

					eBoardManager.allEnemyArray[column, row] = null;
					column = (int)basicEnemyNextPos.x;
					row = (int)basicEnemyNextPos.y;
					eBoardManager.allEnemyArray[column, row] = this.gameObject;

					transform.DOLocalMove(basicEnemyNextPos, 0.5f);

					if (row == 0)
					{
						enemyLastRowAttack = true;
					}
				}

			}

		}
	}

	private void MoveJumpingEnemy()
	{
		if (CompareTag("EnemyJumping"))
		{
			if (enemyLastRowAttack == false)
			{
				int randJumpColumn = Random.Range(0, eBoardManager.width-1);
				int randJumpRow = Random.Range(0, eBoardManager.height-1);

				while (eBoardManager.allEnemyArray[randJumpColumn, randJumpRow] != null)
				{
					randJumpColumn = Random.Range(0, eBoardManager.width-1);
					randJumpRow = Random.Range(0, eBoardManager.height-1);
				}

				Vector2 jumpingEnemyNextPos = new Vector2(randJumpColumn, randJumpRow);

				eBoardManager.allEnemyArray[column, row] = null;
				column = (int)jumpingEnemyNextPos.x;
				row = (int)jumpingEnemyNextPos.y;
				eBoardManager.allEnemyArray[column, row] = this.gameObject;
				transform.DOLocalMove(jumpingEnemyNextPos, 0.8f);
				transform.DOScale(1.5f, 0.4f).OnComplete(() =>
				{
					transform.DOScale(0.9f, 0.4f);
				});

				if (row == 0)
				{
					enemyLastRowAttack = true;
				}

			}

		}

	}

	private void moveShootingEnemy()
	{
		if (CompareTag("EnemyShooting"))
		{
			if (enemyLastRowAttack == false)
			{
				Vector2 basicEnemyNextPos = new Vector2(column, row - 1);
				if (eBoardManager.allEnemyArray[column, row - 1] == null)
				{

					eBoardManager.allEnemyArray[column, row] = null;
					column = (int)basicEnemyNextPos.x;
					row = (int)basicEnemyNextPos.y;
					eBoardManager.allEnemyArray[column, row] = this.gameObject;

					transform.DOLocalMove(basicEnemyNextPos, 0.5f);

					if (row == 0)
					{
						enemyLastRowAttack = true;
					}
				}
			}
		}
	}

	public void enemyAttack()
	{
		if (enemyLastRowAttack == true && isFrozen == false)
		{

			transform.DOScale(1.1f, 0.2f).OnComplete(() =>
			{
				transform.DOLocalMoveY(row + 0.2f, 0.1f).OnComplete(() =>
				{
					transform.DOLocalMoveY(row - 0.5f, 0.2f);
				});
			});
		}

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("IceProjectile"))
		{
			isFrozen = true;
			spriteRenderer.color = frozenColor;
		}
	}
}
