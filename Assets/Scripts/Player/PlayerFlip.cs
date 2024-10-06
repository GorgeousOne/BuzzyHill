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
			if (flipRenderer) {
				_renderer.flipX = Mathf.Sign(horizontalInput) < 0;
			}
			else {
				transform.localPosition = new Vector2(v2LocalPosStart.x * Mathf.Sign(horizontalInput),
					transform.localPosition.y);
			}
		}
	}
}