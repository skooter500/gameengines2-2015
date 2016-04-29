using UnityEngine;
using System.Collections;

public class PlaneAvoidanceController : MonoBehaviour {

    Boid boid;
    // Use this for initialization
    void Start () {
        boid = GetComponent<Boid>();
        boid.seekEnabled = true;
        boid.planeAvoidanceEnabled = true;
        boid.planes.Add(new Plane(new Vector3(0, 1, 0), 50));        
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject camera = (GameObject)GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 newTargetPos = camera.transform.position + camera.transform.forward * 100.0f;
            boid.seekTargetPosition= newTargetPos;
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject camera = (GameObject)GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 newTargetPos = camera.transform.position;
            boid.seekTargetPosition = newTargetPos;
        }
    }
}
