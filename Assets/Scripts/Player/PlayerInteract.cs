using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour {
	private Transform grabArea;
	private Transform head;
	private List<Interactable> nearInteractables = new();
	private Interactable focused;
	private Pickup carrying;
	private Rigidbody2D rigid;
	
	public PickupType PickupType {
		get
		{
			// Check if carrying is not null, then return its type
			return carrying != null ? carrying.Type : PickupType.None;
		}
	}
	
	public static PlayerInteract Instance;
	private bool hasFood { get; }

	private void Awake() {
		Instance = this;
		grabArea = transform.GetChild(0);
		head = transform.GetChild(1);
		rigid = GetComponent<Rigidbody2D>();
	}

	private void Start() {
		gameObject.GetComponent<PlayerMovement>().controls.Player.Interact.performed += PlayerInteract.Instance.OnInteract;
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
	}
	public void UnfocusInteractable(Interactable thing) {
		nearInteractables.Remove(thing);
		UpdateHighlight();
	}
	
	public void OnInteract(InputAction.CallbackContext context) {
		if (focused == null) {
			if (carrying) {
				Drop();
			}
			return;
		}
		focused.OnInteract(this);
	}

	public void PickUp(Pickup thing) {
		Drop();
		carrying = thing;
		thing.transform.parent = head.transform;
		thing.GetComponent<Collider2D>().enabled = false;
		thing.GetComponent<Rigidbody2D>().isKinematic = true;
		thing.transform.localPosition = Vector3.up;
		//TODO put in right position
	}

	public Pickup Drop() {
		if (carrying == null) {
			return null;
		}
		carrying.transform.parent = null;
		carrying.GetComponent<Collider2D>().enabled = true;
		Rigidbody2D rb = carrying.GetComponent<Rigidbody2D>();
		rb.isKinematic = false;
		rb.velocity = rigid.velocity;
		Pickup temp = carrying;
		carrying = null;
		return temp;
	}

	public void OnStartTalk() {
		PlayerMovement.Instance.Freeze();
	}

	public void OnFinishTalk() {
		PlayerMovement.Instance.Unfreeze();
	}
}