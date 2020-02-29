using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private TimeCount timeCount;

    // Start is called before the first frame update
    void Start()
    {
        timeCount = this.GetComponent<TimeCount>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Car")
        {
            timeCount.Stop();
            float nowTotalTime = timeCount.totalTime;
            float oldTotalTime = 99999;
            if (PlayerPrefs.HasKey("bestTime"))
            {
                // 破纪录
                oldTotalTime = PlayerPrefs.GetFloat("bestTime");
                
            }
            timeCount.label.text = "当前时间：" + nowTotalTime + "s\n" + "最快时间：" + oldTotalTime + "s";
            if (nowTotalTime < oldTotalTime)
            {
                oldTotalTime = nowTotalTime;
            }
            PlayerPrefs.SetFloat("bestTime", oldTotalTime);
        }
    }
}
