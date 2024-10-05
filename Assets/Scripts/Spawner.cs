using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject prefab;
	public float fuel;
	public float maxFuel = 1;
	public float fuelDepleteTime = 20;
	public float regenTime = 10;
	private float currentTime;
	
    
	void Start() {
	}

	void Update() {
		currentTime -= Time.deltaTime;
		if (currentTime < 0) {
			
		}
	}

	private void spawnItem() {
		
	}

	public void addFuel() {
		fuel = maxFuel;
	}
}