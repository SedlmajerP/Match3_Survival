using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	private TrAnimations trAnimations;

	private PlayerBoardManager pBoardManager;
	private MatchManager matchManager;
	private ShootingManager shootingManager;
	private GameObject otherElement;
	private HealthBar healthBar;
	private SpriteRenderer spriteRenderer;

	//SpriteRenderer mySprite;
	Color defaultColor;

	private Vector3 enlarge = new Vector3(1f, 1f, 0);
	private Vector3 defaultSize = new Vector3(0.8f, 0.8f, 0);
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
		matchManager = FindObjectOfType<MatchManager>();
		shootingManager = FindObjectOfType<ShootingManager>();
		trAnimations = FindObjectOfType<TrAnimations>();
		healthBar = FindObjectOfType<HealthBar>();
	}

	//private void Start()
	//{

	//	pBoardManager = FindObjectOfType<PlayerBoardManager>();
	//	matchManager = FindObjectOfType<MatchManager>();
	//	shootingManager = FindObjectOfType<ShootingManager>();
	//	destroyAnimation = GetComponent<DestroyAnimation>();
	//}

	private void Update()
	{


		MovingElements();
		matchManager.GetMatches();

	}

	private void OnMouseDown()
	{
		if (GameManager.Instance.currentState == GameManager.GameState.PlayerTurn)
		{
			//transform.localScale = enlarge;
			mouseDownPosition = GetMousePosition();
		}
	}

	private Vector2 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void OnMouseUp()
	{


		mouseUpPos = GetMousePosition();
		//transform.localScale = defaultSize;
		if (pBoardManager.elementsMoving == false)
		{
			CalculateAngle();
		}

	}

	private void MovingElements()
	{
		objectOrigPosX = column;
		objectOrigPosY = row;

		if (Mathf.Abs(objectOrigPosY - transform.localPosition.y) > 0.1f)
		{
			tempPos = new Vector2(transform.localPosition.x, objectOrigPosY);
			transform.localPosition = Vector2.Lerp(transform.localPosition, tempPos, movingSpeed * Time.deltaTime);

			if (pBoardManager.allElementsArray[column, row] != this.gameObject)
			{
				pBoardManager.allElementsArray[column, row] = this.gameObject;
			}
		}
		else
		{
			tempPos = new Vector2(transform.localPosition.x, objectOrigPosY);
			transform.localPosition = tempPos;


		}
		if (Mathf.Abs(objectOrigPosX - transform.localPosition.x) > 0.1f)
		{
			tempPos = new Vector2(objectOrigPosX, transform.localPosition.y);
			transform.localPosition = Vector2.Lerp(transform.localPosition, tempPos, movingSpeed * Time.deltaTime);

			if (pBoardManager.allElementsArray[column, row] != this.gameObject)
			{
				pBoardManager.allElementsArray[column, row] = this.gameObject;
			}

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
		pBoardManager.elementsMoving = true;
		StartCoroutine(CheckMachCor());
	}

	IEnumerator CheckMachCor()
	{
		yield return new WaitForSeconds(0.2f);
		if (otherElement != null)
		{
			if (!isMatched && !otherElement.GetComponent<PlayerControls>().isMatched)
			{
				otherElement.GetComponent<PlayerControls>().row = row;
				otherElement.GetComponent<PlayerControls>().column = column;
				row = previousRow;
				column = previousColumn;
				pBoardManager.elementsMoving = false;
			}
			else
			{
				
				
				pBoardManager.HealedByNature();					
				
				//shootingManager.ShootThem();
				yield return new WaitForSeconds(0.2f);

				shootingManager.ShootElemets();

				yield return new WaitForSeconds(0.1f);

				trAnimations.PlayDestroyAnim();

				yield return new WaitForSeconds(0.6f);

				pBoardManager.DestroyAllMatches();

			}
			yield return new WaitForSeconds(0.1f);


		}
		otherElement = null;
	}



}
