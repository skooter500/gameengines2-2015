using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Path
{
    public List<Vector3> waypoints = new List<Vector3>();
    public int next = 0;
    public bool Looped;

    public void DrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 1; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i], waypoints[(i + 1) % waypoints.Count]);
        }
    }
    
    public Vector3 NextWaypoint()
    {
        return waypoints[next];
    }

    public bool IsLast
    {
        get
        {
            return (next == waypoints.Count() - 1);
        }
    }

    public void AdvanceToNext()
    {
        if (Looped)
        {
            next = (next + 1) % waypoints.Count();
        }
        else
        {
            if (next != waypoints.Count() - 1)
            {
                next = next + 1;
            }
        }
    }
}