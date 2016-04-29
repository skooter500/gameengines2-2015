using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {
    public float hunger = 0;
    public float maxHunger = 30.0f;

    State state = null;

	// Use this for initialization
	void Start () {
        SwitchState(new IdleState(this));
        StartCoroutine("Consume");
	}

    System.Collections.IEnumerator Consume()
    {
        while (hunger < maxHunger)
        {
            hunger++;
            // Change to black the more hungry we are
            Color spawnColor = GetComponent<FishParts>().spawnColor;
            GetComponent<Boid>().maxSpeed = ((maxHunger - hunger) / maxHunger) * 5.0f;
            for (int j = 0; j < transform.childCount; j++)
            {                
                transform.GetChild(j).GetComponent<Renderer>().material.color = Color.Lerp(spawnColor, Color.black, hunger / maxHunger);
            }
            yield return new WaitForSeconds(1.0f);
        }
        SwitchState(new DeadState(this));
    }

    public void SwitchState(State state)
    {
        if (this.state != null)
        {
            this.state.Exit();
        }

        this.state = state;

        if (this.state != null)
        {
            this.state.Enter();
        }
    }


    // Update is called once per frame
    void Update () {
        if (state != null)
        {
            state.Update();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "food"))
        {
            if ((food == null) && (hunger > 10))
            {
                food = other.gameObject;
                SwitchState(new ChaseFoodState(this, other.gameObject));
            }

        }
    }

    /*
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == hunger)
        {
            GetComponent<Boid>().seekTargetPosition = hunger.transform.position;
        }       
    }
    */

    GameObject food;
    /*void OnTriggerExit(Collider other)
    {
        if ((other.gameObject == food))
        {
            food = null;            
            SwitchState(new IdleState(this));
        }
    }
    */
}
