using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using System.Net;
using UnityEditorInternal;

public class PlayerBoardManager : MonoBehaviour
{
	[SerializeField] private GameObject playerBoard;
	[SerializeField] private List<GameObject> elementList;
	//[SerializeField]	private		GameObject			enemyBoard;
	//[SerializeField]	private		List<GameObject>	enemyList;
	[SerializeField] private MatchManager matchManager;
	[SerializeField] private ShootingManager shootingManager;
	[SerializeField] private TrAnimations animations;
	[SerializeField] private EnemyBoardManager eBoardManager;
	[SerializeField] public bool elementsMoving = false;



	public int width = 6;
	public int height = 6;

	public GameObject[,] allElementsArray;



	private void Start()
	{
		allElementsArray = new GameObject[width, height];
	}


	public void GeneratePlayerBoard()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int randomIndex = Random.Range(0, elementList.Count);
				Vector3 spawnLoacation = new Vector3(x, y, 0) + playerBoard.transform.position;

				while (MatchesAt(x, y, elementList[randomIndex])) //Starting a board with no matches
				{
					randomIndex = Random.Range(0, elementList.Count);
				}
				GameObject spawnedTile = Instantiate(elementList[randomIndex], spawnLoacation, Quaternion.identity, playerBoard.transform);
				spawnedTile.GetComponent<PlayerControls>().column = x;
				spawnedTile.GetComponent<PlayerControls>().row = y;

				spawnedTile.name = $"({x},{y})";
				allElementsArray[x, y] = spawnedTile;
			}
		}


	}

	private bool MatchesAt(int column, int row, GameObject element) //For starting with NO MATCHES in GeneratePlayerBoard()
	{
		if (column > 1)
		{
			if (allElementsArray[column - 1, row].tag == element.tag && allElementsArray[column - 2, row].tag == element.tag)
			{
				return true;
			}
		}
		if (row > 1)
		{
			if (allElementsArray[column, row - 1].tag == element.tag && allElementsArray[column, row - 2].tag == element.tag)
			{
				return true;
			}
		}
		return false;
	}

	private void DestroyMatchesAt(int column, int row)
	{
		if (allElementsArray[column, row].GetComponent<PlayerControls>().isMatched)
		{


			matchManager.matchCounter.Remove(allElementsArray[column, row]);
			Destroy(allElementsArray[column, row]);
			allElementsArray[column, row] = null;

		}
	}

	public void DestroyAllMatches()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allElementsArray[x, y] != null)
				{
					DestroyMatchesAt(x, y);

				}
			}
		}
		StartCoroutine(FillTheBoardCor());

	}


	private IEnumerator FillTheBoardCor()
	{
		yield return new WaitForSeconds(0.2f);

		CollapseRow();

		yield return new WaitForSeconds(0.2f);

		RefillElements();

		yield return new WaitForSeconds(0.5f);

		while (IsMatchedAt())
		{
			elementsMoving = true;
			//shootingManager.ShootThem();
			yield return new WaitForSeconds(0.2f);

			shootingManager.ShootThemDEBUG();

			//yield return new WaitForSeconds(0.2f);

			animations.PlayDestroyAnim();

			yield return new WaitForSeconds(0.5f);

			DestroyAllMatches();

			yield break;
		}




		yield return new WaitForSeconds(0.2f);


		GameManager.Instance.currentState = GameManager.GameState.EnemyTurn;

		yield return new WaitForSeconds(0.5f);

		eBoardManager.allEnemiesAttack();

		yield return new WaitForSeconds(0.5f);

		eBoardManager.MoveAllEnemies();
		//eBoardManager.resetTempEnemyPosArr();

		yield return new WaitForSeconds(0.2f);
		eBoardManager.GenerateFirstEnemies(3);

		yield return new WaitForSeconds(0.5f);

		GameManager.Instance.currentState = GameManager.GameState.PlayerTurn;
		elementsMoving = false;

	}

	private void CollapseRow()
	{
		int nullCount = 0;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allElementsArray[x, y] == null)
				{
					nullCount++;
				}
				else if (nullCount > 0)
				{
					allElementsArray[x, y].GetComponent<PlayerControls>().row -= nullCount;
					allElementsArray[x, y] = null;
				}
			}

			nullCount = 0; //after iterating up one column
		}
	}

	private void RefillElements()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allElementsArray[x, y] == null)
				{
					Vector3 tempPos = new Vector3(x, y, 0) + playerBoard.transform.position;
					int randomIndex = Random.Range(0, elementList.Count);
					GameObject newElement = Instantiate(elementList[randomIndex], tempPos, Quaternion.identity, playerBoard.transform);
					newElement.name = $"({x},{y})";
					newElement.GetComponent<PlayerControls>().column = x;
					newElement.GetComponent<PlayerControls>().row = y;
					allElementsArray[x, y] = newElement;
					Transform newEleTransform = newElement.GetComponent<Transform>();
					newEleTransform.localScale = new Vector2(0, 0);
					animations.PlayRefillAnim(newEleTransform);

				}
			}
		}
	}

	private bool IsMatchedAt()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allElementsArray[x, y] != null && allElementsArray[x, y].GetComponent<PlayerControls>().isMatched)
				{
					return true;
				}
			}
		}
		return false;
	}


















}
