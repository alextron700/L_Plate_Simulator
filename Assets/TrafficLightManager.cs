using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficLightManager : MonoBehaviour
{
    public string lightState; // initial light state
    public Collider TrafficDetector;
    public Collider TrafficZone;
    public bool isSmart;
    public float ProceedTransitionTimer;
    public float DangerTransitionTimer;
    public float timer;
    public float maxTimer;
    // Start is called before the first frame update
    void Start()
    {
        if(lightState == null)
        {
            lightState = "Stop";
        }
        if(isSmart == true && TrafficDetector == null)
        {
            isSmart = false;
        }
        if(ProceedTransitionTimer <=0  && ! isSmart)
        {
            ProceedTransitionTimer = 20f;
        }
        if (DangerTransitionTimer <= 0  && ! isSmart)
        {
            DangerTransitionTimer = 20f;
        }
        maxTimer = ProceedTransitionTimer + DangerTransitionTimer;
       // timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (isSmart)
        {

            Collider[] objectInTrafficDetector = Physics.OverlapBox(TrafficDetector.bounds.center, TrafficDetector.bounds.extents, TrafficDetector.transform.rotation);
            foreach( var obj in objectInTrafficDetector)
            {
                if (obj.CompareTag("Player"))
                {
                    timer = 0;
                }
            }
            if(timer >= maxTimer)
            {
                lightState = "Proceed";
            }
            else
            {
                lightState = "Danger";
            }
        }
        if(!isSmart)
        {
            if(timer >= ProceedTransitionTimer)
            {
                lightState = "Proceed";
            }
            if(timer >= ProceedTransitionTimer + DangerTransitionTimer)
            {
                lightState = "Danger";
                timer = 0;
            }
        }
        
    }
 //  void OnTriggerEnter(Collider other)
 //  {
   //    Debug.Log("Trigger entered by: " + other.gameObject.name);

  // }
}
