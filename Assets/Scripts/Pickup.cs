using UnityEngine;

public class Pickup : Interactable {

	public override void OnInteract(PlayerInteract player) {
		player.PickUp(this);
	}
}