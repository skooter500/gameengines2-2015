using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour {
    public GameObject foodPrefab;
    public float range = 50;
	// Use this for initialization
	void Start () {
        StartCoroutine("SpawnFood");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    IEnumerator SpawnFood()
    {
        while (true)
        {
            GameObject food = GameObject.Instantiate(foodPrefab);
            Vector3 pos = Random.insideUnitSphere;
            pos *= range;
            pos.y = transform.position.y;
            food.transform.position = pos;
            yield return new WaitForSeconds(2);
        }        
    }
}
