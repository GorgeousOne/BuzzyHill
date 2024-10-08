using TMPro;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	public int numAntsWin = 10;
	public int timeLimit = 10;

	public TextMeshProUGUI antsLabel;
	public TextMeshProUGUI countdownLabel;
	public GameObject winScreen;
	public GameObject loseScreen;
	
	public static GameLogic Instance;
	private int antCounter;
	private float startTime;

	public bool showTutorial = true;
	
	public bool TimerPaused { get; set; }
	
	void Awake() {
		Instance = this;
	}

	private void Start() {
		Time.timeScale = 1;
		antCounter = -1;
		startTime = Time.time;
		NotifyAntSpawn();
		
		showTutorial = PlayerPrefs.GetInt("ShowTutorial", 1) == 1;
		
		if (showTutorial) {
			ReadTutorial();
		}
	}

	private void ReadTutorial() {
		string[] tut = {
			"Welcome, new soldier, we are proud to have you in our ranks!",
			"Our nest is in spring renovation and we urgently need reinforcements.", 
			"You will find freshly cut leaves in the Entrance.",
			"Give them to Mr. Fungus so that he can grow fruit for us.",
			"Give the fruit to me, the Queen, so I'm able give birth to new larva.",
			"The larva will then have to go to the Nursery, they will also need food.",
			string.Format("Can you manage to raise {0} larva? You have {1} minutes.", numAntsWin, timeLimit),
			"Hurry up, hun, time starts now!"
		};
		DialogBox.Instance.ReadOut(tut, null);
	}

	public void RestartScene() {
		PlayerPrefs.SetInt("ShowTutorial", 0);
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
	
	private void Update() {
		if (TimerPaused) {
			startTime += Time.deltaTime;
			return;
		}
		float timeLeft = timeLimit * 60 - (Time.time - startTime);
		if (timeLeft < 0) {
			GameOver("Time's up. The queen's patience has worn thin.");
		}

		int minutes = Mathf.FloorToInt(timeLeft / 60f); // Divide total seconds by 60 to get minutes
		int seconds = Mathf.FloorToInt(timeLeft % 60); // Get the remaining seconds
		countdownLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	public void GameOver(string message) {
		Time.timeScale = 0;
		TimerPaused = true;
		
		DialogBox.Instance.gameObject.SetActive(false);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		loseScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message;
		loseScreen.SetActive(true);
	}

	
	public void NotifyAntSpawn() {
		antCounter++;
		antsLabel.text = antCounter + "/" + numAntsWin;
		
		if (antCounter >= numAntsWin) {
			Time.timeScale = 0;
			TimerPaused = true;
			
			DialogBox.Instance.gameObject.SetActive(false);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			winScreen.SetActive(true);
		}
	}
}