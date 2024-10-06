using TMPro;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	public int numAntsWin = 10;
	public int timeLimit = 5;

	public TextMeshProUGUI antsLabel;
	public TextMeshProUGUI countdownLabel;

	public static GameLogic Instance;
	private int antCounter;
	private float startTime;

	public bool showTutorial = true;
	
	public bool TimerPaused { get; set; }
	
	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject); 
		}
	}

	private void Start() {
		showTutorial = PlayerPrefs.GetInt("ShowTutorial", 1) == 1;  

		if (showTutorial && false) {
			ReadTutorial();
		}
		antCounter = 0;
		startTime = Time.time;
	}

	private void ReadTutorial() {
		DialogBox.Instance.gameObject.SetActive(true);
		PlayerInteract.Instance.OnStartTalk();
		string[] tut = {
			"Welcome, new soldier, we are proud to have you in our ranks!",
			"Our nest is in spring renovation and we urgently need reinforcements.", 
			"You will find freshly cut leaves in the Entrance.",
			"Give them to Mr. Fungus so that he can grow fruit for us.",
			"Give the fruit to me, the Queen, so I'm able give birth to new larvae.",
			"The larvae will then have to go to the Nursery, they will also need food.",
			string.Format("Can you manage to raise {0} larvae? You have {1} minutes.", numAntsWin, timeLimit),
			"Hurry up, hun, time starts now!"
		};
		DialogBox.Instance.ReadOut(tut, null, PlayerInteract.Instance.OnFinishTalk);
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
			HandleLose();
		}

		int minutes = Mathf.FloorToInt(timeLeft / 60f); // Divide total seconds by 60 to get minutes
		int seconds = Mathf.FloorToInt(timeLeft % 60); // Get the remaining seconds
		countdownLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	public void GameOver(string message) {
		Debug.Log("help, game over" + message);	
	}
	
	private void HandleLose() { }

	public void NotifyAntSpawn() {
		antCounter++;

		if (antCounter > numAntsWin) {
			//TODO win
		}
	}
}