using System;
using System.Collections;
using UnityEngine;

public class QueenInteract : Dialog {

	public GameObject larvaPrefab;
	public int maxFuel = 6;
	public int fuel;
	public float regenTime = 20;
	private Pickup eatenFood;

	public bool IsHungry {
		get { return fuel < 1; }
	}
	
	private void Awake() {
		fuel = 0;
	}

	private void TakeFood() {
		EatFood(PlayerInteract.Instance.Drop());
	}
	
	IEnumerator RegenLarva() {
		yield return new WaitForSeconds(regenTime);
		SpawnLarva(null);
		Debug.Log("left " + fuel);
		if (fuel < 1) {
			Debug.Log("destroy!");
			Destroy(eatenFood.gameObject);
			eatenFood = null;
			yield break;
		}
		StartCoroutine(RegenLarva());
	}

	void SpawnLarva(Pickup lastLarva) {
		Pickup larva = Instantiate(larvaPrefab, transform.position+(Vector3.up*4), Quaternion.identity).GetComponent<Pickup>();
		// larva.OnLiftAction += SpawnLarva;
		fuel -= 1;
	}

	void EatFood(Pickup food) {
		eatenFood = food;
		eatenFood.Freeze();
		eatenFood.transform.parent = transform;
		eatenFood.transform.localPosition = Vector3.up;
		fuel = maxFuel;
		StartCoroutine(RegenLarva());
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);
		if (!IsHungry) {
			return;
		}
		//pickup layer
		if (other.gameObject.layer != 8) {
			return;
		}
		Pickup pickup = other.transform.parent.GetComponent<Pickup>();

		if (pickup.Type == PickupType.Food) {
			EatFood(pickup);
		}
	}

	public override void OnInteract(PlayerInteract player) {
		switch (PlayerInteract.Instance.PickupType) {
			case PickupType.Food:
				if (IsHungry) {
					TakeFood();
					ReadOut("yummi!");
				}
				else {
					ReadOut("Ah thanks, but I'm good hun!");
				}
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