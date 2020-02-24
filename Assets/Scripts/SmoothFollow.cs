using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;

    public float height = 3.5f;
    public float distance = 7f;
    public float smoothSpeed = 1;

    public AudioClip audioClip;//获取音乐
    private AudioSource audioSource;

    private void Start()
    {
        //audioSource = this.GetComponent<AudioSource>();//获取Audio Source
        //audioSource.Play();//播放
    }

    // Update is called once per frame
    void Update()
    {
        // 目标的前方向
        Vector3 targetForward = target.forward;
        targetForward.y = 0;
        // 摄像机的前方向
        Vector3 currentForward = transform.forward;
        currentForward.y = 0;
        // 平滑移动
        Vector3 forward = Vector3.Lerp(currentForward, targetForward.normalized, smoothSpeed * Time.deltaTime);
        // 摄像机要调整的距离
        Vector3 targetPos = target.position + Vector3.up * height - forward * distance;
        // 更改摄像机的坐标与朝向
        transform.position = targetPos;
        transform.LookAt(target);
    }
}
