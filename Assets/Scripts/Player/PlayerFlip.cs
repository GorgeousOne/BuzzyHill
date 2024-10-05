using UnityEngine;

public class PlayerFlip : MonoBehaviour {
	Vector2 v2LocalPosStart;
	private float horizontalInput;
	
	void Start() {
		v2LocalPosStart = transform.localPosition;
		PlayerMovement.Instance.controls.Player.Move.performed += ctx => horizontalInput = ctx.ReadValue<float>();
	}

	void Update() {
		if (Mathf.Abs(horizontalInput) > 0.01f) {
			transform.localPosition = new Vector2(v2LocalPosStart.x * Mathf.Sign(horizontalInput),
				transform.localPosition.y);
		}
	}
}