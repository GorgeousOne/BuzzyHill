using UnityEngine;

public class Interactable : MonoBehaviour {

	public Color highlightColor = Color.white;
	public Color defaultColor = new (0.12f, 0.06f, 0.00f);
	
	private SpriteRenderer highlight;
	
	protected virtual void OnEnable() {
		highlight = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		PlayerInteract.Instance.FocusInteractable(this);
	}

	private void OnTriggerExit2D(Collider2D other) {
		PlayerInteract.Instance.UnfocusInteractable(this);
	}

	public void Highlight() {
		highlight.color = highlightColor;
	}
	public void UnHighlight() {
		highlight.color = defaultColor;
	}
	public virtual void OnInteract(PlayerInteract player) {
		
	}
}