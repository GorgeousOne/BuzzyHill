using UnityEngine;

public class Interactable : MonoBehaviour {

	public Color highlightColor = Color.white;
	public Color defaultColor = new (0.12f, 0.06f, 0.00f);
	public GameObject ghostPrefab;
	private SpriteRenderer highlight;
	
	protected virtual void OnEnable() {
		highlight = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		//grab layer
		if (other.gameObject.layer == 9) {
			PlayerInteract.Instance.FocusInteractable(this);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D other) {
		//grab layer
		if (other.gameObject.layer == 9) {
			PlayerInteract.Instance.UnfocusInteractable(this);
		}
	}

	public void Highlight() {
		highlight.color = highlightColor;
	}
	public void UnHighlight() {
		highlight.color = defaultColor;
	}
	public virtual void OnInteract(PlayerInteract player) {
		
	}

	protected virtual void Die() {
		Instantiate(ghostPrefab, transform.position, Quaternion.identity);
	}
	
	public void ReadOut(params string[] text) {
		DialogBox.Instance.ReadOut(text, gameObject, PlayerInteract.Instance.OnFinishTalk);
	}
}