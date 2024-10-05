using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour {
	public TextMeshProUGUI textComp;
	public string[] lines;
	public float textSpeed;

	private int index;
	private DialogControls controls;

	private void OnEnable() {
		controls = new DialogControls();
		controls.Dialog.Skip.performed += _ => SkipLine();
		controls.Enable();
	}

	private void OnDisable() {
		controls.Disable();
	}

	// Start is called before the first frame update
	void Start() {
		textComp.text = string.Empty;
		StartDialog();
	}

	void StartDialog() {
		index = 0;
		StartCoroutine(TypeLine());
	}

	IEnumerator TypeLine() {
		foreach (char c in lines[index]) {
			textComp.text += c;
			yield return new WaitForSeconds(1 / textSpeed);
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
		if (index < lines.Length-1) {
			index++;
			textComp.text = string.Empty;
			StartCoroutine(TypeLine());
		}
		else {
			gameObject.SetActive(false);
			//TODO  continue game
		}
	}
}