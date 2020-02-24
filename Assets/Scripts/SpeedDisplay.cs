using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    public WheelCollider flWheelCollider;
    private UILabel label;
    public float currentSpeed;
    public Transform pointContainer;
    private float zRotation;

    // Start is called before the first frame update
    void Start()
    {
        label = this.GetComponent<UILabel>();
        zRotation = pointContainer.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // 获取速度
        float speed = Mathf.Round(flWheelCollider.rpm * (flWheelCollider.radius * 2 * Mathf.PI) * 60 / 1000);
        if (speed < 0) speed = Mathf.Abs(speed);
        label.text = speed.ToString();
        // 获取码表度数
        currentSpeed = speed;
        float newZRotation = zRotation - currentSpeed * (270 / 140f);
        pointContainer.eulerAngles = new Vector3(0, 0, newZRotation);
    }
}
