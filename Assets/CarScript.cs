using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IDEAS: max steer angle is fun for reverse handbrake turns - stunt driving / stunt parking...
//From unity docs:
//You might want to decrease physics timestep length in the Time window to get more stable car physics, especially if it’s a racing car that can achieve high velocities.
//To keep a car from flipping over too easily you can lower its Rigidbody
//center of mass a bit from script, and apply “down pressure” force that depends on car velocity.

public class CarScript : MonoBehaviour
{
    public WheelCollider wheelColliderLeftFront;
    public WheelCollider wheelColliderLeftBack;
    public WheelCollider wheelColliderRightFront;
    public WheelCollider wheelColliderRightBack;

    public Transform wheelLeftFront;
    public Transform wheelLeftBack;
    public Transform wheelRightFront;
    public Transform wheelRightBack;
    
    public Light leftLight;
    public Light rightLight;
    
    public float motorTorque = 2000f;
    public bool boostEngaged = false;
    public float boostForce = 1000f;
    public float maxSteerAngle = 90f;




    // Start is called before the first frame update
    void Start()
    {
        // var lights = GetComponents<Light>();
        // leftLight = lights[0];
        // rightLight = lights[1];
        Debug.Log("Brrm!  Car script started.");        
    }

    void SetLights(bool state){
            leftLight.enabled = state;
            rightLight.enabled = state;        
    }

    void FixedUpdate() {
        Rigidbody rb = GetComponent<Rigidbody>();
        var torque = motorTorque * (boostEngaged ? 10 : 1);
        wheelColliderLeftFront.motorTorque = Input.GetAxis("Vertical") * torque;
        wheelColliderRightFront.motorTorque = Input.GetAxis("Vertical") * torque;
        wheelColliderLeftFront.steerAngle = Input.GetAxis("Horizontal") * maxSteerAngle;
        wheelColliderRightFront.steerAngle = Input.GetAxis("Horizontal") * maxSteerAngle;
        if (boostEngaged){
            rb.AddForce(transform.forward * boostForce);
        }
    }

    void DropReset(){

        Rigidbody rb = GetComponent<Rigidbody>();
        transform.rotation =Quaternion.Euler(0, 180, 0);
        transform.Translate(0, 5, 0);
        rb.angularVelocity = new Vector3(0,0,0);
        
    }
    void policeLights(){

        float duration = 0.2f;
        Color color0 = Color.red;
        Color color1 = Color.blue;
        // set light color
        float t = Mathf.PingPong(Time.time, duration) / duration;
        leftLight.color = Color.Lerp(color0, color1, t);
        rightLight.color = Color.Lerp(color1, color0, t);
    }
    void Update(){
        
        boostEngaged = (Input.GetButton("Fire1") || Input.GetButton("Fire3"));

        if (Input.GetButtonDown("Fire2")){
            DropReset();
            SetLights(false);
        }        
        if (Input.GetButtonDown("Fire3")){
            SetLights(true);
        }        
        policeLights();
        
        //Update the visuals of the wheels
        //Align each wheel (visual element) to where the wheel collider is (in position and rotation)
        placeWheelAtCollider(wheelLeftFront, wheelColliderLeftFront, false);
        placeWheelAtCollider(wheelLeftBack, wheelColliderLeftBack, false);
        placeWheelAtCollider(wheelRightFront, wheelColliderRightFront, true);
        placeWheelAtCollider(wheelRightBack,  wheelColliderRightBack,  true);
        
    }

    void placeWheelAtCollider(Transform wheel, WheelCollider wheelCollider, bool shouldFlipOnZ){
        
        var pos = Vector3.zero;
        var rot = Quaternion.identity;
        wheelCollider.GetWorldPose(out pos, out rot);

        wheel.position = pos;
        if (shouldFlipOnZ){
            wheel.rotation = rot * Quaternion.Euler(0, 180, 0);
        }
        else {
            wheel.rotation = rot;
        }        
    }
}
