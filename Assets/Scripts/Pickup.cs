using System;
using Unity.VisualScripting;
using UnityEngine;

public enum PickupType {
	Food,
	Larva,
	Leaf,
	None
}

public class Pickup : Interactable {
	[SerializeField] private PickupType type;
	public event Action<Pickup> OnPickupAction;
	protected Rigidbody2D rigid;
	protected Collider2D collid;
	protected Collider2D childCollid;
	
	public PickupType Type {
		get { return type; }
		private set { type = value; }
	}

	private new void OnEnable() {
		base.OnEnable();
		rigid = GetComponent<Rigidbody2D>();
		collid = GetComponent<Collider2D>();
		childCollid = transform.GetChild(0).GetComponent<Collider2D>();
	}

	public void Freeze(bool disableGrab=true) {
		collid.enabled = !disableGrab;
		childCollid.enabled = !disableGrab;
		rigid.velocity = Vector2.zero;
		rigid.isKinematic = true;
	}

	public void UnFreeze(Vector2 velocity) {
		collid.enabled = true;
		childCollid.enabled = true;
		rigid.isKinematic = false;
		rigid.velocity = velocity;
	}
	
	public override void OnInteract(PlayerInteract player) {
		player.PickUp(this);
		OnPickupAction?.Invoke(this);
	}

	protected void EjectSelf() {
		OnPickupAction?.Invoke(this);
	}
}