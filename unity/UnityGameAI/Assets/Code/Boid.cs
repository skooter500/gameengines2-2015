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

    public bool arriveEnabled;
    public Vector3 arriveTargetPosition;
    public float slowingDistance = 15;

    public bool fleeEnabled;
    public float fleeRange = 15.0f;
    public Vector3 fleeTargetPosition;

    // Use this for initialization
    void Start () {
	
	}

    public Vector3 Flee(Vector3 targetPos, float range)
    {
        Vector3 desiredVelocity;
        desiredVelocity = transform.position - targetPos;
        if (desiredVelocity.magnitude > range)
        {
            return Vector3.zero;
        }
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Debug.Log("Flee");
        return desiredVelocity - velocity;
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
        if (arriveEnabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, arriveTargetPosition);
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

    // Update is called once per frame
    void Update()
    {
        force = Vector3.zero;

        if (seekEnabled)
        {
            force += Seek(seekTargetPosition);
        }
        if (arriveEnabled)
        {
            force += Arrive(arriveTargetPosition);
        }
        if (fleeEnabled)
        {
            force += Flee(fleeTargetPosition, fleeRange);
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
