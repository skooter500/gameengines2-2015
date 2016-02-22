using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject prefab;
    public int count = 3;
    public float range = 50;
    public Color color;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < count; i++)
        {
            GameObject fish = Instantiate(prefab);
            fish.transform.position = Random.insideUnitSphere * range;

            Vector3 pos = Random.insideUnitSphere * range;
            fish.GetComponent<Boid>().arriveTargetPosition = pos;

            fish.GetComponent<FishParts>().spawnColor = color;
            fish.GetComponent<Boid>().arriveEnabled = true;
            fish.GetComponent<Boid>().maxSpeed = Random.Range(4.0f, 6.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
