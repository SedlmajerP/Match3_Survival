using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
	[SerializeField] private PlayerBoardManager pBoardManager;
	public List<GameObject> matchCounter = new List<GameObject>();


	

	public void GetMatches()
	{
		StartCoroutine(GetMatchesCor());
		
	}
	IEnumerator GetMatchesCor()
	{
		yield return new WaitForSeconds(0.5f);
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

						if ((rightElement != null && leftElement != null) && (rightElement.CompareTag(currentElement.tag) && leftElement.CompareTag(currentElement.tag)))
						{
							rightElement.GetComponent<PlayerControls>().isMatched = true;
							if (!matchCounter.Contains(rightElement))
							{
								matchCounter.Add(rightElement);
							}
							leftElement.GetComponent<PlayerControls>().isMatched = true;

							if (!matchCounter.Contains(leftElement))
							{
								matchCounter.Add(leftElement);
							}
							currentElement.GetComponent<PlayerControls>().isMatched = true;

							if (!matchCounter.Contains(currentElement))
							{
								matchCounter.Add(currentElement);
							}

						}
							
							
					}

					if (y > 0 && y < pBoardManager.height - 1)
					{
						GameObject topElement = pBoardManager.allElementsArray[x, y + 1];
						GameObject bottomElement = pBoardManager.allElementsArray[x, y - 1];

						if ((topElement != null && bottomElement != null) && (topElement.CompareTag(currentElement.tag) && bottomElement.CompareTag(currentElement.tag)))
						{
							
							topElement.GetComponent<PlayerControls>().isMatched = true;
							
							if (!matchCounter.Contains(topElement))
							{
								matchCounter.Add(topElement);
							}
							bottomElement.GetComponent<PlayerControls>().isMatched = true;

							if (!matchCounter.Contains(bottomElement))
							{
								matchCounter.Add(bottomElement);
							}
							currentElement.GetComponent<PlayerControls>().isMatched = true;

							if (!matchCounter.Contains(currentElement))
							{
								matchCounter.Add(currentElement);
							}

						}

					}
				}
			}
		}

		
	}
}
