using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcWalk : MonoBehaviour {

	public float speed;
	public float interval = 5;
	private Rigidbody2D rigid;
	private SpriteRenderer _renderer;
	
	private void OnEnable() {
		rigid = GetComponent<Rigidbody2D>();
		_renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		
		StartCoroutine(WalkRandomly());
		DialogBox.Instance.ReadOut(new[] {"Another strong warrior in our rows!"}, gameObject);
	}

	IEnumerator WalkRandomly() {
		rigid.velocity = Vector2.zero;
		yield return new WaitForSeconds(Random.Range(0, interval));
		rigid.velocity = Vector2.right * (speed * (Random.value > 0.5f ? 1 : -1));
		_renderer.flipX = rigid.velocity.x < 0;
		yield return new WaitForSeconds(Random.Range(1, interval));
		StartCoroutine(WalkRandomly());
	}
}
