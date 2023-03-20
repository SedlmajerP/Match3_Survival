using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> elementProjectiles;
	[SerializeField] private GameObject jumpEnemyProjectile;
	[SerializeField] private PlayerBoardManager pBoardManager;
	[SerializeField] private EnemyBoardManager enemyBoardManager;
	public int fireDamage = 50;
	public int iceDamage = 30;
	public int natureDamage = 30;
	public int earthDamage = 40;


	public void ShootElemets()
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
					if (newProjectile.tag == "FireProjectile")
					{
						newProjectile.GetComponent<ElementProjectile>().projectileDamage = fireDamage;
					}
					else if (newProjectile.tag == "IceProjectile")
					{
						newProjectile.GetComponent<ElementProjectile>().projectileDamage = iceDamage;
					}
					else if (newProjectile.tag == "NatureProjectile")
					{
						newProjectile.GetComponent<ElementProjectile>().projectileDamage = natureDamage;
					}
					else if (newProjectile.tag == "EarthProjectile")
					{
						newProjectile.GetComponent<ElementProjectile>().projectileDamage = earthDamage;
					}
				}
			}
		}
	}

	public void JumpingEnemyShoot(GameObject enemy)
	{
		if (enemy.CompareTag("EnemyJumping"))
		{
			Transform enemyTransform = enemy.GetComponent<Transform>();
			Vector2 projectileSpawnPos = enemyTransform.position;
			GameObject newProjectile = Instantiate(jumpEnemyProjectile, projectileSpawnPos, Quaternion.identity, this.transform);
			Transform newProjTransform = newProjectile.GetComponent<Transform>();
			newProjTransform.DOLocalMove(new Vector2(enemyTransform.position.x, 0), 0.9f);
		}

	}
}
