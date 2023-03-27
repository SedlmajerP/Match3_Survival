using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardManager : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private PlayerBoardManager pBoardManager;
	[SerializeField] private TrAnimations trAnimations;



	[Header("Enemies")]
	[SerializeField] private GameObject basicEnemy;
	[SerializeField] private GameObject jumpingEnemy;
	[SerializeField] private GameObject shootingEnemy;
	public int enemyMaxHealth = 100;
	public int enemyDamage = 20;
	public int enemyProjectileDamage = 5;
	[HideInInspector] public List<GameObject> enemyList;

	[Header("BackGroundTile")]
	[SerializeField] private GameObject backgroundTile;
	[SerializeField] SpriteRenderer tileRenderer;
	[SerializeField] Color tileColor;
	[SerializeField] Color offsetTileColor;

	
	public int width;
	public int height;

	public GameObject[,] allEnemyArray;



	private void Awake()
	{
		width = pBoardManager.width;
		height = pBoardManager.height;
		allEnemyArray = new GameObject[width, height];
		enemyList = new List<GameObject>();
		GenerateBackGroundTiles();
		GenerateEnemies(3, 1);
	}

	private void Start()
	{

	}

	public void GenerateBackGroundTiles()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
				Debug.Log($"{x},{y}");
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

	public void MoveAllEnemies()
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

	public void allEnemiesAttack()
	{
		for (int x = 0; x < width; x++)
		{
			if (allEnemyArray[x, 0] != null)
			{
				allEnemyArray[x, 0].GetComponent<EnemyControls>().enemyAttack();
			}
		}
	}

	public bool IsAnyoneFrozen()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{

				if (allEnemyArray[x, y].GetComponent<EnemyControls>().isFrozen == true)
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


