using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChaseFoodState:State
{
    GameObject food;

    public ChaseFoodState(FSM owner, GameObject food):base(owner)
    {
        this.food = food;
    }

    public override string Description()
    {
        return "Chase Food State";
    }

    public override void Enter()
    {
        Boid boid = owner.GetComponent<Boid>();
        boid.seekEnabled = true;        
    }

    public override void Exit()
    {
        Boid boid = owner.GetComponent<Boid>();
        boid.seekEnabled = false;
    }

    private void EatFood()
    {
        owner.hunger = 0.0f;
        GameObject.Destroy(food);
    }

    public override void Update()
    {
        // Somebody elase eaten the food?
        if (food == null)
        {
            owner.SwitchState(new IdleState(owner));
        }
        else
        {
            Boid boid = owner.GetComponent<Boid>();
            boid.seekTargetPosition = food.transform.position;
            if (Vector3.Distance(owner.transform.position, food.transform.position) < 1.0f)
            {
                EatFood();
                owner.SwitchState(new IdleState(owner));
            }
        }
    }
}