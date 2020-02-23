using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShadow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = transform.parent.position + Vector3.up * 5;
        this.transform.eulerAngles = new Vector3(90, 0, 0);
    }
}
