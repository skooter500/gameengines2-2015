﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Boid))]
public class FishBrain : MonoBehaviour {

    [HideInInspector]
    public int current = 0;
    public List<Vector3> waypoints = new List<Vector3>();
    
    Boid boid;
    GameObject food;

    GameObject enemy = null;
    // Use this for initialization
    void Start () {
        boid = GetComponent<Boid>();        
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "food") && (enemy == null))
        {
            food = other.gameObject;
            GetComponent<Boid>().seekEnabled = true;
            GetComponent<Boid>().seekTargetPosition = food.transform.position;
        }
        if (other.gameObject.tag == "enemy")
        {
            enemy = other.gameObject;
            GetComponent<Boid>().seekEnabled = false;
            GetComponent<Boid>().fleeEnabled = true;
            GetComponent<Boid>().fleeTargetPosition = enemy.transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == food)
        {
            GetComponent<Boid>().seekTargetPosition = food.transform.position;
        }
        if (other.gameObject == enemy)
        {
            GetComponent<Boid>().fleeTargetPosition = enemy.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == food && enemy == null)
        {
            if (waypoints.Count > 0)
            {
                boid.seekTargetPosition = waypoints[current];
            }
        }
        if (other.gameObject == enemy)
        {
            enemy = null;
            boid.seekTargetPosition = waypoints[current];
            boid.seekEnabled = true;
            boid.fleeEnabled = false;
        } 
    }
    
    // Update is called once per frame
    void Update () {
        if (Vector3.Distance(transform.position, boid.seekTargetPosition) < 1.0f)
        {
            current = (current + 1) % waypoints.Count;
            boid.seekTargetPosition = waypoints[current];
        }
            
	}
}
