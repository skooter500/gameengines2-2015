using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;

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

    public bool pursueEnabled;
    public GameObject pursueTarget;
    Vector3 pursueTargetPos;

    public bool offsetPursueEnabled;
    public GameObject offsetPursueTarget;
    Vector3 offset;
    Vector3 offsetPursueTargetPos;


    public Vector3 Pursue(GameObject target)
    {
        Vector3 toTarget = target.transform.position - transform.position;
        float lookAhead = toTarget.magnitude  / maxSpeed;
        pursueTargetPos = target.transform.position
           + (target.GetComponent<Boid>().velocity * lookAhead);
        
        return Seek(pursueTargetPos);
    }

    public Vector3 OffsetPursue(GameObject offsetPursueTarget, Vector3 offset)
    {
        Vector3 target = offsetPursueTarget.transform.TransformPoint(offset);
        
        Vector3 toTarget = target - transform.position;
        float lookAhead = toTarget.magnitude / maxSpeed;

        target += (offsetPursueTarget.GetComponent<Boid>().velocity * lookAhead);

        offsetPursueTargetPos = target;

        return Arrive(target);
    }

    // Use this for initialization
    void Start ()
    {
	    if (offsetPursueEnabled)
        {
           offset = transform.position - offsetPursueTarget.transform.position;
           offset = offsetPursueTarget.transform.rotation * offset;
        }
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
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, seekTargetPosition);
        }
        if (arriveEnabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, arriveTargetPosition);
        }
        if (pursueEnabled)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, pursueTargetPos);
        }

        if (offsetPursueEnabled)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, offsetPursueTargetPos);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force);
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
        if (pursueEnabled)
        {
            force += Pursue(pursueTarget);
        }
        if (offsetPursueEnabled)
        {
            force += OffsetPursue(offsetPursueTarget, offset);
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
