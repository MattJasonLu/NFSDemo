using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberCount : MonoBehaviour
{
    public int startCount = 5;
    public UILabel timeCount;
    private Driving driving;

    // Start is called before the first frame update
    void Start()
    {
        driving = GameObject.FindGameObjectWithTag("Car").GetComponent<Driving>();
        StartCoroutine(Count());
    }

    // Update is called once per frame
    void Update()
    {
        timeCount.text = startCount.ToString();
    }

    // 发车倒计时
    IEnumerator Count()
    {
        while (startCount > 0)
        {
            yield return new WaitForSeconds(1);
            startCount--;
        }
        yield return new WaitForSeconds(0.1f);
        driving.enabled = true;
        timeCount.enabled = false;
        this.GetComponent<CarIdleEngine>().DisableSelf();
    }
}
