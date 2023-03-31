using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameState currentState = GameState.PlayerTurn;
	public int numWaves = 1;
	public int maxWaves = 0;

	private void Awake()
	{

		if (GameManager.Instance != null)
		{
			Destroy(gameObject);
			return;

		}
		Instance = this;
		DontDestroyOnLoad(this.gameObject);
		maxWaves = PlayerPrefs.GetInt("maxWaves");
	}
	private void Start()
	{
		UpdateGameState(GameState.PlayerTurn);
		
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

	public void setMaxWaves()
	{
		numWaves++;

		if (numWaves > maxWaves)
		{
			maxWaves = numWaves-1;
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



	public void LoadMainScene()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("StartScreen");
	}
}

