using UnityEngine;
using System.Collections.Generic;


public class CircleFollowing : MonoBehaviour {
    public float radius = 5;
    public int waypointCount = 10;
    int curent = 0;
    private List<Vector3> waypoints = new List<Vector3>();

    System.Collections.IEnumerator fireProjectile()
    {
        while (true)
        {
            // Use a line renderer
            GameObject lazer = new GameObject();
            lazer.transform.position = transform.position;
            lazer.transform.rotation = transform.rotation;
            LineRenderer line = lazer.AddComponent<LineRenderer>();
            lazer.AddComponent<Shoot>();
            line.material = new Material(Shader.Find("Particles/Additive"));
            line.SetColors(Color.red, Color.blue);
            line.SetWidth(0.1f,0.1f);
            line.SetVertexCount(2);
            yield return new WaitForSeconds(2.0f);
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i], waypoints[(i + 1) % waypointCount]);
        }
    }

	// Use this for initialization
	void Start () {
        float thetaInc = (Mathf.PI * 2.0f) / waypointCount;
        Vector3 basis = new Vector3(0, 0, radius);
        for (int i = 0; i < waypointCount; i++)
        {
            float theta = i * thetaInc;

            Vector3 pos = new Vector3();
            pos.x = Mathf.Sin(theta) * radius;
            pos.z = Mathf.Cos(theta) * radius;
            pos.y = 0;
            // Alternatively, use a quaternion
            //Quaternion q = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, Vector3.up);
            //Vector3 pos = q * basis;
            waypoints.Add(pos);
        }
        StartCoroutine("fireProjectile");
    }
    float speed = 2.0f;
	// Update is called once per frame
	void Update () {
        float dist = Vector3.Distance(transform.position, waypoints[curent]);
        if (dist < 0.5f)
        {
            curent = (curent + 1) % waypoints.Count;
        }
        transform.LookAt(waypoints[curent]);

        transform.position = Vector3.MoveTowards(transform.position, waypoints[curent], speed * Time.deltaTime);
        
        // Or use translate
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
}
