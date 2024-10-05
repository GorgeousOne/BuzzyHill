using UnityEngine;

public class Dialog : Interactable {

	public override void OnInteract(PlayerInteract player) {}
	
	public void ReadOut(params string[] text) {
		DialogBox.Instance.gameObject.SetActive(true);
		PlayerMovement.Instance.Wait();
		DialogBox.Instance.ReadOut(text, PlayerMovement.Instance.Continue);
	}
}