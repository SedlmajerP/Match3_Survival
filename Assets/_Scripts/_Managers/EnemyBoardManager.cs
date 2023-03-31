
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardManager : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private PlayerBoardManager pBoardManager;
	[SerializeField] private TrAnimations trAnimations;
	[SerializeField] private MainUI mainUI;
	[SerializeField] private ProjectileSpawner projectileSpawner;



	[Header("Enemies")]
	[SerializeField] private GameObject basicEnemy;
	[SerializeField] private GameObject jumpingEnemy;
	[SerializeField] private GameObject shootingEnemy;
	public int enemyMaxHealth = 100;
	public int enemyDamage = 20;
	public int enemyProjectileDamage = 5;
	[SerializeField] private List<GameObject> enemyList;

	[Header("BackGroundTile")]
	[SerializeField] private GameObject backgroundTile;
	[SerializeField] SpriteRenderer tileRenderer;
	[SerializeField] Color tileColor;
	[SerializeField] Color offsetTileColor;


	public int width;
	public int height;

	public GameObject[,] allEnemyArray;



	public void Init()
	{
		width = pBoardManager.width;
		height = pBoardManager.height;
		allEnemyArray = new GameObject[width, height];
		enemyList = new List<GameObject>();
		GenerateBackGroundTiles();
		GenerateEnemies(3, 1);
	}

	public void GenerateBackGroundTiles()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
				if (isOffset == true)
				{
					tileRenderer.color = offsetTileColor;
				}
				else if (isOffset == false)
				{
					tileRenderer.color = tileColor;

				}
				Vector2 tilePos = new Vector3(x, y, 0) + gameObject.transform.position;
				GameObject newTile = Instantiate(backgroundTile, tilePos, Quaternion.identity, gameObject.transform);

			}
		}
	}

	public void GenerateEnemies(int enemiesThisTurn = 1, int numWaves = 1)
	{
		if (enemiesThisTurn <= width)
		{
			enemyPooling(numWaves);
			for (int x = 0; x < enemiesThisTurn; x++)
			{
				int randomColumn = Random.Range(0, width);
				int randomIndex = Random.Range(0, enemyList.Count);
				while (allEnemyArray[randomColumn, height - 1] != null)
				{
					randomColumn = Random.Range(0, width);
				}

				Vector3 spawnLoacation = new Vector3(randomColumn, height - 1, 0) + gameObject.transform.position;
				GameObject spawnedEnemy = Instantiate(enemyList[randomIndex], spawnLoacation, Quaternion.identity, gameObject.transform);
				spawnedEnemy.name = $"Enemy({randomColumn},{height - 1})";
				spawnedEnemy.GetComponent<EnemyControls>().column = randomColumn;
				spawnedEnemy.GetComponent<EnemyControls>().row = height - 1;

				allEnemyArray[randomColumn, height - 1] = spawnedEnemy;
				Transform spawnedEnemyTr = spawnedEnemy.GetComponent<Transform>();
				spawnedEnemyTr.localScale = new Vector2(0, 0);
				trAnimations.PlaySpawnAnim(spawnedEnemyTr);

			}
		}
	}

	public IEnumerator EnemyTurnCor()
	{
		yield return new WaitForSeconds(0.2f);

		GameManager.Instance.currentState = GameManager.GameState.EnemyTurn;

		yield return new WaitForSeconds(0.2f);

		if (EnemyBoardClear() == false)
		{
			if (isAnyoneAttacking() == true)
			{
				allEnemiesAttack();
				yield return new WaitForSeconds(0.6f);
			}
			if (EnemyTypeOnBoard("EnemyShooting") == true)
			{
				projectileSpawner.EnemyShoot();
				yield return new WaitForSeconds(0.6f);
			}

			MoveAllEnemies();

			if (EnemyTypeOnBoard("EnemyJumping") == true)
			{
				yield return new WaitForSeconds(0.8f);
			}
			else
			{
				yield return new WaitForSeconds(0.6f);

			}

			if (IsAnyoneFrozen() == true)
			{
				yield return new WaitForSeconds(0.3f);

			}
		}

		enemyList.Clear();
		GenerateEnemies(3, GameManager.Instance.numWaves);

		yield return new WaitForSeconds(0.6f);

		GameManager.Instance.setMaxWaves();
		PlayerPrefs.SetInt("maxWaves", GameManager.Instance.maxWaves);
		mainUI.UpdateWavesText();

		GameManager.Instance.currentState = GameManager.GameState.PlayerTurn;
		pBoardManager.elementsMoving = false;

	}

	private void MoveAllEnemies()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allEnemyArray[x, y] != null)
				{
					allEnemyArray[x, y].GetComponent<EnemyControls>().Move();
				}
			}
		}
	}

	private void allEnemiesAttack()
	{
		for (int x = 0; x < width; x++)
		{
			if (allEnemyArray[x, 0] != null)
			{
				allEnemyArray[x, 0].GetComponent<EnemyControls>().enemyAttack();
			}
		}
	}

	private bool IsAnyoneFrozen()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allEnemyArray[x, y] != null && allEnemyArray[x, y].GetComponent<EnemyControls>().isFrozen == true)
				{

					return true;
				}

			}
		}
		return false;
	}

	private bool isAnyoneAttacking()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allEnemyArray[x, y] != null && allEnemyArray[x, y].GetComponent<EnemyControls>().enemyLastRowAttack == true)
				{

					return true;
				}

			}
		}
		return false;
	}

	private bool EnemyBoardClear()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allEnemyArray[x, y] != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	private bool EnemyTypeOnBoard(string enemyTag)
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allEnemyArray[x, y] != null && allEnemyArray[x, y].CompareTag(enemyTag))
				{

					return true;
				}

			}
		}
		return false;
	}

	private void enemyPooling(int numwave = 1)
	{
		int basicEnemyCount = 30;
		int jumpingEnemyCount = 20;
		int shootngEnemyCount = 10;
		for (int i = 0; i < basicEnemyCount; i++)
		{
			enemyList.Add(basicEnemy);
		}

		if (numwave >= 5)
		{
			for (int i = 0; i < jumpingEnemyCount; i++)
			{
				enemyList.Add(jumpingEnemy);
			}
		}

		if (numwave >= 10)
		{
			for (int i = 0; i < shootngEnemyCount; i++)
			{
				enemyList.Add(shootingEnemy);
			}
		}
	}



}


