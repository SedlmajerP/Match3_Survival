using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
	[SerializeField] PlayerBoardManager playerBoardManager;
	[SerializeField] EnemyBoardManager enemyBoardManager;

	private void Awake()
	{
		playerBoardManager.Init();
		enemyBoardManager.Init();
	}
}
