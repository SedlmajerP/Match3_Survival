using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	private PlayerBoardManager pBoardManager;
	private MatchFinder matchFinder;
	private ProjectileSpawner projectileSpawner;
	private GameObject otherElement;


	private Vector2 mouseDownPosition;
	private Vector2 mouseUpPos;
	private Vector2 tempPos;

	private float dragAngle = 0;
	private float minimalDrag = 1f;
	public float movingSpeed = 12f;
	public int column;
	public int row;
	private int previousColumn;
	private int previousRow;
	private int objectOrigPosX;
	private int objectOrigPosY;
	public bool isMatched = false;

	private void Awake()
	{
		pBoardManager = FindObjectOfType<PlayerBoardManager>();
		matchFinder = FindObjectOfType<MatchFinder>();
		projectileSpawner = FindObjectOfType<ProjectileSpawner>();
	}

	private void Update()
	{
		MovingElements();
	}

	private Vector2 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void OnMouseDown()
	{
		if (pBoardManager.elementsMoving == false)
		{
			mouseDownPosition = GetMousePosition();
		}
	}

	private void OnMouseUp()
	{
		if (pBoardManager.elementsMoving == false)
		{

			mouseUpPos = GetMousePosition();
			CalculateAngle();
		}
	}

	private void MovingElements()
	{
		objectOrigPosX = column;
		objectOrigPosY = row;

		if (Mathf.Abs(objectOrigPosY - transform.localPosition.y) > 0.1f)
		{
			pBoardManager.elementsMoving = true;
			tempPos = new Vector2(transform.localPosition.x, objectOrigPosY);
			transform.localPosition = Vector2.Lerp(transform.localPosition, tempPos, movingSpeed * Time.deltaTime);


			if (pBoardManager.allElementsArray[column, row] != this.gameObject)
			{
				pBoardManager.allElementsArray[column, row] = this.gameObject;
			}
			StartCoroutine(matchFinder.FindMatches(pBoardManager.allElementsArray));


		}
		else
		{
			tempPos = new Vector2(transform.localPosition.x, objectOrigPosY);
			transform.localPosition = tempPos;
		}
		if (Mathf.Abs(objectOrigPosX - transform.localPosition.x) > 0.1f)
		{
			pBoardManager.elementsMoving = true;

			tempPos = new Vector2(objectOrigPosX, transform.localPosition.y);
			transform.localPosition = Vector2.Lerp(transform.localPosition, tempPos, movingSpeed * Time.deltaTime);

			if (pBoardManager.allElementsArray[column, row] != this.gameObject)
			{
				pBoardManager.allElementsArray[column, row] = this.gameObject;
			}
			StartCoroutine(matchFinder.FindMatches(pBoardManager.allElementsArray));

		}
		else

		{
			tempPos = new Vector2(objectOrigPosX, transform.localPosition.y);
			transform.localPosition = tempPos;
		}


	}

	private void CalculateAngle()
	{
		if (Mathf.Abs(mouseUpPos.x - mouseDownPosition.x) > minimalDrag || Mathf.Abs(mouseUpPos.y - mouseDownPosition.y) > minimalDrag)
		{
			dragAngle = Mathf.Atan2(mouseUpPos.y - mouseDownPosition.y, mouseUpPos.x - mouseDownPosition.x) * 180 / Mathf.PI;
			MovingLogic();
		}
	}

	private void MovingLogic()
	{

		if (dragAngle > 45 && dragAngle < 135 && row < pBoardManager.height - 1)
		{
			otherElement = pBoardManager.allElementsArray[column, row + 1];
			previousColumn = column;
			previousRow = row;
			otherElement.GetComponent<PlayerControls>().row -= 1;
			row += 1;
			//UP

		}
		else if ((dragAngle < -45 && dragAngle > -135) && row > 0)
		{
			otherElement = pBoardManager.allElementsArray[column, row - 1];
			previousColumn = column;
			previousRow = row;
			otherElement.GetComponent<PlayerControls>().row += 1;
			row -= 1;
			//DOWN
		}
		else if ((dragAngle > 135 || dragAngle < -135) && column > 0)
		{
			otherElement = pBoardManager.allElementsArray[column - 1, row];
			previousColumn = column;
			previousRow = row;
			otherElement.GetComponent<PlayerControls>().column += 1;
			column -= 1;
			//LEFT
		}
		else if (dragAngle > -45 && dragAngle < 45 && column < pBoardManager.width - 1)
		{
			otherElement = pBoardManager.allElementsArray[column + 1, row];
			previousColumn = column;
			previousRow = row;
			otherElement.GetComponent<PlayerControls>().column -= 1;
			column += 1;
			//RIGHT
		}
		StartCoroutine(CheckMachCor());
	}

	IEnumerator CheckMachCor()
	{
		yield return new WaitForSeconds(0.3f);
		if (otherElement != null)
		{
			if (!isMatched && !otherElement.GetComponent<PlayerControls>().isMatched)
			{
				otherElement.GetComponent<PlayerControls>().row = row;
				otherElement.GetComponent<PlayerControls>().column = column;
				row = previousRow;
				column = previousColumn;

				yield return new WaitForSeconds(0.3f);
				pBoardManager.elementsMoving = false;
			}
			else
			{
				pBoardManager.HealedByNature();
				projectileSpawner.ShootElemets();
				pBoardManager.PlayDestroyAnim();

				yield return new WaitForSeconds(0.6f);

				pBoardManager.DestroyAllMatches();

			}
			yield return new WaitForSeconds(0.1f);


		}
		//otherElement = null;
	}



}
