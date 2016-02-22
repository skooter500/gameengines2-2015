using UnityEngine;
using System.Collections.Generic;

public class FishSpawner1 : MonoBehaviour
{
    public float range = 50;
    public int count = 10;

    public Color color;

    public GameObject prefab;


// Use this for initialization
    void Start () {
        for (int i = 0; i < count; i++)
        {
            GameObject fish = Instantiate(prefab);
            fish.transform.parent = transform;
            fish.transform.position = Random.insideUnitSphere * range;
            fish.GetComponent<FishParts>().spawnColor = color;
            fish.GetComponent<Boid>().maxSpeed = Random.Range(4.0f, 6.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
