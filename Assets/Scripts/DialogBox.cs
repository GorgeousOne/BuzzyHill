using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour {
	public static DialogBox Instance;

	public TextMeshProUGUI textComp;
	public string[] lines;
	public float textSpeed;

	private int index;
	private DialogControls controls;
	private Action afterTextCall;
	private Queue<DialogItem> dialogQueue = new ();

	private struct DialogItem {
		public string[] Lines;
		public GameObject Focus;

		public DialogItem(string[] lines, GameObject focus) {
			Lines = lines;
			Focus = focus;
		}
	}


	private void Awake() {
		Instance = this;
		controls = new DialogControls();
		controls.Dialog.Skip.performed += _ => SkipLine();
		textComp.text = string.Empty;
		gameObject.SetActive(false);
	}

	private void OnEnable() {
		controls.Enable();
	}

	private void OnDisable() {
		controls.Disable();
	}

	private void StartDialog() {
		if (dialogQueue.Count < 1) {
			return;
		}
		gameObject.SetActive(true);
		PlayerInteract.Instance.OnStartTalk();
		
		DialogItem currentItem = dialogQueue.Peek();
		if (currentItem.Focus) {
			SetCinemachineTarget(currentItem.Focus);
		}
		lines = currentItem.Lines;
		index = 0;
		GameLogic.Instance.TimerPaused = true;
		StartCoroutine(TypeLine());
	}

	IEnumerator TypeLine() {
		foreach (char c in lines[index]) {
			textComp.text += c;
			yield return new WaitForSeconds(1 / textSpeed);
		}
	}

	public void ReadOut(string[] text, GameObject focus) {
		dialogQueue.Enqueue(new DialogItem(text, focus));
		if (dialogQueue.Count == 1)
		{
			StartDialog();
		}
	}

	void SkipLine() {
		if (textComp.text == lines[index]) {
			NextLine();
		}
		else {
			StopAllCoroutines();
			textComp.text = lines[index];
		}
	}

	void NextLine() {
		if (lines == null || index < lines.Length - 1) {
			index++;
			textComp.text = string.Empty;

			// Check if the line contains a scene object name from the predefined list
			string currentLine = lines[index];
			GameObject targetObject = CheckForSceneObjectInLine(currentLine);
			if (targetObject != null) {
				SetCinemachineTarget(targetObject);
			}

			StartCoroutine(TypeLine());
		}
		else {
			FinishDialog();
		}
	}

	void FinishDialog() {
		textComp.text = string.Empty;
		gameObject.SetActive(false);
		SetCinemachineTarget(PlayerMovement.Instance.gameObject);
		PlayerInteract.Instance.OnFinishTalk();
		GameLogic.Instance.TimerPaused = false;
		//next or quit
		dialogQueue.Dequeue();
		StartDialog();
	}

	List<string> focusableNames = new() {"Fungus", "Entrance", "Queen", "Nursery"};

	GameObject CheckForSceneObjectInLine(string line) {
		string[] words = Regex.Replace(line, @"[^\w\s]", "").Split(' ');

		foreach (string word in words) {
			if (focusableNames.Contains(word)) {
				return GameObject.Find(word);
			}
		}

		return null;
	}

	private void SetCinemachineTarget(GameObject targetObject) {
		CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
		virtualCamera.Follow = targetObject.transform;
		virtualCamera.LookAt = targetObject.transform;
	}
}