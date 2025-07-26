using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{ public int timeOfDay;
   public float degreesPerHour;
    public bool doDayightCycle;
    public float timeScale;
    public float stopTime;
    public float currentTime;
    public bool stopCycle;
    // Start is called before the first frame update
    void Start()
    {
        if(timeOfDay > 24 || timeOfDay < 0) // check that time is not beyond the 24 hour range
        {
            timeOfDay = 0;
        }
        if(Mathf.Abs(degreesPerHour) > 360) // check that the degrees perHour is not more than a full circle
        {
            degreesPerHour = 0;
        }
        transform.Rotate(new Vector3(timeOfDay * degreesPerHour, 0f, 0f)); // rotate to match the corresponding time
        if(doDayightCycle == true || stopCycle == true) // cancel the rotation if we are to cycle 
        {
            transform.Rotate(new Vector3(-timeOfDay * degreesPerHour, 0f, 0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doDayightCycle == true || stopCycle == true) // if we are cycling, or need to stop at a specific time
        {
            if (!(currentTime >= stopTime && stopCycle == true)) // acts only if we need time to stop
            {
              
                transform.Rotate(new Vector3((degreesPerHour) * (Time.deltaTime * timeScale), 0f, 0f)); // rotate at the approriate rate to simulate a proper time scale
                currentTime += degreesPerHour * Time.deltaTime * timeScale;
            }
            if (stopCycle == false) {
                transform.Rotate(new Vector3((degreesPerHour) * (Time.deltaTime * timeScale), 0f, 0f));
            }
        }
    }
}
