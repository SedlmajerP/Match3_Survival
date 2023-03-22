using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardManager : MonoBehaviour
{
    [SerializeField] private PlayerBoardManager pBoardManager;
	[SerializeField] private List<GameObject> enemyList;
	[SerializeField] private GameObject enemyBoard;
	[SerializeField] private GameObject backgroundTile;
	[SerializeField] private TrAnimations trAnimations;
	public int enemyMaxHealth = 100;
	public int enemyDamage = 20;
	public int enemyProjectileDamage = 5;
	public int width;
	public int height;
	public GameObject [,] allEnemyArray;
	public GameObject[,] tempEnemyPosArr;
	


	private void Awake()
	{
		width = pBoardManager.width;
		height = pBoardManager.height;
		allEnemyArray = new GameObject[width, height];
		tempEnemyPosArr = new GameObject[width, height];
	}
	

	public void GenerateBackGroundTiles()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vector3 spawnLoacation = new Vector3(x, y, 0) + enemyBoard.transform.position;
				GameObject newTile = Instantiate(backgroundTile, spawnLoacation, Quaternion.identity, enemyBoard.transform);
				newTile.name = $"Tile({x},{y})";
			}
		}
	}

	public void GenerateFirstEnemies(int enemiesThisTurn = 1)
	{
		if (enemiesThisTurn <= width)
		{
			for (int x = 0; x < enemiesThisTurn; x++)
			{
				int randomColumn = Random.Range(0, width);
				int randomIndex = Random.Range(0, enemyList.Count);
				while (allEnemyArray[randomColumn,height-1] != null)
				{
					randomColumn = Random.Range(1, width);
				}
				
				Vector3 spawnLoacation = new Vector3(randomColumn, height - 1, 0) + enemyBoard.transform.position;
				GameObject spawnedEnemy = Instantiate(enemyList[randomIndex], spawnLoacation, Quaternion.identity, enemyBoard.transform);
				spawnedEnemy.name = $"Enemy({randomColumn},{height - 1})";
				spawnedEnemy.GetComponent<EnemyControls>().column = randomColumn;
				spawnedEnemy.GetComponent<EnemyControls>().row = height - 1;

				allEnemyArray[randomColumn, height-1] = spawnedEnemy;
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
				if (allEnemyArray[x,y] != null)
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


	


}


