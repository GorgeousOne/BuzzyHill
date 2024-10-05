using UnityEngine;

public class Interactable : MonoBehaviour {

	public Color highlightColor = Color.white;
	public Color defaultColor = new (31, 15, 0);
	
	private SpriteRenderer highlight;
	
	private void OnEnable() {
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