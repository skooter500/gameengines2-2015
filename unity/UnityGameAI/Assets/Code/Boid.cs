﻿using UnityEngine;
using System.Collections.Generic;

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

    public bool offsetPursueEnabled = false;
    public GameObject offsetPursueTarget = null;
    Vector3 offset;
    Vector3 offsetPursueTargetPos;

    [Header("Wander")]
    public bool wanderEnabled;
    public float wanderRadius = 10.0f;
    public float wanderJitter = 20.0f;
    public float wanderDistance = 15.0f;
    private Vector3 wanderTargetPos = Vector3.zero;


    [HideInInspector]
    public int current = 0;

    [Header("Path Following")]
    public bool pathFollowEnabled = false;
    public Path path = new Path();

    public void TurnOffAll()
    {
        seekEnabled = arriveEnabled = fleeEnabled = pursueEnabled = offsetPursueEnabled = pathFollowEnabled = wanderEnabled = false;
    }
    
    public Vector3 FollowPath()
    {
        float epsilon = 5.0f;
        float dist = (transform.position - path.NextWaypoint()).magnitude;
        if (dist < epsilon)
        {
            path.AdvanceToNext();
        }
        if ((!path.Looped) && path.IsLast)
        {
            return Arrive(path.NextWaypoint());
        }
        else
        {
            return Seek(path.NextWaypoint());
        }
    }
    
    
    public Vector3 Pursue(GameObject target)
    {
        Vector3 toTarget = target.transform.position - transform.position;
        float lookAhead = toTarget.magnitude  / maxSpeed;
        pursueTargetPos = target.transform.position
           + (target.GetComponent<Boid>().velocity * lookAhead);
        
        return Seek(pursueTargetPos);
    }

    // Use this for initialization
    void Start ()
    {
        if (offsetPursueEnabled)
        {
            offset = transform.position - offsetPursueTarget.transform.position;
            offset = Quaternion.Inverse(
                   offsetPursueTarget.transform.rotation) * offset;
        }

        wanderTargetPos = Random.insideUnitSphere * wanderRadius;
    }

    public Vector3 Wander()
    {
        float jitterTimeSlice = wanderJitter * Time.deltaTime;

        Vector3 toAdd = Random.insideUnitSphere * jitterTimeSlice;

        wanderTargetPos += toAdd;
        wanderTargetPos.Normalize();
        wanderTargetPos *= wanderRadius;
        //Vector3 localTarget = wanderTargetPos + (Vector3.forward * wanderDistance);
        //Vector3 worldTarget = transform.TransformPoint(localTarget);
        Vector3 worldTarget = transform.position + (transform.forward * wanderDistance) + wanderTargetPos;
        
        return (worldTarget - transform.position);
    }

    public Vector3 OffsetPursue(GameObject leader, Vector3 offset)
    {
        Vector3 target = leader.transform.TransformPoint(offset);
        Vector3 toTarget = transform.position - target;
        float dist = toTarget.magnitude;
        float lookAhead = dist / maxSpeed;

        offsetPursueTargetPos = target + (lookAhead * leader.GetComponent<Boid>().velocity);
        return Arrive(offsetPursueTargetPos);
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
            return Vector3.zero;
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, offsetPursueTargetPos);
        }

        if (wanderEnabled)
        {
            Gizmos.color = Color.blue;
            Vector3 wanderCircleCenter = transform.TransformPoint((Vector3.forward * wanderDistance));
            Gizmos.DrawWireSphere(wanderCircleCenter, wanderRadius);
            Vector3 worldTarget = transform.TransformPoint(wanderTargetPos + (Vector3.forward * wanderDistance));
            Gizmos.DrawLine(transform.position, worldTarget);
        }

        if (pathFollowEnabled)
        {
            path.DrawGizmos();
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

        if (pathFollowEnabled)
        {
            force += FollowPath();
        }

        if (wanderEnabled)
        {
            force += Wander();
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
