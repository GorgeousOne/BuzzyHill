using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FungusInteract : Dialog {

	public Transform growth;
	private Pickup eatenLeaf;
	private List<FungusStem> foodStems = new();
	private List<FungusStem> freeStems = new();
	
	public int maxFuel = 6;
	public int fuel;
	public float regenTime = 10;
	private float currentTime;
	private bool isRegenning;
	public bool IsHungry {
		get { return fuel < 1; }
	}
	
	private void Start() {
		for (int i = 0; i < growth.childCount; ++i) {
			FungusStem stem = growth.GetChild(i).GetComponent<FungusStem>();
			foodStems.Add(stem);
			freeStems.Add(stem);
			stem.OnFoodPickup += OnStemFreed;
		}
	}

	protected override void OnEnable() {
		base.OnEnable();
		StartCoroutine(RegenFood());
	}

	IEnumerator RegenFood() {
		isRegenning = true;
		yield return new WaitForSeconds(regenTime);
		SpawnFood();

		if (fuel < 1) {
			Destroy(eatenLeaf);
			eatenLeaf = null;
			isRegenning = false;
			yield break;
		}
		if (freeStems.Count > 0) {
			StartCoroutine(RegenFood());
		} else {
			eatenLeaf = null;
			isRegenning = false;
		}
	}
	
	private void TakeLeaf() {
		EatLeaf(PlayerInteract.Instance.Drop());
	}

	void SpawnFood() {
		if (freeStems.Count == 0 || fuel < 1) {
			return;
		}
		fuel -= 1;
		int rndIndex = Random.Range(0, freeStems.Count - 1);
		FungusStem stem = freeStems[rndIndex];
		freeStems.RemoveAt(rndIndex);
		stem.SpawnPickup();
	}

	void OnStemFreed(FungusStem stem) {
		freeStems.Add(stem);
		if (!isRegenning) {
			StartCoroutine(RegenFood());
		}
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

		if (pickup.Type == PickupType.Leaf) {
			EatLeaf(pickup);
		}
	}
	
	void EatLeaf(Pickup food) {
		eatenLeaf = food;
		eatenLeaf.Freeze();
		eatenLeaf.transform.parent = transform;
		eatenLeaf.transform.localPosition = Vector3.up;
		fuel = maxFuel;
		
		if (!isRegenning) {
			StartCoroutine(RegenFood());
		}
	}

	public override void OnInteract(PlayerInteract player) {
		switch (PlayerInteract.Instance.PickupType) {
			case PickupType.Food:
				ReadOut("I can only eat leaves!", "whatever");
				break;
			case PickupType.Larva:
				ReadOut("Bro, this is your offspring, give me leaves!", "whatever");
				break;
			case PickupType.Leaf:
				ReadOut("yummi!");
				TakeLeaf();
				break;
			case PickupType.None:
				break;
		}
	}
}