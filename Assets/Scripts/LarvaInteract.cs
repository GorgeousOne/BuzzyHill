using System.Collections;
using UnityEngine;

public class LarvaInteract : Pickup {

	public GameObject npcPrefab;
	public bool IsStored { get; set;}
	public bool IsHungry {
		get { return eatenFood == null; }
	}
	
	public float eatTime = 40;
	public float breedTime = 80;
	public float starveTime = 40;
	private Pickup eatenFood;
	private float starveTimer;
	
	private void Update() {
		if (IsStored && !IsHungry) {
			breedTime -= Time.deltaTime;
		}
		if (IsHungry && !GameLogic.Instance.TimerPaused) {
			starveTimer += Time.deltaTime;
			if (starveTimer > starveTime) {
				Die();				
			}
		}
		if (breedTime < 0) {
			//signal disappearing to breeder
			EjectSelf();
			GameLogic.Instance.NotifyAntSpawn();
			Instantiate(npcPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	private void TakeFood(Pickup pickup) {
		eatenFood = pickup;
		eatenFood.Freeze();
		eatenFood.transform.parent = transform;
		eatenFood.transform.localPosition = Vector3.zero;
		StartCoroutine(EatFood());
	}
	
	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);
		if (!IsHungry || !IsStored) {
			return;
		}
		//pickup layer
		if (other.gameObject.layer != 8) {
			return;
		}
		Pickup pickup = other.transform.parent.GetComponent<Pickup>();

		if (pickup.Type == PickupType.Food) {
			TakeFood(pickup);
		}
	}
	
	IEnumerator EatFood() {
		starveTimer = 0;
		yield return new WaitForSeconds(eatTime);
		Destroy(eatenFood.gameObject);
		eatenFood = null;
		//TODO look hungry
	}
	
	public override void OnInteract(PlayerInteract player) {
		PickupType other = PlayerInteract.Instance.PickupType;
		
		//dont get picked up by food if in a breeeder
		if (other == PickupType.Food && IsStored) {
			//eat food if hungry
			if (IsHungry) {
				TakeFood(PlayerInteract.Instance.Drop());
			}
			return;
		}
		base.OnInteract(player);
	}

	protected override void Die() {
		base.Die();
		EjectSelf();
		Destroy(gameObject);
	}
}