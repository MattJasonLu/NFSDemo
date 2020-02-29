using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidDriving : MonoBehaviour
{
    [HideInInspector]
    public WheelCollider flWheelCollider;
    [HideInInspector]
    public WheelCollider frWheelCollider;
    [HideInInspector]
    public WheelCollider rlWheelCollider;
    [HideInInspector]
    public WheelCollider rrWheelCollider;
    [HideInInspector]
    public Transform flWheelModel;
    [HideInInspector]
    public Transform frWheelModel;
    [HideInInspector]
    public Transform rlWheelModel;
    [HideInInspector]
    public Transform rrWheelModel;
    [HideInInspector]
    public Transform flDiscBrake;
    [HideInInspector]
    public Transform frDiscBrake;

    public float motorTorque = 20f;
    public float steerAngle = 10f;
    // 刹车力量
    public float brakeTorque = 100f;
    [HideInInspector]
    public Transform centerOfMass;

    public float maxSpeed = 140;
    public float minSpeed = 30;
    private bool isBrake = false;

    public AudioSource carEngineAudio;
    public AudioSource skidAudio;
    public AudioSource crashAudio;
    private float currentSpeed;

    public int[] speedArray;
    [HideInInspector]
    public GameObject leftLight;
    [HideInInspector]
    public GameObject rightLight;

    public GameObject skidMark;
    private bool isGround;

    private float vertical = 1;
    private float horizontal = 0;

    private void Awake()
    {
        InitProperty();
    }

    private void InitProperty()
    {
        this.flWheelCollider = transform.Find("WheelColliders/WheelFLCollider").GetComponent<WheelCollider>();
        this.frWheelCollider = transform.Find("WheelColliders/WheelFRCollider").GetComponent<WheelCollider>();
        this.rlWheelCollider = transform.Find("WheelColliders/WheelRLCollider").GetComponent<WheelCollider>();
        this.rrWheelCollider = transform.Find("WheelColliders/WheelRRCollider").GetComponent<WheelCollider>();
        this.flWheelModel = transform.Find("WheelFL");
        this.frWheelModel = transform.Find("WheelFR");
        this.rlWheelModel = transform.Find("WheelRL");
        this.rrWheelModel = transform.Find("WheelRR");
        this.flDiscBrake = transform.Find("WheelFL/DiscBrakeFL");
        this.frDiscBrake = transform.Find("WheelFR/DiscBrakeFR");
        this.centerOfMass = transform.Find("CenterOfMass").transform;
        this.leftLight = transform.Find("Lights/LeftLight").gameObject;
        this.rightLight = transform.Find("Lights/RightLight").gameObject;
    }

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
        horizontal = Input.acceleration.x;
        if (currentSpeed * vertical < 0)
        {
            isBrake = true;
        }
        else
        {
            isBrake = false;
        }
        if (currentSpeed > maxSpeed && vertical > 0 ||
            currentSpeed < -minSpeed && vertical < 0)
        {
            flWheelCollider.motorTorque = 0;
            frWheelCollider.motorTorque = 0;
        }
        else
        {
            flWheelCollider.motorTorque = vertical * motorTorque;
            frWheelCollider.motorTorque = vertical * motorTorque;
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

        flWheelCollider.steerAngle = horizontal * steerAngle;
        flWheelCollider.steerAngle = horizontal * steerAngle;

        RotateWheel();
        SteerWheel();
        EngineSound();
        ControlLight();
    }

    private void FixedUpdate()
    {
        Skid();
    }

    void ControlLight()
    {
        if (currentSpeed < -3)
        {
            leftLight.SetActive(true);
            rightLight.SetActive(true);
        }
        else
        {
            leftLight.SetActive(false);
            rightLight.SetActive(false);
        }
    }

    private Vector3 lastFLSkidPos = Vector3.zero;
    private Vector3 lastFRSkidPos = Vector3.zero;

    void Skid()
    {
        if (currentSpeed > 40 && Mathf.Abs(flWheelCollider.steerAngle) > 30)
        {
            WheelHit hit;
            if (flWheelCollider.GetGroundHit(out hit))
            {
                isGround = true;
                if (lastFLSkidPos.x != 0 && lastFLSkidPos.y != 0 && lastFLSkidPos.z != 0)
                {
                    Vector3 pos = hit.point;
                    pos.y += 0.05f;
                    Quaternion rotation = Quaternion.LookRotation(hit.point - lastFLSkidPos);
                    GameObject.Instantiate(skidMark, pos, rotation);
                }
                lastFLSkidPos = hit.point;
            }
            else
            {
                lastFLSkidPos = Vector3.zero;
            }
            if (frWheelCollider.GetGroundHit(out hit))
            {
                isGround = true;
                if (lastFRSkidPos.x != 0 && lastFRSkidPos.y != 0 && lastFRSkidPos.z != 0)
                {
                    Vector3 pos = hit.point;
                    pos.y += 0.05f;
                    Quaternion rotation = Quaternion.LookRotation(hit.point - lastFRSkidPos);
                    GameObject.Instantiate(skidMark, pos, rotation);
                }
                lastFRSkidPos = hit.point;
            }
            else
            {
                lastFRSkidPos = Vector3.zero;
            }
            if (!skidAudio.isPlaying && isGround)
            {
                skidAudio.Play();
            }
            else
            {
                skidAudio.Stop();
            }
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            crashAudio.Play();
        }
    }
}
