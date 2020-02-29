using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCount : MonoBehaviour
{
    public UILabel label;
    public float totalTime = 0;
    private bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        label.enabled = false;
        totalTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            totalTime += Time.deltaTime;
            label.text = totalTime.ToString();
        }
    }

    public void Show()
    {
        isStart = true;
        label.enabled = true;
    }

    public void Stop()
    {
        isStart = false;
    }
}
