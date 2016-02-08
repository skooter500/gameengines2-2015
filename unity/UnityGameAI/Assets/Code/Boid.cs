using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass;

    public float maxSpeed = 5.0f;
    public float maxForce = 5.0f;

    public bool seekEnabled;
    public Vector3 seekTargetPosition;
    
    // Use this for initialization
    void Start () {
	
	}

    public Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

        float slowingDistance = 15.0f;
        float distance = toTarget.magnitude;
        if (distance < 0.5f)
        {
            velocity = Vector3.zero;
        } 
        float ramped = maxSpeed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - velocity;
    }

    void OnDrawGizmos()
    {
        if (seekEnabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, seekTargetPosition);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.position + force);
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }

    /*
    Vector3 Arrive(Vector3 target)
    {

    }
    */

    // Update is called once per frame
    void Update()
    {
        force = Vector3.zero;

        if (seekEnabled)
        {
            force = Arrive(seekTargetPosition);
        }

        force = Vector3.ClampMagnitude(force, maxForce);
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }        
    }
        
}
