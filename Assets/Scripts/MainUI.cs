using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainUI : MonoBehaviour
{
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private TMP_Text startScreenMaxWavesText;
	[SerializeField] private PlayerBoardManager playerBoardManager;
	[SerializeField] private TMP_Text playScreenWavesText;
	[SerializeField] private TMP_Text looseWavesText;
	[SerializeField] private TMP_Text looseMaxWavesText;

	[SerializeField] private GameObject loosePanel;
	

	

	private void Awake()
	{

	}
	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "StartScreen")
			SetStartScreenMaxWaves();
	}

	public void UpdateWavesText()
	{
		playScreenWavesText.text = $"Wave: {GameManager.Instance.numWaves}";
	}

	public void UpdateLooseScreen()
	{
		looseWavesText.text = $"You survived {GameManager.Instance.numWaves-1} waves";
		looseMaxWavesText.text = $"Your Best: {GameManager.Instance.maxWaves} waves";
	}


	public void LooseGame()
	{
		UpdateLooseScreen();
		loosePanel.SetActive(true);
		Time.timeScale = 0;
	}
	public void LoadMainScene()
	{
		SceneManager.LoadScene("MainScene");
		GameManager.Instance.playerHealth = GameManager.Instance.playerMaxHealth;
		GameManager.Instance.numWaves = 1;
		Time.timeScale = 1.0f;

	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("StartScreen");
		Time.timeScale = 1.0f;
	}

	public void PauseGame()
	{if (loosePanel.activeInHierarchy == false)
		{
			pausePanel.SetActive(true);
			Time.timeScale = 0;
		}
	}

	public void ResumeGame()
	{
		pausePanel.SetActive(false);
		Time.timeScale = 1;
	}

	public void SetStartScreenMaxWaves()
	{
		startScreenMaxWavesText.text = $"Longest Survive: {GameManager.Instance.maxWaves} waves";
	}

}
