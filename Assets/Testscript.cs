using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// This script is the main car physics script, allowing the player to play as any player GameObject. This script can be given an AI script to act as an NPC car. 
// COPYRIGHT ALEX G (C) 2025
public class Testscript : MonoBehaviour
{
    public float maxSpeed; // the maximum speed the car should travel in meters per second
    private Rigidbody rb; // a rigidbody for collision
    public float maxAcceleration; // the maximum rate the car can accelerate
    public float currentSpeed; // how fast the car is travelling
    public float speedLimit; // the maximum speed the car can travel without speeding logic applying
    private float calcRotation = 0; // internal calulation value
    public float turnRate; // how fast the car turns
    public TMP_Text playerStatus; // the speedo and indicators
    public Camera driverPOV; // the viewport
    private bool indicatorLeft = false; // the indicators
    private bool indicatorRight = false;
    public bool isAnNPC; // determines if the car follows the logic of an external script
    public float brake; // the brake input
    public float accelerator;
    public float steering;
    private float stopTimer = 0; // how long the car has been stopped
    private float lastSpeedZone = 0; // for stop signs
    public Vector3 RespawnLoc;
    private TrafficLightManager CurrentLight = null; 
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // make sure user intentionally avoids assigning a camera to a car
        if (!driverPOV)
        {
            Debug.Log("No Camera assigned! ( this may be because the assigned vehicle is not the player vehicle)");
        }

    }
    // all internal calculations are in meters per second
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");
        // clamp acceleration values
        accelerator = Mathf.Min(accelerator, 1);
        accelerator = Mathf.Max(accelerator, -1);
        steering = Mathf.Max(steering, -1);
        steering = Mathf.Min(steering, 1);

        if (isAnNPC == false) // if this script is assigned to the player vehicle, use keyboard inputs, else use assigned AI script as input. Note: AI must be in the same oject
        {
            float multipliedTime = 600.0f * Time.deltaTime;
            Vector3 movement = new Vector3(moveVertical * multipliedTime* Mathf.Sin(calcRotation * Mathf.Deg2Rad), 0.0f, moveVertical * multipliedTime * Mathf.Cos(calcRotation * Mathf.Deg2Rad));
            rb.AddForce(movement * maxSpeed);
            if (moveVertical != 0) 
            {
                // The factors are different depending on if the car is an NPC
                if (Mathf.Abs(currentSpeed) < maxSpeed)
                {
                    currentSpeed += moveVertical * maxAcceleration;
                }
                if (Mathf.Abs(currentSpeed) > maxSpeed)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
                }
                if (Mathf.Abs(moveVertical) < 0.05)
                {
                    Debug.Log("coast");
                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
                }
                // The above is the second half of the main calculation, based on the results from the first half, executed in the previous frame.
                // how it works, for curious people: say you have a right angle triange, you have a known angle X ( calculated earlier, in degrees ), and the hypotenuse has a length of 1. your goal is to calculate the lengths of the other two sides,
                // If you remember SOHCAHTOA, you can use sin(X) and cos(X) to calculate the remaining two sides.  this correspons to the force exerted to the car along the x and z axes respectively

            }
        }
        else
        {
            Vector3 movement = new Vector3(accelerator * Mathf.Sin(calcRotation * Mathf.Deg2Rad), 0.0f, accelerator * Mathf.Cos(calcRotation * Mathf.Deg2Rad));
            rb.AddForce(movement * maxSpeed);
            if (accelerator != 0)
            {
                if (Mathf.Abs(currentSpeed) < maxSpeed)
                {
                    currentSpeed += accelerator * maxAcceleration;
                }
                if (Mathf.Abs(currentSpeed) > maxSpeed)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
                }
                if (Mathf.Abs(accelerator) < 0.05)
                {
                    Debug.Log("coast");
                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
                }
               

            }
        }
        if (Mathf.Abs(currentSpeed) > speedLimit && isAnNPC == false) // speeding offences work differentlty to NPC 
        {
            Debug.Log("You are speeding!");
        } else if (currentSpeed > speedLimit && isAnNPC == true)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speedLimit, brake);
        }

        if (moveVertical != 0 && isAnNPC == false) // speed math
        {
            transform.Rotate(new Vector3(0f, moveHorizontal, 0f) * turnRate * Time.deltaTime);
            calcRotation += moveHorizontal * turnRate * Time.deltaTime;
        }
        if (accelerator != 0 && isAnNPC == true)
        {
            transform.Rotate(new Vector3(0f, steering, 0f) * turnRate * Time.deltaTime);
            calcRotation += steering * turnRate * Time.deltaTime;
        }
        // the above is the first half of the calculation
        if (playerStatus && isAnNPC == false) // check that a canvas object has been assigned, and that this vehicle is not an npc
        {
            playerStatus.text = "Speed: " + Mathf.Abs(currentSpeed * 3.6f) + "KM / H";
            if (Input.GetKeyDown(KeyCode.Q)) // turn signal logic
            {
                indicatorLeft = !indicatorLeft;
                indicatorRight = false;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                indicatorLeft = false;
                indicatorRight = !indicatorRight;
            }
            if (indicatorLeft)
            {
                playerStatus.text = "Speed: " + Mathf.Abs(currentSpeed * 3.6f) + "KM / H" + " <-";
            }
            if (indicatorRight)
            {
                playerStatus.text = "Speed: " + Mathf.Abs(currentSpeed * 3.6f) + "KM / H" + " ->";
            }
        }
        if (driverPOV && isAnNPC == false)
        {
            driverPOV.transform.Rotate(0f, MouseX, 0f);

        }


    }
    // sign logic
    /*
  
     TO ADD A NEW SIGN:
     1. At line 159, press "Enter" or "Return" to create a new line.
     2. On line 160, insert your new block following this pattern:

        if(other.CompareTag("x"))
         {
       // What should happen to the player if they encounter this sign?
         }

        - Replace "x" with the exact tag name as shown in the Unity Inspector (case sensitive!).
        - Do not change "CompareTag"�keep the capitalization exactly as shown.
        - Make sure there are no duplicates! ( may cause unpredictable behavior!)

        EXAMPLE 1 (new block):
     if(other.CompareTag("SpeedLimit70kmh"))
     {
       speedLimit = 19.4;
     }
       EXAMPLE 2 (valid):
    if(other.CompareTag("SpeedLimit70kmh")){
      speedLimit = 19.4;
    }
    if(other.CompareTag("SpeedLimit80kmh")){
      speedLimit = 22.2;
    }
     EXAMPLE 3 (Not valid):
    if(other.CompareTag("SpeedLimit70kmh")){
      speedLimit = 19.4;
    }
    if(other.CompareTag("SpeedLimit70kmh")){
      speedLimit = 22.2;
    }

     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TestSpeedZoneA")) 
        {
            speedLimit = 7;
            Debug.Log(speedLimit);
        }
        if (other.CompareTag("TestSpeedZoneB"))
        {
            speedLimit = 3;
            Debug.Log(speedLimit);
        }
        if (other.CompareTag("StopSign"))
        {
            Debug.Log("Stop!");
            lastSpeedZone = speedLimit;
            stopTimer = 0;

        }
        if (other.CompareTag("Checkpoint"))
        {
            RespawnLoc = transform.position;
        }
        if (other.CompareTag("Respawn"))
        {
            Debug.Log("YOU DIED!");
            if (RespawnLoc == null)
            {
                Debug.LogError("Attempted to find a respawn location, but none was found");
            }
            else
            {
                transform.position = RespawnLoc;
            }

        }
        if (other.CompareTag("TrafficZone")) 
        {
            lastSpeedZone = speedLimit;
            CurrentLight = other.GetComponent<TrafficLightManager>();
        }
       
    }

       
    
    private void OnTriggerStay( Collider other)
    {
        if (other.CompareTag("StopSign"))
        {
            stopTimer += Time.deltaTime;
            if (stopTimer < 5)
            {
                speedLimit = 0;
                Debug.Log(stopTimer);
            }
            else
            {
                speedLimit = lastSpeedZone;
                Debug.Log("You May Proceed");
            }
        }
        if (other.CompareTag("TrafficZone"))
        {
            if (CurrentLight.lightState == "Danger")
            {
                speedLimit = 0;
            }
            if (CurrentLight.lightState == "Proceed")
            {
             
                speedLimit = lastSpeedZone;
            }
        }
    }
    private void OnTriggerExit( Collider other)
    {
        if(other.CompareTag("StopSign"))
        {
            stopTimer = 0;
        }

     
    }
}