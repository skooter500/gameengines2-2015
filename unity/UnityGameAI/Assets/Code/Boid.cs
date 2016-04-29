using UnityEngine;
using BGE;
using System.Collections.Generic;

public class Boid : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public bool applyBanking = true;
    public float straighteningTendancy = 0.2f;
    public float damping = 0.0f;

    public float maxSpeed = 20.0f;
    public float maxForce = 10.0f;

    [Header("Seek")]
    public bool seekEnabled;
    public Vector3 seekTargetPosition;

    [Header("Arrive")]
    public bool arriveEnabled;
    public Vector3 arriveTargetPosition;
    public float slowingDistance = 15;

    [Header("Flee")]
    public bool fleeEnabled;
    public float fleeRange = 15.0f;
    public Vector3 fleeTargetPosition;

    [Header("Pursue")]
    public bool pursueEnabled;
    public GameObject pursueTarget;
    Vector3 pursueTargetPos;

    [Header("Offset Pursue")]
    public bool offsetPursueEnabled = false;
    public GameObject offsetPursueTarget = null;
    Vector3 offset;
    Vector3 offsetPursueTargetPos;


    [HideInInspector]
    public int current = 0;

    [Header("Path Following")]
    public bool pathFollowEnabled = false;
    public Path path = new Path();

    [Header("Wander")]
    public bool wanderEnabled = false;
    public float wanderRadius = 10.0f;
    public float wanderJitter = 1.0f;
    public float wanderDistance = 15.0f;
    private Vector3 wanderTargetPos;

    [Header("PlaneAvoidance")]
    public bool planeAvoidanceEnabled;
    public float feelerDistance = 20.0f;
    private bool planeAvoidanceActive = false;
    List<Vector3> planeAvoidanceFeelers = new List<Vector3>();
    public List<Plane> planes = new List<Plane>();

    public void MakeFeelers()
    {
        planeAvoidanceFeelers.Clear();
        Vector3 newFeeler = Vector3.forward * feelerDistance;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(45, Vector3.up) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(-45, Vector3.up) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(45, Vector3.right) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(-45, Vector3.right) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);
    }

    public Vector3 PlaneAvoidance()
    {
        MakeFeelers();
        planeAvoidanceActive = false;
        Vector3 force = Vector3.zero;
        foreach (Vector3 feeler in planeAvoidanceFeelers)
        {
            foreach (Plane plane in planes)
            {
                if (!plane.GetSide(feeler))
                {
                    planeAvoidanceActive = true;
                    float distance = Mathf.Abs(plane.GetDistanceToPoint(feeler));
                    force += plane.normal * distance;
                }
            }            
        }        
        return force;
    }

    public void TurnOffAll()
    {
        seekEnabled = arriveEnabled = fleeEnabled = pursueEnabled = offsetPursueEnabled = pathFollowEnabled = wanderEnabled = planeAvoidanceEnabled = false;
    }

    public Vector3 Wander()
    {
        float jitterTimeSlice = wanderJitter * Time.deltaTime;

        Vector3 toAdd = Random.insideUnitSphere * jitterTimeSlice;
        wanderTargetPos += toAdd;
        wanderTargetPos.Normalize();
        wanderTargetPos *= wanderRadius;
        
        /*Quaternion jitter = Quaternion.AngleAxis(jitterTimeSlice, Random.insideUnitSphere);
        wanderTargetPos = jitter * wanderTargetPos;
        */
        Vector3 localTarget = wanderTargetPos + Vector3.forward * wanderDistance;
        Vector3 worldTarget = transform.TransformPoint(localTarget);        
        return (worldTarget - transform.position);
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

        if (pathFollowEnabled)
        {
            path.DrawGizmos();
        }

        if (wanderEnabled)
        {
            Gizmos.color = Color.blue;
            Vector3 wanderCircleCenter = transform.TransformPoint(Vector3.forward * wanderDistance);
            Gizmos.DrawWireSphere(wanderCircleCenter, wanderRadius);
            Gizmos.color = Color.green;
            Vector3 worldTarget = transform.TransformPoint(wanderTargetPos + Vector3.forward * wanderDistance);
            Gizmos.DrawLine(transform.position, worldTarget);
            path.DrawGizmos();
        }

        if (planeAvoidanceEnabled && planeAvoidanceActive)
        {
            foreach (Vector3 feeler in planeAvoidanceFeelers)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(transform.position, feeler);
            }
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

        if (planeAvoidanceEnabled)
        {
            force += PlaneAvoidance();
        }

        force = Vector3.ClampMagnitude(force, maxForce);
        Vector3 acceleration = force / mass;        
        velocity += acceleration * Time.deltaTime;        
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }

        transform.position += velocity * Time.deltaTime;

        if (applyBanking)
        {
            float smoothRate = Utilities.Clip(9.0f * Time.deltaTime, 0.15f, 0.4f) / 2.0f;
            Utilities.BlendIntoAccumulator(smoothRate, acceleration, ref acceleration);            
            // the length of this global-upward-pointing vector controls the vehicle's
            // tendency to right itself as it is rolled over from turning acceleration
            Vector3 globalUp = new Vector3(0, straighteningTendancy, 0);
            // acceleration points toward the center of local path curvature, the
            // length determines how much the vehicle will roll while turning
            Vector3 accelUp = acceleration * 0.05f;
            // combined banking, sum of UP due to turning and global UP
            Vector3 bankUp = accelUp + globalUp;
            // blend bankUp into vehicle's UP basis vector
            smoothRate = Time.deltaTime * 3.0f;
            Vector3 tempUp = transform.up;
            Utilities.BlendIntoAccumulator(smoothRate, bankUp, ref tempUp);

            transform.forward.Normalize();
            transform.LookAt(transform.position + transform.forward, tempUp);            
        }

        // Apply damping
        velocity *= (1.0f - damping * Time.deltaTime);

        /*
        force = Vector3.ClampMagnitude(force, maxForce);
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }     
        */
    }
        
}
