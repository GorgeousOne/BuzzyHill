using System;
using UnityEngine;

public class BreederInteract : Interactable {

	private LarvaInteract storedLarva;
	private Collider2D collid;
	private void Awake() {
		collid = GetComponent<Collider2D>();
	}

	private bool IsFree {
		get { return storedLarva == null; }
	}
	
	private void TakeLarva() {
		StoreLarva(PlayerInteract.Instance.Drop().transform.GetComponent<LarvaInteract>());
	}

	private void StoreLarva(LarvaInteract larva) {
		collid.enabled = false;
		storedLarva = larva;
		storedLarva.IsStored = true;
		storedLarva.Freeze(false);
		storedLarva.transform.parent = transform;
		storedLarva.OnPickupAction += FreeStoredLarva;
		storedLarva.transform.localPosition = Vector3.zero;
	}
	
	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);
		if (!IsFree) {
			return;
		}
		//pickup layer
		if (other.gameObject.layer != 8) {
			return;
		}
		Pickup pickup = other.transform.parent.GetComponent<Pickup>();

		if (pickup.Type == PickupType.Larva) {
			StoreLarva(other.transform.parent.GetComponent<LarvaInteract>());
		}
	}

	private void FreeStoredLarva(Pickup larva) {
		larva.OnPickupAction -= FreeStoredLarva;
		storedLarva = null;
		collid.enabled = true;
	}
	
	public override void OnInteract(PlayerInteract player) {
		if (PlayerInteract.Instance.PickupType != PickupType.Larva) {
			return;
		}
		if (IsFree) {
			TakeLarva();
		}
	}
}
