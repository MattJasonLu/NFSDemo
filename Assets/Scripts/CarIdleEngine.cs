using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarIdleEngine : MonoBehaviour
{
    public AudioSource engineSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 按键油门
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!engineSound.isPlaying)
            {
                engineSound.Play();
            }
            engineSound.pitch = 0.2f + Mathf.Abs(Input.GetAxis("Vertical"));
        } 
        else
        {
            engineSound.pitch = 0.2f;
        }
    }

    public void DisableSelf()
    {
        engineSound.pitch = 0.2f;
        this.enabled = false;
    }
}
