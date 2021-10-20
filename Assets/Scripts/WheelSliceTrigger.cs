using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelSliceTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other){

        WheelFunctions.currentSlice = this.name;
        WheelFunctions.amount = Convert.ToInt32(transform.GetChild(0).gameObject.tag);
        WheelFunctions.type = transform.GetChild(1).gameObject.tag;
    }
}
