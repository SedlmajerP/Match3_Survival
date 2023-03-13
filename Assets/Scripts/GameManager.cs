using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameState currentState = GameState.PlayerTurn;
	private PlayerBoardManager pBoardManager;
	private EnemyBoardManager eBoardManager;

	private void Awake()
	{
		Instance = this;
		pBoardManager = FindObjectOfType<PlayerBoardManager>();
		eBoardManager = FindObjectOfType<EnemyBoardManager>();

	}
	private void Start()
	{
		pBoardManager.GeneratePlayerBoard();
		eBoardManager.GenerateEnemies();
	}

	
	public enum GameState
	{
		StartScreen,
		EnemyTurn,
		PlayerTurn,
		ScoreScreen,
	}


}
