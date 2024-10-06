using UnityEngine;

public class PlayerFlip : MonoBehaviour {
	Vector2 v2LocalPosStart;
	public bool flipRenderer = false;
	private float horizontalInput;
	private SpriteRenderer _renderer;
	
	void Start() {
		v2LocalPosStart = transform.localPosition;
		PlayerMovement.Instance.controls.Player.Move.performed += ctx => horizontalInput = ctx.ReadValue<float>();

		_renderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		if (Mathf.Abs(horizontalInput) > 0.01f) {
			bool flipX = Mathf.Sign(horizontalInput) < 0;
			if (flipRenderer) {
				_renderer.flipX = flipX;
			}
			// else {
				transform.localPosition = new Vector2(v2LocalPosStart.x * Mathf.Sign(horizontalInput),
					transform.localPosition.y);
			// }
			FlipChildren(flipX);
		}
	}
	
	void FlipChildren(bool flipX) {
		foreach (Transform child in transform) {
			// Reverse the local scale along the X-axis to flip the child
			Vector3 childPos = child.localPosition;
			childPos.x = Mathf.Abs(childPos.x) * (flipX ? -1 : 1); // Flip based on the parent's direction
			child.localPosition = childPos;
		}
	}
}