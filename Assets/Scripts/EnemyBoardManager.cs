using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardManager : MonoBehaviour
{
    private PlayerBoardManager pBoardManager;
	[SerializeField] private List<GameObject> enemyList;
	[SerializeField] private GameObject enemyBoard;
	private int width;
	private int height;
	public GameObject [,] allEnemyArray;


	private void Awake()
	{
		pBoardManager = FindObjectOfType<PlayerBoardManager>();
		width = pBoardManager.width;
		height = pBoardManager.height;
		allEnemyArray = new GameObject[width, height];
	}
	//private void Start()
	//{
	//	pBoardManager = FindObjectOfType<PlayerBoardManager>();
	//	width = pBoardManager.width;
	//	height = pBoardManager.height;
	//	allEnemyArray = new GameObject[width, height];
	//}


	public void GenerateEnemies(int enemiesThisTurn = 1)
	{
		Debug.Log("generating enemies...");
		if(enemiesThisTurn <= width - 1)
		{
			for (int x = 0; x <= enemiesThisTurn; x++)
			{
				int randomColumn = Random.Range(0, width - 1);
				int randomIndex = Random.Range(0, enemyList.Count);
				Vector3 spawnLoacation = new Vector3(randomColumn, width - 1, 0) + enemyBoard.transform.position;

				GameObject spawnedEnemy = Instantiate(enemyList[randomIndex], spawnLoacation, Quaternion.identity, enemyBoard.transform);

				spawnedEnemy.name = $"Enemy({randomColumn},{width - 1})";
				allEnemyArray[randomColumn, width - 1] = spawnedEnemy;
			}
		}




		



	}

}


