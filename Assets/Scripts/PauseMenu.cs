using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	public GameObject pauseMenu;
	public static bool isGamePaused;
	private MenuControls menuControls;

	private void OnEnable() {
		menuControls = new MenuControls();
		menuControls.Menu.Pause.performed += _ => TogglePause();
		menuControls.Enable();
	}

	void Start() {
		pauseMenu.SetActive(false);	
	}

	void TogglePause() {
		if (isGamePaused) {
			ResumeGame();
		}
		else {
			PauseGame();
		}
	}

	public void PauseGame() {
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		isGamePaused = true;
	}

	public void ResumeGame() {
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		isGamePaused = false;
	}

	public void GoToMainMenu() {
		Time.timeScale = 1f;
		isGamePaused = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void QuitGame() {
		Application.Quit();
	}
}