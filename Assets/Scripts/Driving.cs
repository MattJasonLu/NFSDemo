using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    public WheelCollider flWheelCollider;
    public WheelCollider frWheelCollider;
    public float motorTorque = 20f;
    public float steerAngle = 10f;
    public Transform centerOfMass;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        flWheelCollider.motorTorque = Input.GetAxis("Vertical") * motorTorque;
        frWheelCollider.motorTorque = Input.GetAxis("Vertical") * motorTorque;

        flWheelCollider.steerAngle = Input.GetAxis("Horizontal") * steerAngle;
        flWheelCollider.steerAngle = Input.GetAxis("Horizontal") * steerAngle;
    }
}
