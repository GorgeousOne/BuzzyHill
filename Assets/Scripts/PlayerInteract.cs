using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour {
	PlayerControls controls;
	private Transform grabArea;
	private Transform head;
	private List<Interactable> nearInteractables = new();
	private Interactable focused;
	private Interactable carrying;
	
	public static PlayerInteract Instance;
	
	private void OnEnable() {
		Instance = this;
		controls = new PlayerControls();
		controls.Player.Interact.performed += OnInteract;
		controls.Enable();

		grabArea = transform.GetChild(0);
		head = transform.GetChild(1);
	}

	private void OnDisable() {
		controls.Disable();
	}

	public void FocusInteractable(Interactable thing) {
		nearInteractables.Add(thing);
		UpdateHighlight();
	}

	private void Update() {
		UpdateHighlight();
	}

	private void UpdateHighlight() {
		Interactable closest = nearInteractables
			.OrderBy(thing => Vector2.Distance(thing.transform.position, grabArea.position)).FirstOrDefault();
		if (focused == closest) {
			return;
		}
		if (focused != null) {
			focused.UnHighlight();
		}
		if (closest == null) {
			focused = null;
			return;
		}
		focused = closest;
		closest.Highlight();
		Debug.Log("Focus " + focused);
	}
	public void UnfocusInteractable(Interactable thing) {
		nearInteractables.Remove(thing);
		UpdateHighlight();
	}
	
	void OnInteract(InputAction.CallbackContext context) {
		if (focused == null) {
			if (carrying) {
				Drop();
			}
			return;
		}
		Debug.Log("interact " + focused);
		focused.OnInteract(this);
	}

	public void PickUp(Interactable thing) {
		Drop();
		Debug.Log("pick up " + thing);
		carrying = thing;
		thing.transform.parent = head.transform;
		thing.GetComponent<Collider2D>().enabled = false;
		thing.GetComponent<Rigidbody2D>().isKinematic = true;
		thing.transform.localPosition = Vector3.up;
		//TODO put in right position
	}

	private void Drop() {
		if (carrying == null) {
			return;
		}
		carrying.transform.parent = null;
		carrying.GetComponent<Collider2D>().enabled = true;
		carrying.GetComponent<Rigidbody2D>().isKinematic = false;
		carrying = null;
	}
}