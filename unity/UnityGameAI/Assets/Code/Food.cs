using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, Random.Range(0.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
