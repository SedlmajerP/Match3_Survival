using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameState currentState = GameState.PlayerTurn;
	[SerializeField] private PlayerBoardManager pBoardManager;
	[SerializeField] private EnemyBoardManager eBoardManager;
	public int playerMaxHealth = 100;
	public int playerHealth;

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		UpdateGameState(GameState.PlayerTurn);
		playerHealth = playerMaxHealth;

		pBoardManager.GeneratePlayerBoard();
		eBoardManager.GenerateBackGroundTiles();
		eBoardManager.GenerateFirstEnemies(5);
	}

	public void UpdateGameState(GameState newState)
	{
		currentState = newState;

		switch (newState)
		{
			case GameState.StartScreen:
				HandleStartScreen();
				break;
			case GameState.EnemyTurn:
				HandleEnemyTurn();
				break;
			case GameState.PlayerTurn:
				HandlePlayerTurn();
				break;
			case GameState.ScoreScreen:
				HandleScoreScreen();
				break;
		}
	}

	private void HandleScoreScreen()
	{
		
	}

	private void HandlePlayerTurn()
	{
		
	}

	private void HandleEnemyTurn()
	{
		
	}

	private void HandleStartScreen()
	{
		
	}

	public enum GameState
	{
		StartScreen,
		EnemyTurn,
		PlayerTurn,
		ScoreScreen,
	}


}
