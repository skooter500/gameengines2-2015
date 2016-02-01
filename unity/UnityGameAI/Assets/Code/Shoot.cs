using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
    public float speed = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position + transform.forward);
        line.SetPosition(1, transform.position - transform.forward);

    }
}
