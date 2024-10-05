using UnityEngine;

public class CutLeafSpawn : MonoBehaviour {

	public GameObject leafPrefab;
	
	private void OnEnable() {
		SpawnLeaf(null);
	}

	void SpawnLeaf(Pickup lastLeaf) {
		if (lastLeaf != null) {
			lastLeaf.OnPickupAction -= SpawnLeaf;
		}

		Pickup leaf = Instantiate(leafPrefab, transform).GetComponent<Pickup>();
		leaf.OnPickupAction += SpawnLeaf;
	}
}