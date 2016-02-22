
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FishParts : MonoBehaviour
{
    [HideInInspector]
    public GameObject head;
    [HideInInspector]
    public GameObject body;
    [HideInInspector]
    public GameObject tail;

    List<GameObject> segments;

    float segmentExtents = 3;
    public float gap;

    // Animation stuff
    float theta;
    float angularVelocity = 5.00f;

    private Vector3 headRotPoint;
    private Vector3 tailRotPoint;

    private Vector3 headSize;
    private Vector3 bodySize;
    private Vector3 tailSize;

    public float speedMultiplier;

    public float headField;
    public float tailField;

    public GameObject boidGameObject;

    [HideInInspector]
    public Boid boid;

    public Color spawnColor;

    public void RagDoll()
    {
        // Attach Rigidbodies to each of the parts and connect them with joints
        for (int j = 0; j < transform.childCount; j++)
        {
            // Physically simulate the boid
            GameObject child = transform.GetChild(j).gameObject;
            child.AddComponent<Rigidbody>();
            child.AddComponent<BoxCollider>();
        }
        // Add two hinge joints
        HingeJoint hj1 = body.AddComponent<HingeJoint>();
        hj1.connectedBody = head.GetComponent<Rigidbody>();
        hj1.axis = Vector3.up;
        hj1.anchor = new Vector3(0, 0, 0.6f);
        hj1.autoConfigureConnectedAnchor = false;

        HingeJoint hj2 = tail.AddComponent<HingeJoint>();
        hj2.connectedBody = body.GetComponent<Rigidbody>();
        hj2.axis = Vector3.up;
        hj2.anchor = new Vector3(0, 0, 0.6f);
        hj2.autoConfigureConnectedAnchor = true;
        
        GetComponent<Boid>().enabled = false;
        GetComponent<FishParts>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;

    }

    public FishParts()
    {
        segments = new List<GameObject>();

        theta = 0;
        speedMultiplier = 1.0f;
        headField = 5;
        tailField = 50;
    }

    public GameObject InstiantiateDefaultShape()
    {

        GameObject segment = null;
        segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 scale = new Vector3(segmentExtents / 2, segmentExtents, segmentExtents);
        segment.transform.localScale = scale;
        segment.transform.parent = transform;
        return segment;
    }

    public void OnDrawGizmos()
    {
        float radius = (1.5f * segmentExtents) + gap;
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, radius);
    }


    public void Start()
    {

        if (transform.childCount != 3)
        {
            head = InstiantiateDefaultShape();
            body = InstiantiateDefaultShape();
            tail = InstiantiateDefaultShape();
            LayoutSegments();
        }
        else
        {
            head = transform.GetChild(0).gameObject;
            body = transform.GetChild(1).gameObject;
            tail = transform.GetChild(2).gameObject;
        }

        segments.Add(head);
        segments.Add(body);
        segments.Add(tail);

        if (head.GetComponent<Collider>() != null)
        {
            head.GetComponent<Collider>().enabled = false;
        }
        if (body.GetComponent<Collider>() != null)
        {
            body.GetComponent<Collider>().enabled = false;
        }
        if (tail.GetComponent<Collider>() != null)
        {
            tail.GetComponent<Collider>().enabled = false;
        }

        boid = (boidGameObject == null) ? GetComponent<Boid>() : boidGameObject.GetComponent<Boid>();

    }

    float headOffset;
    float tailOffset;
    
    private void LayoutSegments()
    {
        bodySize = body.GetComponent<Renderer>().bounds.size;
        headSize = head.GetComponent<Renderer>().bounds.size;
        tailSize = tail.GetComponent<Renderer>().bounds.size;

        body.transform.position = transform.position;

        headOffset = (bodySize.z / 2.0f) + gap + (headSize.z / 2.0f);
        head.transform.position = transform.position + new Vector3(0, 0, headOffset);

        tailOffset = (bodySize.z / 2.0f) + gap + (tailSize.z / 2.0f);
        tail.transform.position = transform.position + new Vector3(0, 0, -tailOffset);

        head.transform.parent = transform;
        tail.transform.parent = transform;
        body.transform.parent = transform;

        headRotPoint = head.transform.localPosition;
        headRotPoint.z -= headSize.z / 2;

        tailRotPoint = tail.transform.localPosition;
        tailRotPoint.z += tailSize.z / 2;
       
        for (int j = 0; j < transform.childCount; j++)
        {            
            transform.GetChild(j).GetComponent<Renderer>().material.color = spawnColor;
        }
        
    }

    float oldHeadRot = 0;
    float oldTailRot = 0;

    public void Update()
    {
        // Animate the head            
        float headRot = Mathf.Sin(theta) * headField;
        head.transform.RotateAround(transform.TransformPoint(headRotPoint), transform.up, headRot - oldHeadRot);

        oldHeadRot = headRot;

        // Animate the tail
        float tailRot = Mathf.Sin(theta) * tailField;
        tail.transform.RotateAround(transform.TransformPoint(tailRotPoint), transform.up, tailRot - oldTailRot);
        oldTailRot = tailRot;

        float speed = boid.velocity.magnitude;
        theta += speed * angularVelocity * Time.deltaTime * speedMultiplier;
        if (theta >= Mathf.PI * 2.0f)
        {
            theta -= (Mathf.PI * 2.0f);
        }
    }
}
