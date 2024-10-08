using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FungusInteract : Interactable {
	public Transform growth;
	private Pickup eatenLeaf;
	private List<FungusStem> foodStems = new();
	private List<FungusStem> freeStems = new();

	public int maxFuel = 6;
	private int fuel;
	public float regenTime = 10;
	private float currentTime;
	private bool isRegenning;

	public float starveTime = 40;
	private float starveTimer;
	private bool hasWarnedStarve = false;
	private bool isDead = false;


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
		starveTimer = -0.5f * starveTime;
	}

	private void Update() {
		if (IsHungry && !GameLogic.Instance.TimerPaused) {
			starveTimer += Time.deltaTime;

			if (starveTimer > starveTime / 2 && !hasWarnedStarve) {
				hasWarnedStarve = true;
				ReadOut(String.Format("Oh boy, I don't know if I can make it {0} seconds anymore.",
					starveTime - starveTimer));
			}

			if (starveTimer > starveTime && !isDead) {
				isDead = true;
				Die();
				GameLogic.Instance.GameOver("Good 'ol Mr. Fungus has starved :(((");
			}
		}
	}

	IEnumerator RegenFood() {
		if (fuel < 1) {
			yield break;
		}

		isRegenning = true;
		yield return new WaitForSeconds(regenTime);
		SpawnFood();

		if (fuel < 1) {
			if (eatenLeaf != null) {
				Destroy(eatenLeaf.gameObject);
				eatenLeaf = null;
			}

			isRegenning = false;
			ReadOut("That leaf was splendid! Do you happen to have more where that came from?");
			yield break;
		}

		StartCoroutine(RegenFood());
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
		eatenLeaf.transform.localPosition = new Vector2(-0.42f, 0.26f);

		fuel = maxFuel;
		starveTimer = 0;
		hasWarnedStarve = false;

		if (!isRegenning) {
			StartCoroutine(RegenFood());
		}

		ReadOut("MOMPF!");
	}

	public override void OnInteract(PlayerInteract player) {
		switch (PlayerInteract.Instance.PickupType) {
			case PickupType.Food:
				ReadOut("You give me leaves, I'll grow you some fruit.", "Now everybody has something to eat!");
				break;
			case PickupType.Larva:
				ReadOut("Oh my dear! Bring her to the Nursery! She needs warmth and food!");
				break;
			case PickupType.Leaf:
				if (IsHungry) {
					TakeLeaf();
				}

				break;
			case PickupType.None:
				if (IsHungry) {
					ReadOut("Do you happen to have one of those fresh leaves from the Entrance for me?");
				}
				else {
					ReadOut("*mompfing happily*");
				}

				break;
		}
	}
}