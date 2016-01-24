﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FPSController : MonoBehaviour
{
    Camera ovrCamera;
    float speed = 200.0f;
    // Use this for initialization
    void Start()
    {

        if (GameObject.FindGameObjectWithTag("ovrplayer") != null)
        {
            //ovrCamera = GameObject.FindGameObjectWithTag("ovrplayer").GetComponentInChildren<Camera>();
        }
    }

    void Yaw(float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = rot * transform.rotation;
    }

    void Roll(float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rot * transform.rotation;
    }

    void Pitch(float angle)
    {
        float invcosTheta1 = Vector3.Dot(transform.forward, Vector3.up);
        float threshold = 0.95f;
        if ((angle > 0 && invcosTheta1 < (-threshold)) || (angle < 0 && invcosTheta1 > (threshold)))
        {
            return;
        }

        // A pitch is a rotation around the right vector
        Quaternion rot = Quaternion.AngleAxis(angle, transform.right);

        transform.rotation = rot * transform.rotation;
    }

    void Walk(float units)
    {
        if (ovrCamera != null)
        {
            transform.position += ovrCamera.transform.forward * units;
        }
        else
        {
            transform.position += transform.forward * units;
        }
    }

    void Fly(float units)
    {
        transform.position += Vector3.up * units;
    }

    void Strafe(float units)
    {
        if (ovrCamera != null)
        {
            transform.position += ovrCamera.transform.right * units;
        }
        else
        {
            transform.position += transform.right * units;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX, mouseY;
        float speed = this.speed;

        float runAxis = Input.GetAxis("Run Axis");

        if (Input.GetKey(KeyCode.LeftShift) || runAxis != 0)
        {
            speed *= 5.0f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Walk(Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Walk(-Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Strafe(-Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Strafe(Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Roll(-Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Roll(Time.deltaTime * speed);
        }
        //BoidManager.PrintVector("OVR Forward: ", ovrCamera.transform.forward);

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");


        Yaw(mouseX);
        float contYaw = Input.GetAxis("Yaw Axis");
        float contPitch = Input.GetAxis("Pitch Axis");
        Yaw(contYaw);

        // If in Rift mode, dont pitch
        if (ovrCamera == null)
        {
            Pitch(-mouseY);
            Pitch(contPitch);
        }
        else
        {
            Fly(-contPitch * speed * Time.deltaTime);
        }

        float contWalk = Input.GetAxis("Walk Axis");
        float contStrafe = Input.GetAxis("Strafe Axis");
        if (Mathf.Abs(contWalk) > 0.1f)
        {
            Walk(-contWalk * speed * Time.deltaTime);
        }
        if (Mathf.Abs(contStrafe) > 0.1f)
        {
            Strafe(contStrafe * speed * Time.deltaTime);
        }
    }
}