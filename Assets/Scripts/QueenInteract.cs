using System;
using System.Collections;
using UnityEngine;

public class QueenInteract : Dialog {

	public GameObject larvaPrefab;
	public int maxFuel = 6;
	public int fuel;
	public float regenTime = 20;
	private float currentTime;
	private bool isSpawning;
	private Pickup eatenFood;
	
	private void Awake() {
		fuel = 0;
	}

	private void TakeFood() {
		eatenFood = PlayerInteract.Instance.Drop();
		eatenFood.Freeze();
		eatenFood.transform.parent = transform;
		eatenFood.transform.localPosition = Vector3.up;
		fuel = maxFuel;
		StartCoroutine(RegenLarva());
	}
	
	IEnumerator RegenLarva() {
		isSpawning = true;
		yield return new WaitForSeconds(regenTime);
		SpawnLarva(null);
		
		if (fuel < 1) {
			Destroy(eatenFood);
			eatenFood = null;
			isSpawning = false;
			yield break;
		}
		StartCoroutine(RegenLarva());
	}

	void SpawnLarva(Pickup lastLarva) {
		Pickup larva = Instantiate(larvaPrefab, transform.position, Quaternion.identity).GetComponent<Pickup>();
		// larva.OnLiftAction += SpawnLarva;
		fuel -= 1;
	}

	public override void OnInteract(PlayerInteract player) {
		switch (PlayerInteract.Instance.PickupType) {
			case PickupType.Food:
				TakeFood();
				ReadOut("yummi!", "whatever");
				break;
			case PickupType.Larva:
				break;
			case PickupType.Leaf:
				break;
			case PickupType.None:
				
				break;
		}
	}
}