using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public static PlayerControls controls;
	[SerializeField] LayerMask lmWalls;
	[SerializeField] float fJumpVelocity = 5;
	
	Rigidbody2D rigid;

	float fJumpPressedRemember = 0;
	[SerializeField] float fJumpPressedRememberTime = 0.2f;

	float fGroundedRemember = 0;
	[SerializeField] float fGroundedRememberTime = 0.1f;

	[SerializeField] float fHorizontalAcceleration = 0.2f;
	[SerializeField] [Range(0, 1)] float fHorizontalDampingBasic = 0.5f;
	[SerializeField] [Range(0, 1)] float fHorizontalDampingWhenStopping = 0.5f;
	[SerializeField] [Range(0, 1)] float fHorizontalDampingWhenTurning = 0.5f;

	[SerializeField] [Range(0, 1)] float fCutJumpHeight = 0.5f;

	private void OnEnable() {
		controls = new PlayerControls();
		controls.Player.Jump.performed += _ => jumpPressed = true;
		controls.Player.Jump.performed += _ => jumpPressed = true;
		controls.Enable();
	}

	private void OnDisable() {
		controls.Disable();
	}

	void Start() {
		rigid = GetComponent<Rigidbody2D>();
	}

	private bool jumpPressed;
	private bool jumpReleased;
	private float horizontalInput;
	
	void Update() {
		horizontalInput = controls.Player.Move.ReadValue<float>();
	}

	private void FixedUpdate() {
		Vector2 v2GroundedBoxCheckPosition = (Vector2) transform.position + new Vector2(0, -0.01f);
		Vector2 v2GroundedBoxCheckScale = (Vector2) transform.localScale + new Vector2(-0.02f, 0);
		bool bGrounded = Physics2D.OverlapCapsule(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, lmWalls);

		fGroundedRemember -= Time.deltaTime;
		if (bGrounded) {
			fGroundedRemember = fGroundedRememberTime;
		}

		fJumpPressedRemember -= Time.deltaTime;
		if (jumpPressed) {
			fJumpPressedRemember = fJumpPressedRememberTime;
		}

		if (jumpReleased) {
			if (rigid.velocity.y > 0) {
				rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * fCutJumpHeight);
			}
		}

		if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0)) {
			fJumpPressedRemember = 0;
			fGroundedRemember = 0;
			rigid.velocity = new Vector2(rigid.velocity.x, fJumpVelocity);
		}

		float fHorizontalVelocity = rigid.velocity.x;
		fHorizontalVelocity += horizontalInput * fHorizontalAcceleration;

		if (Mathf.Abs(horizontalInput) < 0.01f)
			fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * 10f);
		else if (Mathf.Sign(horizontalInput) != Mathf.Sign(fHorizontalVelocity))
			fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * 10f);
		else
			fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingBasic, Time.deltaTime * 10f);

		rigid.velocity = new Vector2(fHorizontalVelocity, rigid.velocity.y);
		jumpPressed = false;
		jumpReleased = false;
	}
}