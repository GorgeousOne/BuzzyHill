using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRise : MonoBehaviour {
	public float speed;
	public float time = 1;
	private SpriteRenderer _renderer;
	private float counter;
	// Start is called before the first frame update
	void Start() {
		_renderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update() {
		counter += Time.deltaTime;
		Color color = _renderer.color;
		color.a = Mathf.Lerp(1f, 0f, counter / time);  // Gradually reduce alpha
		_renderer.color = color;

		if (counter > time) {
			Destroy(gameObject);
		}
	}

	private void FixedUpdate() {
		transform.position += Vector3.up * speed;
	}
}