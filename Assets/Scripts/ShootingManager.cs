using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> elementProjectiles;
	[SerializeField] private Transform myTransform;
	private PlayerBoardManager pBoardManager;
	


	private void Awake()
	{
		pBoardManager = FindObjectOfType<PlayerBoardManager>();
	}
	
	private IEnumerator ShootProjectilesFrom()
	{
		yield return new WaitForSeconds(0.5f);

		for (int x = 0; x < pBoardManager.width; x++)
		{
			for (int y = 0; y < pBoardManager.height; y++)
			{
				GameObject element = pBoardManager.allElementsArray[x, y];

				if (element.GetComponent<PlayerControls>().isMatched) 
				{ 
					GameObject projectile = elementProjectiles.Find((proj) => proj.name == pBoardManager.allElementsArray[x,y].tag);
					Vector2 projectilePos = pBoardManager.allElementsArray[x, y].GetComponent<Transform>().GetChild(0).position;
					GameObject newProjectile = Instantiate(projectile, projectilePos, Quaternion.identity, this.transform);
				}
			}
		}
	}

	public void ShootThem()
	{
		StartCoroutine(ShootProjectilesFrom());
	}
}
