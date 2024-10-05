using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaInteract : Pickup {
	
	public bool IsStored { get; set;}
	public bool IsHungry {
		get { return eatenFood == null; }
	}
	
	public float eatTime = 40;
	public float breedTime = 80;
	private Pickup eatenFood;
	
	private void Update() {
		if (IsStored && !IsHungry) {
			breedTime -= Time.deltaTime;
		}
		if (breedTime <= 0) {
			//signal disappearing to breeder
			EjectSelf();
			Destroy(gameObject);
		}
	}
	private void TakeFood() {
		eatenFood = PlayerInteract.Instance.Drop();
		eatenFood.Freeze();
		eatenFood.transform.parent = transform;
		eatenFood.transform.localPosition = Vector3.zero;
		StartCoroutine(EatFood());
	}

	IEnumerator EatFood() {
		yield return new WaitForSeconds(eatTime);
		Destroy(eatenFood);
		eatenFood = null;
		//TODO look hungry
	}
	
	public override void OnInteract(PlayerInteract player) {
		PickupType other = PlayerInteract.Instance.PickupType;
		
		//dont get picked up by food if in a breeeder
		if (other == PickupType.Food && IsStored) {
			//eat food if hungry
			if (IsHungry) {
				TakeFood();
			}
			return;
		}
		base.OnInteract(player);
	}
}