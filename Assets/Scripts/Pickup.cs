using System;
using UnityEngine;

public enum PickupType {
	Food,
	Larva,
	Leaf,
	None
}

public class Pickup : Interactable {
	[SerializeField] private PickupType type;
	public event Action<Pickup> OnLiftAction;
	
	public PickupType Type {
		get { return type; }
		private set { type = value; }
	}
	
	public override void OnInteract(PlayerInteract player) {
		player.PickUp(this);
		OnLiftAction?.Invoke(this);
	}
}