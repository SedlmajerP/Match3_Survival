using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> elementProjectiles;
	[SerializeField] private Transform myTransform;
	[SerializeField] private PlayerBoardManager pBoardManager;
	public int fireDamage = 50;
	public int iceDamage = 30;
	public int natureDamage = 30;
	public int earthDamage = 40;




	
	
	//private IEnumerator ShootProjectilesFrom()
	//{
	//	yield return new WaitForSeconds(0.3f);

	//	for (int x = 0; x < pBoardManager.width; x++)
	//	{
	//		for (int y = 0; y < pBoardManager.height; y++)
	//		{
	//			GameObject element = pBoardManager.allElementsArray[x, y];

	//			if (element.GetComponent<PlayerControls>().isMatched) 
	//			{ 
	//				GameObject projectile = elementProjectiles.Find((proj) => proj.name == pBoardManager.allElementsArray[x,y].tag);
	//				Vector2 projectilePos = pBoardManager.allElementsArray[x, y].GetComponent<Transform>().GetChild(0).position;
	//				GameObject newProjectile = Instantiate(projectile, projectilePos, Quaternion.identity, this.transform);
	//			}
	//		}
	//	}
	//}

	//public void ShootThem()
	//{
	//	StartCoroutine(ShootProjectilesFrom());
	//}


	public void ShootThemDEBUG()
	{
		for (int x = 0; x < pBoardManager.width; x++)
		{
			for (int y = 0; y < pBoardManager.height; y++)
			{
				GameObject element = pBoardManager.allElementsArray[x, y];

				if (element.GetComponent<PlayerControls>().isMatched)
				{
					GameObject projectile = elementProjectiles.Find((proj) => proj.name == pBoardManager.allElementsArray[x, y].tag);
					Vector2 projectilePos = pBoardManager.allElementsArray[x, y].GetComponent<Transform>().GetChild(0).position;
					GameObject newProjectile = Instantiate(projectile, projectilePos, Quaternion.identity, this.transform);
					if(newProjectile.tag == "FireProjectile")
					{
						newProjectile.GetComponent<Projectile>().projectileDamage = fireDamage;
					}else if(newProjectile.tag == "IceProjectile")
					{
						newProjectile.GetComponent<Projectile>().projectileDamage = iceDamage;
					}
					else if (newProjectile.tag == "NatureProjectile")
					{
						newProjectile.GetComponent<Projectile>().projectileDamage = natureDamage;
					}
					else if (newProjectile.tag == "EarthProjectile")
					{
						newProjectile.GetComponent<Projectile>().projectileDamage = earthDamage;
					}
				}
			}
		}
	}
}
