using UnityEngine;

public class PlayerFlip : MonoBehaviour {
	Vector2 v2LocalPosStart;

	void Start() {
		v2LocalPosStart = transform.localPosition;
	}

	void Update() {
		float horizontalInput = PlayerMovement.controls.Player.Move.ReadValue<float>();
		if (Mathf.Abs(horizontalInput) > 0.01f) {
			transform.localPosition = new Vector2(v2LocalPosStart.x * Mathf.Sign(horizontalInput),
				transform.localPosition.y);
		}
	}
}