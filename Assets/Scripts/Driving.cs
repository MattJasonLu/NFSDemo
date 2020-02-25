using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    public WheelCollider flWheelCollider;
    public WheelCollider frWheelCollider;
    public WheelCollider rlWheelCollider;
    public WheelCollider rrWheelCollider;

    public Transform flWheelModel;
    public Transform frWheelModel;
    public Transform rlWheelModel;
    public Transform rrWheelModel;

    public Transform flDiscBrake;
    public Transform frDiscBrake;

    public float motorTorque = 20f;
    public float steerAngle = 10f;
    // 刹车力量
    public float brakeTorque = 100f;
    public Transform centerOfMass;

    public float maxSpeed = 140;
    public float minSpeed = 30;
    private bool isBrake = false;

    public AudioSource carEngineAudio;
    private float currentSpeed;

    public int[] speedArray;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // 获取速度
        currentSpeed = flWheelCollider.rpm * (flWheelCollider.radius * 2 * Mathf.PI) * 60 / 1000;

        if (currentSpeed * Input.GetAxis("Vertical") < 0)
        {
            isBrake = true;
        }
        else
        {
            isBrake = false;
        }
        if (currentSpeed > maxSpeed && Input.GetAxis("Vertical") > 0 ||
            currentSpeed < -minSpeed && Input.GetAxis("Vertical") < 0)
        {
            flWheelCollider.motorTorque = 0;
            frWheelCollider.motorTorque = 0;
        }
        else
        {
            flWheelCollider.motorTorque = Input.GetAxis("Vertical") * motorTorque;
            frWheelCollider.motorTorque = Input.GetAxis("Vertical") * motorTorque;
        }

        if (isBrake)
        {
            flWheelCollider.motorTorque = 0;
            frWheelCollider.motorTorque = 0;
            flWheelCollider.brakeTorque = brakeTorque;
            frWheelCollider.brakeTorque = brakeTorque;
        }
        else
        {
            flWheelCollider.brakeTorque = 0;
            frWheelCollider.brakeTorque = 0;
        }

        flWheelCollider.steerAngle = Input.GetAxis("Horizontal") * steerAngle;
        flWheelCollider.steerAngle = Input.GetAxis("Horizontal") * steerAngle;

        RotateWheel();
        SteerWheel();
        EngineSound();
    }

    // 轮胎前后转动
    void RotateWheel()
    {
        flDiscBrake.Rotate(flWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
        frDiscBrake.Rotate(flWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
        rlWheelModel.Rotate(flWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
        rrWheelModel.Rotate(flWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
    }

    // 轮胎左右转动
    void SteerWheel()
    {
        Vector3 localEulerAngles = flWheelModel.localEulerAngles;
        localEulerAngles.y = flWheelCollider.steerAngle;
        flWheelModel.localEulerAngles = localEulerAngles;
        frWheelModel.localEulerAngles = localEulerAngles;
    }

    // 根据速度调节引擎声音的音高
    void EngineSound()
    {
        int index = 0;
        for (int i = 0; i < speedArray.Length - 2; i++)
        {
            if (currentSpeed >= speedArray[i])
            {
                index = i;
            }
        }
        int minSpeed = speedArray[index];
        int maxSpeed = speedArray[index + 1];

        carEngineAudio.pitch = 0.05f + (currentSpeed - minSpeed) / (maxSpeed - minSpeed) * 0.8f;
    }
}
