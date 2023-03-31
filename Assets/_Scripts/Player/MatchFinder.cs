using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MatchFinder : MonoBehaviour
{
	[SerializeField] private PlayerBoardManager pBoardManager;
	public List<GameObject> matchCounter = new List<GameObject>();
	

	public IEnumerator FindMatches()
	{
		yield return new WaitForSeconds(0.1f);
		for (int x = 0; x < pBoardManager.width; x++)
		{
			for (int y = 0; y < pBoardManager.height; y++)
			{
				GameObject currentElement = pBoardManager.allElementsArray[x, y];

				if (currentElement != null)
				{
					if (x > 0 && x < pBoardManager.width - 1)
					{
						GameObject rightElement = pBoardManager.allElementsArray[x + 1, y];
						GameObject leftElement = pBoardManager.allElementsArray[x - 1, y];

						SetMatches(rightElement, leftElement, currentElement);
					}

					if (y > 0 && y < pBoardManager.height - 1)
					{
						GameObject topElement = pBoardManager.allElementsArray[x, y + 1];
						GameObject bottomElement = pBoardManager.allElementsArray[x, y - 1];

						SetMatches (topElement, bottomElement, currentElement);
					}
				}
			}
		}

		
	}

	private void SetMatches(GameObject elementRightOrTop, GameObject elementLeftOrBottom, GameObject currentElement)
	{
		if ((elementRightOrTop != null && elementLeftOrBottom != null) && (elementRightOrTop.CompareTag(currentElement.tag) && elementLeftOrBottom.CompareTag(currentElement.tag)))
		{
			AddToListAndMatch(elementRightOrTop);
			AddToListAndMatch(elementLeftOrBottom);
			AddToListAndMatch(currentElement);

		}
	}

	private void AddToListAndMatch(GameObject element)
	{
		element.GetComponent<PlayerControls>().isMatched = true;
		if (!matchCounter.Contains(element))
		{
			matchCounter.Add(element);
		}
	}

}
