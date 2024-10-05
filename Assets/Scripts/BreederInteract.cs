using System;
using UnityEngine;

public class BreederInteract : Interactable {

	private LarvaInteract storedLarva;
	private Collider2D collider;
	private void Awake() {
		collider = GetComponent<Collider2D>();
	}

	private bool IsFree {
		get { return storedLarva == null; }
	}
	
	private void TakeLarva() {
		collider.enabled = false;
		storedLarva = PlayerInteract.Instance.Drop().transform.GetComponent<LarvaInteract>();
		storedLarva.IsStored = true;
		storedLarva.Freeze(false);
		storedLarva.transform.parent = transform;
		storedLarva.OnPickupAction += FreeStoredLarva;
		storedLarva.transform.localPosition = Vector3.zero;
	}



	private void FreeStoredLarva(Pickup larva) {
		larva.OnPickupAction -= FreeStoredLarva;
		storedLarva = null;
		collider.enabled = true;
	}
	
	public override void OnInteract(PlayerInteract player) {
		if (PlayerInteract.Instance.PickupType != PickupType.Larva) {
			return;
		}
		if (IsFree) {
			TakeLarva();
			Debug.Log("take");
		}
		else {
			Debug.Log("oh no");
		}
	}
}
