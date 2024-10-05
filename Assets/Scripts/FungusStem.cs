using System;
using UnityEngine;

public class FungusStem : MonoBehaviour {
	public GameObject foodPrefab;
	private Pickup food;
	
	public bool HasFood {
		get { return food != null; }
	}

	public event Action<FungusStem> OnFoodPickup;
	
	public void SpawnPickup() {
		if (HasFood) {
			return;
		}
		food = Instantiate(foodPrefab, transform).GetComponent<Pickup>();
		food.OnLiftAction += OnFoodTake;

	}

	private void OnFoodTake(Pickup pickup) {
		food = null;
		pickup.OnLiftAction -= OnFoodTake;
		OnFoodPickup?.Invoke(this);
	}
}