using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IdleState:State
{
    public IdleState(FSM owner):base(owner)
    {
        
    }

    public override string Description()
    {
        return "Idle State";
    }

    public override void Enter()
    {
        Boid boid = owner.GetComponent<Boid>();
        float range = 50.0f;

        boid.path.waypoints.Clear();
        Vector3 min = new Vector3(Random.Range(-range, range), Random.Range(-range, range), -range);

        boid.path.waypoints.Add(min);

        Vector3 max = new Vector3(Random.Range(-range, range), Random.Range(-range, range), range);
        boid.path.waypoints.Add(max);

        boid.TurnOffAll();
        boid.pathFollowEnabled = true;
        boid.path.Looped = true;

        boid.path.next = (int)Random.Range(0, 2);
    }

    public override void Exit()
    {
        Boid boid = owner.GetComponent<Boid>();
        boid.pathFollowEnabled = false;
    }

    public override void Update()
    {
        // Nothing for now
    }
}