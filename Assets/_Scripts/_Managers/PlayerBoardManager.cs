using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardManager : MonoBehaviour
{
	[SerializeField] private GameObject playerBoard;
	[SerializeField] private List<GameObject> elementList;
	[SerializeField] private MatchFinder matchFinder;
	[SerializeField] private ProjectileSpawner projectileSpawner;
	[SerializeField] private TrAnimations animations;
	[SerializeField] private EnemyBoardManager eBoardManager;
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private MainUI mainUI;

	[SerializeField] private int healAmount = 5;
	[SerializeField] public int playerMaxHealth = 100;
	[SerializeField] public int playerHealth;




	public bool elementsMoving = false;
	public int width = 6;
	public int height = 6;

	public GameObject[,] allElementsArray;



	public void Init()
	{
		playerHealth = playerMaxHealth;
		allElementsArray = new GameObject[width, height];
		GeneratePlayerBoard();
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

				Transform tileTransform = spawnedTile.GetComponent<Transform>();

				tileTransform.localScale = Vector3.zero;
				animations.PlaySpawnAnim(tileTransform);

				spawnedTile.GetComponent<PlayerControls>().column = x;
				spawnedTile.GetComponent<PlayerControls>().row = y;

				spawnedTile.name = $"({x},{y})";

				allElementsArray[x, y] = spawnedTile;
			}
		}
	}

	//For starting with NO MATCHES in GeneratePlayerBoard()
	private bool MatchesAt(int column, int row, GameObject element) 
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
			matchFinder.matchCounter.Remove(allElementsArray[column, row]);
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
		StartCoroutine(matchFinder.FindMatches());
		yield return new WaitForSeconds(0.5f);
		
		while (IsMatchedAt())
		{
			elementsMoving = true;

			//yield return new WaitForSeconds(0.2f);

			HealedByNature();
			projectileSpawner.ShootElemets();
			PlayDestroyAnim();

			yield return new WaitForSeconds(0.5f);

			DestroyAllMatches();

			yield break;
		}

		StartCoroutine(eBoardManager.EnemyTurnCor());
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
					animations.PlaySpawnAnim(newEleTransform);

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

	public void HealedByNature()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allElementsArray[x, y].GetComponent<PlayerControls>().isMatched && allElementsArray[x, y].CompareTag("Nature"))
				{
					int missingHelth = playerMaxHealth - playerHealth;
					if (missingHelth >= healAmount)
					{
						playerHealth += healAmount;
						healthBar.UpdateHealthBar(playerHealth,playerMaxHealth, "Player");
					}
					else if (missingHelth < healAmount && missingHelth != 0)
					{
						playerHealth += missingHelth;
					}
				}
			}


		}
	}

	public void PlayDestroyAnim()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allElementsArray[x, y].GetComponent<PlayerControls>().isMatched)
				{
					Transform elementTranform = allElementsArray[x, y].GetComponent<Transform>();
					if (elementTranform != null)
					{
						elementTranform.DOScale(1, 0.2f).OnComplete(() =>
						{
							elementTranform.DOScale(0, 0.2f);
						});
					}
				}
			}

		}
	}


}






















