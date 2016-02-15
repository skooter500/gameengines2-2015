using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Boid))]
public class EnemyBrain : MonoBehaviour {

    Boid boid;
    public float range = 50.0f;

    // Use this for initialization
    void Start () {
        boid = GetComponent<Boid>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, boid.arriveTargetPosition) < 1.0f)
        {
            Vector3 pos = Random.insideUnitSphere * range;
            boid.arriveTargetPosition = pos;
        }
    }
}
