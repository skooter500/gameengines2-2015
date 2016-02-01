using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1;

    public float maxSpeed = 5;
    public float maxForce = 5;

    public bool seekEnabled;
    public Vector3 seekTargetPosition;
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (seekEnabled)
        {
            Gizmos.DrawRay(transform.position, seekTargetPosition);
        }
    }
    
    // Use this for initialization
    void Start () {
	
	}

    public Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }
    
	
	// Update is called once per frame
	void Update () {

        if (seekEnabled)
        {
            force += Seek(seekTargetPosition);
        }

        force = Vector3.ClampMagnitude(force, maxForce);
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        float speed = velocity.magnitude;
        // Make the boid point in the direction its facing
        if (speed > float.Epsilon)
        {
            transform.forward = velocity;
        }
        transform.position += velocity * Time.deltaTime;
	}
}
