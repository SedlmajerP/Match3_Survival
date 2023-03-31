using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private List<GameObject> elementProjectiles;
	[SerializeField] private GameObject enemyProjectile;
	[SerializeField] private PlayerBoardManager pBoardManager;
	[SerializeField] private EnemyBoardManager enemyBoardManager;

	[Header("Damage of Elements")]
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
					Transform elementTransform = element.GetComponent<Transform>();

					GameObject projectile = elementProjectiles.Find((proj) => proj.name == element.tag);
					Vector2 projectilePos = elementTransform.GetChild(0).position;

					GameObject newProjectile = Instantiate(projectile, projectilePos, Quaternion.identity, this.transform);

					Transform newProjTransform = newProjectile.GetComponent<Transform>();
					ElementProjectile elementProjectile = newProjectile.GetComponent<ElementProjectile>();

					newProjTransform.DOMove(new Vector2(elementTransform.position.x, 10), 1.5f);

					if (newProjectile.tag == "FireProjectile")
					{
						elementProjectile.projectileDamage = fireDamage;
					}
					else if (newProjectile.tag == "IceProjectile")
					{
						elementProjectile.projectileDamage = iceDamage;
					}
					else if (newProjectile.tag == "NatureProjectile")
					{
						elementProjectile.projectileDamage = natureDamage;
					}
					else if (newProjectile.tag == "EarthProjectile")
					{
						elementProjectile.projectileDamage = earthDamage;
					}
				}
			}
		}
	}

	public void EnemyShoot()
	{


		for (int x = 0; x < enemyBoardManager.width; x++)
		{
			for (int y = 0; y < enemyBoardManager.height; y++)
			{
				if (enemyBoardManager.allEnemyArray[x, y] != null)
				{
					GameObject enemy = enemyBoardManager.allEnemyArray[x, y];

					if (enemy.CompareTag("EnemyShooting"))
					{
						Transform enemyTransform = enemy.GetComponent<Transform>();
						Vector2 projectileSpawnPos = enemyTransform.position;
						GameObject newProjectile = Instantiate(enemyProjectile, projectileSpawnPos, Quaternion.identity, this.transform);
						Transform newProjTransform = newProjectile.GetComponent<Transform>();
						newProjTransform.DOLocalMove(new Vector2(enemyTransform.position.x, 0), 0.5f);
					}
				}

			}
		}
	}
}
