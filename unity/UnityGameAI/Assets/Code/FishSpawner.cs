using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour {
    public float range = 50;
    public int count = 10;

    public Color color;

    public GameObject prefab;

	void Start () {
        for (int i = 0; i < count; i++)
        {
            GameObject fish = Instantiate(prefab);
            fish.transform.position = Random.insideUnitSphere * range;

            List<Vector3> waypoints = fish.GetComponent<FishBrain>().waypoints;
            Vector3 min = fish.transform.position;
            min.z = -range;
            waypoints.Add(min);

            Vector3 max = fish.transform.position;
            max.z = range;
            waypoints.Add(max);

            int whichToStart = (int) Random.Range(0, 2);
            fish.GetComponent<Boid>().seekTargetPosition = waypoints[whichToStart];
            fish.GetComponent<FishBrain>().current = whichToStart;

            fish.GetComponent<FishParts>().spawnColor = color;
            fish.GetComponent<Boid>().seekEnabled = true;
            fish.GetComponent<Boid>().maxSpeed = Random.Range(4.0f, 6.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
