using UnityEngine;

public class Dialog : Interactable {

	public override void OnInteract(PlayerInteract player) {}
	
	public void ReadOut(params string[] text) {
		DialogBox.Instance.gameObject.SetActive(true);
		PlayerInteract.Instance.OnStartTalk();
		DialogBox.Instance.ReadOut(text, PlayerInteract.Instance.OnFinishTalk);
	}
}