using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcWalk : MonoBehaviour {

	public float speed;
	public float interval = 5;
	private Rigidbody2D rigid;

	private void OnEnable() {
		rigid = GetComponent<Rigidbody2D>();
		StartCoroutine(WalkRandomly());
		DialogBox.Instance.gameObject.SetActive(true);
		PlayerInteract.Instance.OnStartTalk();
		DialogBox.Instance.ReadOut(new[] {"Another strong warrior in our rows!"}, gameObject, PlayerInteract.Instance.OnFinishTalk);
	}

	IEnumerator WalkRandomly() {
		rigid.velocity = Vector2.zero;
		yield return new WaitForSeconds(Random.Range(0, interval));
		rigid.velocity = Vector2.right * (speed * (Random.value > 0.5f ? 1 : -1));
		yield return new WaitForSeconds(Random.Range(1, interval));
		StartCoroutine(WalkRandomly());
	}
}
