using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DeadState : State
{
    public DeadState(FSM owner):base(owner)
    {
        
    }

    public override string Description()
    {
        return "Dead State";
    }

    public override void Enter()
    {
        Boid boid = owner.GetComponent<Boid>();
        boid.TurnOffAll();
        owner.GetComponent<FishParts>().RagDoll();

    }

    public override void Exit()
    {
        // We never 
    }

    public override void Update()
    {
        
    }
}
