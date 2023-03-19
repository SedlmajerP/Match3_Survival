using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyControls : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	private EnemyBoardManager eBoardManager;
	public int column;
	public int row;
	public int health;
	public bool enemyLastRowAttack = false;

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
		MoveBasicEnemy();
		//yield return new WaitForSeconds(0.6f);
		MoveJumpingEnemy();
		yield break;
	}


	private bool isRowFull(int rowNum)
	{
		int numOfEnemies = 0;
		for (int x = 0; x < eBoardManager.width; x++)
		{
			if (eBoardManager.allEnemyArray[x, rowNum] != null || eBoardManager.tempEnemyPosArr[x, rowNum] != null)
			{
				numOfEnemies++;
			}
		}
		if (numOfEnemies < eBoardManager.width - 1)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	private bool isTempRowFull(int rowNum)
	{
		int numOfEnemies = 0;
		for (int x = 0; x < eBoardManager.width; x++)
		{
			if (eBoardManager.tempEnemyPosArr[x, rowNum] != null)
			{
				numOfEnemies++;
			}
		}
		if (numOfEnemies < eBoardManager.width - 1)
		{
			return false;
		}
		else
		{
			return true;
		}
	}


	private void MoveBasicEnemy()
	{
		if (CompareTag("EnemyBasic") || CompareTag("EnemyShooting"))
		{
			if (enemyLastRowAttack == false)
			{
				Vector2 basicEnemyNextPos = new Vector2(column, row - 1);
				if (eBoardManager.allEnemyArray[column, row - 1] == null)
				{

					eBoardManager.allEnemyArray[column, row] = null;
					column = (int)basicEnemyNextPos.x;
					row = (int)basicEnemyNextPos.y;
					Debug.Log(basicEnemyNextPos);
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
				int randJumpColumn = Random.Range(0, eBoardManager.width - 1);
				int randJumpRow = Random.Range(0, eBoardManager.height - 1);

				while (eBoardManager.allEnemyArray[randJumpColumn, randJumpRow] != null)
				{
					randJumpColumn = Random.Range(0, eBoardManager.width - 1);
					randJumpRow = Random.Range(0, eBoardManager.height - 1);
				}

				Vector2 jumpingEnemyNextPos = new Vector2(randJumpColumn, randJumpRow);

				eBoardManager.allEnemyArray[column, row] = null;
				column = (int)jumpingEnemyNextPos.x;
				row = (int)jumpingEnemyNextPos.y;
				Debug.Log(jumpingEnemyNextPos);
				eBoardManager.allEnemyArray[column, row] = this.gameObject;
				transform.DOLocalMove(jumpingEnemyNextPos, 0.5f);

				if (row == 0)
				{
					enemyLastRowAttack = true;
				}

			}

		}

	}

	public void enemyAttack()
	{
		if (enemyLastRowAttack == true)
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
}
